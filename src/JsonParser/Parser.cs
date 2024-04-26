
public class Parser
{
  private List<Token> tokens;
  private int tokenIndex;
  public Parser(List<Token> tokens) 
  {
    this.tokens = tokens;
    tokenIndex = 0;
  }

  public object ParseTokens()
  {
    return ParseValue();
  }

  private object ParseValue()
  {
    var token = PeekToken();
    switch(token.type)
    {
      case TokenType.LeftCurlyBracket:
        return ParseObject();
      case TokenType.String:
        return ParseString();
      case TokenType.Number:
        var numberToken = PopToken();
        return int.Parse(numberToken.value);
      case TokenType.True:
        PopToken();
        return true;
      case TokenType.False:
        PopToken();
        return false;
      case TokenType.Null:
        PopToken();
        return null;
      case TokenType.LeftArrayBracket:
        return ParseArray();
      default:
        throw new InvalidDataException($"Invalid token type as value {token.type}");
    }
  }

  public Dictionary<string, object> ParseObject() 
  {
    Dictionary<string, object> output = new();
    // Left bracket token
    PopToken();
    bool parsedComma = false;

    while (tokenIndex < tokens.Count) 
    {
      // Passed through a comma but no key value pair left
      if (parsedComma && PeekToken().type == TokenType.RightCurlyBracket)
      {
        throw new InvalidDataException("Extra trailing comma at the final key value pair");
      }

      // Check for right bracket
      if (PeekToken().type == TokenType.RightCurlyBracket)
      {
        PopToken();
        return output;
      }

      // Pop key
      var key = ParseString();

      // Pop colon
      ParseColon();

      // Pop Value
      var value = ParseValue();
      output[key] = value;

      // Check if next key value pair exists
      if (PeekToken().type == TokenType.RightCurlyBracket)
      {
        PopToken();
        return output;
      }
      else if (PeekToken().type == TokenType.Comma)
      {
        parsedComma = true;
        PopToken();
      }
      else
      {
        throw new InvalidDataException("Invalid token found after key value pair");
      }
    }

    throw new InvalidDataException("Object does not have closing bracket");
  }

  private object[] ParseArray()
  {
    var output = new List<object>();
    // Left array bracket token
    PopToken();
    while (tokenIndex < tokens.Count) 
    {
      // Check if its closed
      if (PeekToken().type == TokenType.RightArrayBracket)
      {
        PopToken();
        return output.ToArray();
      }
      // Get token value
      var val = ParseValue();
      output.Add(val);

      // Check for next value or closing
      if (PeekToken().type == TokenType.RightArrayBracket)
      {
        PopToken();
        return output.ToArray();
      }
      else if (PeekToken().type == TokenType.Comma)
      {
        PopToken();
      }
      else
      {
        throw new InvalidDataException("Invalid token found in Array");
      }
    }

    throw new InvalidDataException("Array does not have closing bracket");
  }

  private string ParseString()
  {
    if (PeekToken().type != TokenType.String)
    {
      throw new InvalidDataException("Keys should be String Tokens");
    }
    return PopToken().value;
  }

  private void ParseColon() {
    if (PeekToken().type != TokenType.Colon)
    {
      throw new InvalidDataException("Colon should follow Key");
    }
    PopToken();
  }

  private Token PopToken()
  {
    if (tokenIndex < tokens.Count) 
    {
      var token = tokens[tokenIndex];
      tokenIndex++;
      // Console.WriteLine($"Popped Token Type: {token.type}, Token Value: {token.value}");
      return token;
    }
    else
    {
      throw new InvalidDataException("Invalid Pop: There are no more tokens left");
    }
   
  }

  private Token PeekToken()
  {
    if (tokenIndex < tokens.Count) 
    {
      return tokens[tokenIndex];
    }
    else 
    {
      throw new InvalidDataException("Invalid Peek: There are no more tokens left'");
    }
  }
}