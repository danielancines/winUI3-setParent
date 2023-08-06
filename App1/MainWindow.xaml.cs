using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1;

public enum WindowStyles : long
{
    /// <summary>
    /// The window has a thin-line border
    /// </summary>
    WS_BORDER = 0x00800000L,

    /// <summary>
    /// The window has a title bar (includes the WS_BORDER style)
    /// </summary>
    WS_CAPTION = 0x00C00000L,

    /// <summary>
    /// The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
    /// </summary>
    WS_CHILD = 0x40000000L,

    /// <summary>
    /// Same as the WS_CHILD style.
    /// </summary>
    WS_CHILDWINDOW = 0x40000000L,

    /// <summary>
    /// The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
    /// </summary>
    WS_SYSMENU = 0x00080000L,

    /// <summary>
    /// The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
    /// </summary>
    WS_DISABLED = 0x08000000L,

    /// <summary>
    /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
    /// </summary>
    WS_MAXIMIZEBOX = 0x00010000L,

    /// <summary>
    /// The window is initially maximized.
    /// </summary>
    WS_MAXIMIZE = 0x01000000L,

    /// <summary>
    /// The window has a sizing border. Same as the WS_SIZEBOX style.
    /// </summary>
    WS_THICKFRAME = 0x00040000L,

    /// <summary>
    /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style.
    /// </summary>
    WS_TILED = 0x00000000L,

    /// <summary>
    /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
    /// </summary>
    WS_OVERLAPPED = 0x00000000L,

    /// <summary>
    /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
    /// </summary>
    WS_MINIMIZEBOX = 0x00020000L,

    /// <summary>
    /// The window is an overlapped window. Same as the WS_OVERLAPPEDWINDOW style.
    /// </summary>
    WS_TILEDWINDOW = (long)WS_OVERLAPPED | (long)WS_CAPTION | (long)WS_SYSMENU | (long)WS_THICKFRAME | (long)WS_MINIMIZEBOX | (long)WS_MAXIMIZEBOX,

    /// <summary>
    /// The window is a pop-up window. This style cannot be used with the WS_CHILD style.
    /// </summary>
    WS_POPUP = 0x80000000L
}

public static class User32
{
    [DllImport("user32.dll")]
    public static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName,
       string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
       IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = false)]
    private static extern IntPtr SetParent(IntPtr child, IntPtr parent);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

    public static IntPtr GetParent(Window window)
    {
        return GetParent(GetWindowHandle(window));
    }

    public static void SetParent(Window child, Window parent)
    {
        SetParent(GetWindowHandle(child), GetWindowHandle(parent));
    }

    public static IntPtr SetWindowLong(Window window, int nIndex, long dwNewLong)
    {
        return SetWindowLong(GetWindowHandle(window), nIndex, dwNewLong);
    }

    public static IntPtr GetWindowHandle(Window window)
    {
        return WinRT.Interop.WindowNative.GetWindowHandle(window);
    }
}
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    SmartWindow _childWindow;
    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        var newWIndow = User32.CreateWindowEx((int)WindowStyles.WS_TILEDWINDOW, string.Empty, string.Empty, (int)WindowStyles.WS_TILEDWINDOW, 0, 0, 200, 200, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        //this.parentWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
        //this.childWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(childWindow);
        //SetWindowLong(this.childWindowHandle, -16, 0x00040000L | 0x00c00000L);
        //SetParent(this.childWindowHandle, this.parentWindowHandle);
        //this._childWindow = new SmartWindow { Content = new Border { Background = new SolidColorBrush(Colors.Blue) } };
        //User32.SetWindowLong(this._childWindow, -16, (long)WindowStyles.WS_TILEDWINDOW);
        //User32.SetWindowLong(this._childWindow, -16, (long)WindowStyles.WS_POPUP | (long)WindowStyles.WS_BORDER);
        //this._childWindow.Activate();
    }

    private void myButton1_Click(object sender, RoutedEventArgs e)
    {
        var a = User32.SetWindowLong(this._childWindow, -16, (long)WindowStyles.WS_DISABLED);
    }

    private void myButton2_Click(object sender, RoutedEventArgs e)
    {
        this._childWindow.ParentWindow = this;
    }
}

public class SmartWindow : Window
{
    private Window _parentWindow;
    public Window ParentWindow
    {
        get { return _parentWindow; }
        set
        {
            _parentWindow = value;
            this.SetParentWindow();
        }
    }

    private void SetParentWindow()
    {
        ThrowHelper.IsNull(this.ParentWindow);
        User32.SetParent(this, this.ParentWindow);
    }
}

public class ThrowHelper
{
    public static void IsNull(object item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
    }
}
