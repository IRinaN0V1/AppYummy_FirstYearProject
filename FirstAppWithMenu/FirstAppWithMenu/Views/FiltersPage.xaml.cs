using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersPage : ContentPage
    {
        List<string> ingredientsList;
        private List<Switch> mealTypeSwitches; // список свитчей для типов приема пищи
        private List<Switch> dishTypeSwitches; // список свитчей для типов блюд
        private Switch switchControl;

        public FiltersPage()
        {
            InitializeComponent();
            Title = "Фильтры";

            GetSearchIngredients();
            GetFiltersButton();
        }

        private async Task GetFiltersButton()
        {
            var typesOfMealNames = await App.Database.GetTypesOfMealAsync();
            var typesOfDishNames = await App.Database.GetTypesOfDishAsync();

            var (mealButtonRow, mealSwitches) = CreateButtonRow(" Тип приема пищи", Color.Gray, typesOfMealNames);
            var (dishButtonRow, dishSwitches) = CreateButtonRow(" Тип блюда", Color.Gray, typesOfDishNames);

            filtersButtonLayout.Children.Add(mealButtonRow);
            filtersButtonLayout.Children.Add(dishButtonRow);

            mealTypeSwitches = mealSwitches; // сохраняем список свитчей
            dishTypeSwitches = dishSwitches; // сохраняем список свитчей
        }



        private List<string> switchNames = new List<string>();

        //private Dictionary<string, bool> switchStates = new Dictionary<string, bool>();
        private List<string> GetSelectedSwitchNames()
        {
            return switchNames;
        }

        private (StackLayout, List<Switch>) CreateButtonRow(string labelText, Color buttonColor, List<string> buttonNames)
        {
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var label = new Label
            {
                Text = labelText,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(5, 0, 0, 0),
                TextColor = Color.Black
            };

            stackLayout.Children.Add(label);

            var scrollView = new ScrollView
            {
                Orientation
            = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            List<Switch> switchControls = new List<Switch>(); // Создаем список для свитчей

            foreach (String item in buttonNames)
            {
                var frame = new Frame
                {
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                    BackgroundColor = Color.NavajoWhite,
                    CornerRadius = 5
                };

                var switchControl = new Switch
                {
                    HorizontalOptions = LayoutOptions.Start,
                    IsToggled = false,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                switchControl.Toggled += (sender, e) =>
                {
                    // Запись названия переключателя в массив строк при изменении его состояния
                    string switchName = item.Trim();
                    if (switchControl.IsToggled && !switchNames.Contains(switchName))
                    {
                        switchNames.Add(switchName);
                    }
                    else if (!switchControl.IsToggled && switchNames.Contains(switchName))
                    {
                        switchNames.Remove(switchName);
                    }
                };

                // Добавляем свитч в список
                switchControls.Add(switchControl);

                var buttonLabel = new Label
                {
                    Text = item.Trim(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black
                };

                var stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { switchControl, buttonLabel }
                };

                frame.Content = stack;
                buttonsStackLayout.Children.Add(frame);
            }

            scrollView.Content = buttonsStackLayout;
            stackLayout.Children.Add(scrollView);

            return (stackLayout, switchControls); // Возвращаем также список свитчей
        }

        private async void GetSearchIngredients()
        {
            ingredientsList = await App.Database.GetAllIngredientsAsync();
            ingredientsList.Sort(); // Сортировка ингредиентов


            foreach (var ingredient in ingredientsList)
            {
                StackLayout ingredientLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                    new CheckBox(), // Чекбокс для выбора ингредиента
                    new Label { Text = ingredient } // Название ингредиента
                    }
                };

                ingredientsLayout.Children.Add(ingredientLayout); // Добавление макета ингредиента к макету всех ингредиентов
            }

            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            searchEntry.TextChanged += SearchBar_TextChanged;
        }

        private Dictionary<string, bool> ingredientSelections = new Dictionary<string, bool>();

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            StackLayout parentLayout = (StackLayout)checkBox.Parent;
            Label label = (Label)parentLayout.Children[1];

            string ingredientName = label.Text;

            if (ingredientSelections.ContainsKey(ingredientName))
            {
                ingredientSelections[ingredientName] = checkBox.IsChecked;
            }
            else
            {
                ingredientSelections.Add(ingredientName, checkBox.IsChecked);
            }
        }

        private void UpdateCheckBoxes()
        {
            // Перебор всех дочерних элементов в ingredientsLayout
            foreach (var ingredientLayout in ingredientsLayout.Children)
            {
                // Получение Label из второго дочернего элемента StackLayout
                Label label = (Label)((StackLayout)ingredientLayout).Children[1];
                // Получение текста ингредиента
                string ingredientName = label.Text;

                // Проверка, содержит ли ingredientSelections ингредиент с данным именем
                if (ingredientSelections.ContainsKey(ingredientName))
                {
                    // Получение CheckBox из первого дочернего элемента StackLayout
                    CheckBox checkBox = (CheckBox)((StackLayout)ingredientLayout).Children[0];
                    // Установка состояния галочки CheckBox в соответствии с ingredientSelections
                    checkBox.IsChecked = ingredientSelections[ingredientName];
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();

            ingredientsLayout.Children.Clear();

            var filteredItems = ingredientsList.Where(item => item.ToLower().Contains(searchText)).ToList();

            foreach (var ingredient in filteredItems)
            {
                StackLayout ingredientLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new CheckBox { Margin = new Thickness(10, 0) },
                        new Label { Text = ingredient }
                    }
                };

                CheckBox checkBox = (CheckBox)ingredientLayout.Children[0];
                checkBox.CheckedChanged += CheckBox_CheckedChanged;

                ingredientsLayout.Children.Add(ingredientLayout);
            }

            UpdateCheckBoxes();
        }

        private async void ResetAllButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchEntry.Text))
            {
                searchEntry.Text = string.Empty;
            }
            // Сбросить все галочки
            foreach (var ingredientLayout in ingredientsLayout.Children)
            {
                if (ingredientLayout is StackLayout stackLayout)
                {
                    if (stackLayout.Children.Count > 0 && stackLayout.Children[0] is CheckBox checkBox)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }
            importantSwitchControl.IsToggled = false;

            foreach (var mealSwitch in mealTypeSwitches)
            {
                mealSwitch.IsToggled = false;
            }

            foreach (var dishSwitch in dishTypeSwitches)
            {
                dishSwitch.IsToggled = false;
            }
        }

        private async void ReadyButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchEntry.Text))
            {
                searchEntry.Text = string.Empty;
            }

            SaveSelectedIngredients();
            bool isSwitch = importantSwitchControl.IsToggled;

            List<string> selectedSwitchNames = GetSelectedSwitchNames();
            FilteredRecipes filteredRecipePage = new FilteredRecipes(selectedIngredients, selectedSwitchNames, isSwitch);


            await Navigation.PushAsync(filteredRecipePage);

        }

        private List<string> selectedIngredients = new List<string>();

        //СПИСОК ВЫБРАННЫХ ИНГРЕДИЕНТОВ
        private void SaveSelectedIngredients()
        {
            selectedIngredients.Clear(); // Очищаем массив для предотвращения дублирования данных

            foreach (var childLayout in ingredientsLayout.Children)
            {
                if (childLayout is StackLayout stackLayout)
                {
                    var label = stackLayout.Children.OfType<Label>().FirstOrDefault();
                    var checkBox = stackLayout.Children.OfType<CheckBox>().FirstOrDefault();

                    if (label != null && checkBox != null && checkBox.IsChecked)
                    {
                        string ingredientName = label.Text;
                        selectedIngredients.Add(ingredientName);
                    }
                }
            }
        }
    }
}