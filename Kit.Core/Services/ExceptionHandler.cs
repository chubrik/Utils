﻿using System;
using System.Collections.Generic;

namespace Kit {
    public class ExceptionHandler {

        private ExceptionHandler() { }

        private static int _counter = 0;

        public static readonly List<IDataClient> DataClients = new List<IDataClient> {
            FileClient.Instance
        };

        public static void Register(Exception exception, LogLevel level = LogLevel.Error) {

            lock (DataClients) {

                if (exception.Data.Contains("registered"))
                    return;

                exception.Data["registered"] = true;
            }

            var startTime = DateTimeOffset.Now;

            if (exception.IsCanceled())
                level = LogLevel.Log;

            var message = ExceptionHelper.OneLineMessageWithPlace(exception);
            var count = ++_counter;
            var logLabel = $"Exception #{count}";
            LogService.Log($"{logLabel}: {message}", level);
            var text = $"{logLabel}\n{message}\n\n\nDUMP:\n\n{ExceptionHelper.FullDump(exception)}";
            var fileName = PathHelper.SafeFileName($"{count.ToString().PadLeft(3, '0')} {message}.txt");

            foreach (var client in DataClients)
                client.PushToWrite(fileName, text, Kit.DiagnisticsCurrentDirectory);

            LogService.Log($"{logLabel} registered at {TimeHelper.FormattedLatency(startTime)}");
        }
    }
}
