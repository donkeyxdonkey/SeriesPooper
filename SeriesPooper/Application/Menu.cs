using SeriesPooper.Data;
using SeriesPooper.Enumerations;
using SeriesPooper.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPooper.Application;
internal class Menu
{
    const char BLUE = 'b';
    const char WHITE = 'w';
    const char SKIP = '.';

    const string MENU_CURSOR = ">>";
    const string CLEAR_CURSOR = "  ";

    const ushort SEPARATOR_LENGTH = 79;

    private static string[] menu =
    [
        @"   ..           .             ..    ..      .       .         ..",
        @"  + .\ ... . ..(.) ...  ...  + + +\ \ \.. .+ +. ...+ +..     + +  ...   .. .",
        @"  \ \ + . \ '..+ ++ . \+ ..+ \ \+  \+ + .` + ..+ ..+ '. \   + +  + . \ + .` +",
        @"  .\ \  ..+ +  + +  ..+\.. \  \  +\  + (.+ + ++ (..+ + + + + +..+ (.) + (.+ +",
        @"  \..+\...+.+  +.+\...++...+   \+  \+ \..,.+\..\...+.+ +.+ \....+\...+ \.., +",
        @"                                                                       +...+"
    ];

    private static string[] colorMask =
    [
        @"b...............................................................",
        @"...w..x........b..x.........w..........x...w..x....w..x.....w....x..........",
        @"..bw.......................................................................b.",
        @"....w......................................................................b.",
        @".............................................................................",
        @"............................................................................"
    ];

    private static readonly Dictionary<MenuItems, ApplicationAction> _actionDictionary = new()
    {
        { MenuItems.MENU, ApplicationAction.Menu},
        { MenuItems.RECENT_WATCHES, ApplicationAction.ListRecent },
        { MenuItems.LIST_SERIES, ApplicationAction.ListSeries },
        { MenuItems.EXIT, ApplicationAction.Exit }
    };

    private static ushort _menuIndex;
    private static ushort _menuCursorLocation;
    private static ushort _menuLength;
    private static ushort _clearLength;
    private static MenuItems[] _menuItems = [];

    public static void Draw()
    {
        char prev = SKIP;

        foreach ((string Line, string Mask) in menu.Zip(colorMask))
        {
            foreach ((char L, char M) in Line.Zip(Mask))
            {
                if (M != SKIP && M != prev)
                {
                    Console.ForegroundColor = M switch
                    {
                        BLUE => ConsoleColor.DarkBlue,
                        WHITE => ConsoleColor.White,
                        _ => ConsoleColor.Yellow
                    };

                    prev = M;
                }

                Console.Write(L);
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void DrawSeparator()
    {
        const char SEPARATOR = '+';

        for (int i = 0; i < SEPARATOR_LENGTH; i++)
        {
            Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Magenta : ConsoleColor.White;
            Console.Write(SEPARATOR);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void DrawEmpty()
    {
        Console.WriteLine();
    }

    public static void DrawMenuItems(IEnumerable<MenuItems> items, ushort index = 0)
    {
        _menuItems = items.ToArray();
        _menuIndex = index;
        _menuLength = (ushort)items.Count();

        for (int i = 0; i < _menuLength; i++)
        {
            if (_menuItems[i] == MenuItems.LINE_SEPARATOR)
            {
                Console.WriteLine(ApplicationData.LINE_SEPARATOR);
                ConsoleUtility.LineSeparators[1]++;
                _menuLength--;
                continue;
            }

            if (i == index)
            {
                _menuCursorLocation = (ushort)Console.GetCursorPosition().Top;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(" >> ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
                Console.Write("    ");

            Console.WriteLine(items.ElementAt(i).ToString().Replace('_', ' '));
            ConsoleUtility.LineSeparators[1]++;
        }
    }

    public static void UpdateMenuItemsCallback(IEnumerable<MenuItems> items, int count, ushort cursorLocation, ushort index = 0)
    {
        _menuCursorLocation = cursorLocation;
        _menuItems = items.ToArray();
        _menuIndex = index;
        _menuLength = (ushort)items.Count();
    }

    internal static ApplicationAction Act(ConsoleKey keyPressed)
    {
        switch (keyPressed)
        {
            case ConsoleKey.DownArrow:
                if (_menuIndex + 1 == _menuLength)
                    break;

                MoveMenu(keyPressed);
                break;
            case ConsoleKey.UpArrow:
                if (_menuIndex - 1 < 0)
                    break;

                MoveMenu(keyPressed);
                break;
            case ConsoleKey.Enter:
                return GetAction();
            case ConsoleKey.Backspace:
                return ApplicationAction.BrowseBack;
        }

        return ApplicationAction.None;
    }

    private static ApplicationAction GetAction()
    {
        return _actionDictionary[_menuItems[_menuIndex]];
    }

    internal static void MoveMenu(ConsoleKey keyPressed)
    {
        Console.SetCursorPosition(1, _menuCursorLocation);
        Console.Write(CLEAR_CURSOR);

        switch (keyPressed)
        {
            case ConsoleKey.DownArrow:
                _menuCursorLocation++;
                _menuIndex++;
                break;
            case ConsoleKey.UpArrow:
                _menuCursorLocation--;
                _menuIndex--;
                break;
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.SetCursorPosition(1, _menuCursorLocation);
        Console.Write(MENU_CURSOR);
        Console.ForegroundColor = ConsoleColor.White; // kanske inte behövs...
    }
}
