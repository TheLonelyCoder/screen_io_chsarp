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
    2026-02-14    0.4.0.29    'ReadScreen' renamed to 'Read', 'ShowScreen' renamed to 'Show' and 'EditScreen' => 'ScreenForm'
    2026-02-14    0.3.3.28    Color settings for 'ReadScreen'
    2026-02-13    0.3.2.27    'ReadScreen' Arrow Keys now working correct, Tab & Shift-Tab too
    2026-02-13    0.3.1.26    'ReadScreen' color quirks
    2026-02-13    0.3.1.25    'ReadScreen' added
    2026-02-13    0.3.1.24    change 'ShowScreen' => moved into new class 'EditScreen'
    2026-02-13    0.3.0.23    'ShowScreen' better solution for color quirsk
    2026-02-13    0.3.0.22    'ShowScreen' color quirks fixed
    2026-02-13    0.3.0.21    'ShowScreen' added
    2026-02-12    0.2.0.20    'EditTextAt' without Password char (optional now)
    2026-02-12    0.2.0.19    Added 'EditTextAt' method to 'VideoTerminal'
    2026-02-12    0.1.0.18    Added 'ScreenItem' Class
    2026-02-12    0.1.0.17    Bugfix with version number (mixed up 'bugfix' with 'feature' Major.Minor.Error.Build
    2026-02-11    0.0.1.16    Bugfix (ReadKey echo)
    2026-02-11    0.0.1.15    Bugfix (to many "state" vars)
    2026-02-11    0.0.1.14    Bugfix (SetColor with ItemColor)
    2026-02-11    0.0.1.13    Added "prompt/menue" functionality into lib (inspired by Clipper©)
    2026-02-08    0.0.0.12    code cleanup 
    2026-02-08    0.0.0.11    code cleanup
    2026-02-08    0.0.0.10    primary/secondary screen buffer bug fixing
    2026-02-07    0.0.0.9     primary/secondary screen buffer
    2026-02-07    0.0.0.8     code cleanup after AI consultation
    2026-02-07    0.0.0.7     addes some features from existing code
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

public class VideoTerminal : IDisposable
{

    int _row = 1;
    int _col = 1;

    bool _inSecondary = false;

    TerminalColors _defaultColorForeGround = TerminalColors.Green;
    TerminalColors _defaultColorBackGround = TerminalColors.Black;
    TerminalColors _currentColorForeGround = TerminalColors.Green;
    TerminalColors _currentColorBackGround = TerminalColors.Black;

    public VideoTerminal()
    {
        this.SetColor(_defaultColorForeGround, _defaultColorBackGround);
    }

    public void UseSecondaryBuffer()
    {
        Console.Write("\x1b[?1049h");
        _inSecondary = true;
    }

    public void UsePrimaryBuffer()
    {
        Console.Write("\x1b[?1049l");
        _inSecondary = false;
    }

    public void CursorOff()
    {
        Console.Write("\x1b[?25l");
    }

    public void CursorOn()
    {
        Console.Write("\x1b[?25h");
    }

    /*

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
    */

    public void Write(int row, int col, string text)
    {
        _col = col;
        _row = row;
        Console.Write($"\x1b[{_row};{_col}H{text}");
    }

    // changes colors back to default value (used by some drawing procedures)
    public void ResetColorToDefault()
    {
        SetColor(_defaultColorForeGround, _defaultColorBackGround);
    }

    public int MaxRow => Console.WindowHeight;
    public int MaxCol => Console.WindowWidth;

    public void ClearScreen(int mode = 2)
    {
        Console.Write($"\x1b[{mode}J");
        _col = 1;
        _row = 1;
    }

    public void ClearScreenAndBuffer()
    {
        Console.Write("\x1b[3J");
        _col = 1;
        _row = 1;
    }

    public void ClearScreenToHome()
    {
        Console.Write("\x1b[1J");
        _col = 1;
        _row = 1;
    }

    public int Row()
    {
        return this._row;
    }

    public int Col()
    {
        return _col;
    }

    public void ClearHome()
    {
        Console.Write("\x1b[2J\x1b[1;1H");
        _col = 1;
        _row = 1;
    }

    public void ClearToEOL(int row, int col = 0)
    {
        Console.Write($"\x1b[{row};{col}H\x1b[0K");
    }

    public void Box(bool useDECLineDrawing = true)
    {
        if (useDECLineDrawing)
        {
            Console.Write($"\x1b[H\x1b(0");

            Console.Write("l");
            Console.Write(new string('q', this.MaxCol - 2));
            Console.Write("k");
            Console.Write($"\x1b[{this.MaxRow - 1}H");
            Console.Write("m");
            Console.Write(new string('q', this.MaxCol - 2));
            Console.Write("j");

            for (int r = 2; r < this.MaxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1Hx");
                Console.Write($"\x1b[{r};{this.MaxCol}Hx");
            }

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[H");

            Console.Write("+");
            Console.Write(new string('-', this.MaxCol - 2));
            Console.Write("+");
            Console.Write($"\x1b[{this.MaxRow - 1}H");
            Console.Write("+");
            Console.Write(new string('-', this.MaxCol - 2));
            Console.Write("+");

            for (int r = 2; r < this.MaxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1H|");
                Console.Write($"\x1b[{r};{this.MaxCol}H|");
            }
        }

        this.Position(1, 1);
    }

    public void Box(int row, int col, bool useDECLineDrawing = true)
    {
        if (useDECLineDrawing)
        {
            Console.Write($"\x1b[{row};{col}H\x1b(0");

            Console.Write("l");
            Console.Write(new string('q', this.MaxCol - 2));
            Console.Write("k");
            Console.Write($"\x1b[{this.MaxRow - 1}H");
            Console.Write("m");
            Console.Write(new string('q', this.MaxCol - 2));
            Console.Write("j");

            for (int r = 2; r < this.MaxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1Hx");
                Console.Write($"\x1b[{r};{this.MaxCol}Hx");
            }

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write(new string('-', this.MaxCol - col - 2));
            Console.Write("+");
            Console.Write($"\x1b[{this.MaxRow - 1};{col}H");
            Console.Write("+");
            Console.Write(new string('-', this.MaxCol - col - 2));
            Console.Write("+");

            for (int r = 2; r < this.MaxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};{col}H|");
                Console.Write($"\x1b[{r};{this.MaxCol}H|");
            }
        }

        this.Position(1, 1);
    }

    public void BoxVL(int row, int col, bool useDECLineDrawing = true)
    {
        if (useDECLineDrawing)
        {
            Console.Write($"\x1b(0");

            for (int r = 2; r <= this.MaxRow - 2; r++)
            {
                Console.Write($"\x1b[{r};{col}Hx");
            }

            Console.Write($"\x1b[{row};{col}H");
            Console.Write("w");
            Console.Write($"\x1b[{this.MaxRow - 1};{col}H");
            Console.Write("v");

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write($"\x1b[{this.MaxRow - 1};{col}H");
            Console.Write("+");

            for (int r = 2; r < this.MaxRow - 2; r++)
            {
                Console.Write($"\x1b[{r};{col}H|");
            }
        }

        this.Position(1, 1);
    }

    public void BoxHL(int row, int col, bool useDECLineDrawing = true)
    {
        if (useDECLineDrawing)
        {
            Console.Write($"\x1b(0");

            for (int c = 2; c <= this.MaxCol - 1; c++)
            {
                Console.Write($"\x1b[{row};{c}Hq");
            }

            Console.Write($"\x1b[{row};{col}H");
            Console.Write("t");
            Console.Write($"\x1b[{row};{this.MaxCol}H");
            Console.Write("u");

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write($"\x1b[{row};{this.MaxCol - 1}H");
            Console.Write("+");

            for (int c = 2; c < this.MaxCol - 1; c++)
            {
                Console.Write($"\x1b[{row};{c}H-");
            }
        }

        this.Position(1, 1);
    }

    public void Position(int row, int col)
    {
        _col = col;
        _row = row;
        Console.Write($"\x1b[{_row};{_col}H");
    }

    public ConsoleKeyInfo WaitMessage(int row, int col, string text)
    {
        _col = col;
        _row = row;
        Console.Write($"\x1b[{_row};{_col}H{text}");
        return Console.ReadKey();
    }

    public int CursorUp()
    {
        if (_row > 1)
        {
            _row--;
            Console.Write("\x1b[1A");
        }
        return _row;
    }

    public int CursorDown()
    {
        // TODO: Check if bottom reached
        _row++;
        Console.Write("\x1b[1B");
        return _row;
    }

    public int CursorLeft()
    {
        if (_col > 1)
        {
            _col--;
            Console.Write("\x1b[1D");
        }
        return _col;
    }

    public int CursorRight()
    {
        // TODO: Check if right margin reached
        _col++;
        Console.Write("\x1b[1C");
        return _col;
    }

    public int CursorLineBegin()
    {
        _col = 1;
        Console.Write("\x1b[1G");
        return _col;
    }

    public void ColorGreen()
    {
        Console.Write("\x1b[91m");
    }

    public void ColorGreenOnBlack()
    {
        Console.Write("\x1b[32m\x1b[40m");
        // Console.Write("\x1b[92m");
    }

    public void ColorBlackOnGreen()
    {
        Console.Write("\x1b[30m\x1b[42m");
        // Console.Write("\x1b[93m");
    }

    public void ColorReset()
    {
        Console.Write("\x1b[0m");
    }

    public void ColorInverseOn()
    {
        Console.Write("\x1b[7m");
    }

    public void ColorInverseOff()
    {
        Console.Write("\x1b[27m");
    }

    /// <summary>
    /// Set color AND change current settings
    /// </summary>
    /// <param name="foreGround"></param>
    /// <param name="backGround"></param>
    public void SetColor(TerminalColors foreGround, TerminalColors backGround)
    {
        _currentColorForeGround = foreGround;
        _currentColorBackGround = backGround;

        Console.Write($"\x1b[{(int)TerminalColorsArea.ForeGround + (int)foreGround}m\x1b[{(int)TerminalColorsArea.BackGround + (int)backGround}m");
    }

    public void SetColor(ItemColor color)
    {
        SetColor(color.ForeGround, color.BackGround);
    }

    /// <summary>
    /// Set color BUT DOESNT TOCH current settings
    /// </summary>
    /// <param name="foreGround"></param>
    /// <param name="backGround"></param>
    public void ChangeColor(TerminalColors foreGround, TerminalColors backGround)
    {
        Console.Write($"\x1b[{(int)TerminalColorsArea.ForeGround + (int)foreGround}m\x1b[{(int)TerminalColorsArea.BackGround + (int)backGround}m");
    }

    /*
    public void SwapColors()
    {
        TerminalColors swap = _currentColorBackGround;
        _currentColorBackGround = _currentColorForeGround;
        _currentColorForeGround = swap;
        Console.Write($"\x1b[{(int)TerminalColorsArea.ForeGround + (int)_currentColorForeGround}m\x1b[{(int)TerminalColorsArea.BackGround + (int)_currentColorBackGround}m");
    }
    */

    public void SetBold(bool bold = true)
    {
        if (bold)
        {
            Console.Write("\x1b[1m");
        }
        else
        {
            Console.Write("\x1b[22m");   // <-- statt 21m
        }
    }

    public void SetUnderline(bool underline = true)
    {
        if (underline)
        {
            Console.Write("\x1b[4m");
        }
        else
        {
            Console.Write("\x1b[24m");
        }
    }

    #region
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        try { ColorReset(); } catch { }     // ESC[0m
        try { CursorOn(); } catch { }
        try { UsePrimaryBuffer(); } catch { }
        try { Console.Out.Flush(); } catch { }

        _disposed = true;
    }
    #endregion

    public int ShowMenue(Menue menue)
    {
        TerminalColors baseFg = _currentColorForeGround;
        TerminalColors baseBg = _currentColorBackGround;

        int c = 0;

        if (menue.LastSelectedItem > -1)
        {
            menue.CurrentItem = menue.LastSelectedItem;
        }

        foreach (MenueItem mi in menue.Items)
        {
            if (c == menue.CurrentItem)
            {
                if (mi.UseItemColors)
                {
                    this.SetColor(mi.ColorSelected);
                }
                else if (menue.UseMenueColors)
                {
                    this.SetColor(menue.ColorSelected);
                }
                else
                {
                    this.SetColor(baseBg, baseFg);
                }
            }
            else
            {
                if (mi.UseItemColors)
                {
                    this.SetColor(mi.Color);
                }
                else if (menue.UseMenueColors)
                {
                    this.SetColor(menue.Color);
                }
                else
                {
                    this.SetColor(baseFg, baseBg);
                }
            }
            this.Write(mi.Row, mi.Col, mi.Value);
            this.SetColor(baseFg, baseBg);
            c++;
        }

        return menue.CurrentItem;
    }

    public int ReadMenue(Menue menue)
    {
        // this.ShowMenue(menue);
        int result = -1;

        ConsoleKeyInfo pressedKey;
        while (true)
        {
            pressedKey = Console.ReadKey(true);
            if (pressedKey.Key == ConsoleKey.Enter)
            {
                result = menue.CurrentItem;
                menue.LastSelectedItem = menue.CurrentItem;
                break;
            }
            else if (pressedKey.Key == ConsoleKey.Escape)
            {
                break;
            }
            else if (pressedKey.Key == ConsoleKey.Home)
            {
                menue.CurrentItem = 0;
                this.ShowMenue(menue);
            }
            else if (pressedKey.Key == ConsoleKey.End)
            {
                menue.CurrentItem = menue.Items.Count - 1;
                this.ShowMenue(menue);
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                menue.LastSelectedItem = -1;
                menue.CurrentItem = menue.CurrentItem - 1;
                if (menue.CurrentItem < 0)
                {
                    menue.CurrentItem = menue.Items.Count - 1;
                }
                this.ShowMenue(menue);
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                menue.LastSelectedItem = -1;
                menue.CurrentItem = menue.CurrentItem + 1;
                if (menue.CurrentItem >= menue.Items.Count)
                {
                    menue.CurrentItem = 0;
                }
                this.ShowMenue(menue);
            }
        }

        return result;
    }

    public ConsoleKeyInfo EditTextAt(int row, int col, ref string text, int length, string passwordChar = "")
    {
        string originalText = text;
        ConsoleKeyInfo result = new ConsoleKeyInfo();
        string editBuffer = text.PadRight(length);
        bool IsInsertMode = false;

        // this.SetColor(_currentColorBackGround, _currentColorForeGround); 
        this.ChangeColor(_currentColorBackGround, _currentColorForeGround); 
        this.Position(row, col);

        while (true)
        {
            int backupRow = _row;
            int backupCol = _col;
                
            if (String.IsNullOrEmpty(passwordChar)) 
            {
            this.Write(row, col, editBuffer);
            }
            else
            {
                // TODO: fix 'visible spaces in passwords' bug
                text = editBuffer.TrimEnd();
                this.Write(row, col, (new string(passwordChar[0], text.Length)).PadRight(length));
            }

            // this.Write(11, 85, "Wert '_col' : " + backupCol.ToString());
            // this.Write(12, 85, "Wert 'col'  : " + col.ToString());
            // this.Write(13, 85, "Wert 'idx'  : " + (backupCol - col).ToString());
            // this.Write(14, 85, "Textwert ...: " + editBuffer);
            
            this.Position(backupRow, backupCol);

            result = Console.ReadKey(true);

            if (result.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (result.Modifiers == ConsoleModifiers.Control) 
            {
                if (result.Key == ConsoleKey.S)
                {
                    break; 
                }
            }
            else if (result.Key == ConsoleKey.Escape)
            {
                text = originalText;
                break;
            }
            else if (result.Key == ConsoleKey.UpArrow)
            {
                break;
            }            
            else if (result.Key == ConsoleKey.Tab)
            {
                break;
            }            
            else if (result.Key == ConsoleKey.DownArrow)
            {
                break;
            }         
            else if (result.Key == ConsoleKey.RightArrow)
            {
                if (_col < (col + length) - 1)
                {
                    this.Position(row, _col + 1);
                }
            }   
            else if (result.Key == ConsoleKey.LeftArrow)
            {
                if (_col > col)
                {
                    this.Position(row, _col - 1);
                }
            }   
            else if (result.Key == ConsoleKey.Home)
            {
                this.Position(row, col);
            }              
            else if (result.Key == ConsoleKey.End)
            {
                this.Position(row, col + length - 1);
            }              
            else if (result.Key == ConsoleKey.Insert)
            {
                IsInsertMode = !IsInsertMode;
            }              
            else if (result.Key == ConsoleKey.Backspace)
            {
                int pos = (backupCol - col);

                if (pos > 0)
                {
                    editBuffer = editBuffer.Substring(0, pos - 1) + editBuffer.Substring(pos);
                    editBuffer = editBuffer.PadRight(length);
                    this.Position(row, _col - 1);
                }
            }              
            else if (result.Key == ConsoleKey.Delete)
            {
                int pos = (backupCol - col);

                if (pos < length)
                {
                    editBuffer = editBuffer.Substring(0, pos) + editBuffer.Substring(pos + 1);
                    editBuffer = editBuffer.PadRight(length);
                }
            }              
            else
            {
                backupRow = _row;
                backupCol = _col;

                char charInput = result.KeyChar;

                // this.Write(15, 85, "Textwert ...: " + editBuffer);
                // this.Write(15, 85, "Pressed Key : " + charInput);

                this.Position(backupRow, backupCol);

                int pos = (backupCol - col);

                if (IsInsertMode)
                {
                    if (pos == 0)
                    {
                        editBuffer = charInput + editBuffer;
                        editBuffer = editBuffer.Substring(0, length);
                    }
                    else
                    {
                        editBuffer = editBuffer.Substring(0, pos) + charInput + editBuffer.Substring(pos);
                        editBuffer = editBuffer.Substring(0, length);
                    }
                }
                else
                {
                    char[] edit = editBuffer.ToCharArray(0, length);
                    edit[pos] = charInput;
                    editBuffer = new string(edit);

                }

                if (_col < (col + length) - 1)
                {
                    this.Position(row, _col + 1);
                }
            }
        }

        text = editBuffer.TrimEnd();

        this.SetColor(_currentColorForeGround, _currentColorBackGround); 

        if (String.IsNullOrEmpty(passwordChar)) 
        {
            this.Write(row, col, editBuffer);
        }
        else
        {
            this.Write(row, col, (new string(passwordChar[0], text.Length)).PadRight(length));
        }

        return result;
    }

    public ConsoleKeyInfo EditTextAt(ScreenItem item)
    {
        string text = item.Value;
        ConsoleKeyInfo result = EditTextAt(item.Row, item.Col, ref text, item.Length, item.PasswordChar);
        if (result.Key != ConsoleKey.Escape)
        {
            item.Value = text;
        }
        return result;
    }

}    

public class MenueItem
{
    public int Col { get; set; }
    public int Row { get; set; }
    public string Value { get; set; }
    public ItemColor Color { get; set; }
    public ItemColor ColorSelected { get; set; }
    public bool UseItemColors { get; set; }
    public string Cargo { get; set; }

    public MenueItem(int row, int col, string value)
    {
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Cargo = String.Empty;
        this.UseItemColors = false;
    }

    public MenueItem(int row, int col, string value, string cargo)
    {
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Cargo = cargo;
        this.UseItemColors = false;
    }

    public MenueItem(int row, int col, string value, ItemColor colorValue, ItemColor colorValueSelected)
    {
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Cargo = String.Empty;
        this.Color = colorValue;
        this.ColorSelected = colorValueSelected;
        this.UseItemColors = true;
    }    

    public MenueItem(int row, int col, string value, string cargo, ItemColor colorValue, ItemColor colorValueSelected)
    {
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Cargo = cargo;
        this.Color = colorValue;
        this.ColorSelected = colorValueSelected;
        this.UseItemColors = true;
    } 
}

public class Menue
{
    public List<MenueItem> Items { get; set; }
    public int CurrentItem { get; set; }
    public ItemColor Color { get; set; }
    public ItemColor ColorSelected { get; set; }
    public bool UseMenueColors { get; set; }
    public int LastSelectedItem { get; set; }

    public Menue()
    {
        this.Items = new List<MenueItem>();
        this.CurrentItem = 0;
        this.UseMenueColors = false;
    }

    public Menue(int startWithSelectItem)
    {
        this.Items = new List<MenueItem>();
        this.CurrentItem = 0;
        this.LastSelectedItem = startWithSelectItem;
        this.UseMenueColors = false;
    }

    public Menue(ItemColor color, ItemColor colorSelected)
    {
        this.Items = new List<MenueItem>();
        CurrentItem = 0;
        this.Color = color;
        this.ColorSelected = colorSelected;
        this.UseMenueColors = true;
    }

    public Menue(List<MenueItem> items)
    {
        this.Items = items;
        CurrentItem = 0;
        this.UseMenueColors = false;
    }

    public Menue(List<MenueItem> items, ItemColor color, ItemColor colorSelected)
    {
        this.Items = items;
        CurrentItem = 0;
        this.Color = color;
        this.ColorSelected = colorSelected;
        this.UseMenueColors = true;
    }    
}

public class ItemColor
{
    public TerminalColors ForeGround { get; set; }
    public TerminalColors BackGround { get; set; }

    public ItemColor(TerminalColors foreGround, TerminalColors backGround)
    {
        this.ForeGround = foreGround;
        this.BackGround = backGround;
    }
}

public class ScreenItem
{
    public string ID { get; set; }
    public int Col { get; set; }
    public int Row { get; set; }
    public string Value { get; set; }
    public ItemColor Color { get; set; }
    public ItemColor ColorSelected { get; set; }
    public bool UseItemColors { get; set; }
    public string Cargo { get; set; }
    public bool IsNumeric { get; set; }
    public bool IsEditable { get; set; }
    public int Length { get; set; }
    public string Message { get; set; }
    public string PasswordChar { get; set; }
    public string UndoValue { get; set; }

    public ScreenItem(int row, int col, string value, int length, string id = "", string cargo = "", string passwordChar = "")
    {
        this.ID = id;
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Length = length;
        this.IsEditable = true;
        this.Cargo = cargo; 
        this.PasswordChar = passwordChar;
        this.UndoValue = value;
    }

    public ScreenItem(int row, int col, string value)
    {
        this.ID = value;                // same as display text
        this.Row = row;
        this.Col = col;
        this.Value = value;
        this.Length = -1;
        this.IsEditable = false;
        this.Cargo = value;             // same as display text
        this.PasswordChar = "";
        this.UndoValue = value; ;       // no deep thoughts about that shit
    }
}

public class ScreenForm
{
    VideoTerminal _vt = null;
    bool _useCurrentTerminalColors = true;
    
    TerminalColors _foreGroundLabel;
    TerminalColors _backGroundLabel;
    TerminalColors _foreGroundEdit;
    TerminalColors _backGroundEdit;
    TerminalColors _foreGroundCurrentEdit;
    TerminalColors _backGroundCurrentEdit;

    public ScreenForm(VideoTerminal vt)
    {
        _vt = vt;
    }

    public void SetColorScheme()
    {
        _useCurrentTerminalColors = false;
    }

    public void Show(List<ScreenItem> items)
    {
        foreach (ScreenItem item in items)
        {
            if (!item.IsEditable)
            {
                // simple output
                _vt.Write(item.Row, item.Col, item.Value);
            }
            else
            {
                // string dummyText = item.Value.PadRight(item.Length);
                // this.SetColor(_currentColorBackGround, _currentColorForeGround); 
                // this.Write(item.Row, item.Col, item.Value);
                // this.SetColor(_currentColorForeGround, _currentColorBackGround); 

                string dummyText = item.Value.PadRight(item.Length);
                // this.SetColor(_currentColorBackGround, _currentColorForeGround); 
                // this.SetBold();
                // this.SwapColors();
                // _vt.ColorInverseOn();
                if (_useCurrentTerminalColors)
                {
                    _vt.SetUnderline(true);
                }
                else
                {
                    // TODO: use class color set
                }

                if (String.IsNullOrEmpty(item.PasswordChar))
                {
                    // simple output
                    _vt.Write(item.Row, item.Col, item.Value.PadRight(item.Length));
                }
                else
                {
                    _vt.Write(item.Row, item.Col, (new string(item.PasswordChar[0], item.Value.Length)).PadRight(item.Length));
                }

                // this.SetColor(_currentColorForeGround, _currentColorBackGround); 
                // this.SetBold(false);
                // this.SwapColors();
                //_vt.ColorInverseOff();
                if (_useCurrentTerminalColors)
                {
                    _vt.SetUnderline(false);
                }
                else
                {
                    // TODO: use class color set
                }
                
            }
        }
    }

    public void Read(List<ScreenItem> items, bool exitOnEnter = false)
    {
        List<int> editItems = new List<int>();
        int c = 0;

        foreach (ScreenItem item in items)
        {
            if (item.IsEditable)
            {
                editItems.Add(c);
            }
            c++;
        }

        // edit as long you wish ....
        if (editItems.Count > 0)
        {
            // int currentItem = editItems[0];
            int currentItem = 0;
            while (true)
            {
                ScreenItem si = items[editItems[currentItem]];

                // ConsoleKeyInfo cki = _vt.EditTextAt(si);
                ConsoleKeyInfo cki = this.EditTextBox(si);
                if (cki.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (cki.Modifiers == ConsoleModifiers.Control)
                {
                    if (cki.Key == ConsoleKey.S)
                    {
                        break;
                    }
                }

                // handle results or whatever is needed

                // depending on the result ...
                if (cki.Key == ConsoleKey.UpArrow || (cki.Key == ConsoleKey.Tab && cki.Modifiers == ConsoleModifiers.Shift))
                {
                    currentItem--;
                    if (currentItem < 0)
                    {
                        currentItem = editItems.Count - 1;
                    }
                }
                else
                {
                    currentItem++;
                }
                if (currentItem == editItems.Count)
                {
                    // depending on "roll over" or "quit" (actually we quit)
                    // but not on testing ....
                    // break;
                    if (exitOnEnter && cki.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else
                    {
                        currentItem = 0;
                    }
                }
            }
        }

        if (_useCurrentTerminalColors)
        {
            _vt.SetUnderline(false);
            _vt.SetBold(false);
        }
        else
        {
            // TODO: use class color set
        }
    }
    
    public ConsoleKeyInfo EditTextBox(int row, int col, ref string text, int length, string passwordChar = "")
    {
        string originalText = text;
        ConsoleKeyInfo result = new ConsoleKeyInfo();
        string editBuffer = text.PadRight(length);
        bool IsInsertMode = false;

        if (_useCurrentTerminalColors)
        {
            _vt.SetUnderline(false);
            _vt.SetBold(true);
        }
        else
        {
            // TODO: use class color set
        }
        
        _vt.Position(row, col);

        while (true)
        {
            int backupRow = _vt.Row();
            int backupCol = _vt.Col();
                
            if (String.IsNullOrEmpty(passwordChar)) 
            {
                _vt.Write(row, col, editBuffer);
            }
            else
            {
                // TODO: fix 'visible spaces in passwords' bug
                text = editBuffer.TrimEnd();
                _vt.Write(row, col, (new string(passwordChar[0], text.Length)).PadRight(length));
            }

            _vt.Position(backupRow, backupCol);

            result = Console.ReadKey(true);

            if (result.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (result.Modifiers == ConsoleModifiers.Control) 
            {
                if (result.Key == ConsoleKey.S)
                {
                    break; 
                }
            }
            else if (result.Key == ConsoleKey.Escape)
            {
                text = originalText;
                break;
            }
            else if (result.Key == ConsoleKey.UpArrow)
            {
                break;
            }            
            else if (result.Key == ConsoleKey.Tab)
            {
                break;
            }            
            else if (result.Key == ConsoleKey.DownArrow)
            {
                break;
            }         
            else if (result.Key == ConsoleKey.RightArrow)
            {
                if (_vt.Col() < (col + length) - 1)
                {
                    _vt.Position(row, _vt.Col() + 1);
                }
            }   
            else if (result.Key == ConsoleKey.LeftArrow)
            {
                if (_vt.Col() > col)
                {
                    _vt.Position(row, _vt.Col() - 1);
                }
            }   
            else if (result.Key == ConsoleKey.Home)
            {
                _vt.Position(row, col);
            }              
            else if (result.Key == ConsoleKey.End)
            {
                _vt.Position(row, col + length - 1);
            }              
            else if (result.Key == ConsoleKey.Insert)
            {
                IsInsertMode = !IsInsertMode;
            }              
            else if (result.Key == ConsoleKey.Backspace)
            {
                int pos = (backupCol - col);

                if (pos > 0)
                {
                    editBuffer = editBuffer.Substring(0, pos - 1) + editBuffer.Substring(pos);
                    editBuffer = editBuffer.PadRight(length);
                    _vt.Position(row, _vt.Col() - 1);
                }
            }              
            else if (result.Key == ConsoleKey.Delete)
            {
                int pos = (backupCol - col);

                if (pos < length)
                {
                    editBuffer = editBuffer.Substring(0, pos) + editBuffer.Substring(pos + 1);
                    editBuffer = editBuffer.PadRight(length);
                }
            }              
            else
            {
                backupRow = _vt.Row();
                backupCol = _vt.Col();

                char charInput = result.KeyChar;

                _vt.Position(backupRow, backupCol);

                int pos = (backupCol - col);

                if (IsInsertMode)
                {
                    if (pos == 0)
                    {
                        editBuffer = charInput + editBuffer;
                        editBuffer = editBuffer.Substring(0, length);
                    }
                    else
                    {
                        editBuffer = editBuffer.Substring(0, pos) + charInput + editBuffer.Substring(pos);
                        editBuffer = editBuffer.Substring(0, length);
                    }
                }
                else
                {
                    char[] edit = editBuffer.ToCharArray(0, length);
                    edit[pos] = charInput;
                    editBuffer = new string(edit);
                }

                if (_vt.Col() < (col + length) - 1)
                {
                    _vt.Position(row, _vt.Col() + 1);
                }
            }
        }

        text = editBuffer.TrimEnd();

        if (_useCurrentTerminalColors)
        {
            _vt.SetUnderline(true);
            _vt.SetBold(false);
        }
        else
        {
            // TODO: use class color set
        }

        if (String.IsNullOrEmpty(passwordChar))
        {
            _vt.Write(row, col, editBuffer);
        }
        else
        {
            _vt.Write(row, col, (new string(passwordChar[0], text.Length)).PadRight(length));
        }

        return result;
    }

    public ConsoleKeyInfo EditTextBox(ScreenItem item)
    {
        string text = item.Value;
        ConsoleKeyInfo result = EditTextBox(item.Row, item.Col, ref text, item.Length, item.PasswordChar);
        if (result.Key != ConsoleKey.Escape)
        {
            item.Value = text;
        }
        return result;
    }
}