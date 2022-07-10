using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace WeatherAPI.Application.Tests;

public static class LoggerMockExtensions
{
    public static void VerifyAtLeastOneLogMessagesContains<T>(this Mock<ILogger<T>> loggerMock, string message)
    {
        var messageUpper = message.ToUpper();
        loggerMock.Verify
        (
            x => x.Log
            (
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((object v, Type _) => v.ToString().ToUpper().Contains(messageUpper)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            ),
            Times.AtLeastOnce
        );
    }
}
