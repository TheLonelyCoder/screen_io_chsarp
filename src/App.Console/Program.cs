/* 
    --------------------------------------------------------------------------------------------------------------------------
    TEST PROGRAM for VideoTerminal Class (.NET Core, ANSI Terminals only)
    --------------------------------------------------------------------------------------------------------------------------
    
    

    --------------------------------------------------------------------------------------------------------------------------
    --- Module History -------------------------------------------------------------------------------------------------------
    --------------------------------------------------------------------------------------------------------------------------
    DATE          VERSION     DESCRITPION
    --------------------------------------------------------------------------------------------------------------------------
    2026-02-11    0.0.1.7     Code structure 
    2026-02-08    0.0.0.6     "Ctrl-C" Cleanup activated
    2026-02-08    0.0.0.5     Cursor ON / OFF testing
    2026-02-07    0.0.0.4     Primary/Secondary Screen Buffer tested
    2026-02-07    0.0.0.3     Box Method tested
    2026-02-07    0.0.0.2     code cleanup in VideoTerminal started
    2026-02-07    0.0.0.1     changed to new class methods
    2026-02-06    0.0.0.0     First Test of class
    --------------------------------------------------------------------------------------------------------------------------

*/

using AnsiVideoTerminal;

var _appVer = typeof(Program).Assembly.GetName().Version?.ToString() ?? "?";
var _libVer = typeof(VideoTerminal).Assembly.GetName().Version?.ToString() ?? "?";

AnsiSetup.SetupAnsiTerminal();

using (var _vt = new VideoTerminal())
{
    AppDomain.CurrentDomain.ProcessExit += (_, __) => _vt.Dispose();
    Console.CancelKeyPress += (_, __) => _vt.Dispose();
    RunBasicTest(_vt, _appVer, _libVer);
}


void RunBasicTest(VideoTerminal vt, string appVer, string libVer)
{
    vt.CursorOff();
    vt.UseSecondaryBuffer();
    vt.ClearScreen();
    vt.SetColor(TerminalColors.Green, TerminalColors.Black);
    vt.Box();
    vt.Write( 2, 10, "Programm Version " + appVer);
    vt.Write( 3, 10, "VideoTerminal Version " + libVer);
    vt.Write( 5,  5, "ANSI Terminal ready for more code!");
    vt.Write( 9,  5, "Press any key...");
    Console.ReadKey(true);
    vt.UsePrimaryBuffer();
    vt.CursorOn();
}