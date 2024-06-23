using System;
using System.Collections.Generic;
using FirstAppWithMenu.Models;
using System.Threading.Tasks;
using SQLite;

namespace FirstAppWithMenu.Data
{
    public class FavoriteDatabase //класс для работы с базой данных избранных элементов
    {

        SQLiteAsyncConnection database; //поле для асинхронного соединения с базой данных SQLite 
        public FavoriteDatabase(string databasePath) //конструктор класса FavoriteDataBase, иницициализирует соединение с БД и создает таблицу
        {
            database = new SQLiteAsyncConnection(databasePath);
            CreateTable();
        }

        //Метод для создания таблицы в базе данных, если она не существует
        public async Task CreateTable()
        {
            await database.CreateTableAsync<Recipe>();
        }

        // Метод для получения всех элементов из базы данных
        public Task<List<Recipe>> GetItemsAsync() //получение всех элементов базы данных
        {
            return database.Table<Recipe>().ToListAsync();
        }

        // Метод для получения элемента из базы данных по имени
        public Task<Recipe> GetItemByNameAsync(string name)
        {
            return database.Table<Recipe>().Where(recipe => recipe.Name == name).FirstOrDefaultAsync();
        }

        // Метод для получения элемента из базы данных по его уникальному id
        public async Task<Recipe> GetItemAsync(int id) //получение конкретного элемента по его уникальному id
        {
            return await database.GetAsync<Recipe>(id);
        }

        // Метод для добавления элемента в базу данных
        public Task<int> AddItemAsync(Recipe item)
        {
            return database.InsertAsync(item);
        }


        // Метод для удаления элемента из базы данных
        public Task<int> DeleteItemAsync(Recipe recipe)
        {

            return database.DeleteAsync(recipe);
        }
    }
}
