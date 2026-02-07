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
    2ß26-02-07    0.0.0.7     addes some features from existing code
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

    int _row = 1;
    int _col = 1;
    int _maxRow = -1;
    int _maxCol = -1;

    TerminalColors _defaultColorForeGround = TerminalColors.Green;
    TerminalColors _defaultColorBackGround = TerminalColors.Black;
    TerminalColors _currentColorForeGround = TerminalColors.Green;
    TerminalColors _currentColorBackGround = TerminalColors.Black;
    TerminalColors _currentForeGround = TerminalColors.Green;
    TerminalColors _currentBackGround = TerminalColors.Black;

    public VideoTerminal()
    {
        this.SetColor(_defaultColorForeGround, _defaultColorBackGround);
        this.ClearHome();
        _maxRow = Console.WindowHeight;
        _maxCol = Console.WindowWidth;
    }

    /*
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

    public void ResetColor()
    {
        WriteRaw("\x1b[0m");
    }

    private static void WriteRaw(string ansi)
    {
        Console.Write(ansi);
    }

    public void Write(int row, int col, string text)
    {
        _col = col;
        _row = row;
        Console.Write($"\x1b[{_row};{_col}H{text}");
    }

    public void ResetColors()
    {
        _currentColorForeGround = _defaultColorForeGround;
        _currentColorBackGround = _defaultColorBackGround;
    }

    public int MaxRow
    {
        get
        {
            return _maxRow;
        }
        set
        {
            _maxRow = Console.WindowHeight;
        }
    }

    public int MaxCol
    {
        get
        {
            return _maxCol;
        }
        set
        {
            _maxCol = Console.WindowWidth;
        }
    }

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
            Console.Write(new string('q', _maxCol - 2));
            Console.Write("k");
            Console.Write($"\x1b[{_maxRow - 1}H");
            Console.Write("m");
            Console.Write(new string('q', _maxCol - 2));
            Console.Write("j");

            for (int r = 2; r < _maxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1Hx");
                Console.Write($"\x1b[{r};{_maxCol}Hx");
            }

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[H");

            Console.Write("+");
            Console.Write(new string('-', _maxCol - 2));
            Console.Write("+");
            Console.Write($"\x1b[{_maxRow - 1}H");
            Console.Write("+");
            Console.Write(new string('-', _maxCol - 2));
            Console.Write("+");

            for (int r = 2; r < _maxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1H|");
                Console.Write($"\x1b[{r};{_maxCol}H|");
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
            Console.Write(new string('q', _maxCol - 2));
            Console.Write("k");
            Console.Write($"\x1b[{_maxRow - 1}H");
            Console.Write("m");
            Console.Write(new string('q', _maxCol - 2));
            Console.Write("j");

            for (int r = 2; r < _maxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};1Hx");
                Console.Write($"\x1b[{r};{_maxCol}Hx");
            }

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write(new string('-', _maxCol - col - 2));
            Console.Write("+");
            Console.Write($"\x1b[{_maxRow - 1};{col}H");
            Console.Write("+");
            Console.Write(new string('-', _maxCol - col - 2));
            Console.Write("+");

            for (int r = 2; r < _maxRow - 1; r++)
            {
                Console.Write($"\x1b[{r};{col}H|");
                Console.Write($"\x1b[{r};{_maxCol}H|");
            }
        }

        this.Position(1, 1);
    }

    public void BoxVL(int row, int col, bool useDECLineDrawing = true)
    {
        if (useDECLineDrawing)
        {
            Console.Write($"\x1b(0");

            for (int r = 2; r <= _maxRow - 2; r++)
            {
                Console.Write($"\x1b[{r};{col}Hx");
            }

            Console.Write($"\x1b[{row};{col}H");
            Console.Write("w");
            Console.Write($"\x1b[{_maxRow - 1};{col}H");
            Console.Write("v");

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write($"\x1b[{_maxRow - 1};{col}H");
            Console.Write("+");

            for (int r = 2; r < _maxRow - 2; r++)
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

            for (int c = 2; c <= _maxCol - 1; c++)
            {
                Console.Write($"\x1b[{row};{c}Hq");
            }

            Console.Write($"\x1b[{row};{col}H");
            Console.Write("t");
            Console.Write($"\x1b[{row};{_maxCol}H");
            Console.Write("u");

            Console.Write($"\x1b[H\x1b(B");
        }
        else
        {
            Console.Write($"\x1b[{row};{col}H");

            Console.Write("+");
            Console.Write($"\x1b[{row};{_maxCol - 1}H");
            Console.Write("+");

            for (int c = 2; c < _maxCol - 1; c++)
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

    public void SetColor(TerminalColors foreGround, TerminalColors backGround)
    {
        Console.Write($"\x1b[{(int)TerminalColorsArea.ForeGround + (int)foreGround}m\x1b[{(int)TerminalColorsArea.BackGround + (int)backGround}m");
    }

    /*
    public void SetColor(ItemColor color)
    {
        Console.Write($"\x1b[{(int)TerminalColorsArea.ForeGround + (int)color.ForeGround}m\x1b[{(int)TerminalColorsArea.BackGround + (int)color.BackGround}m");
    }
    */
    
    public void SetBold(bool bold = true)
    {
        if (bold)
        {
            Console.Write($"\x1b[1m");
        }
        else
        {
            Console.Write($"\x1b[21m");
        }
    }
    
}    

