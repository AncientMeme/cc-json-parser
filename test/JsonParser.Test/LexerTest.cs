using Xunit.Abstractions;

namespace LexerUnitTest
{
  public class LexerTest
  {
    private readonly ITestOutputHelper output;
    public LexerTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Theory]
    [JsonData("TestFiles/BasicObject/valid.json")]
    public void ValidEmptyJson(string content)
    {
      // Arrange
      var lexer = new Lexer(content);
      // Act
      List<Token> tokens = lexer.GetTokens();

      // Assert: All tokens should match
      List<Token> expectedTokens = new List<Token>
      {
        new Token(TokenType.LeftCurlyBracket, "{"),
        new Token(TokenType.RightCurlyBracket, "}")
      };
      Assert.Equal(expectedTokens.Count, tokens.Count);

      for(int i = 0; i < expectedTokens.Count; ++i) 
      {
        Assert.Equal(expectedTokens[i], tokens[i]);
      }
    }

    [Theory]
    [JsonData("TestFiles/BasicObject/invalid.json")]
    public void InvalidEmptyJson(string content)
    {
      // Arrange
      var lexer = new Lexer(content);
      // Act
      // Assert
      Assert.Throws<InvalidDataException>(() => lexer.GetTokens());
    }

    [Theory]
    [JsonData("TestFiles/BasicObject/invalid2.json")]
    public void InvalidBracketJson(string content)
    {
      // Arrange
      var lexer = new Lexer(content);
      // Act
      // Assert
      Assert.Throws<InvalidDataException>(() => lexer.GetTokens());
    }

    [Theory]
    [JsonData("TestFIles/String/valid2.json")]
    public void ValidStringJson(string content)
    {
      // Arrange
      var lexer = new Lexer(content);
      // Act
      List<Token> tokens = lexer.GetTokens();
      // Assert: All tokens should match
      List<Token> expectedTokens = new List<Token>
      {
        new Token(TokenType.LeftCurlyBracket, "{"),
        new Token(TokenType.String, "key"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.String, "value"),
        new Token(TokenType.Comma, ","),
        new Token(TokenType.String, "key2"),
        new Token(TokenType.Colon, ":"),
        new Token(TokenType.String, "value"),
        new Token(TokenType.RightCurlyBracket, "}"),
      };
      Assert.Equal(expectedTokens.Count, tokens.Count);

      for(int i = 0; i < expectedTokens.Count; ++i) 
      {
        Assert.Equal(expectedTokens[i], tokens[i]);
      }
    }

    private void PrintOutput(string testOutput)
    {
      output.WriteLine($"{testOutput}");
    }

    private void PrintTokens(List<Token> tokens) 
    {
      foreach(Token t in tokens)
        output.WriteLine($"Type: {t.type}, Value: {t.value}");
    }
  }
}