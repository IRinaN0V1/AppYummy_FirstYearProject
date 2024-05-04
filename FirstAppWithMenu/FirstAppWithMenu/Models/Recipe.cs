using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace FirstAppWithMenu.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Ingredients { get; set; }
        public string Recip { get; set; }
        public string TypeOfMeal { get; set; }
        public string TypeOfDish { get; set; }
    }
}
