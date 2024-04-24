
public enum TokenType
{
    LeftCurlyBracket,
    RightCurlyBracket
}

public struct Token
{
  public TokenType type;
  public string value;

  public Token(TokenType type, string value) 
  {
    this.type = type;
    this.value = value;
  }
}