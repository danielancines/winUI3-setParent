using Microsoft.UI;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr child, IntPtr parent);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);


        IntPtr parentWindowHandle, childWindowHandle;
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);

            var childWindow = new Window { Content = new Border { Background = new SolidColorBrush(Colors.Blue) } };
            this.childWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(childWindow);

            SetWindowLong(this.childWindowHandle, -16, 0x00040000L | 0x00c00000L);

            childWindow.Activate();


        }

        private void myButton1_Click(object sender, RoutedEventArgs e)
        {
            SetWindowLong(this.childWindowHandle, -16, 0x00040000L | 0x00c00000L);
        }

        private void myButton2_Click(object sender, RoutedEventArgs e)
        {
            SetParent(this.childWindowHandle, this.parentWindowHandle);

        }
    }
}
