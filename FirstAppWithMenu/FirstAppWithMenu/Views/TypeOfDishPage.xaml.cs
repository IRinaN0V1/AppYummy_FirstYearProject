using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TypeOfDishPage : ContentPage
    {
        public TypeOfDishPage()
        {
            InitializeComponent();

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical, // Устанавливаем ориентацию на вертикальную
                VerticalOptions = LayoutOptions.FillAndExpand // Размеры будут занимать все доступное место по вертикали
            };

            // Создаем список текста перед скроллом, цвета кнопок и список названий кнопок для каждого скролла
            var scrollItems = new List<(string labelText, Color buttonColor, List<string> buttonNames)>
            {
                ("Горячее", Color.Wheat, new List<string> {"мясо по французски", "спагетти", "плов", "котлеты", "каша"}),
                ("Десерты", Color.LightCyan, new List<string> {"Чизкейк", "панна котта", "Тирамису", "эклер", "Наполеон"}),
                ("Салаты", Color.PowderBlue, new List<string> {"цезарь", "оливье", "греческий", "Мимоза", "винегрет"}),
                ("Супы", Color.MistyRose, new List<string> {"щи", "солянка", "рассольник", "борщ", "сырный", "суп-пюре"}),
                ("Выпечка", Color.Lavender, new List<string> {"шарлотка", "рыбный пирог", "кулич", "сахарная плюшка", "манник"})
            };

            // Создаем ScrollView для каждого элемента в списке
            foreach (var item in scrollItems)
            {
                layout.Children.Add(CreateButtonRow(item.labelText, item.buttonColor, item.buttonNames));
            }

            Content = layout;
        }

        // Метод для создания горизонтального ScrollView с одной строкой кнопок и текстовой меткой
        private StackLayout CreateButtonRow(string labelText, Color buttonColor, List<string> buttonNames)
        {
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand // Размеры будут занимать все доступное место по горизонтали
            };

            var label = new Label
            {
                Text = labelText,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), // Увеличиваем размер текста
                HorizontalOptions = LayoutOptions.Start, // Размещаем метку слева
                Margin = new Thickness(5, 0, 0, 5), // Добавляем отступы слева и снизу
                TextColor = Color.Black
            };

            stackLayout.Children.Add(label);

            var scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            // Создаем кнопки на основе переданных названий
            foreach (var buttonName in buttonNames)
            {
                var button = new Button
                {
                    Text = buttonName.Trim(), // Устанавливаем текст кнопки
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)), // Увеличиваем размер кнопки
                    Margin = new Thickness(10), // Увеличиваем отступы вокруг кнопок
                    WidthRequest = 160, // Устанавливаем фиксированную ширину кнопок
                    HeightRequest = 100, // Устанавливаем фиксированную высоту кнопок
                    BackgroundColor = buttonColor, // Устанавливаем цвет кнопок
                    CornerRadius = 10, // Скругляем углы кнопок
                    TextColor = Color.Black
                };

                buttonsStackLayout.Children.Add(button);
            }

            scrollView.Content = buttonsStackLayout;

            stackLayout.Children.Add(scrollView);

            return stackLayout;
        }
    }
}