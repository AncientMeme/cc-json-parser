namespace LexerUnitTest
{
  public class LexerTest
  {
    [Fact]
    public void ValidSimpleObject()
    {
      var lexer = new Lexer("{}");
      List<Token> tokens = lexer.GetTokens();

      // All tokens
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
  }
}