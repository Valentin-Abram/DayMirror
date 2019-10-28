using DayMirror.Enums.UserAction;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror.Models.UserAction
{
    public class UserAction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Title { get; set; }
        public UserActionStatus Status { get; set; }

        [ForeignKey(typeof(UserActionContext))]
        public int? UserActionContextId { get; set; }
    }
}
