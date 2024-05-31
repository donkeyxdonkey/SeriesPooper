using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPooper.Application;
internal class Menu
{
    const char BLUE = 'b';
    const char WHITE = 'w';
    const char SKIP = '.';

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

    private static ushort separatorLength = 79;

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

        for (int i = 0; i < separatorLength; i++)
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
}
