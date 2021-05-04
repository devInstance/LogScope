using Xunit;
using DevInstance.LogScope.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Formaters.Tests
{
    public class DefaultFormatterTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var formater = new DefaultFormatter(null);
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
            var formater = new DefaultFormatter(new DefaultFormattersOptions { ShowThreadNumber = threadnumber, ShowTimestamp = timestamp });
            var result = formater.FormatLine(scope, message);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("scope", "message", "scope:message")]
        [InlineData("", "message", ":message")]
        public void FormatNestedScopesTest(string scope, string schildScope, string expected)
        {
            var formater = new DefaultFormatter(null);
            var result = formater.FormatNestedScopes(scope, schildScope);
            Assert.Equal(expected, result);
        }
    }
}