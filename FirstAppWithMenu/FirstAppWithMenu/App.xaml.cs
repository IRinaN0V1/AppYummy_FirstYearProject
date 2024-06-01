using FirstAppWithMenu.Data;
using FirstAppWithMenu.Views;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstAppWithMenu
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "DatabaseWithRecipes.db"; //название базы данных с рецептами
        public static DataBase DatabaseWithRecipes; //объект БД с рецептами
        public static DataBase Database  //свойство для доступа к базе данных с рецептами
        {
            get
            {

                if (DatabaseWithRecipes == null)
                {
                    // путь, по которому будет находиться таблица
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);

                    // если таблица не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"FirstAppWithMenu.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs);  // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }


                    }
                    DatabaseWithRecipes = new DataBase(dbPath);
                }
                return DatabaseWithRecipes;
            }
        }
        //название БД элементов, добавленных в избранное
        public const string FavDATABASE_NAME = "favoriteDB.db";
        public static FavoriteDatabase favDatabase; //объект базы данных для избранных элементов

        public static FavoriteDatabase FavDatabase //свойство для доступа к базе данных для избранного
        {
            get
            {

                if (favDatabase == null)
                {                    // путь, по которому будет находиться база данных
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FavDATABASE_NAME);

                    // если база данных не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"FirstAppWithMenu.{FavDATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs);  // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }
                    }

                    favDatabase = new FavoriteDatabase(dbPath);
                }
                return favDatabase;
            }
        }

        public App() //конструктор класса App
        {
            InitializeComponent();

            MainPage = new AppShell(); //устанавливаем главную страницу приложения как AppShell
        }

        //методы, обрабатывающие события запуска, приостановки и возобновления работы приложения
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
