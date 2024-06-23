using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstAppWithMenu.Data;
using FirstAppWithMenu.Models;
using System.Xml;


namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageOfRecipe : ContentPage
    {
        static FavoriteDatabase favoriteRecipesDb;

        public PageOfRecipe()
        {
            InitializeComponent();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoriteDB.db");
            favoriteRecipesDb = new FavoriteDatabase(dbPath);

            BindingContext = this; // Привязка контекста к текущей странице
        }

        private async void ButtonFav_Clicked(object sender, EventArgs e)
        {
            await AddToFavorite();
            await DisplayAlert("", "Рецепт добавлен в избранное", "Ок");

        }

        private async Task<bool> AddToFavorite()
        {
            addToFavoriteButton.IsEnabled = false;

            // Получение данных текущего рецепта из базы данных через BindingContext
            Recipe selectedRecipe = (Recipe)BindingContext;

            // Проверка, есть ли рецепт уже в избранном
            List<Recipe> favoriteRecipes = await favoriteRecipesDb.GetItemsAsync();
            bool isExist = favoriteRecipes.Any(r => r.Name == selectedRecipe.Name);

            //если рецепт не содержится в избранном, осуществляется добавление рецепта в избранное
            if (!isExist)
            {
                await favoriteRecipesDb.AddItemAsync(selectedRecipe);
                return true;
            }
            
            return false;
        }
    }
}
