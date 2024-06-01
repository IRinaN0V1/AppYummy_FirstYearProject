using FirstAppWithMenu.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace FirstAppWithMenu.Views
{
    // Класс SelectedCategoryPage отображает страницу с рецептами по выбранной категории
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedCategoryPage : ContentPage
    {
        // Свойство ButtonTitle для хранения названия кнопки, с помощью которой осуществлен переход на эту страницу
        public string ButtonTitle { get; set; }

        // Конструктор класса SelectedCategoryPage, принимающий название кнопки
        public SelectedCategoryPage(string buttonTitle)
        {
            InitializeComponent();

            ButtonTitle = buttonTitle; // Присваиваем переданное значение названия кнопки свойству ButtonTitle
            Title = ButtonTitle; // Устанавливаем название страницы равным названию кнопки
        }

        // Метод OnAppearing вызывается, когда страница появляется на экране
        protected override async void OnAppearing()
        {
            // Привязка данных списка рецептов к элементу управления recipesList
            recipesList.ItemsSource = await App.Database.GetItemsByTypeOfMealAsync(ButtonTitle);

            base.OnAppearing(); // Вызываем базовый метод OnAppearing
        }

        // Обработчик события нажатия на элемент в списке
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