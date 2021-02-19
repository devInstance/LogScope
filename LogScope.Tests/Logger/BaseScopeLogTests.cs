using Xunit;
using DevInstance.LogScope.Logger;
using System;
using System.Collections.Generic;
using System.Text;
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
        public void ContructorTest(bool logConstructor, LogLevel mainLevel, LogLevel scopeLevel, string name, bool result)
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.Provider).Returns(provider.Object);
            manager.Setup(x => x.Level).Returns(mainLevel);
            var formater = new Mock<IScopeFormater>();

            BaseScopeLog scopeLog = new BaseScopeLog(manager.Object, formater.Object, scopeLevel, name, logConstructor);

            provider.Verify(x => x.WriteLine(It.IsAny<string>()), result ? Times.Once() : Times.Never());

            Assert.Equal(name, scopeLog.Name);
            Assert.Equal(scopeLevel, scopeLog.ScopeLevel);
        }

        [Fact()]
        public void ContructorArgumentNullExceptionTest()
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.Provider).Returns(provider.Object);
            var formater = new Mock<IScopeFormater>();

            Assert.Throws<ArgumentNullException>(() => new BaseScopeLog(null, formater.Object, LogLevel.INFO, "test", false));
            Assert.Throws<ArgumentNullException>(() => new BaseScopeLog(manager.Object, null, LogLevel.INFO, "test", false));
        }

        [Theory]
        [InlineData("parent:child", "parent", "child")]
        [InlineData("child", null, "child")]
        public void ScopeNameTest(string expectedResult, string inputParentName, string inputChildName)
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.Provider).Returns(provider.Object);
            manager.Setup(x => x.Level).Returns(LogLevel.DEBUG);
            var formater = new Mock<IScopeFormater>();
            
            formater.Setup(x => x.FormatNestedScopes(It.Is<string>(v=> v == inputParentName), It.Is<string>(v => v == inputChildName))).Returns(expectedResult);

            var scopeLog = new BaseScopeLog(manager.Object, formater.Object, LogLevel.DEBUG, inputParentName, false);

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
            manager.Setup(x => x.Provider).Returns(provider.Object);
            manager.Setup(x => x.Level).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormater>();
            formater.Setup(x => x.ScopeStart(It.IsAny<DateTime>(), It.IsAny<string>())).Returns("startscope");
            formater.Setup(x => x.ScopeEnd(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<TimeSpan>())).Returns("endscope");

            var scopeLog = new BaseScopeLog(manager.Object, formater.Object, scopeLevel, "test", logConstructor);

            scopeLog.Dispose();

            provider.Verify(x => x.WriteLine(It.Is<string>(v=> v == "startscope")), expectStartScope ? Times.Once() : Times.Never());
            provider.Verify(x => x.WriteLine(It.Is<string>(v => v == "endscope")), expectEndScope ? Times.Once() : Times.Never());
        }


        [Theory]
        [InlineData(LogLevel.ERROR, "test message", true)]
        [InlineData(LogLevel.INFO, "test message", true)]
        [InlineData(LogLevel.DEBUG, "test message", false)]
        public void LineTest(LogLevel scopeLevel, string message, bool expectWrite)
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.Provider).Returns(provider.Object);
            manager.Setup(x => x.Level).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormater>();
            formater.Setup(x => x.FormatLine(It.Is<string>(v => v == "test"), It.Is<string>(v => v.Contains(message)))).Returns(message);

            var scopeLog = new BaseScopeLog(manager.Object, formater.Object, scopeLevel, "test", false);
            scopeLog.Line(scopeLevel, message);

            provider.Verify(x => x.WriteLine(It.Is<string>(v => v.Contains(message))), expectWrite ? Times.Once() : Times.Never());
        }

        [Fact()]
        public void LineTestArgumentNullExceptionTest()
        {
            var provider = new Mock<ILogProvider>();
            var manager = new Mock<IScopeManager>();
            manager.Setup(x => x.Provider).Returns(provider.Object);
            manager.Setup(x => x.Level).Returns(LogLevel.INFO);
            var formater = new Mock<IScopeFormater>();

            var scopeLog = new BaseScopeLog(manager.Object, formater.Object, LogLevel.INFO, "test", false);
            Assert.Throws<ArgumentNullException>(() => scopeLog.Line(LogLevel.INFO, null));

            provider.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never());
        }
    }
}