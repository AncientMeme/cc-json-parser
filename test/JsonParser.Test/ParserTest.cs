
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
  }
}