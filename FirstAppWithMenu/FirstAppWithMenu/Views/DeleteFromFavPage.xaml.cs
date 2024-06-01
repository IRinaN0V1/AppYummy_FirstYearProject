using FirstAppWithMenu.Data;
using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DeleteFromFavPage : ContentPage
    {
        static FavoriteDatabase favoriteRecipesDb;

        public DeleteFromFavPage()
        {
            InitializeComponent();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoriteDB.db");
            favoriteRecipesDb = new FavoriteDatabase(dbPath);

            BindingContext = this; // Привязка контекста к текущей странице
        }

        private async void ButtonFav_Clicked(object sender, EventArgs e)
        {
            await RemoveFromFavorite();
            await DisplayAlert("", "Рецепт удален из избранного", "Ок");
        }

        private async Task RemoveFromFavorite()
        {
            removeFromFavoriteButton.IsEnabled = false;

            // Получение данных текущего рецепта из базы данных через BindingContext
            Recipe selectedRecipe = (Recipe)BindingContext;

            // Поиска и удаление рецепта из базы данных избранных рецептов
            await favoriteRecipesDb.DeleteItemAsync(selectedRecipe);

            // Показать сообщение об успешном удалении
        }
    }

    //public partial class DeleteFromFavPage : ContentPage
    //{
    //    Label nameLabel;
    //    Image imageURL;
    //    Label ingredientsContentLabel;
    //    Label recipeContentLabel;
    //    Button buttonFav;

    //    static FavoriteDatabase favoriteRecipesDb;

    //    public DeleteFromFavPage()
    //    {
    //        InitializeComponent();

    //        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoriteDB.db");

    //        favoriteRecipesDb = new FavoriteDatabase(dbPath);
    //        BindingContext = this;

    //        nameLabel = new Label
    //        {
    //            TextColor = Color.Black,
    //            FontSize = 25,
    //            HorizontalOptions = LayoutOptions.Center,
    //            FontAttributes = FontAttributes.Bold
    //        };
    //        nameLabel.SetBinding(Label.TextProperty, "Name");

    //        imageURL = new Image
    //        {
    //            HorizontalOptions = LayoutOptions.Center,
    //        };
    //        imageURL.SetBinding(Image.SourceProperty, "Image");

    //        buttonFav = new Xamarin.Forms.Button
    //        {
    //            Text = "Удалить из избранного",
    //            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Xamarin.Forms.Button)),
    //            Margin = new Thickness(5),
    //            HeightRequest = 45,
    //            BackgroundColor = Color.Pink,
    //            CornerRadius = 10,
    //            TextColor = Color.Black
    //        };

    //        buttonFav.Clicked += async (sender, e) =>
    //        {
    //            RemoveFromFavorite();
    //            await DisplayAlert("", "Рецепт удален из избранного", "Ок");
    //        };

    //        var ingredientsLabel = new Label
    //        {
    //            Text = "  Вам понадобится:",
    //            TextColor = Color.Black,
    //            FontSize = 18,
    //            FontAttributes = FontAttributes.Bold,
    //            Margin = new Thickness(0, 10, 0, 0)
    //        };

    //        ingredientsContentLabel = new Label
    //        {
    //            VerticalOptions = LayoutOptions.Center,
    //            TextColor = Color.Black,
    //            Margin = new Thickness(5, 0, 0, 0)
    //        };
    //        ingredientsContentLabel.SetBinding(Label.TextProperty, "Ingredients");


    //        var recipeLabel = new Label
    //        {
    //            Text = "Рецепт",
    //            TextColor = Color.Black,
    //            FontSize = 20,
    //            HorizontalOptions = LayoutOptions.Center,
    //            FontAttributes = FontAttributes.Bold
    //        };

    //        recipeContentLabel = new Label
    //        {
    //            TextColor = Color.Black,
    //            Margin = new Thickness(5, 0, 0, 0)
    //        };
    //        recipeContentLabel.SetBinding(Label.TextProperty, "Text_Of_Recipe");

    //        Content = new ScrollView
    //        {
    //            Content = new StackLayout
    //            {
    //                Children =
    //                {
    //                    nameLabel,
    //                    imageURL,
    //                    buttonFav,
    //                    ingredientsLabel,
    //                    ingredientsContentLabel,
    //                    recipeLabel,
    //                    recipeContentLabel,
    //                }
    //            }
    //        };
    //    }

    //    protected override async void OnAppearing()
    //    {
    //        base.OnAppearing();

    //    }

    //    protected override void OnDisappearing()
    //    {
    //        base.OnDisappearing();
    //    }

    //    async void RemoveFromFavorite()
    //    {
    //        // Делаем кнопку недоступной для нажатия
    //        buttonFav.IsEnabled = false;

    //        string name = BindingContext?.GetType().GetProperty("Name")?.GetValue(BindingContext, null)?.ToString();

    //        // Используйте имя для поиска и удаления рецепта из базы данных
    //        Recipe recipeToRemove = await favoriteRecipesDb.GetItemByNameAsync(name);

    //        if (recipeToRemove != null)
    //        {
    //            // Удалите рецепт из избранного
    //            await favoriteRecipesDb.DeleteItemAsync(recipeToRemove);

    //            // Показать сообщение об успешном удалении
    //        }
    //    }
    //}
}