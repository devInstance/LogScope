using Xunit;
using System;
using System.Collections.Generic;
using Moq;
using DevInstance.LogScope.Templates;

namespace DevInstance.LogScope.Templates.Tests
{
    public class MessageTemplateTests
    {
        #region Basic Rendering

        [Theory]
        [InlineData("Hello World", "Hello World")]
        [InlineData("", "")]
        [InlineData("No placeholders here", "No placeholders here")]
        public void Render_NoPlaceholders_ReturnsTemplate(string template, string expected)
        {
            var result = MessageTemplate.Render(template, Array.Empty<object>());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Render_NullTemplate_ReturnsEmpty()
        {
            var result = MessageTemplate.Render(null, new object[] { "arg" });
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Render_NullArgs_ReturnsTemplate()
        {
            var result = MessageTemplate.Render("Hello {Name}", null);
            Assert.Equal("Hello {Name}", result);
        }

        [Fact]
        public void Render_SinglePlaceholder_ReplacesValue()
        {
            var result = MessageTemplate.Render("Hello {Name}", new object[] { "World" });
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Render_MultiplePlaceholders_ReplacesInOrder()
        {
            var result = MessageTemplate.Render("{First} and {Second}", new object[] { "A", "B" });
            Assert.Equal("A and B", result);
        }

        [Fact]
        public void Render_IntArgument_ConvertsToString()
        {
            var result = MessageTemplate.Render("Count: {Count}", new object[] { 42 });
            Assert.Equal("Count: 42", result);
        }

        #endregion

        #region Format Specifiers

        [Fact]
        public void Render_FormatSpecifier_AppliesFormat()
        {
            var result = MessageTemplate.Render("Elapsed: {Elapsed:000} ms", new object[] { 34 });
            Assert.Equal("Elapsed: 034 ms", result);
        }

        [Fact]
        public void Render_DateFormat_AppliesFormat()
        {
            var date = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var result = MessageTemplate.Render("Date: {Date:yyyy-MM-dd}", new object[] { date });
            Assert.Equal("Date: 2026-01-15", result);
        }

        [Fact]
        public void Render_DecimalFormat_AppliesFormat()
        {
            var result = MessageTemplate.Render("Price: {Price:F2}", new object[] { 19.9m });
            Assert.Equal("Price: 19.90", result);
        }

        [Fact]
        public void Render_FormatOnNonFormattable_FallsBackToToString()
        {
            var result = MessageTemplate.Render("Value: {Val:X}", new object[] { "notformattable" });
            Assert.Equal("Value: notformattable", result);
        }

        #endregion

        #region Destructuring

        [Fact]
        public void Render_Destructure_Object_ShowsProperties()
        {
            var pos = new { Latitude = 25, Longitude = 134 };
            var result = MessageTemplate.Render("Position: {@Pos}", new object[] { pos });
            Assert.Contains("Latitude: 25", result);
            Assert.Contains("Longitude: 134", result);
            Assert.StartsWith("Position: { ", result);
            Assert.EndsWith(" }", result);
        }

        [Fact]
        public void Render_Destructure_List_ShowsArray()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = MessageTemplate.Render("Items: {@Items}", new object[] { list });
            Assert.Equal("Items: [1, 2, 3]", result);
        }

        [Fact]
        public void Render_Destructure_Array_ShowsArray()
        {
            var arr = new[] { "a", "b", "c" };
            var result = MessageTemplate.Render("Tags: {@Tags}", new object[] { arr });
            Assert.Equal("Tags: [a, b, c]", result);
        }

        [Fact]
        public void Render_Destructure_String_PassesThrough()
        {
            var result = MessageTemplate.Render("Name: {@Name}", new object[] { "Alice" });
            Assert.Equal("Name: Alice", result);
        }

        [Fact]
        public void Render_Destructure_Primitive_PassesThrough()
        {
            var result = MessageTemplate.Render("Val: {@Val}", new object[] { 42 });
            Assert.Equal("Val: 42", result);
        }

        [Fact]
        public void Render_Destructure_Null_ReturnsNull()
        {
            var result = MessageTemplate.Render("Val: {@Val}", new object[] { null });
            Assert.Equal("Val: null", result);
        }

        #endregion

        #region Escaped Braces

        [Theory]
        [InlineData("{{literal}}", "{literal}")]
        [InlineData("before {{text}} after", "before {text} after")]
        [InlineData("{{{Name}}}", "{World}")]
        public void Render_EscapedBraces_ProducesLiteralBraces(string template, string expected)
        {
            var result = MessageTemplate.Render(template, new object[] { "World" });
            Assert.Equal(expected, result);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Render_MissingArgs_KeepsLiteralPlaceholder()
        {
            var result = MessageTemplate.Render("Hello {Name} and {Other}", new object[] { "World" });
            Assert.Equal("Hello World and {Other}", result);
        }

        [Fact]
        public void Render_ExtraArgs_IgnoresExtras()
        {
            var result = MessageTemplate.Render("Hello {Name}", new object[] { "World", "Extra" });
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Render_NullArg_RendersNullString()
        {
            var result = MessageTemplate.Render("Value: {Val}", new object[] { null });
            Assert.Equal("Value: null", result);
        }

        [Fact]
        public void Render_EmptyArgs_ReturnsTemplate()
        {
            var result = MessageTemplate.Render("Hello {Name}", Array.Empty<object>());
            Assert.Equal("Hello {Name}", result);
        }

        [Fact]
        public void Render_UnclosedBrace_TreatsAsLiteral()
        {
            var result = MessageTemplate.Render("Hello {Name", new object[] { "World" });
            Assert.Equal("Hello {Name", result);
        }

        [Fact]
        public void Render_EmptyPlaceholder_StillConsumesArg()
        {
            var result = MessageTemplate.Render("Hello {} world", new object[] { "big" });
            Assert.Equal("Hello big world", result);
        }

        #endregion

        #region Extension Method Integration

        [Fact]
        public void ExtensionMethod_I_WithTemplate_CallsLineWithRenderedMessage()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.I("Processed {Count} items in {Elapsed} ms", 5, 120);
            mock.Verify(x => x.Line(LogLevel.INFO, "Processed 5 items in 120 ms"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_D_WithTemplate_CallsLineWithRenderedMessage()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.D("Debug {Msg}", "test");
            mock.Verify(x => x.Line(LogLevel.DEBUG, "Debug test"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_T_WithTemplate_CallsLineWithRenderedMessage()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.T("Trace {Val}", 99);
            mock.Verify(x => x.Line(LogLevel.TRACE, "Trace 99"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_W_WithTemplate_CallsLineWithRenderedMessage()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.W("Warning: {Msg}", "slow");
            mock.Verify(x => x.Line(LogLevel.WARNING, "Warning: slow"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_E_WithTemplate_CallsLineWithRenderedMessage()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.E("Failed after {Attempts} attempts", 3);
            mock.Verify(x => x.Line(LogLevel.ERROR, "Error!!!: Failed after 3 attempts"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_PlainString_StillWorks()
        {
            var mock = new Mock<IScopeLog>();
            mock.Object.I("Plain message");
            mock.Verify(x => x.Line(LogLevel.INFO, "Plain message"), Times.Once());
        }

        [Fact]
        public void ExtensionMethod_E_WithException_StillWorks()
        {
            var mock = new Mock<IScopeLog>();
            var ex = new InvalidOperationException("test error");
            // Explicitly call the (string, Exception) overload
            LoggingExtensions.E(mock.Object, "Custom message", ex);
            mock.Verify(x => x.Line(LogLevel.ERROR, It.Is<string>(s => s != null && s.Contains("Custom message"))), Times.Once());
        }

        #endregion
    }
}
