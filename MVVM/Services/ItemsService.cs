using MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MVVM.Services
{
    public static class ItemsService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ItemsDB.db");
            db = new SQLiteAsyncConnection(dbPath);

            await db.CreateTableAsync<Item>();
        }

        public static async Task<IEnumerable<Item>> GetItems()
        {
            await Init();

            var items = await db.Table<Item>().ToListAsync();
            return items;
        }
        
        public static async Task AddItem(string name, string image)
        {
            await Init();
            var item = new Item
            {
                Name = name,
                Image = image
            };

            var id = await db.InsertAsync(item);
        }
        
        public static async Task RemoveItem(int id)
        {
            await Init();

            await db.DeleteAsync<Item>(id);
        }
    }
}
