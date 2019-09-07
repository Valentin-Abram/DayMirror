using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayMirror.ViewModels
{
    public class EditUserActionViewModel
    {
        
        public string Title { get; set; }
        public UserActionContext CurrentActionContext { get; set; }
    }
}
