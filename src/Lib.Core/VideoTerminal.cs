namespace AnsiVideoTerminal;

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
     
    2026-02-06    0.0.0.4     Transfer to empty Linux project
    2025          0.0.0.3     Some experience collected with Test implementation
    2024          0.0.0.2     Test implementation for Windows with .NET Core
    2017          0.0.0.1     Implementation with .NET Framework with Console Class
    --------------------------------------------------------------------------------------------------------------------------

*/

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
public class Class1
{

}
