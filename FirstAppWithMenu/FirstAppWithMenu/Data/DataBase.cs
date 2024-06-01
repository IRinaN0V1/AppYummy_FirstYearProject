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
    public class DataBase //класс для работы с базой данных рецептов
    {
        SQLiteAsyncConnection DatabaseWithRecipes; //поле для асинхронного соединения с базой данных SQLite 

        public DataBase(string databasePath) //конструктор класса DataBase, иницициализирует соединение с БД и создает таблицу
        {
            DatabaseWithRecipes = new SQLiteAsyncConnection(databasePath);
            FillDataBase(); //вызов методя для заполнения БД из текстового файла
        }

        public string[] OpenTxtFile() //метод для подключение к текстовому файлу с рецептами и его чтение
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DataBase)).Assembly; //устанавливаем связь с файлом
            Stream stream = assembly.GetManifestResourceStream("FirstAppWithMenu.Данные.txt");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd(); //считываем данные из файла 
            }
            //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Данные.txt");
            string[] arr = text.Split('*'); //разделяем строку по рецептам (между рецептами стоит звездочка)
            return arr; //возвращаем массив рецептов
        }

        public async Task CreateTable() // Метод для создания таблицы в базе данных
        {
            await DatabaseWithRecipes.CreateTableAsync<Recipe>();
        }

        public Task<List<Recipe>> GetItemsAsync() //получение всех элементов базы данных
        {
            return DatabaseWithRecipes.Table<Recipe>().ToListAsync();
        }

        public async Task<Recipe> GetItemAsync(int id) //получение конкретного элемента по его уникальному id
        {
            return await DatabaseWithRecipes.GetAsync<Recipe>(id);
        }

        public Task<int> DeleteItemAsync(Recipe item) //удаление конкретного элемента из БД
        {
            return DatabaseWithRecipes.DeleteAsync(item);
        }

        public Task<int> SaveItemAsync(Recipe item) //добавление нового элемента БД
        {
            return DatabaseWithRecipes.InsertAsync(item);
        }

        public async Task<List<Recipe>> GetItemsByTypeOfMealAsync(string typeOfMeal) //создание новой таблицы по типу прима пищи
        {
            return await DatabaseWithRecipes.Table<Recipe>().Where(x => x.TypeOfMeal == typeOfMeal).ToListAsync();
        }

        public async Task<List<Recipe>> GetItemsByTypeOfDishAsync(string typeOfDish) //создание новой таблицы по типу блюда
        {
            return await DatabaseWithRecipes.Table<Recipe>().Where(x => x.TypeOfDish == typeOfDish).ToListAsync();
        }

        // Метод для получения рецептов по типу приема пищи
        public async Task<List<string>> GetTypesOfMealAsync()
        {
            var typesOfMeal = (await DatabaseWithRecipes.Table<Recipe>().ToListAsync())
                                .Select(x => x.TypeOfMeal)
                                .Distinct()
                                .ToList();
            return typesOfMeal;
        }

        // Метод для получения рецептов по типу блюда
        public async Task<List<string>> GetTypesOfDishAsync()
        {
            var typesOfDish = (await DatabaseWithRecipes.Table<Recipe>().ToListAsync())
                                .Select(x => x.TypeOfDish)
                                .Distinct()
                                .ToList();
            return typesOfDish;
        }

        // Метод для получения всех ингредиентов из рецептов
        public async Task<List<string>> GetAllIngredientsAsync()
        {
            List<Recipe> allRecipes = await DatabaseWithRecipes.Table<Recipe>().ToListAsync();
            HashSet<string> uniqueIngredients = new HashSet<string>();

            foreach (Recipe recipe in allRecipes)
            {
                //разделяем список ингредиентов по запятым
                string[] ingredientsArray = recipe.ListOfIngredients.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string ingredient in ingredientsArray)
                {
                    uniqueIngredients.Add(ingredient.Trim());
                }
            }

            // Преобразование HashSet в List
            List<string> allIngredients = uniqueIngredients.ToList();

            return allIngredients;
        }

        private async Task FillDataBase() //метод заполнения базы данных элементами
        {
            CreateTable();
            // Проверяем, есть ли уже элементы в базе данных
            var existingItems = await GetItemsAsync();
            if (existingItems != null && existingItems.Any())
            {
                // Если элементы уже присутствуют, просто выходим из метода
                return;
            }

            string Name = "", Image = "", ListOfIngredients = "", Ingredients = "", Recip = "", TypeOfMeal = "", TypeOfDish = "";
            string[] arr = OpenTxtFile(); //открывает текстовый файл, записываем массив рецептов в arr
            foreach (string s in arr) //перебираем рецепты
            {
                string[] recip = s.Split('_'); //разделяем поля каждого рецепта по нижнему подчеркиванию
                for (int i = 0; i < recip.Length; i++) //проходим по полям рецепта
                {
                    if (i == 0) Name = recip[i].Trim(); //получаем имя блюда
                    if (i == 1) Image = recip[i].Trim(); //получаем изображение рецепта
                    if (i == 2) ListOfIngredients = recip[i].Trim(); //получаем список ингредиентов
                    if (i == 3) Ingredients = recip[i].Trim(); //список ингредиентов для отображения в рецепте
                    if (i == 4) Recip = recip[i].Trim(); //рецепт
                    if (i == 5) TypeOfMeal = recip[i].Trim(); //тип приема пищи
                    if (i == 6) TypeOfDish = recip[i].Trim(); //тип блюда
                }
                //сохраняем новое значение в БД
                await SaveItemAsync(new Recipe { Name = Name, Image = Image, ListOfIngredients = ListOfIngredients, Ingredients = Ingredients, Text_Of_Recipe = Recip, TypeOfMeal = TypeOfMeal, TypeOfDish = TypeOfDish});
            }
        }
    }
}