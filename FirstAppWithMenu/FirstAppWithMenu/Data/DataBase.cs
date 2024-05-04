using System;
using System.Collections.Generic;
using System.Text;
using FirstAppWithMenu.Models;
using SQLite;
using System.Threading.Tasks;

namespace FirstAppWithMenu.Data
{
    public class DataBase
    {
        SQLiteAsyncConnection database;

        public DataBase(string databasePath, bool isNewDatabase)
        {
            database = new SQLiteAsyncConnection(databasePath);

            if (isNewDatabase)
               FillDataBase();

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

        private void FillDataBase()
        {
            CreateTable();
            SaveItemAsync(new Recipe { Name = "Окрошка", Recip = "123" });
            SaveItemAsync(new Recipe { Name = "Гуляш", Recip = "123" });
            SaveItemAsync(new Recipe { Name = "Салат1", Recip = "123" });
            SaveItemAsync(new Recipe { Name = "Салат2", Recip = "123" });
        }
    }
}