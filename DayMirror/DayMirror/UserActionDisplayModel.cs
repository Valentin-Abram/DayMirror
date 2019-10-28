using DayMirror.Enums.UserAction;
using DayMirror.Models;
using DayMirror.Models.UserAction;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror
{
    class UserActionDisplayModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Title { get; set; }
        public UserActionStatus Status { get; set; }
        public UserActionContext ActionContext { get; set; }

        public UserActionDisplayModel(UserAction userAction, UserActionContext actionContext)
        {
            this.ID = userAction.ID;
            this.Date = userAction.Date;
            this.StartTime = userAction.StartTime;
            this.EndTime = userAction.EndTime;
            this.Title = userAction.Title;
            this.Status = userAction.Status;
            this.ActionContext = actionContext;
        }
    }
}
