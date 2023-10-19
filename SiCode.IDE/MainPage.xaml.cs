using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;

namespace SiCode.IDE
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public TabViewer ParentTabViewer { get; internal set; }
        public TabViewItem TargetTabItem { get; internal set; }

        private void CodeEditorControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Needs to set focus explicitly due to WinUI 3 regression https://github.com/microsoft/microsoft-ui-xaml/issues/8816 
            //((Control)sender).Focus(FocusState.Programmatic);
        }
    }
}
