using Xunit;
using System;
using Moq;

namespace DevInstance.LogScope.Logger.Tests
{
    public class BaseScopeLogTests
    {
        [Theory]
        [InlineData(true, LogLevel.TRACE, LogLevel.DEBUG, "test1", true)]
        [InlineData(true, LogLevel.TRACE, LogLevel.TRACE, "test2", true)]
        [InlineData(true, LogLevel.DEBUG, LogLevel.TRACE, "test3", false)]
        [InlineData(true, LogLevel.INFO, LogLevel.DEBUG, "test-test", false)]
        [InlineData(true, LogLevel.NOLOG, LogLevel.INFO, "test1", false)]
        [InlineData(false, LogLevel.INFO, LogLevel.DEBUG, "test1", false)]
        [InlineData(false, LogLevel.DEBUG, LogLevel.INFO, "test1", false)]
        public void ConstructorTest(bool logConstructor, LogLevel mainLevel, LogLevel scopeLevel, string name, bool result)
        {
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(mainLevel);
            var provider = new Mock<ILogProvider>();
            var formater = new Mock<IScopeFormatter>();

            DefaultScopeLog scopeLog = new DefaultScopeLog(manager.Object, formater.Object, provider.Object, scopeLevel, name, logConstructor);

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

            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(null, formater.Object, provider.Object, LogLevel.INFO, "test", false));
            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(manager.Object, null, provider.Object, LogLevel.INFO, "test", false));
            Assert.Throws<ArgumentNullException>(() => new DefaultScopeLog(manager.Object, formater.Object, null, LogLevel.INFO, "test", false));
        }

        [Theory]
        [InlineData("parent:child", "parent", "child")]
        [InlineData("child", null, "child")]
        public void ScopeNameTest(string expectedResult, string inputParentName, string inputChildName)
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(LogLevel.DEBUG);
            var formater = new Mock<IScopeFormatter>();
            
            formater.Setup(x => x.FormatNestedScopes(It.Is<string>(v=> v == inputParentName), It.Is<string>(v => v == inputChildName))).Returns(expectedResult);

            var scopeLog = new DefaultScopeLog(manager.Object, formater.Object, provider.Object, LogLevel.DEBUG, inputParentName, false);

            var scope = scopeLog.Scope(LogLevel.DEBUG, inputChildName);

            Assert.Equal(expectedResult, scope.Name);
        }


        [Theory]
        [InlineData(false, false, false, LogLevel.DEBUG)]
        [InlineData(true, false, false, LogLevel.DEBUG)]
        [InlineData(true, true, true, LogLevel.ERROR)]
        [InlineData(false, false, true, LogLevel.ERROR)]
        public void DisposeTest(bool logConstructor, bool expectStartScope, bool expectEndScope, LogLevel scopeLevel)
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormatter>();
            formater.Setup(x => x.ScopeStart(It.IsAny<DateTime>(), It.IsAny<string>())).Returns("startscope");
            formater.Setup(x => x.ScopeEnd(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns("endscope");

            var scopeLog = new DefaultScopeLog(manager.Object, formater.Object, provider.Object, scopeLevel, "test", logConstructor);

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
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormatter>();
            formater.Setup(x => x.FormatLine(It.Is<string>(v => v == "test"), It.Is<string>(v => v.Contains(message)))).Returns(message);

            var scopeLog = new DefaultScopeLog(manager.Object, formater.Object, provider.Object, scopeLevel, "test", false);
            scopeLog.Line(scopeLevel, message);

            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == scopeLevel), It.Is<string>(v => v.Contains(message))), expectWrite ? Times.Once() : Times.Never());
        }

        [Fact()]
        public void LineTestArgumentNullExceptionTest()
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.BaseLevel).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormatter>();

            var scopeLog = new DefaultScopeLog(manager.Object, formater.Object, provider.Object, LogLevel.INFO, "test", false);
            Assert.Throws<ArgumentNullException>(() => scopeLog.Line(LogLevel.INFO, null));

            provider.Verify(x => x.WriteLine(It.Is<LogLevel>(v => v == LogLevel.INFO), It.IsAny<string>()), Times.Never());
        }
    }
}