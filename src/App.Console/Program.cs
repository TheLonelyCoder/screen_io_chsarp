/* 
    --------------------------------------------------------------------------------------------------------------------------
    TEST PROGRAM for VideoTerminal Class (.NET Core, ANSI Terminals only)
    --------------------------------------------------------------------------------------------------------------------------
    
    

    --------------------------------------------------------------------------------------------------------------------------
    --- Module History -------------------------------------------------------------------------------------------------------
    --------------------------------------------------------------------------------------------------------------------------
    DATE          VERSION     DESCRITPION
    --------------------------------------------------------------------------------------------------------------------------
    2026-02-07    0.0.0.4     Primary/Secondary Screen Buffer tested
    2026-02-07    0.0.0.3     Box Method tested
    2026-02-07    0.0.0.2     code cleanup in VideoTerminal started
    2026-02-07    0.0.0.1     changed to new class methods
    2026-02-06    0.0.0.0     First Test of class
    --------------------------------------------------------------------------------------------------------------------------

*/

using AnsiVideoTerminal;

AnsiSetup.SetupAnsiTerminal();

var vt = new VideoTerminal();
vt.UseSecondoryBuffer();
vt.ClearScreen();
// vt.Clear();
vt.SetColor(TerminalColors.Green, TerminalColors.Black);
vt.Box();// vt.WriteAt(1, 1, "ANSI ready");
vt.Write( 3, 5, "ANSI ready");
vt.Write( 7, 5, "Press any key...");
Console.ReadKey(true);
vt.UsePrimaryBuffer();

