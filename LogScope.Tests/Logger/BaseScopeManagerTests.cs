using DevInstance.LogScope.Formatters;
using Moq;
using System;
using Xunit;

namespace DevInstance.LogScope.Logger.Tests
{
    public class BaseScopeManagerTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var scopeLevel = LogLevel.ERROR;
            var provider = new Mock<ILogProvider>();
            var formater = new DefaultFormatter(null);

            var manager = new DefaultScopeManager(scopeLevel, provider.Object, formater);

            Assert.Equal(scopeLevel, manager.BaseLevel);
        }

        [Fact()]
        public void CreateLoggerTest()
        {
            var scopeLevel = LogLevel.ERROR;
            var name = "testscope";
            var provider = new Mock<ILogProvider>();
            var formater = new DefaultFormatter(null);

            var manager = new DefaultScopeManager(scopeLevel, provider.Object, formater);
            var scope = manager.CreateLogger(name);

            Assert.Equal(name, scope.Name);

            scope = manager.CreateLogger(null);
            Assert.Null(scope.Name);
        }
    }
}