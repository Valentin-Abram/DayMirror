using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror.Models.UserAction   
{
    public class UserActionContext
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
    }
}
