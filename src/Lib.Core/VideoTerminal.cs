/* 
    --------------------------------------------------------------------------------------------------------------------------
    VideoTerminal Class (.NET Core, ANSI Terminals only (on Windows, cmd must be started with 'Admin' rights to
    enable the support of ANSI Control sequences.
    --------------------------------------------------------------------------------------------------------------------------
    
    

    --------------------------------------------------------------------------------------------------------------------------
    --- Module History -------------------------------------------------------------------------------------------------------
    --------------------------------------------------------------------------------------------------------------------------
    DATE          VERSION     DESCRITPION
    --------------------------------------------------------------------------------------------------------------------------
    2026-02-06    0.0.0.5     Make Terminal Platform independent     
    2026-02-06    0.0.0.4     Transfer to empty Linux project
    2025          0.0.0.3     Some experience collected with Test implementation
    2024          0.0.0.2     Test implementation for Windows with .NET Core
    2017          0.0.0.1     Implementation with .NET Framework with Console Class
    --------------------------------------------------------------------------------------------------------------------------

*/

namespace AnsiVideoTerminal;

public enum TerminalColors
{
    Black = 0,
    Red = 1,
    Green = 2,
    Yellow = 3,
    Blue = 4,
    Magenta = 5,
    Cyan = 6,
    White = 7,
}

public enum TerminalColorsArea
{
    ForeGround = 30,
    BackGround = 40,
}

public class VideoTerminal
{
    int _row = 1;
    int _col = 1;
    int _maxRow = -1;
    int _maxCol = -1;

    TerminalColors _defaultColorForeGround = TerminalColors.Green;
    TerminalColors _defaultColorBackGround = TerminalColors.Black;
    TerminalColors _currentColorForeGround = TerminalColors.Green;
    TerminalColors _currentColorBackGround = TerminalColors.Black;

    #region Some static methods ....

    // P/Invoke declarations
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    public static void SetupAnsiTerminal()
    {
        // Get the handle to the standard output stream
        var handle = VideoTerminal.GetStdHandle(VideoTerminal.STD_OUTPUT_HANDLE);

        // Get the current console mode
        uint mode;
        if (!VideoTerminal.GetConsoleMode(handle, out mode))
        {
            Console.Error.WriteLine("Failed to get console mode");
            return;
        }

        // Enable the virtual terminal processing mode
        mode |= VideoTerminal.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        if (!VideoTerminal.SetConsoleMode(handle, mode))
        {
            Console.Error.WriteLine("Failed to set console mode");
            return;
        }
    }

}
