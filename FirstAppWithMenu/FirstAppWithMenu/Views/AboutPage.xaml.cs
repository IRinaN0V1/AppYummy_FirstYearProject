using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstAppWithMenu.Data;
using FirstAppWithMenu.Models;

namespace FirstAppWithMenu.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // привязка данных
            friendsList.ItemsSource = await App.Database.GetItemsAsync();

            base.OnAppearing();
        }
        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Recipe selectedFriend = (Recipe)e.SelectedItem;
            Breakfast friendPage = new Breakfast();
            friendPage.BindingContext = selectedFriend;
            await Navigation.PushAsync(friendPage);
        }
        // обработка нажатия кнопки добавления
        private async void CreateFriend(object sender, EventArgs e)
        {
            Recipe friend = new Recipe();
            Breakfast friendPage = new Breakfast();
            friendPage.BindingContext = friend;
            await Navigation.PushAsync(friendPage);
        }

    }
}