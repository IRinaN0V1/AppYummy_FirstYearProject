using System;
using System.Collections.Generic;
using System.Text;
using FirstAppWithMenu.Models;
using SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;


namespace FirstAppWithMenu.Data
{
    public class DataBase
    {
        SQLiteAsyncConnection database;

        public DataBase(string databasePath, bool isNewDatabase)
        {
            isNewDatabase = false;
            database = new SQLiteAsyncConnection(databasePath);

            if (isNewDatabase)
               FillDataBase();

        }

        public string[] OpenTxtFile()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DataBase)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FirstAppWithMenu.Данные.txt");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Данные.txt");
            string[] arr = text.Split('*');
            return arr;
        }

        public async Task CreateTable()
        {
            await database.CreateTableAsync<Recipe>();
        }

        public Task<List<Recipe>> GetItemsAsync()
        {
            return database.Table<Recipe>().ToListAsync();
        }

        public async Task<Recipe> GetItemAsync(int id)
        {
            return await database.GetAsync<Recipe>(id);
        }

        public Task<int> DeleteItemAsync(Recipe item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> SaveItemAsync(Recipe item)
        {
            return database.InsertAsync(item);
        }

        public async Task<List<Recipe>> GetItemsByTypeOfMealAsync(string typeOfMeal)
        {
            return await database.Table<Recipe>().Where(x => x.TypeOfMeal == typeOfMeal).ToListAsync();
        }

        private async Task FillDataBase()
        {
            CreateTable();
            string Name = "", Image = "", Ingredients = "", Recip = "", TypeOfMeal = "", TypeOfDish = "";
            string[] arr = OpenTxtFile();
            foreach (string s in arr)
            {
                string[] recip = s.Split('_');
                for (int i = 0; i < recip.Length; i++)
                {
                    if (i == 0) Name = recip[i];
                    if (i == 1) Image = recip[i];
                    if (i == 2) Ingredients = recip[i];
                    if (i == 3) Recip = recip[i];
                    if (i == 4) TypeOfMeal = recip[i];
                    if (i == 5) TypeOfDish = recip[i];
                }
                await SaveItemAsync(new Recipe {Name = Name, Image = Image, Ingredients = Ingredients, Recip = Recip, TypeOfMeal = TypeOfMeal, TypeOfDish =TypeOfDish});
            }
        }
    }
}