using NativeFileDialogSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.ui.widgets
{
    public class FileDialog
    {
        public string SelectedFile { get; set; } = "";

        public bool ShowDialog(string title, string filter)
        {
            DialogResult dialogResult = Dialog.FileOpen(filter);
            SelectedFile = dialogResult.Path;
            return dialogResult.IsOk;
        }
    }
}
