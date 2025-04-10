﻿using Microsoft.Extensions.Logging;

namespace Application.Logging
{
    //This interface and the class implementing it is a wrapper to facilitate unit testing the ILogger
    //It removes the problems created by all the extension methods in the ILogger interface, which make it hard to mock and test
    public interface ILoggerAdapter<TType>
    {
        void Log(LogLevel logLevel, EventId eventId, Exception? exception, string? message, params object?[] args);
        void Log(LogLevel logLevel, EventId eventId, string? message, params object?[] args);
        void Log(LogLevel logLevel, Exception? exception, string? message, params object?[] args);
        void Log(LogLevel logLevel, string? message, params object?[] args);
        void LogCritical(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogCritical(EventId eventId, string? message, params object?[] args);
        void LogCritical(Exception? exception, string? message, params object?[] args);
        void LogCritical(string? message, params object?[] args);
        void LogDebug(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogDebug(EventId eventId, string? message, params object?[] args);
        void LogDebug(Exception? exception, string? message, params object?[] args);
        void LogDebug(string? message, params object?[] args);
        void LogError(string? message, params object?[] args);
        void LogError(Exception? exception, string? message, params object?[] args);
        void LogError(EventId eventId, string? message, params object?[] args);
        void LogError(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogInformation(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogInformation(EventId eventId, string? message, params object?[] args);
        void LogInformation(Exception? exception, string? message, params object?[] args);
        void LogInformation(string? message, params object?[] args);
        void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogTrace(EventId eventId, string? message, params object?[] args);
        void LogTrace(Exception? exception, string? message, params object?[] args);
        void LogTrace(string? message, params object?[] args);
        void LogWarning(EventId eventId, Exception? exception, string? message, params object?[] args);
        void LogWarning(EventId eventId, string? message, params object?[] args);
        void LogWarning(Exception? exception, string? message, params object?[] args);
        void LogWarning(string? message, params object?[] args);
    }
}
