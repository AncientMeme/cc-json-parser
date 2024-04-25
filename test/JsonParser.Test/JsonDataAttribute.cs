
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

public class JsonDataAttribute : DataAttribute
{
  private readonly string _path;

  public JsonDataAttribute(string path)
  {
    _path = path;
  }

  public override IEnumerable<object[]> GetData(MethodInfo testMethod)
  {
    string content;
    var path = Path.GetRelativePath(Directory.GetCurrentDirectory(), _path);
    var fileInfo = new FileInfo(path);
    using (var fileStream = fileInfo.OpenRead())
    using (var sr = new StreamReader(fileStream))
    {
      content = sr.ReadToEnd();
    }
    
    string[] wrapper = {content};
    var output = new List<object[]>()
    {
      wrapper
    };

    return output;
  }
}