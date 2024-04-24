
class Parser
{
  private List<Token> tokens;
  private int tokenIndex;
  public Parser(List<Token> tokens) 
  {
    this.tokens = tokens;
    tokenIndex = 0;
  }

  public Dictionary<string, object> ParseObject() 
  {
    Dictionary<string, object> output = new();
    if (PeepToken().type != TokenType.LeftCurlyBracket)
    {
      throw new InvalidDataException("Object did not start with \'{\'");
    }
    PopToken();

    while (tokenIndex < tokens.Count)
    {
      if (PeepToken().type == TokenType.RightCurlyBracket)
      {
        Console.WriteLine($"{PeepToken().type}");
        PopToken();
        return output;
      }
    }

    throw new InvalidDataException("Object did not end with \'}\'");
  }

  private Token PopToken()
  {
    if (tokenIndex < tokens.Count) 
    {
      var token = tokens[tokenIndex];
      tokenIndex++;
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
}