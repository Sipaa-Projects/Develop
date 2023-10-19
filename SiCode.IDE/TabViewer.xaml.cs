using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SiCode.IDE
{
    /// <summary>
    /// An interface used to get a WinUI 3 window handle.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
    internal interface IWindowNative
    {
        IntPtr WindowHandle { get; }
    }

    public sealed partial class TabViewer : Window
    {
        public ApplicationDataContainer localSettings;

        public TabViewer()
        {
            this.InitializeComponent();

            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Activated += TabViewWindowingSamplePage_Loaded;
            Closed += (s, e) => { Environment.Exit(0); };
            Tabs.SelectionChanged += Tabs_SelectionChanged;
            AddNewTab();
            LoadIcon("sicode.ico");
        }

        private void LoadIcon(string iconName)
        {
            // Since WinUI 3 doesn't have a Window.Icon property, we will use P/Invoke to set the icon
            var hwnd = this.As<IWindowNative>().WindowHandle;
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName,
                      PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);

            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.Title = $"{((TabViewItem)Tabs.TabItems[Tabs.SelectedIndex]).Header} - Browse";
            }
            catch { }
        }

        public void AddNewTab()
        {
            TabViewItem t = new()
            {
                Header = "New Tab",
                Content = new MainPage() { ParentTabViewer = this }
            };

            ((MainPage)t.Content).TargetTabItem = t;

            Tabs.TabItems.Add(t);
        }

        private void TabViewWindowingSamplePage_Loaded(object sender, WindowActivatedEventArgs e)
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(CustomDragRegion);
            CustomDragRegion.MinWidth = 188;
        }

        private void Tabs_AddTabButtonClick(TabView sender, object args)
        {
            AddNewTab();
        }

        private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            Tabs.TabItems.Remove(args.Item);
        }
    }
}
