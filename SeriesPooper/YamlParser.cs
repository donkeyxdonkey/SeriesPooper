using SeriesPooper.Interface;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SeriesPooper;

public class YamlParser
{
    public static T ParseConfig<T>(FileInfo config) => ParseConfig<T>(config, null);

    public static T ParseConfig<T>(FileInfo config, INamingConvention? namingConventionNullDefault)
    {
        if (!config.Exists)
            throw new FileNotFoundException($"Config not found: {config.FullName}");

        string contents = File.ReadAllText(config.FullName);

        INamingConvention namingConvention = namingConventionNullDefault is not null ? namingConventionNullDefault : PascalCaseNamingConvention.Instance;

        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build();

        return deserializer.Deserialize<T>(contents);
    }

    public static void SaveConfig<T>(T content, FileInfo config)
    {
        if (content is null)
            throw new ArgumentNullException("glurp");

        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();
        string yaml = serializer.Serialize(content);
        System.Console.WriteLine(yaml);

        // TODO:
    }
}
