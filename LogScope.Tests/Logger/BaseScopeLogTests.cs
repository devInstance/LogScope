using Xunit;
using System;
using Moq;

namespace DevInstance.LogScope.Logger.Tests
{
    public class BaseScopeLogTests
    {
        [Theory]
        [InlineData(true, LogLevel.TRACE, LogLevel.TRACE, LogLevel.DEBUG, "test1", true)]
        [InlineData(true, LogLevel.TRACE, LogLevel.TRACE, LogLevel.TRACE, "test2", true)]
        [InlineData(true, LogLevel.DEBUG, LogLevel.DEBUG, LogLevel.TRACE, "test3", false)]
        [InlineData(true, LogLevel.INFO, LogLevel.INFO, LogLevel.DEBUG, "test-test", false)]
        [InlineData(true, LogLevel.NOLOG, LogLevel.NOLOG, LogLevel.INFO, "test1", false)]
        [InlineData(false, LogLevel.INFO, LogLevel.INFO, LogLevel.DEBUG, "test1", false)]
        [InlineData(false, LogLevel.DEBUG, LogLevel.DEBUG, LogLevel.INFO, "test1", false)]
        [InlineData(true, LogLevel.NOLOG, LogLevel.INFO, LogLevel.INFO, "test1", true)]
        [InlineData(true, LogLevel.INFO, LogLevel.TRACE, LogLevel.DEBUG, "test1", true)]
        [InlineData(true, LogLevel.DEBUG, LogLevel.NOLOG, LogLevel.INFO, "test1", false)]
        public void ConstructorTest(bool logConstructor, LogLevel baseLevel, LogLevel overideLevel, LogLevel scopeLevel, string name, bool result)
        {
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(baseLevel);
            var provider = new Mock<ILogProvider>();
            var formater = new Mock<IScopeFormatter>();

            DefaultScopeLog scopeLog = new DefaultScopeLog(manager.Object, overideLevel, formater.Object, provider.Object, scopeLevel, name, logConstructor);

            provider.Verify(x => x.WriteLine(It.IsAny<LogLevel>(), It.IsAny<string>()), result ? Times.Once() : Times.Never());

            Assert.Equal(name, scopeLog.Name);
            Assert.Equal(scopeLevel, scopeLog.ScopeLevel);
        }

        [Fact()]
        public void ConstructorArgumentNullExceptionTest()
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            var formater = new Mock<IScopeFormatter>();

            var logLevel = LogLevel.INFO;

            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(null, logLevel, formater.Object, provider.Object, logLevel, "test", false));
            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(manager.Object, logLevel, null, provider.Object, logLevel, "test", false));
            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(manager.Object, logLevel, formater.Object, null, logLevel, "test", false));
        }

        [Theory]
        [InlineData("parent:child", "parent", "child")]
        [InlineData("child", null, "child")]
        public void ScopeNameTest(string expectedResult, string inputParentName, string inputChildName)
        {
            var logLevel = LogLevel.DEBUG;
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(logLevel);
            var formater = new Mock<IScopeFormatter>();
            
            formater.Setup(x => x.FormatNestedScopes(It.Is<string>(v=> v == inputParentName), It.Is<string>(v => v == inputChildName))).Returns(expectedResult);

            var scopeLog = new DefaultScopeLog(manager.Object, logLevel, formater.Object, provider.Object, logLevel, inputParentName, false);

            var scope = scopeLog.Scope(logLevel, inputChildName);

            Assert.Equal(expectedResult, scope.Name);
        }


        [Theory]
        [InlineData(false, false, false, LogLevel.DEBUG, LogLevel.INFO)]
        [InlineData(true, false, false, LogLevel.DEBUG, LogLevel.INFO)]
        [InlineData(true, true, true, LogLevel.ERROR, LogLevel.INFO)]
        [InlineData(false, false, false, LogLevel.DEBUG, LogLevel.ERROR)]
        [InlineData(true, true, true, LogLevel.ERROR, LogLevel.DEBUG)]
        public void DisposeTest(bool logConstructor, bool expectStartScope, bool expectEndScope, LogLevel scopeLevel, LogLevel overideLevel)
        {
            var logLevel = LogLevel.INFO;
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(logLevel);
            var formater = new Mock<IScopeFormatter>();
            formater.Setup(x => x.ScopeStart(It.IsAny<DateTime>(), It.IsAny<string>())).Returns("startscope");
            formater.Setup(x => x.ScopeEnd(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns("endscope");

            var scopeLog = new DefaultScopeLog(manager.Object, overideLevel, formater.Object, provider.Object, scopeLevel, "test", logConstructor);

            scopeLog.Dispose();

            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == scopeLevel), It.Is<string>(v => v == "startscope")), expectStartScope ? Times.Once() : Times.Never());
            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == scopeLevel), It.Is<string>(v => v == "endscope")), expectEndScope ? Times.Once() : Times.Never());
        }


        [Theory]
        [InlineData(LogLevel.ERROR, "test message", true)]
        [InlineData(LogLevel.INFO, "test message", true)]
        [InlineData(LogLevel.DEBUG, "test message", false)]
        public void LineTest(LogLevel scopeLevel, string message, bool expectWrite)
        {
            var logLevel = LogLevel.INFO;
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(logLevel);
            var formater = new Mock<IScopeFormatter>();
            formater.Setup(x => x.FormatLine(It.Is<string>(v => v == "test"), It.Is<string>(v => v.Contains(message)))).Returns(message);

            var scopeLog = new DefaultScopeLog(manager.Object, logLevel, formater.Object, provider.Object, scopeLevel, "test", false);
            scopeLog.Line(scopeLevel, message);

            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == scopeLevel), It.Is<string>(v => v.Contains(message))), expectWrite ? Times.Once() : Times.Never());
        }

        [Fact()]
        public void LineTestArgumentNullExceptionTest()
        {
            var logLevel = LogLevel.INFO;
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(logLevel);
            var formater = new Mock<IScopeFormatter>();

            var scopeLog = new DefaultScopeLog(manager.Object, logLevel, formater.Object, provider.Object, logLevel, "test", false);
            Assert.Throws<ArgumentNullException>(() => scopeLog.Line(logLevel, null));

            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == logLevel), It.IsAny<string>()), Times.Never());
        }
    }
}