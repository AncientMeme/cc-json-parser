using Xunit.Abstractions;

namespace ParserUnitTest
{
  public class ParserTest
  {
    [Fact]
    public void ParseEmptyJson()
    {
      // Arrange
      var tokens = new List<Token>()
      {
        new Token(TokenType.LeftCurlyBracket, "{"),
        new Token(TokenType.RightCurlyBracket, "}"), 
      };
      var parser = new Parser(tokens);
      var expectedOutput = new Dictionary<string, object>();
      // Act
      var dict = parser.ParseObject();

      // Assert
      Assert.IsType<Dictionary<string, object>>(dict);
      Assert.Empty(dict);
    }

    [Fact]
    public void ParseComplexJson()
    {
      // Arrange
      var tokens = new List<Token>
      {
        new Token(TokenType.LeftCurlyBracket, "{"),
        new Token(TokenType.String, "key"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.String, "value"),
        new Token(TokenType.Comma, ","),
        new Token(TokenType.String, "key-n"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.Number, "101"),
        new Token(TokenType.Comma, ","),
        new Token(TokenType.String, "key-o"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.LeftCurlyBracket, "{"),
        new Token(TokenType.String, "inner key"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.String, "inner value"),
        new Token(TokenType.RightCurlyBracket, "}"),
        new Token(TokenType.Comma, ","),
        new Token(TokenType.String, "key-l"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.LeftArrayBracket, "["),
        new Token(TokenType.String, "list value1"),
        new Token(TokenType.Comma, ","),
        new Token(TokenType.String, "list value2"),
        new Token(TokenType.RightArrayBracket, "]"),
        new Token(TokenType.RightCurlyBracket, "}"),
      };
      var expectedKeys = new string[]{"key", "key-n", "key-o", "key-l"};

      // Act
      var parser = new Parser(tokens);
      var dict = (Dictionary<string, object>)parser.ParseTokens();

      // Assert
      foreach (string key in expectedKeys) {
        Assert.True(dict.ContainsKey(key));
      }
      Assert.Equal("value", dict["key"]);
      Assert.Equal(101, dict["key-n"]);

      var innerObject = (Dictionary<string, object>)dict["key-o"];
      Assert.True(innerObject.ContainsKey("inner key"));
      Assert.Equal("inner value", innerObject["inner key"]);

      var innerArray = (object[])dict["key-l"];
      Assert.Equal("list value1", innerArray[0]);
      Assert.Equal("list value2", innerArray[1]);
    }
  }
}