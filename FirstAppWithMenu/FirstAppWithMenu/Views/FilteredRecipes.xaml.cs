using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace FirstAppWithMenu.Views
{
    // Класс, представляющий страницу с отфильтрованными рецептами
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilteredRecipes : ContentPage
    {
        // Конструктор класса, принимающий выбранные ингредиенты, типы блюд и флаг переключателя
        public FilteredRecipes(List<string> selectedIngredients, List<string> selectedTypes, bool isSwitch)
        {
            InitializeComponent();
            FilterRecipesByIngredients(selectedIngredients, selectedTypes, isSwitch);
        }

        // Метод, вызываемый при появлении страницы
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        // Метод для фильтрации рецептов по выбранным ингредиентам и типам блюд
        private async void FilterRecipesByIngredients(List<string> selectedIngredients, List<string> selectedTypes, bool isSwitch)
        {
            // Получаем все рецепты из базы данных
            List<Recipe> allRecipes = await App.Database.GetItemsAsync();

            // Фильтруем рецепты, чтобы оставить только те, которые содержат выбранные ингредиенты и удовлетворяют выбранным типам
            List<Recipe> filteredRecipes = new List<Recipe>();
            //перебираем рецепты из базы данных
            foreach (Recipe recipe in allRecipes)
            {
                //если включена функция "рецепты с дополнительными ингредиентами"
                if (isSwitch)
                {
                    string[] arr = recipe.ListOfIngredients.Split(','); //Все ингредиенты рецепта
                    bool containsAllIngredients = false;//Для приготовление не хватает больше 2 ингредиентов -> false
                    //Перебираем ингредиенты 1 рецепта
                    foreach (string ingredient in arr)
                    {
                        //если ингредиент содержатся в выбранных пользователем элементах
                        if (selectedIngredients.Contains(ingredient.Trim()))
                        {
                            //Если ингредиент содержится в галках, то нам подходит этот рецепт
                            containsAllIngredients = true;
                        }
                    }

                    if (selectedIngredients.Count == 0) //если пользователь не выбрал ингредиенты в фильтрах
                    {
                        if (selectedTypes.Contains(recipe.TypeOfDish) || selectedTypes.Contains(recipe.TypeOfMeal)) //выводим те рецепты, у которых тип блюда или тип приема пищи совпадаент с теми, которые выбрал пользователь
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }

                    if (selectedTypes.Count == 0) //если пользователь не выбрал ни одного типа
                    {
                        if (containsAllIngredients) //добавляем только те рецепты, которые содержать какой-нибудь из выбранных пользователем ингредиентов
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }

                    else //если пользователь выбрал какой-нибудь тип блюда
                    {
                        //добавляем рецепт в том случае, если он содержит ингредиент, выбранный пользователем и соответсвует выбранному типу блюда
                        if (containsAllIngredients && (selectedTypes.Contains(recipe.TypeOfDish) || selectedTypes.Contains(recipe.TypeOfMeal)))
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }
                }
                else //если выключена функция "рецепты с дополнительными ингредиентами"
                {
                    string[] arr = recipe.ListOfIngredients.Split(','); //Все ингредиенты рецепта
                    bool containsAllIngredients = true; //Для приготовление не хватает больше 2 ингредиентов -> false
                    //Перебираем ингредиенты 1 рецепта
                    foreach (string ingredient in arr)
                    {
                        //если ингредиент рецепта не содержится в выбранных пользователем и ингредиент не является водой, сахаром или солью
                        if (!selectedIngredients.Contains(ingredient.Trim()) && (ingredient.Trim() != "Вода" && ingredient.Trim() != "Соль" && ingredient.Trim() != "Сахар"))
                        {
                            containsAllIngredients = false; //устанавливаем флаг false(посторонний ингредиент)
                            break;
                        }

                    }

                    if (selectedIngredients.Count == 0) //если пользователь не выбрал ингредиенты в фильтрах
                    {
                        if (selectedTypes.Contains(recipe.TypeOfDish) || selectedTypes.Contains(recipe.TypeOfMeal))  //выводим те рецепты, у которых тип блюда или тип приема пищи совпадаент с теми, которые выбрал пользователь
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }

                    else if (selectedTypes.Count == 0) //иначе если пользователь не выбрал категорию блюда(тип приема пищи или тип блюда) в фильтрах
                    {
                        if (containsAllIngredients) //добавляем тот рецепт, который содержит ТОЛЬКО ингредиенты, выбранные пользователем
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }

                    else
                    {
                        //добавляем рецепт в том случае, если он содержит ингредиент, выбранный пользователем и соответсвует выбранному типу блюда
                        if (containsAllIngredients && (selectedTypes.Contains(recipe.TypeOfDish) || selectedTypes.Contains(recipe.TypeOfMeal)))
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }
                }


            }

            // Проверяем, есть ли отфильтрованные рецепты
            if (!filteredRecipes.Any())
            {
                // Вывод сообщения об отсутствии найденных элементов
                Label emptyLabel = new Label
                {
                    Text = "По вашему запросу ничего не найдено",
                    FontSize = 20,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                Content = emptyLabel;
            }
            else
            {
                recipesList.ItemsSource = filteredRecipes;
            }
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
    }
}