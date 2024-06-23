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

        public FiltersPage()
        {
            InitializeComponent();
            Title = "Фильтры";

            GetSearchIngredients();
            searchEntry.TextChanged += SearchBar_TextChanged;
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
                    // Получение названия переключателя путем удаления лишних пробелов
                    string switchName = item.Trim();

                    // Если переключатель включен (IsToggled) и название не содержится в массиве switchNames
                    if (switchControl.IsToggled && !switchNames.Contains(switchName))
                    {
                        // Добавляем название переключателя в массив switchNames
                        switchNames.Add(switchName);
                    }
                    // Если переключатель выключен и название содержится в массиве switchNames
                    else if (!switchControl.IsToggled && switchNames.Contains(switchName))
                    {
                        // Удаляем название переключателя из массива switchNames
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
            //Получние списка всех ингредиентов из таблицы SQLite
            ingredientsList = await App.Database.GetAllIngredientsAsync();
            //Сортировка списка ингредиентов
            ingredientsList.Sort(); 

            //Перебор всех полученных ингредиентов
            foreach (var ingredient in ingredientsList)
            {
                StackLayout ingredientLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new CheckBox(), // Создание чекбокса для выбора ингредиента
                        new Label { Text = ingredient } // Название ингредиента
                    }
                };
                // Добавление макета ингредиента к макету всех ингредиентов
                ingredientsLayout.Children.Add(ingredientLayout); 
            }
        }//Получение списка ингредиентов для поиска по ним

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
            // Если текстовое поле для поиска не пустое, сбросить его содержимое
            if (!string.IsNullOrEmpty(searchEntry.Text))
            {
                searchEntry.Text = string.Empty;
            }

            // Перебираем все дочерние элементы в ingredientsLayout
            foreach (var ingredientLayout in ingredientsLayout.Children)
            {
                // Проверяем, является ли текущий элемент типом StackLayout
                if (ingredientLayout is StackLayout stackLayout)
                {
                    // Если в StackLayout есть дочерний элемент и это CheckBox
                    if (stackLayout.Children.Count > 0 && stackLayout.Children[0] is CheckBox checkBox)
                    {
                        // Сбрасываем значение галочки чекбокса
                        checkBox.IsChecked = false;
                    }
                }
            }
            // Сброс состояния переключателя "Рецепты с доп. ингредиентами"
            additionalIngredientsSwitchControl.IsToggled = false;

            // Сброс состояния всех переключателей типов блюд (mealTypeSwitches)
            foreach (var mealSwitch in mealTypeSwitches)
            {
                mealSwitch.IsToggled = false;
            }

            // Сброс состояния всех переключателей типов блюд (dishTypeSwitches)
            foreach (var dishSwitch in dishTypeSwitches)
            {
                dishSwitch.IsToggled = false;
            }
        }//Обработчик нажатия на кнопку "Сбросить все"

        private async void ReadyButton_Clicked(object sender, EventArgs e)
        {
            // Если текстовое поле для поиска не пустое, сбросить его содержимое
            if (!string.IsNullOrEmpty(searchEntry.Text))
            {
                searchEntry.Text = string.Empty;
            }

            // Сохранить выбранные ингредиенты
            SaveSelectedIngredients();

            // Получение текущего состояния переключателя "Рецепты с доп. ингредиентами"
            bool isSwitch = additionalIngredientsSwitchControl.IsToggled;

            // Получение списка названий выбранных переключателей
            List<string> selectedSwitchNames = GetSelectedSwitchNames();

            // Создание новой страницы FilteredRecipes.
            // Передача ей выбранных ингредиентов, выбранных переключателей и состояние переключателя "Рецепты с доп. ингредиентами"
            FilteredRecipes filteredRecipePage = new FilteredRecipes(selectedIngredients, selectedSwitchNames, isSwitch);

            // Переход на страницу с отфильтрованными рецептами
            await Navigation.PushAsync(filteredRecipePage);
        }

        private List<string> selectedIngredients = new List<string>();

        private void SaveSelectedIngredients()
        {
            // Очистка списка выбранных ингредиентов для предотвращения дублирования данных
            selectedIngredients.Clear();

            // Перебор всех представленных пользователю ингредиентов
            foreach (var childLayout in ingredientsLayout.Children)
            {
                // Проверяем, что текущий элемент является StackLayout
                if (childLayout is StackLayout stackLayout)
                {
                    // Находим элемент Label внутри StackLayout
                    var label = stackLayout.Children.OfType<Label>().FirstOrDefault();
                    // Находим элемент CheckBox внутри StackLayout
                    var checkBox = stackLayout.Children.OfType<CheckBox>().FirstOrDefault();

                    // Проверяем, что элемент Label и CheckBox не равны null и что CheckBox отмечен
                    if (label != null && checkBox != null && checkBox.IsChecked)
                    {
                        // Получаем название ингредиента из элемента Label
                        string ingredientName = label.Text;
                        // Добавляем название ингредиента в список выбранных ингредиентов
                        selectedIngredients.Add(ingredientName);
                    }
                }
            }
        }// Формирование списка выбранных ингредиентов
    }
}