using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCUBA_FINAL.Models;

namespace SCUBA_FINAL.Data
{
    public class GPB_Database
    {
        readonly SQLiteAsyncConnection database;

        public GPB_Database(string database_Path)
        {
            database = new SQLiteAsyncConnection(database_Path);
            database.CreateTableAsync<GPB_Model>().Wait();
        }

        public Task<List<GPB_Model>> GetDataAsync()
        {
            return database.Table<GPB_Model>().ToListAsync();
        }

        public Task<int> SaveDataAsync(GPB_Model data)
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

        public async Task<GPB_Model> GetLastSavedDataAsync()
        {
            var data = await GetDataAsync();
            var lastSavedData = data.OrderByDescending(m => m.ID).FirstOrDefault();
            return lastSavedData;
        }

        public Task<int> ClearDatabaseAsync()
        {
            return database.DeleteAllAsync<GPB_Model>();
        }
    }
}
