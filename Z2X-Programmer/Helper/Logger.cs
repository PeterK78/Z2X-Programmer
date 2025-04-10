/*

Z2X-Programmer
Copyright (C) 2025
PeterK78

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see:

https://github.com/PeterK78/Z2X-Programmer?tab=GPL-3.0-1-ov-file.

*/

using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Z2XProgrammer.DataModel;

namespace Z2XProgrammer.Helper
{

    /// <summary>
    /// Contains the implementation of the MetroLog package.
    /// </summary>
    internal static class Logger
    {

        public static ILogger? _logger;

        private static ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddStreamingFileLogger(options =>
        {
            options.RetainDays = 2;
            options.FolderPath = Path.Combine(FileSystem.CacheDirectory, "Z2XProgrammerLogs");
        }));

        public static ILogger Init(string categoryName)
        {
            _logger = loggerFactory.CreateLogger(categoryName);
            return _logger;
        }

        public static void LogInformation(string information)
        {
            if (Preferences.Default.Get(AppConstants.PREFERENCES_LOGGING_KEY, AppConstants.PREFERENCES_LOGGING_DEFAULT) == "0") return;
            if (_logger == null) { return; }
            _logger.LogInformation(information);
        }

        public static void LogCritical(string information)
        {
            if (Preferences.Default.Get(AppConstants.PREFERENCES_LOGGING_KEY, AppConstants.PREFERENCES_LOGGING_DEFAULT) == "0") return;
            if (_logger == null) { return; }
            _logger.LogCritical(information);
        }

        public static void PrintDevConsole(string text)
        {
            string DebugMessage = DateTime.Now.ToString() + " Z2X-Programmer: " + text;
            Debug.WriteLine(DebugMessage);
        }

    }
}
