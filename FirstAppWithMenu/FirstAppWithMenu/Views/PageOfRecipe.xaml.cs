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
//using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Класс PageOfRecipe представляет страницу отдельного рецепта


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

            if (!isExist)
            {
                await favoriteRecipesDb.AddItemAsync(selectedRecipe);
                return true;
            }

            return false;
        }
    }

    //public partial class PageOfRecipe : ContentPage
    //{
    //    Label nameLabel;
    //    Image imageURL;
    //    Label ingredientsContentLabel;
    //    Label recipeContentLabel;
    //    Button buttonFav;

    //    static FavoriteDatabase favoriteRecipesDb;      // Статическое поле для базы данных избранных рецептов

    //    public PageOfRecipe()
    //    {
    //        InitializeComponent();

    //        // Инициализация базы данных
    //        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoriteDB.db");
    //        favoriteRecipesDb = new FavoriteDatabase(dbPath); // Создание экземпляра базы данных избранных рецептов
    //        BindingContext = this;

    //        // Инициализация элементов интерфейса
    //        nameLabel = new Label
    //        {
    //            // Установка параметров метки
    //            TextColor = Color.Black,
    //            FontSize = 25,
    //            HorizontalOptions = LayoutOptions.Center,
    //            FontAttributes = FontAttributes.Bold
    //        };
    //        nameLabel.SetBinding(Label.TextProperty, "Name");  // Привязка свойства Text к свойству Name

    //        imageURL = new Image
    //        {
    //            // Установка параметров изображения
    //            HorizontalOptions = LayoutOptions.Center,
    //        };
    //        imageURL.SetBinding(Image.SourceProperty, "Image"); // Привязка свойства Source к свойству Image

    //        // Создание кнопки добавления в избранное
    //        buttonFav = new Xamarin.Forms.Button
    //        {
    //            // Установка параметров кнопки
    //            Text = "Добавить в избранное",
    //            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Xamarin.Forms.Button)),
    //            Margin = new Thickness(5),
    //            HeightRequest = 45,
    //            BackgroundColor = Color.PaleGreen,
    //            CornerRadius = 10,
    //            TextColor = Color.Black,

    //        };

    //        // Обработка нажатия на кнопку добавления в избранное
    //        buttonFav.Clicked += async (sender, e) =>
    //        {
    //            AddToFavorite();
    //            await DisplayAlert("","Рецепт добавлен в избранное", "Ок");
    //        };

    //        // Создание меток для ингредиентов и рецепта
    //        var ingredientsLabel = new Label
    //        {
    //            // Установка параметров метки для ингредиентов
    //            Text = "  Вам понадобится:",
    //            TextColor = Color.Black,
    //            FontSize = 18,
    //            FontAttributes = FontAttributes.Bold,
    //            Margin = new Thickness(0, 10, 0, 0)
    //        };

    //        ingredientsContentLabel = new Label
    //        {
    //            // Установка параметров метки для содержимого ингредиентов
    //            VerticalOptions = LayoutOptions.Center,
    //            TextColor = Color.Black,
    //            Margin = new Thickness(5, 0, 0, 0)
    //        };
    //        ingredientsContentLabel.SetBinding(Label.TextProperty, "Ingredients"); // Привязка свойства Text к свойству Ingredients

    //        var recipeLabel = new Label
    //        {
    //            // Установка параметров метки для названия рецепта
    //            Text = "Рецепт",
    //            TextColor = Color.Black,
    //            FontSize = 20,
    //            HorizontalOptions = LayoutOptions.Center,
    //            FontAttributes = FontAttributes.Bold
    //        };

    //        recipeContentLabel = new Label
    //        {
    //            // Установка параметров метки для содержимого рецепта
    //            TextColor = Color.Black,
    //            Margin = new Thickness(5, 0, 0, 0)
    //        };
    //        recipeContentLabel.SetBinding(Label.TextProperty, "Text_Of_Recipe");  // Привязка свойства Text к свойству Recip

    //        // Установка содержимого страницы в прокручиваемой области
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

    //    // Метод вызывается при появлении страницы на экране
    //    protected override async void OnAppearing()
    //    {
    //        base.OnAppearing();

    //    }

    //    // Метод вызывается при скрытии страницы
    //    protected override void OnDisappearing()
    //    {
    //        base.OnDisappearing();
    //    }

    //    // Метод для добавления рецепта в избранное
    //    async void AddToFavorite()
    //    {
    //        // Делаем кнопку недоступной для нажатия
    //        buttonFav.IsEnabled = false;

    //        List<Recipe> items = await App.FavDatabase.GetItemsAsync(); // Получение всех избранных рецептов
    //        Recipe selectedRecipe = (Recipe)BindingContext;

    //        bool isExist = items.Any(r => r.Name == selectedRecipe.Name); // Проверка наличия рецепта в избранном

    //        // Получение значений свойств рецепта
    //        string name = BindingContext?.GetType().GetProperty("Name")?.GetValue(BindingContext, null)?.ToString();
    //        // проверка наличия рецепта в списке избранных
    //        if (!isExist)
    //        {

    //            string image = BindingContext?.GetType().GetProperty("Image")?.GetValue(BindingContext, null)?.ToString();
    //            string listofingredients = BindingContext?.GetType().GetProperty("ListOfIngredients")?.GetValue(BindingContext, null)?.ToString();
    //            string ingredients = BindingContext?.GetType().GetProperty("Ingredients")?.GetValue(BindingContext, null)?.ToString();
    //            string recipe = BindingContext?.GetType().GetProperty("Text_Of_Recipe")?.GetValue(BindingContext, null)?.ToString();
    //            string typeOfMeal = BindingContext?.GetType().GetProperty("TypeOfMeal")?.GetValue(BindingContext, null)?.ToString();
    //            string typeOfDish = BindingContext?.GetType().GetProperty("TypeOfDish")?.GetValue(BindingContext, null)?.ToString();

    //            // Создание объекта избранного рецепта
    //            Recipe favoriteRecipe = new Recipe
    //            {
    //                Name = name,
    //                Image = image,
    //                ListOfIngredients = listofingredients,
    //                Ingredients = ingredients,
    //                Text_Of_Recipe = recipe,
    //                TypeOfMeal = typeOfMeal,
    //                TypeOfDish = typeOfDish
    //            };

    //            // Добавление избранного рецепта в базу данных и дожидание завершения операции
    //            await favoriteRecipesDb.AddItemAsync(favoriteRecipe);
    //        }  
    //    }
    //}
}
