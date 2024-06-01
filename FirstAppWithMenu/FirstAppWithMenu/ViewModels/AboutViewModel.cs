using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstAppWithMenu.ViewModels
{
    public class AboutViewModel 
    {
        public AboutViewModel()
        {
            // Инициализация команд для открытия соответствующих страниц
            OpenSearchCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.SearchPage()); });
            OpenSettingsCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.SettingsPage()); });
            OpenTypeOfMealCategoryCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.TypeOfMealPage()); });
            OpenTypeOfDishCategoryCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.TypeOfDishPage()); });
        }

        //Свойства команд для открытия соответствующих страниц
        public ICommand OpenSearchCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenTypeOfMealCategoryCommand { get; }
        public ICommand OpenTypeOfDishCategoryCommand { get; }
    }
}