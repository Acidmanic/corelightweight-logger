using System;
using System.IO;

namespace Microsoft.Extensions.Logging.LightWeight
{
    
    public class ConsoleLogger : LoggerAdapter
    {
        public ConsoleLogger() : base(
            s => Log(s,Console.Error,ConsoleColor.Black,ConsoleColor.Cyan),
            s => Log(s, Console.Out, ConsoleColor.Black,ConsoleColor.Red),
            s => Log(s, Console.Out, ConsoleColor.Black,ConsoleColor.Yellow),
            Console.WriteLine
        )
        {
            
        }

        private static void Log(string message, TextWriter writer, ConsoleColor b,ConsoleColor f)
        {
            lock (Console.Out)
            {
                var fore = Console.ForegroundColor;
                var back = Console.BackgroundColor;

                Console.BackgroundColor = b;
                Console.ForegroundColor = f;
            
                writer.WriteLine(message);
            
                Console.BackgroundColor = back;
                Console.ForegroundColor = fore;    
            }
            
        }
    }
}