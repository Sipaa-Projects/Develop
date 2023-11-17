using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Develop.EDK
{
    /// <summary>
    /// Utility class to register custom menus into Develop's menu bar.
    /// </summary>
    public class MenuRegister
    {
        /// <summary>
        /// A list of <see cref="MenuFlyoutSubItem"/> that will be shown in Develop's 'Extensions' menu
        /// </summary>
        public static readonly List<MenuFlyoutSubItem> Menus = new();

        /// <summary>
        /// Add a submenu to the 'Extensions' menu.
        /// </summary>
        /// <param name="menu">The submenu to add</param>
        /// <exception cref="OverflowException">Thrown when there's already 100 menus</exception>
        public static void AddMenu(MenuFlyoutSubItem menu)
        {
            if (Menus.Count == 100)
                throw new OverflowException("Too much menus! Limit: 100");
            Menus.Add(menu);
        }

        public static void RemoveMenu(MenuFlyoutSubItem menu)
        {
            Menus.Remove(menu);
        }
    }
}
