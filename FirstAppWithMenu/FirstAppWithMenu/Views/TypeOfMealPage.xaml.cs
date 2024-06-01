using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeOfMealPage : ContentPage
    {
        public TypeOfMealPage()
        {
            InitializeComponent();
            Title = "Тип приема пищи"; // Устанавливаем заголовок страницы
            GetItems(); // Вызываем метод для отображения элементов на странице

        }

        // Асинхронный метод для получения элементов и отображения на странице
        protected async void GetItems()
        {
            // Создаем вертикальный макет и добавляем в него элементы
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            var scroll = new ScrollView // Создаем прокручиваемую область
            {
                Content = layout,
                Orientation = ScrollOrientation.Vertical
            };

            // Создаем список с информацией о типах блюд и соответствующих им рецептах
            var scrollItems = new List<(string labelText, Color buttonColor, List<Recipe> recipes)>
            {
                // Здесь перечислены различные типы блюд и вызывается метод для получения рецептов по типу блюда
                ("  Завтраки", Color.Wheat, await App.Database.GetItemsByTypeOfMealAsync("Завтрак")),
                ("  Обеды", Color.LightCyan, await App.Database.GetItemsByTypeOfMealAsync("Обед")),
                ("  Ужины", Color.PowderBlue, await App.Database.GetItemsByTypeOfMealAsync("Ужин")),
                ("  Перекусы", Color.MistyRose, await App.Database.GetItemsByTypeOfMealAsync("Перекус")),
                ("  Закуски", Color.PapayaWhip, await App.Database.GetItemsByTypeOfMealAsync("Закуска"))
            };

            // Добавляем кнопки с названиями блюд в макет
            foreach (var item in scrollItems)
            {
                layout.Children.Add(CreateButtonRow(item.labelText, item.buttonColor, item.recipes));
            }
            // Устанавливаем прокручиваемую область как содержимое страницы
            Content = scroll;
        }

        // Метод для создания строки с кнопками для конкретного типа блюд
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

            stackLayout.Children.Add(label); // Добавляем метку в макет

            // Создаем горизонтальную прокручиваемую область для кнопок
            var scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            // Создаем горизонтальный макет для кнопок с рецептами
            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            // Для каждого рецепта создаем кнопку и добавляем ее в макет
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
                    List<Recipe> recipes = await App.FavDatabase.GetItemsAsync(); // Получение всех избранных рецептов

                    bool isExist = recipes.Any(r => r.Name == item.Name); // Проверка наличия текущего рецепта в избранном
                                                                          //Переход на страницу рецепта в зависимости от значения isExist
                    if (!isExist)
                    {
                        // Создаем новую страницу рецепта с кнопкой "Добавить в избранное"
                        PageOfRecipe recipePage = new PageOfRecipe();
                        recipePage.BindingContext = item; // Привязываем выбранный рецепт к контексту страницы

                        await Navigation.PushAsync(recipePage); // Переходим на страницу с деталями рецепта

                    }
                    else
                    {
                        // Создаем новую страницу рецепта с кнопкой "Удалить из избранного"
                        DeleteFromFavPage recipePage = new DeleteFromFavPage();
                        recipePage.BindingContext = item; // Привязываем выбранный рецепт к контексту страницы

                        await Navigation.PushAsync(recipePage); // Переходим на страницу с деталями рецепта
                    }
                };

                buttonsStackLayout.Children.Add(button); // Добавляем кнопку в макет горизонтальной прокручиваемой области
            }

            scrollView.Content = buttonsStackLayout; // Устанавливаем содержимое прокручиваемой области

            stackLayout.Children.Add(scrollView); // Добавляем прокручиваемую область в вертикальный макет

            return stackLayout; // Возвращаем сформированный макет
        }
    }
}