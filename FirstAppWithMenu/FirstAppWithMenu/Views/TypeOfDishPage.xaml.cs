using System.Collections.Generic;
using System.Linq;
using FirstAppWithMenu.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeOfDishPage : ContentPage
    {
        // Конструктор класса TypeOfDishPage
        public TypeOfDishPage()
        {
            InitializeComponent();
            Title = "Тип блюда"; // Устанавливаем заголовок страницы
            GetItems(); // Вызываем метод для отображения элементов
        }

        // Асинхронный метод для получения и отображения элементов на странице
        protected async void GetItems()
        {
            // Создаем вертикальный макет для размещения элементов
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Создаем прокручиваемую область
            var scroll = new ScrollView
            {
                Content = layout,
                Orientation = ScrollOrientation.Vertical
            };

            // Список с информацией о типах блюд и их рецептах
            var scrollItems = new List<(string labelText, Color buttonColor, List<Recipe> recipes)>
            {
                // Перечисляем различные типы блюд и получаем соответствующие рецепты из базы данных
                ("  Горячее", Color.Wheat, await App.Database.GetItemsByTypeOfDishAsync("Горячее")),
                ("  Десерты", Color.LightCyan, await App.Database.GetItemsByTypeOfDishAsync("Десерт")),
                ("  Салаты", Color.PowderBlue, await App.Database.GetItemsByTypeOfDishAsync("Салат")),
                ("  Супы", Color.MistyRose, await App.Database.GetItemsByTypeOfDishAsync("Суп")),
                ("  Выпечка", Color.Lavender, await App.Database.GetItemsByTypeOfDishAsync("Выпечка")),
                ("  Гарнир", Color.PapayaWhip, await App.Database.GetItemsByTypeOfDishAsync("Гарнир")),
                ("  Жаркое", Color.Wheat, await App.Database.GetItemsByTypeOfDishAsync("Жаркое")),
                ("  Закуска", Color.PowderBlue, await App.Database.GetItemsByTypeOfDishAsync("Закуска")),
                ("  Каша", Color.MistyRose, await App.Database.GetItemsByTypeOfDishAsync("Каша")),
                ("  Мясное блюдо", Color.Lavender, await App.Database.GetItemsByTypeOfDishAsync("Мясное блюдо")),
                ("  Напиток", Color.PapayaWhip, await App.Database.GetItemsByTypeOfDishAsync("Напиток")),
                ("  Соус", Color.LightCyan, await App.Database.GetItemsByTypeOfDishAsync("Соус")),
                ("  Хлеб", Color.PowderBlue, await App.Database.GetItemsByTypeOfDishAsync("Хлеб"))
            };

            // Для каждого элемента списка создаем кнопки с рецептами и добавляем их на страницу
            foreach (var item in scrollItems)
            {
                layout.Children.Add(CreateButtonRow(item.labelText, item.buttonColor, item.recipes));
            }

            Content = scroll;
        }


        // Метод для создания строки с кнопками для определенного типа блюд
        private StackLayout CreateButtonRow(string labelText, Color buttonColor, List<Recipe> buttonNames)
        {
            // Создаем вертикальный макет
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Создаем метку с текстом типа блюда
            var label = new Label
            {
                Text = labelText,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(5, 0, 0, 0),
                TextColor = Color.Black
            };

            stackLayout.Children.Add(label); // Добавляем метку в вертикальный макет

            // Создаем горизонтальную прокручиваемую область для кнопок
            var scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            // Создаем горизонтальный макет для кнопок
            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            // Для каждого рецепта создаем кнопку, устанавливаем ее параметры и добавляем обработчик для нажатия
            foreach (Recipe item in buttonNames)
            {
                var button = new Xamarin.Forms.Button
                {
                    // Устанавливаем параметры кнопки: текст, цвет фона, размер, обработчик нажатия и т.д.
                    Text = item.Name.Trim(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Xamarin.Forms.Button)),
                    Margin = new Thickness(10),
                    HeightRequest = 118,
                    BackgroundColor = buttonColor,
                    CornerRadius = 10,
                    TextColor = Color.Black
                };

                button.Clicked += async (sender, e) =>
                {
                    List<Recipe> recipes = await App.FavDatabase.GetItemsAsync();

                    bool isExist = recipes.Any(r => r.Name == item.Name);

                    Page recipePage;

                    if (!isExist)
                    {
                        // Создаем новую страницу рецепта с кнопкой "Добавить в избранное"
                        recipePage = new PageOfRecipe();
                    }
                    else
                    {
                        // Создаем новую страницу рецепта с кнопкой "Удалить из избранного"
                        recipePage = new DeleteFromFavPage();
                    }

                    recipePage.BindingContext = item; // Привязываем выбранный рецепт к контексту страницы
                    await Navigation.PushAsync(recipePage); // Переходим на страницу с деталями рецепта

                };

                buttonsStackLayout.Children.Add(button); // Добавляем кнопку в горизонтальный макет
            }
            scrollView.Content = buttonsStackLayout; // Устанавливаем кнопки в прокручиваемую область

            stackLayout.Children.Add(scrollView); // Добавляем прокручиваемую область в вертикальный макет

            return stackLayout; // Возвращаем сформированный макет
        }
    }
}

