using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using Develop.Extensions;
using Develop.EDK;

namespace Develop
{
    public sealed partial class MainPage
    {
        private MenuFlyoutItem _installExtMenu;
        public MainPage()
        {
            InitializeComponent();
            _installExtMenu = ExtInstallMenu;
            ReloadExtMenu();
            ExtensionManager.ExtensionAdded += new((s, e) =>
            {
                // Reload 'Extensions' menu
                ReloadExtMenu();
            });
        }

        void ReloadExtMenu()
        {
            ExtMenu.Items.Clear();
            foreach (var item in MenuRegister.Menus)
            {
                ExtMenu.Items.Add(item);
            }
            ExtMenu.Items.Add(_installExtMenu);
        }

        public TabViewer ParentTabViewer { get; internal set; }
        public TabViewItem TargetTabItem { get; internal set; }

        private void CodeEditorControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Needs to set focus explicitly due to WinUI 3 regression https://github.com/microsoft/microsoft-ui-xaml/issues/8816 
            //((Control)sender).Focus(FocusState.Programmatic);
            Editor.Scintilla(MicaEditor.ScintillaMessage.SetUseTabs, 0, 0); // Use spaces for indentation
            Editor.Scintilla(MicaEditor.ScintillaMessage.SetTabWidth, 4, 0);  // Set tab width to 4 spaces
            //Editor.Scintilla(MicaEditor.ScintillaMessage.SetIndentationGuides,MicaEditor. SC_IV_LOOKBOTH, 0);  // Show both tabs and spaces for indentation guides
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ParentTabViewer);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".dll");
            openPicker.FileTypeFilter.Add(".exe");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                Develop.Extensions.ExtensionManager.LoadLibrary(file.Path);
            }
        }

        private void ViewFullSMenu_Click(object sender, RoutedEventArgs e)
        {
            var aw = ParentTabViewer.m_appWindow;
            if (aw.Presenter.Kind == Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen)
            {
                aw.SetPresenter(ParentTabViewer.m_oldKind);
            }
            else
            {
                ParentTabViewer.m_oldKind = aw.Presenter.Kind;
                aw.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
            }
        }
    }
}
