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
            isNewDatabase = true;
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
            SaveItemAsync(new Recipe { Name = "Азу по-татарски с говядиной", Recip = "Помойте и почистите картофель, лук и чеснок. Вымойте и обсушите мякоть говядины. Профильтруйте воду. Приготовьте сотейник и глубокую форму, подходящую для микроволновой печи.Для микроволновки лучше всего подойдет силиконовая или стеклянная форма.ШАГ 1 Нарежьте лук крупными полукольцами, картофель — полукруглыми ломтиками толщиной около 4-5 мм. Пропустите чеснок через пресс или мелко натрите.\r\nШАГ 2\r\n\r\n\r\nНарежьте говядину кусочками размером около 2-3 см, соленый огурец порубите маленькими кубиками.\r\nШАГ 3\r\n\r\n\r\nПоставьте кастрюлю на огонь выше среднего, налейте в нее 0,5 ст.л. подсолнечного масла, хорошо разогрейте. Опустите в масло нарезанный лук, обжарьте его, помешивая, 2-3 минуты до легкого золотистого оттенка.\r\nШАГ 4\r\n\r\n\r\nДобавьте к луку кусочки говядины, поджарьте их вместе с луком со всех сторон в течение 5-6 минут до плотной корочки.\r\nШАГ 5\r\n\r\n\r\nВыложите к мясу с луком ломтики картофеля, прогрейте все вместе, помешивая, 2-3 минуты. Налейте к мясу с овощами воду, добавьте соленый огурец.\r\nШАГ 6\r\n\r\n\r\nПосолите и поперчите мясо с овощами, хорошо перемешайте. Уменьшите огонь и потушите все вместе в течение 5-6 минут.\r\nШАГ 7\r\n\r\n\r\nОтлейте 1-2 ст.л. бульона из сотейника в форму для микроволновой печи. Положите туда томатную пасту, муку, измельченный чеснок и 0,5 ст.л. масла. Все размешайте.\r\nШАГ 8\r\n\r\n\r\nВыложите в получившийся соус говядину с овощами и оставшимся бульоном, перемешайте. Поставьте в микроволновую печь и готовьте на самой большой мощности в течение 4-5 минут." });
            SaveItemAsync(new Recipe { Name = "Гуляш", Recip = "123" });
            SaveItemAsync(new Recipe { Name = "Салат1", Recip = "123" });
            SaveItemAsync(new Recipe { Name = "Салат2", Recip = "123" });
        }
    }
}