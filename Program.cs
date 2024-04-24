using System.CommandLine;

var jsonArgument = new Argument<FileInfo?>(
  name: "file",
  description: "The JSON file to parse",
  getDefaultValue: () => null);

var rootCommand = new RootCommand("Parse a JSON file to return its data");
rootCommand.AddArgument(jsonArgument);
rootCommand.SetHandler(parseJson, jsonArgument);

await rootCommand.InvokeAsync(args);

void parseJson(FileInfo? file) 
{
  if (file == null) 
  {
    Console.WriteLine("Please provide a file to parse");
    return;
  }

  var lexer = new Lexer(file);
  var tokens = lexer.GetTokens();
  var parser = new Parser(tokens);
  try 
  {
    Dictionary<string, object> dict = parser.ParseObject();
  }
  catch (InvalidDataException e)
  {
    Console.WriteLine($"Invalid JSON format: {e}");
  }
  
}