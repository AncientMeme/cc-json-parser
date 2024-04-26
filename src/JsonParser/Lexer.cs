
using System.Text;
using System.Text.RegularExpressions;

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
          contentIndex++;
          break;
        case '}':
          tokens.Add(new Token(TokenType.RightCurlyBracket, "}"));
          curlyBracketDepth--;
          contentIndex++;
          break;
        case '\"':
          tokens.Add(new Token(TokenType.String, GetString()));
          break;
        case ':':
          tokens.Add(new Token(TokenType.Colon, ":"));
          contentIndex++;
          break;
        case ',':
          tokens.Add(new Token(TokenType.Comma, ","));
          contentIndex++;
          break;
        case '-' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9':
          tokens.Add(new Token(TokenType.Number, GetNumber()));
          break;
        case 't':
          tokens.Add(new Token(TokenType.True, GetTrue()));
          break;
        case 'f':
          tokens.Add(new Token(TokenType.False, GetFalse()));
          break;
        case 'n':
          tokens.Add(new Token(TokenType.Null, GetNull()));
          break;
        case '[':
          tokens.Add(new Token(TokenType.LeftArrayBracket, "["));
          arrayBracketDepth++;
          contentIndex++;
          break;
        case ']':
          tokens.Add(new Token(TokenType.RightArrayBracket, "]"));
          arrayBracketDepth--;
          contentIndex++;
          break;
        case ' ' or '\n' or '\r' or '\t':
          contentIndex++;
          break;
        default:
          throw new InvalidDataException($"Character found in invalid position: {next}");
      }
    }
    // All brackets should be closed
    if (curlyBracketDepth != 0)
    {
      throw new InvalidDataException("Not all curly brackets are closed");
    }
    if (arrayBracketDepth != 0)
    {
      throw new InvalidDataException("Not all array brackets are closed");
    }

    PrintTokens(tokens);
    return tokens;
  }

  private string GetString()
  {
    // Skip the left quotation
    contentIndex++;
    StringBuilder sb = new();

    while (contentIndex < content.Length)
    {
      // return when encounter right quotation
      if (content[contentIndex] == '\"')
      {
        contentIndex++;
        return sb.ToString();
      }
      else if (content[contentIndex] == '\\' && contentIndex + 1 <= content.Length)
      {
        // Account for backslash characters
        sb.Append(content[contentIndex]);
        sb.Append(content[contentIndex + 1]);
        contentIndex += 2;
      }
      else
      {
        sb.Append(content[contentIndex]);
        contentIndex++;
      }
    }

    // No close quotation exists
    throw new InvalidDataException("String is not closed off");
  }

  private string GetNumber()
  {
    int startIndex = contentIndex;
    int length = 0;
    var validChars = "-01234567890.eE";
    while(contentIndex < content.Length && validChars.Contains(content[contentIndex]))
    {
      contentIndex++;
      length++;
    }
    return content.Substring(startIndex, length);
  }

  private string GetTrue()
  {
    if (contentIndex + 4 <= content.Length && content.Substring(contentIndex, 4).Equals("true"))
    {
      contentIndex += 4;
      return "true";
    }

    throw new InvalidDataException("Invalid true token");
  }

  private string GetFalse()
  {
    if (contentIndex + 5 <= content.Length && content.Substring(contentIndex, 5).Equals("false"))
    {
      contentIndex += 5;
      return "false";
    }

    throw new InvalidDataException("Invalid false token");
  }

  private string GetNull()
  {
    if (contentIndex + 4 <= content.Length && content.Substring(contentIndex, 4).Equals("null"))
    {
      contentIndex += 4;
      return "null";
    }

    throw new InvalidDataException("Invalid null token");
  }

  private static void PrintTokens(List<Token> tokens)
  {
    foreach (Token t in tokens)
    {
      Console.WriteLine($"Token Type: {t.type}, Token Value: {t.value}");
    }
    Console.WriteLine("------------------------------------");
  }
}