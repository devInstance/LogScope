using Xunit;
using DevInstance.LogScope.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using DevInstance.LogScope.Formaters;

namespace DevInstance.LogScope.Logger.Tests
{
    public class BaseScopeManagerTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var scopeLevel = LogLevel.ERROR;
            var provider = new Mock<ILogProvider>();
            var formater = new DefaultFormater(null);

            var manager = new BaseScopeManager(scopeLevel, provider.Object, formater);

            Assert.Equal(scopeLevel, manager.Level);
        }

        [Fact()]
        public void CreateLoggerTest()
        {
            var scopeLevel = LogLevel.ERROR;
            var name = "testscope";
            var provider = new Mock<ILogProvider>();
            var formater = new DefaultFormater(null);

            var manager = new BaseScopeManager(scopeLevel, provider.Object, formater);
            var scope = manager.CreateLogger(name);

            Assert.Equal(name, scope.Name);

            scope = manager.CreateLogger(null);
            Assert.Null(scope.Name);
        }
    }
}