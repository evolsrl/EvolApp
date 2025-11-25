// JsonBodyExampleAttribute.cs
namespace EvolApp.API.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class JsonBodyExampleAttribute : Attribute
{
    public string Example { get; }

    public JsonBodyExampleAttribute(string example)
    {
        Example = example;
    }
}
