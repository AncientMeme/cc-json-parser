
public class Lexer 
{
  private string content;
  private int contentIndex;
  public Lexer(string content) 
  {
    this.content = content;
    contentIndex = 0;
  }

  public List<Token> GetTokens() 
  {
    List<Token> tokens = new List<Token>();
    char next;
    while (contentIndex < content.Length)
    {
      next = content[contentIndex];
      if (next == '{') 
      {
        tokens.Add(new Token(TokenType.LeftCurlyBracket, "{"));
      } 
      else if (next == '}')
      {
        tokens.Add(new Token(TokenType.RightCurlyBracket, "}"));
      }
      contentIndex += 1;
    }
    return tokens;
  }
}