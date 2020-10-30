using Microsoft.Extensions.Logging;
using System;

namespace RailEmu.Core.Providers
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly LogLevel minLogLevel;

        public void Dispose() { }

        public LoggerProvider(LogLevel minLogLevel)
        {
            this.minLogLevel = minLogLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomConsoleLogger(categoryName, minLogLevel);
        }

        public class CustomConsoleLogger : ILogger
        {
            public static string LastLoggerUsed;

            private readonly string _categoryName;
            private readonly LogLevel minLogLevel;
            private readonly string _categoryNameMin;

            public CustomConsoleLogger(string categoryName, LogLevel minLogLevel)
            {
                _categoryName = categoryName;
                this.minLogLevel = minLogLevel;
                _categoryNameMin = GetCategoryNameMin();
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                if(LastLoggerUsed != _categoryName)
                {
                    LastLoggerUsed = _categoryName;
                    string category = IsEnabled(LogLevel.Debug) ? _categoryName : _categoryNameMin;
                    Console.WriteLine($"{category}[{eventId.Id}]:");
                }
                SetLogLevelConsoleColors(logLevel);
                Console.WriteLine($"\t{GetLogLevelPrefix(logLevel)}: {formatter(state, exception)}");

                SetConsoleColors(default);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return (int)logLevel >= (int)minLogLevel;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            private string GetCategoryNameMin()
            {
                return _categoryName[(_categoryName.LastIndexOf('.') + 1)..];
            }

            private void SetLogLevelConsoleColors(LogLevel logLevel)
            {
                ConsoleColors colors =
                logLevel switch
                {
                    LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.Red),
                    LogLevel.Error => new ConsoleColors(ConsoleColor.Red, ConsoleColor.Black),
                    LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black),
                    LogLevel.Information => new ConsoleColors(ConsoleColor.Green, ConsoleColor.Black),
                    LogLevel.Debug => new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black),
                    LogLevel.Trace => new ConsoleColors(ConsoleColor.DarkGray, ConsoleColor.Black),
                    _ => default
                };
                SetConsoleColors(colors);
            }

            private string GetLogLevelPrefix(LogLevel logLevel) =>
                logLevel switch
                {
                    LogLevel.Trace => "trace",
                    LogLevel.Debug => "debug",
                    LogLevel.Information => "infos",
                    LogLevel.Warning => " warn",
                    LogLevel.Error => "error",
                    LogLevel.Critical => "fatal",
                    LogLevel.None => " none",
                    _ => "     "
                };

            private void SetConsoleColors(ConsoleColors consoleColors = default)
            {
                Console.ForegroundColor = consoleColors.Foreground;
                Console.BackgroundColor = consoleColors.Background;
            }

            private struct ConsoleColors
            {
                private ConsoleColor? _foreground;
                public ConsoleColor Foreground 
                { 
                    get => _foreground?? ConsoleColor.Gray; 
                    set => _foreground = value; 
                }
                private ConsoleColor? _background;
                public ConsoleColor Background 
                { 
                    get => _background ?? ConsoleColor.Black; 
                    set => _background = value; 
                }

                public ConsoleColors(ConsoleColor foreground, ConsoleColor background) : this()
                {
                    Foreground = foreground;
                    Background = background;
                }
            }
        }
    }
}
