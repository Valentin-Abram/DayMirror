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
            _database.CreateTableAsync<UserActionContext>().Wait();
            _database.CreateTableAsync<UserAction>().Wait();
        }


        // ToDo : find out why Where condition throws exception and how to make it work properly
        public async Task<List<UserAction>> GetDayActionsAsync(DateTime dateTime)
        {
            var result = await _database.Table<UserAction>()
                //.Where(d => ((DateTime)d.Date).Date == dateTime.Date)
                .ToListAsync();

            return result
                .Where(a => a.Date.Date == dateTime.Date)
                .ToList();
        }

        public Task<int> CreateOrUpdateAction(UserAction action)
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

        public Task<int> DeleteAction(UserAction action)
        {
            return _database.DeleteAsync<UserAction>(action);
        }

        public Task<int> CreateOrUpdateActionContextAsync(UserActionContext actionContext)
        {
            if (actionContext.ID == 0)
            {
                return _database.InsertAsync(actionContext);
            }
            else
            {
                return _database.UpdateAsync(actionContext);
            }
        }

        public Task<int> DeleteActionContextAsync(UserActionContext actionContext)
        {
            return _database.DeleteAsync<UserActionContext>(actionContext.ID);
        }

        public Task<List<UserActionContext>> GetActionContextsAsync()
        {
            return _database.Table<UserActionContext>().ToListAsync();
        }

        public Task<UserActionContext> GetActionContextAsync(int ID)
        {
            var context = _database.FindAsync<UserActionContext>(ID);

            return context;
        }
    }
}
