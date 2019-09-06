using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DayMirror.ViewModel
{
    class UserActionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public UserActionContext ActionContext { get; set; }
        public UserActionStatus Status { get; set; }

        public UserAction GetAction()
        {
            return new UserAction
            {
                ID = Id,
                Title = Title,
                StartTime = StartTime,
                EndTime = EndTime,
                UserActionContextId = ActionContext?.ID,
                Status = Status,
                Date = Date
            };
        }

        public static async Task<UserActionViewModel> FromAction(UserAction action)
        {
            var actionContext = await App.Database
                .GetActionContextAsync(action.UserActionContextId);

            return new UserActionViewModel
            {
                Id = action.ID,
                Title = action.Title,
                StartTime = action.StartTime,
                EndTime = action.EndTime,
                ActionContext = actionContext,
                Status = action.Status,
                Date = action.Date
            };

        }

    }
}
