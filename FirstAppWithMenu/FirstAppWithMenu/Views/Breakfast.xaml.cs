using FirstAppWithMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Breakfast : ContentPage
    {
        public Breakfast()
        {
            InitializeComponent();
        }

        private async void SaveFriend(object sender, EventArgs e)
        {
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical, // Устанавливаем ориентацию на вертикальную
                VerticalOptions = LayoutOptions.FillAndExpand // Размеры будут занимать все доступное место по вертикали
            };
            var rr = new Label
            {
                Text = "FFFF"
            };
            layout.Children.Add(rr); // Добавление Label с именем рецепта на layou
            List<Recipe> breakfastRecipes = await App.Database.GetItemsByTypeOfMealAsync("Завтрак");
            foreach (var recipe in breakfastRecipes)
            {
                var recipeLabel = new Label
                {
                    Text = recipe.Name, // Установка имени рецепта как текста для Label
                    FontSize = 20 // Установка размера шрифта
                };
                layout.Children.Add(recipeLabel); // Добавление Label с именем рецепта на layout
            }
        }
        private async void DeleteFriend(object sender, EventArgs e)
        {
            var friend = (Recipe)BindingContext;
            await App.Database.DeleteItemAsync(friend);
            await this.Navigation.PopAsync();
        }
        private async void Cancel(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }

        
    }
}