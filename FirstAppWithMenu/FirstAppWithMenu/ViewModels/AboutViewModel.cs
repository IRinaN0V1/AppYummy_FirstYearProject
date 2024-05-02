using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstAppWithMenu.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            OpenSearchCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.SearchPage());});
            OpenSettingsCommand = new Command( async () =>{ await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.SettingsPage());});
            OpenTypeOfMealCategoryCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.TypeOfMealPage()); });
            OpenTypeOfDishCategoryCommand = new Command(async () => { await Application.Current.MainPage.Navigation.PushAsync(new FirstAppWithMenu.Views.TypeOfDishPage()); });

        }
        public ICommand OpenWebCommand { get; }
        public ICommand OpenSearchCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand OpenTypeOfMealCategoryCommand { get; }
        public ICommand OpenTypeOfDishCategoryCommand { get; }


    }
}