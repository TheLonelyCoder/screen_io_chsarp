using AnsiVideoTerminal;

AnsiSetup.SetupAnsiTerminal();

var vt = new VideoTerminal();
vt.Clear();
vt.SetColor(TerminalColors.Green, TerminalColors.Black);
vt.WriteAt(1, 1, "ANSI ready");
vt.WriteAt(3, 1, "Press any key...");
Console.ReadKey(true);
vt.ResetColor();
