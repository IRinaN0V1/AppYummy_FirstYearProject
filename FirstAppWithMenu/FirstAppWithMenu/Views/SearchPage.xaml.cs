using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    // Класс SearchPage отображает страницу поиска рецептов
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        // Конструктор класса SearchPage
        public SearchPage()
        {
            InitializeComponent();
        }

        // Обработчик изменения текста в поисковой строке
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var items = await App.Database.GetItemsAsync(); // Получаем все элементы из базы данных

            // Фильтруем элементы по названию с учетом регистра
            var filteredItems = items.Where(item => item.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList();

            recipesList.ItemsSource = filteredItems; // Устанавливаем отфильтрованные элементы в список результатов поиска
        }


        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }


            List<Recipe> items = await App.FavDatabase.GetItemsAsync(); // Получение всех избранных рецептов

            Recipe selectedRecipe = (Recipe)e.SelectedItem;//Получение текущего рецепта

            bool isExist = items.Any(r => r.Name == selectedRecipe.Name); // Проверка наличия текущего рецепта в избранном
            //Переход на страницу рецепта в зависимости от значения isExist
            if (!isExist)
            {
                // Создаем новую страницу рецепта с кнопкой "Добавить в избранное"
                PageOfRecipe recipePage = new PageOfRecipe();
                recipePage.BindingContext = selectedRecipe; // Привязываем выбранный рецепт к контексту страницы

                await Navigation.PushAsync(recipePage); // Переходим на страницу с деталями рецепта

            }
            else
            {
                // Создаем новую страницу рецепта с кнопкой "Удалить из избранного"
                DeleteFromFavPage recipePage = new DeleteFromFavPage();
                recipePage.BindingContext = selectedRecipe; // Привязываем выбранный рецепт к контексту страницы

                await Navigation.PushAsync(recipePage); // Переходим на страницу с деталями рецепта
            }

            // Сброс выбранного элемента
            ((ListView)sender).SelectedItem = null;

        }

        // Обработчик нажатия на кнопку фильтров
        private async void FiltersButton_Clicked(object sender, EventArgs e)
        {
            // Открываем новую страницу для выбора типов приема пищи
            FiltersPage filtersPage = new FiltersPage();
            await Navigation.PushAsync(filtersPage);
        }
    }
}