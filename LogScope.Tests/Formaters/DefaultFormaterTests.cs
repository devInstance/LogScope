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
            var formater = new DefaultFormater(false);
            Assert.NotNull(formater);
        }

        [Theory]
        [InlineData(false, "scope", "message", "\tscope:message")]
        [InlineData(false, "", "message", "\tmessage")]
        [InlineData(false, null, "message", "\tmessage")]
        [InlineData(false, "scope", "", "\tscope:")]
        [InlineData(false, "scope", null, "\tscope:")]
        [InlineData(false, "", "", "\t")]
        [InlineData(false, null, null, "\t")]
        public void ConstructorFormatLine(bool timestamp, string scope, string message, string expected)
        {
            var formater = new DefaultFormater(false);
            var result = formater.FormatLine(scope, message);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("scope", "message", "scope:message")]
        [InlineData("", "message", ":message")]
        public void FormatNestedScopesTest(string scope, string schildScope, string expected)
        {
            var formater = new DefaultFormater(false);
            var result = formater.FormatNestedScopes(scope, schildScope);
            Assert.Equal(expected, result);
        }
    }
}