using Xunit;
using DevInstance.LogScope.Formaters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Formaters.Tests
{
    public class DefaultFormaterTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var formater = new DefaultFormater(null);
            Assert.NotNull(formater);
        }

        [Theory]
        [InlineData(false, false, "scope", "message", "\tscope:message")]
        [InlineData(false, false, "", "message", "\tmessage")]
        [InlineData(false, false, null, "message", "\tmessage")]
        [InlineData(false, false, "scope", "", "\tscope:")]
        [InlineData(false, false, "scope", null, "\tscope:")]
        [InlineData(false, false, "", "", "\t")]
        [InlineData(false, false, null, null, "\t")]
        public void ConstructorFormatLine(bool threadnumber, bool timestamp, string scope, string message, string expected)
        {
            var formater = new DefaultFormater(new DefaultFormaterOptions { ShowThreadNumber = threadnumber, ShowTimestamp = timestamp });
            var result = formater.FormatLine(scope, message);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("scope", "message", "scope:message")]
        [InlineData("", "message", ":message")]
        public void FormatNestedScopesTest(string scope, string schildScope, string expected)
        {
            var formater = new DefaultFormater(null);
            var result = formater.FormatNestedScopes(scope, schildScope);
            Assert.Equal(expected, result);
        }
    }
}