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
        }//Обработчик события: нажатия на кнопку "Удалить из Избранного"

        private async Task RemoveFromFavorite()
        {
            removeFromFavoriteButton.IsEnabled = false;

            // Получение данных текущего рецепта из базы данных через BindingContext
            Recipe selectedRecipe = (Recipe)BindingContext;

            List<Recipe> recipes = await App.FavDatabase.GetItemsAsync();

            bool isExist = recipes.Any(r => r.Name == selectedRecipe.Name);
            if (isExist)
            {
                Recipe recipe = await favoriteRecipesDb.GetItemByNameAsync(selectedRecipe.Name);
                // Поиск и удаление рецепта из базы данных избранных рецептов
                await favoriteRecipesDb.DeleteItemAsync(recipe);
            }
        }//Метод удаления рецепта из избранного
    }

}
