using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DayMirror.Models;
using SQLite;
using System.Linq;


namespace DayMirror.Database
{
    public class DayMirrorSqlDb
    {
        readonly SQLiteAsyncConnection _database;

        public DayMirrorSqlDb(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserAction>().Wait();
            _database.CreateTableAsync<UserActionContext>().Wait();
        }


        // ToDo : find out why Where condition throws exception and how to make it work properly
        public Task<List<UserAction>> GetDayActionsAsync(DateTime dateTime)
        {
            var result = _database.Table<UserAction>()
                //.Where(d => ((DateTime)d.Date).Date == dateTime.Date)
                .ToListAsync();

            return result;
        }

        public Task<int> SaveRecord(UserAction action)
        {
            if (action.ID == 0)
            {
                return _database.InsertAsync(action);
            }
            else
            {
                return _database.UpdateAsync(action);
            }
        }

        public Task<int> CreateActionContext(UserActionContext context)
        {
            if (context.ID == 0)
            {
                return _database.InsertAsync(context);
            }
            else
            {
                return _database.UpdateAsync(context);
            }
        }

        public Task<List<UserActionContext>> GetActionContextsAsync()
        {
            return _database.Table<UserActionContext>().ToListAsync();
        }
    }
}
