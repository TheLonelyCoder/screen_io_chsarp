/* 
    --------------------------------------------------------------------------------------------------------------------------
    --- Module History -------------------------------------------------------------------------------------------------------
    --------------------------------------------------------------------------------------------------------------------------
    DATE          VERSION     DESCRITPION
    --------------------------------------------------------------------------------------------------------------------------
    2026-02-06    0.0.0.5     Make Terminal Platform independent
    --------------------------------------------------------------------------------------------------------------------------

*/

using System;
using System.Runtime.InteropServices;

namespace AnsiVideoTerminal;

public static partial class AnsiSetup
{
    public static partial void SetupAnsiTerminal()
    {
        if (Console.IsOutputRedirected)
        {
            return;
        }

        var handle = GetStdHandle(STD_OUTPUT_HANDLE);

        if (!GetConsoleMode(handle, out uint mode))
        {
            return;
        }

        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        SetConsoleMode(handle, mode);
    }

    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
}
