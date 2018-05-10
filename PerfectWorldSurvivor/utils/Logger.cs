using System;

namespace PerfectWorldSurvivor.Utils
{
    public class Logger
    {
        public static void Log(string logInfo)
        {
            Console.Write(logInfo + "\n");
        }

        public static void Error(string errorInfo)
        {
            Console.Write(errorInfo + "\n");
        }
    }
}
