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
        [Fact()]
        public void ContructorTest()
        {
            var manager = new Mock<IScopeManager>();
            var formater = new Mock<IScopeFormater>();

            BaseScopeLog scopeLog = new BaseScopeLog(manager.Object, formater.Object, LogLevel.DEBUG, "test", false);

            Assert.True(false, "This test needs an implementation");
        }
    }
}