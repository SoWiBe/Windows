using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    [DllImport("user32.dll")]
    private static extern int EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    static void Main(string[] args)
    {
        var windowTitles = new List<string>();
        EnumWindows(new EnumWindowsProc((tophandle, _) =>
        {
            var length = GetWindowTextLength(tophandle);
            if (length == 0)
                return true;

            var sb = new StringBuilder(length);
            GetWindowText(tophandle, sb, length + 1);
            if (IsWindowVisible(tophandle) && !string.IsNullOrEmpty(sb.ToString()))
            {
                windowTitles.Add(sb.ToString());
            }
            return true;
        }), IntPtr.Zero);

        foreach (var title in windowTitles)
        {
            Console.WriteLine(title);
        }
    }
}
