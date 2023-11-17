using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Develop.EDK;
using Develop.EDK.Attributes;
using Microsoft.UI.Xaml.Controls;

namespace TestExtension
{
    public class Extension
    {
        [ExtMain]
        public static void ExtMain()
        {
            Debug.WriteLine("Test extension has been loaded!");

            MenuFlyoutSubItem i = new MenuFlyoutSubItem();
            i.Text = "Test Extension";

            MenuFlyoutItem i1 = new MenuFlyoutItem();
            i1.Text = "Print Hello in debug";
            i.Items.Add(i1);

            MenuRegister.AddMenu(i);
        }
    }
}
