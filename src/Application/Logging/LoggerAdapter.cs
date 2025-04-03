using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Logging
{
    //This class and the interface it implements it is a wrapper to facilitate unit testing the ILogger
    //It removes the problems created by all the extension methods in the ILogger interface, which make it hard to mock and test
    [ExcludeFromCodeCoverage]
    public class LoggerAdapter<TType> : ILoggerAdapter<TType>
    {
        private readonly ILogger<TType> _logger;
        public LoggerAdapter(ILogger<TType> logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel logLevel, EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.Log(logLevel, eventId, exception, message, args);
        }

        public void Log(LogLevel logLevel, EventId eventId, string? message, params object?[] args)
        {
            _logger.Log(logLevel, eventId, message, args);
        }

        public void Log(LogLevel logLevel, Exception? exception, string? message, params object?[] args)
        {
            _logger.Log(logLevel, exception, message, args);
        }

        public void Log(LogLevel logLevel, string? message, params object?[] args)
        {
            _logger.Log(logLevel, message, args);
        }

        public void LogCritical(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogCritical(eventId, exception, message, args);
        }

        public void LogCritical(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogCritical(eventId, message, args);
        }

        public void LogCritical(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogCritical(exception, message, args);
        }

        public void LogCritical(string? message, params object?[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogDebug(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogDebug(eventId, exception, message, args);
        }

        public void LogDebug(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogDebug(eventId, message, args);
        }

        public void LogDebug(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogDebug(exception, message, args);
        }

        public void LogDebug(string? message, params object?[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void LogError(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogError(eventId, message, args);
        }

        public void LogError(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogError(eventId, exception, message, args);
        }

        public void LogInformation(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogInformation(eventId, exception, message, args);
        }

        public void LogInformation(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogInformation(eventId, message, args);
        }

        public void LogInformation(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogTrace(eventId, exception, message, args);
        }

        public void LogTrace(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogTrace(eventId, message, args);
        }

        public void LogTrace(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogTrace(exception, message, args);
        }

        public void LogTrace(string? message, params object?[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogWarning(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            _logger.LogWarning(eventId, exception, message, args);
        }

        public void LogWarning(EventId eventId, string? message, params object?[] args)
        {
            _logger.LogWarning(eventId, message, args);
        }

        public void LogWarning(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void LogWarning(string? message, params object?[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
