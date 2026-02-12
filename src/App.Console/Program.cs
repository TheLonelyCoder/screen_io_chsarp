/* 
    --------------------------------------------------------------------------------------------------------------------------
    TEST PROGRAM for VideoTerminal Class (.NET Core, ANSI Terminals only)
    --------------------------------------------------------------------------------------------------------------------------
    
    

    --------------------------------------------------------------------------------------------------------------------------
    --- Module History -------------------------------------------------------------------------------------------------------
    --------------------------------------------------------------------------------------------------------------------------
    DATE          VERSION     DESCRITPION
    --------------------------------------------------------------------------------------------------------------------------
    2026-02-12    0.1.0.11    Bugfix with version number (mixed up 'bugfix' with 'feature' Major.Minor.Error.Build
    2026-02-11    0.0.1.10    more test for prompt/menue
    2026-02-11    0.0.1.9     prompt/menue tests
    2026-02-11    0.0.1.8     Added empty new test ("Menue Tests" like the Clipper© "prompt/menue to")
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

    _vt.CursorOff();
    _vt.UseSecondaryBuffer();

    RunBasicTest(_vt, _appVer, _libVer);

    RunBasicMenueTest(_vt, _appVer, _libVer);
    
    _vt.UsePrimaryBuffer();
    _vt.CursorOn();
}


void RunBasicTest(VideoTerminal vt, string appVer, string libVer)
{
    vt.SetColor(TerminalColors.Green, TerminalColors.Black);
    vt.ClearScreen();
    vt.Box();
    vt.Write(2, 10, "Programm Version " + appVer);
    vt.Write(3, 10, "VideoTerminal Version " + libVer);
    vt.Write(5, 5, "Are you ready for more Tests?");
    vt.Write(9, 5, "Press any key... (or ^C for abort)");
    Console.ReadKey(true);
}

void RunBasicMenueTest(VideoTerminal vt, string appVer, string libVer)
{
    vt.SetColor(TerminalColors.Green, TerminalColors.Black);
    vt.ClearScreen();
    vt.Box();
    vt.Write(2, 10, "Program & Library Version: " + appVer + " / " + libVer);

    Menue m = new Menue(new ItemColor(TerminalColors.Yellow, TerminalColors.Black), new ItemColor(TerminalColors.Black, TerminalColors.Red));
    m.Items.Add(new MenueItem(5, 10, "Test Menue Item 1"));
    m.Items.Add(new MenueItem(7, 10, "Test Menue Item 2"));
    m.Items.Add(new MenueItem(9, 10, "Test Menue Item 3"));
    m.Items.Add(new MenueItem(11, 10, "Test Menue Item 4"));
    vt.ShowMenue(m);
    int result = vt.ReadMenue(m); 

    vt.Write(14, 5, "you selected item # " + result.ToString());
    Console.ReadKey(true);

    // Second run
    vt.ClearScreen();
    vt.Box();
    
    m = new();
    m.Items.Add(new MenueItem(5, 10, "Test Menue Item 1"));
    m.Items.Add(new MenueItem(7, 10, "Test Menue Item 2"));
    m.Items.Add(new MenueItem(9, 10, "Test Menue Item 3"));
    m.Items.Add(new MenueItem(11, 10, "Test Menue Item 4"));
    vt.ShowMenue(m);
    result = vt.ReadMenue(m); 

    vt.Write(14, 5, "you selected item # " + result.ToString());
    
    
    Console.ReadKey(true);
}