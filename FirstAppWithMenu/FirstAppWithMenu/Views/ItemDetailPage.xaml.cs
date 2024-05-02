using FirstAppWithMenu.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FirstAppWithMenu.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}