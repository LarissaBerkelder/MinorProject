using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCUBA_FINAL.Models;

namespace SCUBA_FINAL.Data
{
    public class MSG_Database
    {
        readonly SQLiteAsyncConnection database;

        public MSG_Database(string database_Path)
        {
            database = new SQLiteAsyncConnection(database_Path);
            database.CreateTableAsync<MSG_Model>().Wait();
        }

        public Task<List<MSG_Model>> GetDataAsync()
        {
            return database.Table<MSG_Model>().ToListAsync();
        }

        public Task<int> SaveDataAsync(MSG_Model data)
        {
            if (data.ID != 0)
            {
                // Update an existing message.
                return database.UpdateAsync(data);
            }
            else
            {
                // Save a new message.
                return database.InsertAsync(data);
            }
        }

        public async Task<MSG_Model> GetLastSavedDataAsync()
        {
            var data = await GetDataAsync();
            var lastSavedData = data.OrderByDescending(m => m.ID).FirstOrDefault();
            return lastSavedData;
        }

        public Task<int> ClearDatabaseAsync()
        {
            return database.DeleteAllAsync<MSG_Model>();
        }
    }
}
