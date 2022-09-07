using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Extensions.Logging.LightWeight
{
    public class ConsoleLogger : LoggerAdapter
    {
        private class LevelName
        {
            public string Full { get; }
            public string Short { get; }

            public string ToString(bool fullName)
            {
                return fullName ? Full : Short;
            }

            public LevelName(string full, string @short)
            {
                Full = full;
                Short = @short;
            }
        }

        private static readonly Dictionary<LogLevel, LevelName> LevelNamesLeveled = new Dictionary<LogLevel, LevelName>();
        
        
        public ConsoleLogger() : base(
            (l,s)=> Log(l,s, Console.Error, ConsoleColor.Black, ConsoleColor.Cyan),
            (l,s) => Log(l,s, Console.Out, ConsoleColor.Black, ConsoleColor.Red),
            (l,s) => Log(l,s, Console.Out, ConsoleColor.Black, ConsoleColor.Yellow),
            (l,s) => Console.WriteLine(LevelNamesLeveled[l].ToString(FullLevelNames) + ": " + s)
        )
        {
            lock (LevelNamesLeveled)
            {
                if (LevelNamesLeveled.Count==0)
                {
                    LevelNamesLeveled.Add(LogLevel.Critical, new LevelName("   Critical  ", "[C]"));
                    LevelNamesLeveled.Add(LogLevel.Error, new LevelName("    Error    ","[E]"));
                    LevelNamesLeveled.Add(LogLevel.Information, new LevelName(" Information ","[I]"));
                    LevelNamesLeveled.Add(LogLevel.None, new LevelName("             ","   "));
                    LevelNamesLeveled.Add(LogLevel.Trace, new LevelName("    Trace    ","[T]"));
                    LevelNamesLeveled.Add(LogLevel.Warning, new LevelName("   Warning   ","[W]"));
                    LevelNamesLeveled.Add(LogLevel.Debug, new LevelName(  "    Debug    ","[D]"));
                }
            }
            
        }

        public static bool FullLevelNames { get; set; } = true;

        public ConsoleLogger UseFullNames()
        {
            FullLevelNames = true;

            return this;
        }

        public ConsoleLogger Shorten()
        {
            FullLevelNames = false;

            return this;
        }


        private static void Log(LogLevel level, string message, TextWriter writer, ConsoleColor b, ConsoleColor f)
        {
            lock (Console.Out)
            {

                var levelName = LevelNamesLeveled[level].ToString(FullLevelNames);
                
                var fore = Console.ForegroundColor;
                var back = Console.BackgroundColor;

                Console.BackgroundColor = b;
                Console.ForegroundColor = f;
                
                Console.Write(levelName);

                Console.BackgroundColor = back;
                Console.ForegroundColor = fore;
                
                writer.WriteLine(": " + message);
            }
        }
    }
}