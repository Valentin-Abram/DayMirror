using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DayMirror.Models;
using SQLite;
using System.Linq;
using DayMirror.Pages.Report;

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

        public Task<UserAction> GetUserAction(int id)
        {
            return _database.GetAsync<UserAction>(id);
        }

        public async Task<List<StatisticData>> GetUserActionStatistic(DateTime dateFrom, DateTime dateTo)
        {
            var actions = await _database.Table<UserAction>()
                .ToListAsync();

            var report = actions
                .Where(a => a.Date.Date >= dateFrom.Date && a.Date.Date <= dateTo.Date)
                .GroupBy(a => a.UserActionContextId)
                .Select(a => a);

            List<StatisticData> statisticData = new List<StatisticData>();

            foreach (var item in report)
            {
                var actionContext = await GetActionContextAsync(item.Key);

                statisticData.Add(new StatisticData
                {
                    ActionContextName = actionContext?.Title,
                    TimeDeliveredMinutes = TimeSpan.FromMinutes(item.Sum(a => (a.EndTime.Subtract(a.StartTime)).TotalMinutes))
                }
                );
            }

            return statisticData;
        }


        // ToDo : find out why Where condition throws exception and how to make it work properly
        public async Task<List<UserAction>> GetDayActionsAsync(DateTime dateTime)
        {
            var result = await _database.Table<UserAction>()
                //.Where(d => ((DateTime)d.Date).Date == dateTime.Date)
                .ToListAsync();

            var actions = result
                .Where(a => a.Date.Date == dateTime.Date)
                .ToList(); ;

            return actions;
        }

        public List<UserAction> GetDayActions(DateTime dateTime)
        {
            var result = _database.Table<UserAction>()
                .ToListAsync().GetAwaiter().GetResult();

            return result
                .Where(a => a.Date.Date == dateTime.Date)
                .ToList();
        }

        public async Task<UserAction> CreateUserAction(UserAction action)
        {
            action.Date = DateTime.Now;
            await _database.InsertAsync(action);

            return action;
        }

        public async Task<UserAction> UpdateUserAction(UserAction action)
        {
            await _database.UpdateAsync(action);
            return action;
        }

        [Obsolete]
        public async Task<UserAction> CreateOrUpdateAction(UserAction action)
        {
            if (action.ID == 0)
            {
                await CreateUserAction(action);
                return action;
            }
            else
            {
                await UpdateUserAction(action);
                return action;
            }
        }

        public Task<int> DeleteAction(UserAction action)
        {
            return _database.DeleteAsync<UserAction>(action.ID);
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

        public Task<UserActionContext> GetActionContextAsync(int? ID)
        {
            if (ID is null)
            {
                return Task.FromResult<UserActionContext>(null);
            }

            var context = _database.FindAsync<UserActionContext>(ID);

            return context;
        }
    }
}
