using FirstAppWithMenu.Data;
using FirstAppWithMenu.Services;
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
        public const string DATABASE_NAME = "database.db";
        public static DataBase database;
        public static DataBase Database
        {
            get
            {
                var isNew = false;

                if (database == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);

                    // если база данных не существует (еще не скопирована)
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

                        isNew = true;
                    }
                    database = new DataBase(dbPath, isNew);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

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
