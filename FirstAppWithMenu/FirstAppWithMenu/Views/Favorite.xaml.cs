using FirstAppWithMenu.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstAppWithMenu.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]// Атрибут компиляции XAML файлов
    public partial class Favorite : ContentPage // Определение класса Favorite, унаследованного от ContentPage
    {
        Label emptyLabel; // Объявление переменной для метки

        public Favorite() // Конструктор класса Favorite
        {
            InitializeComponent(); // Инициализация компонентов страницы
            emptyLabel = new Label // Инициализация метки
            {
                Text = "Вы ничего не добавили в избранное",
                FontSize = 20, // Установка размера шрифта
                TextColor = Color.Black, // Установка цвета текста
                HorizontalOptions = LayoutOptions.CenterAndExpand, // Установка горизонтального выравнивания
                VerticalOptions = LayoutOptions.CenterAndExpand, // Установка вертикального выравнивания
                IsVisible = false // Установка видимости метки
            };
            Content = new StackLayout // Установка содержимого страницы
            {
                Children = { emptyLabel, recipesList } // Добавление дочерних элементов в макет
            };
        }

        protected override async void OnAppearing()
        {
            // Сортировка списка рецептов по алфавиту
            var sortedRecipes = (await App.FavDatabase.GetItemsAsync()).OrderBy(recipe => recipe.Name).ToList();
            recipesList.ItemsSource = sortedRecipes;
            // Привязка данных
            /*recipesList.ItemsSource = await App.FavDatabase.GetItemsAsync().Sort();*/ // Установка источника данных для списка рецептов

            // Обновление состояния страницы
            UpdatePageState(); // Вызов метода обновления состояния страницы

            base.OnAppearing();
        }

        // Обновление состояния страницы (показать или скрыть сообщение о пустоте)
        private void UpdatePageState() // Метод обновления состояния страницы
        {
            if (recipesList.ItemsSource == null || !((IEnumerable<Recipe>)recipesList.ItemsSource).Any()) // Проверка на наличие элементов в списке
            {
                emptyLabel.IsVisible = true; // Показать метку с информацией о пустоте
                recipesList.IsVisible = false; // Скрыть список рецептов
            }
            else
            {
                emptyLabel.IsVisible = false; // Скрыть метку
                recipesList.IsVisible = true; // Показать список рецептов
            }
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
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

        }
    }
}