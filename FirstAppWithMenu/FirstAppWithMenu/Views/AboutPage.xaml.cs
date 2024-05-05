using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstAppWithMenu.Data;
using FirstAppWithMenu.Models;

namespace FirstAppWithMenu.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            AddPopularItems();
        }

        private void AddPopularItems()
        {
            // Создание вертикального StackLayout для всех элементов
            var mainStackLayout = new StackLayout();

            // Надпись "Что в меню на сегодня?"
            var titleLabel = new Label
            {
                Text = "Что в меню на сегодня?",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 20),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                FontSize = 25 // Установка большого размера шрифта
            };
            mainStackLayout.Children.Add(titleLabel);

            // Кнопки с типами блюд
            var categoriesStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var categories = new string[] { "Завтраки", "Обеды", "Ужины", "Перекусы" };
            foreach (var category in categories)
            {
                var categoryButton = new Button
                {
                    Text = category,
                    Margin = new Thickness(4),
                    WidthRequest = 90,
                    HeightRequest = 115,
                    BackgroundColor = Color.LightSalmon, // Цвет кнопки
                    Background = new LinearGradientBrush
                    {
                        GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.LightSalmon, 0), // Начальный цвет градиента
                    new GradientStop(Color.Moccasin, 1) // Конечный цвет градиента
                },
                        StartPoint = new Point(0, 0), // Начальная точка градиента
                        EndPoint = new Point(1, 0) // Конечная точка градиента
                    },
                    TextColor = Color.Black,
                    FontSize = 12
                };

                // Обработчик события нажатия на кнопку
                categoryButton.Clicked += async (sender, e) =>
                {
                    // Выполнение навигации на страницу TypeOfMealPage
                    await Navigation.PushAsync(new TypeOfMealPage());
                };

                categoriesStackLayout.Children.Add(categoryButton);
            }
            mainStackLayout.Children.Add(categoriesStackLayout);

            // Надпись "Популярное"
            var popularLabel = new Label
            {
                Text = "Популярное",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 20),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                FontSize = 25
            };
            mainStackLayout.Children.Add(popularLabel);

            // Создание горизонтального ScrollView
            var scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never
            };

            // Создание вертикального StackLayout для кнопок и соответствующих надписей
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            // Создание кнопок с названиями блюд и изображениями
            var dishes = new string[] { "   Сочный греческий салат с фетой", "    Том Ям", "    Профитроли шоколадные" };
            var dishImages = new string[] { "https://i.pinimg.com/564x/5a/ad/eb/5aadeb455c881635ff711a13f96e0d29.jpg", "https://i.pinimg.com/564x/db/5f/cc/db5fcc45298662d8bc3d934d92015dd6.jpg", "https://i.pinimg.com/736x/57/5f/e2/575fe20e686a4720c93877e761cfb72b.jpg" };
            for (int i = 0; i < dishes.Length; i++)
            {
                var buttonStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center
                };

                var button = new ImageButton
                {
                    Margin = new Thickness(10, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    WidthRequest = 350,
                    HeightRequest = 350, // Устанавливаем фиксированную высоту кнопок
                    Source = dishImages[i],
                    BackgroundColor = Color.Transparent // Прозрачный фон кнопки
                };

                var dishLabel = new Label
                {
                    Text = dishes[i],
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 5),
                    TextColor = Color.Black,
                    FontSize = 17
                };

                buttonStackLayout.Children.Add(button);
                buttonStackLayout.Children.Add(dishLabel);

                stackLayout.Children.Add(buttonStackLayout);
            }

            scrollView.Content = stackLayout;
            mainStackLayout.Children.Add(scrollView);

            // Добавление всех элементов в Content
            Content = mainStackLayout;
        }


        protected override async void OnAppearing()
        {
            // привязка данных
            friendsList.ItemsSource = await App.Database.GetItemsAsync();

            base.OnAppearing();
        }
        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Recipe selectedFriend = (Recipe)e.SelectedItem;
            Breakfast friendPage = new Breakfast();
            friendPage.BindingContext = selectedFriend;
            await Navigation.PushAsync(friendPage);
        }
        // обработка нажатия кнопки добавления
        private async void CreateFriend(object sender, EventArgs e)
        {
            Recipe friend = new Recipe();
            Breakfast friendPage = new Breakfast();
            friendPage.BindingContext = friend;
            await Navigation.PushAsync(friendPage);
        }

    }
}