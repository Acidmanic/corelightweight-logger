using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.LightWeight
{
    public class LoggerAdapter : ILogger
    {
        private readonly Dictionary<LogLevel, bool> _enabledLevels;
        private readonly Dictionary<LogLevel, Action<LogLevel, string>> _actionsByLevel;

        public LoggerAdapter(Action<string> logAction) : this((lname, msg) => logAction(lname.ToString() + ": " + msg))
        {
        }

        public LoggerAdapter(Action<LogLevel, string> logAction) : this(logAction, logAction, logAction)
        {
        }

        public LoggerAdapter(
            Action<LogLevel, string> goodLogAction,
            Action<LogLevel, string> badLogAction,
            Action<LogLevel, string> defaultLogAction) : this(
            goodLogAction,
            badLogAction,
            defaultLogAction,
            defaultLogAction)
        {
        }

        public LoggerAdapter(
            Action<LogLevel, string> goodLogAction,
            Action<LogLevel, string> badLogAction,
            Action<LogLevel, string> uglyAction,
            Action<LogLevel, string> defaultLogAction)
        {
            _actionsByLevel = new Dictionary<LogLevel, Action<LogLevel, string>>();

            _actionsByLevel.Add(LogLevel.Critical, badLogAction);
            _actionsByLevel.Add(LogLevel.Debug, defaultLogAction);
            _actionsByLevel.Add(LogLevel.Error, badLogAction);
            _actionsByLevel.Add(LogLevel.Information, goodLogAction);
            _actionsByLevel.Add(LogLevel.None, defaultLogAction);
            _actionsByLevel.Add(LogLevel.Trace, uglyAction);
            _actionsByLevel.Add(LogLevel.Warning, uglyAction);

            _enabledLevels = new Dictionary<LogLevel, bool>();

            _enabledLevels.Add(LogLevel.Critical, true);
            _enabledLevels.Add(LogLevel.Debug, false);
            _enabledLevels.Add(LogLevel.Error, true);
            _enabledLevels.Add(LogLevel.Information, true);
            _enabledLevels.Add(LogLevel.None, false);
            _enabledLevels.Add(LogLevel.Trace, false);
            _enabledLevels.Add(LogLevel.Warning, true);
        }


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (_enabledLevels[logLevel])
            {
                var message = formatter(state, exception);

                _actionsByLevel[logLevel](logLevel, message);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _enabledLevels[logLevel];
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public LoggerAdapter Enable(LogLevel level)
        {
            _enabledLevels[LogLevel.Critical] = true;
            _enabledLevels[LogLevel.Debug] = false;
            _enabledLevels[LogLevel.Error] = true;
            _enabledLevels[LogLevel.Information] = true;
            _enabledLevels[LogLevel.None] = false;
            _enabledLevels[LogLevel.Trace] = false;
            _enabledLevels[LogLevel.Warning] = true;

            return this;
        }

        private void Enable(bool enable)
        {
            _enabledLevels[LogLevel.Critical] = enable;
            _enabledLevels[LogLevel.Debug] = enable;
            _enabledLevels[LogLevel.Error] = enable;
            _enabledLevels[LogLevel.Information] = enable;
            _enabledLevels[LogLevel.None] = enable;
            _enabledLevels[LogLevel.Trace] = enable;
            _enabledLevels[LogLevel.Warning] = enable;
        }

        public LoggerAdapter Disable(LogLevel level)
        {
            _enabledLevels[level] = false;

            return this;
        }

        public LoggerAdapter EnableAll()
        {
            Enable(true);

            return this;
        }

        public LoggerAdapter DisableAll()
        {
            Enable(false);

            return this;
        }
    }
}