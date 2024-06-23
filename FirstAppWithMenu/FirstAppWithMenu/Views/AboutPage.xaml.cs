using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstAppWithMenu.Data;
using FirstAppWithMenu.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FirstAppWithMenu.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            // Инициализация компонентов страницы
            InitializeComponent();
            // Добавление категорий  на главную страницу
            AddCategoriesButtons();
            //// Получение случайных элементов и добавление их на страницу
            GetItemsRandom();
        }

        private void AddCategoriesButtons()
        {
            List<string> categories = new List<string> { "Завтрак", "Обед", "Ужин", "Перекус", "Закуска" };

            foreach (string category in categories)
            {
                Button button = new Button
                {
                    Text = category,
                    Margin = new Thickness(4),
                    WidthRequest = 90,
                    HeightRequest = 115,
                    BackgroundColor = Color.LightSalmon,
                    TextColor = Color.Black,
                    FontSize = 12
                };

                // Привязка данных к кнопке
                button.BindingContext = category;

                button.Clicked += CategoryButton_Clicked;

                categoriesStackLayout.Children.Add(button);
            }
        }

        private async void CategoryButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string category = (string)button.BindingContext;
            // Выполнение перехода на страницу SelectedCategoryPage
            await Navigation.PushAsync(new SelectedCategoryPage(category));
        }

        protected async void GetItemsRandom()
        {
            // Получение списка рецептов из базы данных асинхронно
            List<Recipe> items = await App.Database.GetItemsAsync();

            // Перемешивание списка рецептов в случайном порядке
            var random = new Random();
            var shuffledItems = items.OrderBy(x => random.Next()).ToList();

            List<Recipe> randomRecipes = shuffledItems.Take(3).ToList();
            foreach (Recipe item in randomRecipes)
            {
                // Создание горизонтального стека для каждой пары картинка-текст
                var recipeStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                // Создание кнопки-картинки для перехода на страницу рецепта
                var button = new ImageButton
                {
                    Margin = new Thickness(10, 0),                              // Установка внешних отступов
                    VerticalOptions = LayoutOptions.CenterAndExpand,            // Установка выравнивания заголовка по вертикали
                    WidthRequest = 350,                                         // Установка ширины картинки
                    HeightRequest = 350,                                        // Установка высоты картинки
                    Source = item.Image,                                        // Установка изображения
                    BackgroundColor = Color.Transparent                         // Установка прозрачного фона
                };

                //Название рецепта
                var dishLabel = new Label
                {
                    Text = item.Name,                                           // Установка названия рецепта
                    HorizontalOptions = LayoutOptions.Start,                    // Выравнивание по левому краю контейнера
                    Margin = new Thickness(0, 5),                               // Установка внешних отступов
                    TextColor = Color.Black,                                    // Установка цвета текста
                    FontSize = 17                                               // Установка размера шрифта
                };

                // Привязка данных к кнопке
                button.BindingContext = item;

                // Обработчик события нажатия на ImageButton для открытия страницы рецепта
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

                    recipePage.BindingContext = item;
                    await Navigation.PushAsync(recipePage);

                };

                recipeStackLayout.Children.Add(button);
                recipeStackLayout.Children.Add(dishLabel);

                // Добавление горизонтального стека в основной стек
                recommendationStackLayout.Children.Add(recipeStackLayout);
            }
        }
    }
}

