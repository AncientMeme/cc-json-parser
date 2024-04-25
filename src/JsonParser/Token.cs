
using System.Diagnostics.CodeAnalysis;

public enum TokenType
{
    LeftCurlyBracket,
    RightCurlyBracket
}

public struct Token : IEquatable<Token>
{
  public TokenType type;
  public string value;

  public Token(TokenType type, string value) 
  {
    this.type = type;
    this.value = value;
  }

  public override bool Equals(object? obj) => obj is Token && this.Equals(obj);

  public bool Equals(Token token) 
  {
    return type == token.type && value.Equals(token.value);
  }

  public override int GetHashCode()
  {
    return (type, value).GetHashCode();
  }
}