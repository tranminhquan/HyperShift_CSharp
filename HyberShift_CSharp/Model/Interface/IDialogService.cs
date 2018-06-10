﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.Model.Interface
{
    public interface IDialogService
    {
        void ShowNotification(string message, string caption = "");
        bool ShowConfirmationRequest(string message, string caption = "");
        string OpenFile(string caption, string filter = @"All files (*.*)|*.*");
    }
}
