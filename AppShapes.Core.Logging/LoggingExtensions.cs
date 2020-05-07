using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug<T>(this ILogger logger, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogDebug($"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Debug, memberName) {Message = message});
        }

        public static void Debug<T>(this ILogger logger, Exception exception, string message = null, [CallerMemberName] string memberName = null)
        {
            logger.LogDebug(exception, $"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Debug, memberName) {Exception = $"{exception}", Message = message});
        }

        public static void Error<T>(this ILogger logger, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogError($"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Error, memberName) {Message = message});
        }

        public static void Error<T>(this ILogger logger, Exception exception, string message = null, [CallerMemberName] string memberName = null)
        {
            logger.LogError(exception, $"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Error, memberName) {Exception = $"{exception}", Message = message});
        }

        public static void Information<T>(this ILogger logger, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogInformation($"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Information, memberName) {Message = message});
        }

        public static void Information<T>(this ILogger logger, Exception exception, string message = null, [CallerMemberName] string memberName = null)
        {
            logger.LogInformation(exception, $"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Information, memberName) {Exception = $"{exception}", Message = message});
        }

        public static void Trace<T>(this ILogger logger, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogTrace($"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Trace, memberName) {Message = message});
        }

        public static void Trace<T>(this ILogger logger, Exception exception, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogTrace(exception, $"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Trace, memberName) {Exception = $"{exception}", Message = message});
        }

        public static void Warning<T>(this ILogger logger, string message, [CallerMemberName] string memberName = null)
        {
            logger.LogWarning($"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Warning, memberName) {Message = message});
        }

        public static void Warning<T>(this ILogger logger, Exception exception, string message = null, [CallerMemberName] string memberName = null)
        {
            logger.LogWarning(exception, $"{message} ({{context}})", new LoggingContext(typeof(T).FullName, LogLevel.Warning, memberName) {Exception = $"{exception}", Message = message});
        }
    }
}