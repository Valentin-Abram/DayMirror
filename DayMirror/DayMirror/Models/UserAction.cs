using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror.Models
{
    public class UserAction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Title { get; set; }
    }
}
