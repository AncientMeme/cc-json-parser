
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
    var token = PeepToken();
    switch(token.type)
    {
      case TokenType.LeftCurlyBracket:
        return ParseObject();
      case TokenType.String:
        return ParseString();
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
      if (parsedComma && PeepToken().type == TokenType.RightCurlyBracket)
      {
        throw new InvalidDataException("Extra trailing comma at the final key value pair");
      }

      // Check for right bracket
      if (PeepToken().type == TokenType.RightCurlyBracket)
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
      if (PeepToken().type == TokenType.RightCurlyBracket)
      {
        PopToken();
        return output;
      }
      else if (PeepToken().type == TokenType.Comma)
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

  private string ParseString()
  {
    if (PeepToken().type != TokenType.String)
    {
      throw new InvalidDataException("Keys should be String Tokens");
    }
    return PopToken().value;
  }

  private void ParseColon() {
    if (PeepToken().type != TokenType.Colon)
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

  private Token PeepToken()
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

  private void PrintTokens()
  {
    foreach (Token t in tokens)
    {
      Console.WriteLine($"Token Type: {t.type}, Token Value: {t.value}");
    }
    Console.WriteLine("------------------------------------");
  }
}