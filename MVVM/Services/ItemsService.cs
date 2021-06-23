using MVVM.Helpers;
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
        #region Properties

        static SQLiteAsyncConnection db;

        #endregion

        #region Methods

        static async Task Init()
        {
            try
            {
                if (db != null)
                    return;
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ItemsDB.db");
                db = new SQLiteAsyncConnection(dbPath);

                await db.CreateTableAsync<Item>();
                Utils.LogFirebaseEvent("CreateDB", "Successfull");
            }
            catch(Exception ex)
            {
                Utils.LogFirebaseEvent("CreateDB", "Failed_" + ex.Message.Replace(' ', '_'));
            }
        }

        public static async Task<IEnumerable<Item>> GetItems()
        {
            try
            {
                await Init();

                var items = await db.Table<Item>().ToListAsync();
                Utils.LogFirebaseEvent("AddItem", "Successfull");
                return items;
            }
            catch(Exception ex)
            {
                Utils.LogFirebaseEvent("GetItems", "Failed_" + ex.Message.Replace(' ', '_'));
                return null;
            }
        }
        
        public static async Task AddItem(Item item)
        {
            try
            {
                await Init();

                var id = await db.InsertAsync(item);
                Utils.LogFirebaseEvent($"AddItem_{item.Id}", "Successfull");
            }
            catch(Exception ex)
            {
                Utils.LogFirebaseEvent($"AddItem_{item.Id}", "Failed_" + ex.Message.Replace(' ', '_'));
            }
        }

        public static async Task<Task<Item>> GetItemById(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                await Init();
                var item = db.Table<Item>().Where(x => x.Id == id);
                Utils.LogFirebaseEvent($"GetItem_{itemId}", "Successfull");
                return item.FirstAsync();
            }
            catch(Exception ex)
            {
                Utils.LogFirebaseEvent($"GetItem_{itemId}", "Failed_" + ex.Message.Replace(' ', '_'));
                return null;
            }
        }

        public static async Task RemoveItem(int id)
        {
            try
            {
                await Init();

                await db.DeleteAsync<Item>(id);
                Utils.LogFirebaseEvent($"RemoveItem_{id}", "Successfull");
            }
            catch(Exception ex)
            {
                Utils.LogFirebaseEvent($"RemoveItem{id}", "Failed_" + ex.Message.Replace(' ', '_'));
            }
        }

        #endregion
    }
}
