using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ComtradeHandler.Wpf.App.Utils;

public static class ConsoleHelper
{
    public static bool ShowConsole()
    {
        return SetConsoleVisibility(true);
    }

    public static bool HideConsole()
    {
        return SetConsoleVisibility(false);
    }

    private static bool SetConsoleVisibility(bool visible)
    {
        var hwnd = GetConsoleWindow();
        GetWindowThreadProcessId(hwnd, out var pid);

        // It's not our console - don't mess with it. Might be the IDE Debug Pane.
        if (pid != Process.GetCurrentProcess().Id) {
            return false;
        }

        var visibility = visible ? SW_SHOW : SW_HIDE;

        ShowWindow(hwnd, visibility);
        DeleteMenu(GetSystemMenu(hwnd, false), SC_CLOSE, MF_BYCOMMAND);

        return true;
    }

    #region Win API Functions and Constants

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;
    private const int MF_BYCOMMAND = 0x00000000;
    private const int SC_CLOSE = 0xF060;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    #endregion
}
