
using System.Text;

// Lexer generates tokens from a JSON file. Also handles validation
// so parser would only parse valid tokens
public class Lexer 
{
  private string content;
  private int contentIndex;
  private int curlyBracketDepth;
  private int arrayBracketDepth;
  public Lexer(string content) 
  {
    this.content = content;
    contentIndex = 0;
    curlyBracketDepth = 0;
    arrayBracketDepth = 0;
  }

  public List<Token> GetTokens() 
  {
    if (content.Length == 0)
    {
      throw new InvalidDataException("The Json file is empty!");
    }

    List<Token> tokens = new List<Token>();
    char next;
    while (contentIndex < content.Length)
    {
      next = content[contentIndex];
      switch (next)
      {
        case '{':
          tokens.Add(new Token(TokenType.LeftCurlyBracket, "{"));
          curlyBracketDepth++;
          break;
        case '}':
          tokens.Add(new Token(TokenType.RightCurlyBracket, "}"));
          curlyBracketDepth--;
          break;
        case '\"':
          tokens.Add(new Token(TokenType.String, GetTokenString()));
          break;
        case ':':
          tokens.Add(new Token(TokenType.Colon, ":"));
          break;
        case ',':
          tokens.Add(new Token(TokenType.Comma, ","));
          break;
        case ' ' or '\n' or '\r':
          break;
        default:
          throw new InvalidDataException($"Character found in invalid position: {next}");
      }
      contentIndex++;
    }
    // All curly brackets should be closed
    if (curlyBracketDepth != 0)
    {
      throw new InvalidDataException("Not all curly brackets are closed");
    }

    return tokens;
  }

  private string GetTokenString()
  {
    // Skip the left quotation
    contentIndex++;
    StringBuilder sb = new();

    while (contentIndex < content.Length)
    {
      // return when encounter right quotation
      if (content[contentIndex] == '\"')
      {
        return sb.ToString();
      }

      sb.Append(content[contentIndex]);
      contentIndex++;
    }

    // No close quotation exists
    throw new InvalidDataException("String is not closed off");
  }
}