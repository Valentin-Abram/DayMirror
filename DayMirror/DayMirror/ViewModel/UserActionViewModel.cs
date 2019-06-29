using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror.ViewModel
{
    class UserActionViewModel
    {
        public string Title { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public UserActionContext ActionContext { get; set; }

    }
}
