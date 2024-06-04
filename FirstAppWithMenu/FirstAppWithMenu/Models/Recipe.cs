using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace FirstAppWithMenu.Models
{
    public class Recipe //класс для работы с таблицей рецептов
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //id рецепта
        public string Name { get; set; } //название рецепта
        public string Image { get; set; } //изображение
        public string ListOfIngredients { get; set; } //список ингредиентов для поиска
        public string Ingredients { get; set; } //список ингредиентов для печати на странице рецепта
        public string Text_Of_Recipe { get; set; } //текст рецепта
        public string TypeOfMeal { get; set; } //тип приема пищи
        public string TypeOfDish { get; set; } //тип блюда
    }
}
