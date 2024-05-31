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
}
