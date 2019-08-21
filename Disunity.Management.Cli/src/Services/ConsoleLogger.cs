using System;
using System.IO;

using Disunity.Core;


namespace Disunity.Management.Cli.Services {

    public class ConsoleLogger: LoggerBase{

        public override void Log(LogLevel level, string message) {
            TextWriter writer;

            switch (level) {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    writer = Console.Error;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    writer = Console.Out;
                    break;
                
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    writer = Console.Out;
                    break;
            }
            
            writer.WriteLine(message);
        }
        
    }

}