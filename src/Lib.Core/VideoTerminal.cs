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
    2026-02-06    0.0.0.6     First Test of class
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
    TerminalColors _currentForeGround = TerminalColors.Green;
    TerminalColors _currentBackGround = TerminalColors.Black;

    public void Clear()
    {
        // Clear screen + cursor home
        WriteRaw("\x1b[2J\x1b[H");
    }

    public void GotoXY(int row, int col)
    {
        if (row < 1) row = 1;
        if (col < 1) col = 1;

        WriteRaw($"\x1b[{row};{col}H");
    }

    public void SetColor(TerminalColors foreGround, TerminalColors backGround)
    {
        _currentForeGround = foreGround;
        _currentBackGround = backGround;

        int fg = 30 + (int)foreGround;
        int bg = 40 + (int)backGround;

        WriteRaw($"\x1b[{fg};{bg}m");
    }

    public void ResetColor()
    {
        WriteRaw("\x1b[0m");
    }

    public void Write(string text)
    {
        Console.Write(text);
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public void WriteAt(int row, int col, string text)
    {
        GotoXY(row, col);
        Write(text);
    }

    private static void WriteRaw(string ansi)
    {
        Console.Write(ansi);
    }
}    

