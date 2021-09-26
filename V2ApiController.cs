using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace NuGetV2;

[Produces("application/xml")]
public class V2ApiController : Controller
{
    private readonly V2Builder _builder; // TODO: Interface

    public V2ApiController()
    {
        _builder = new V2Builder(new UrlGenerator());
    }

    public XElement Index() => _builder.BuildIndex();

    public async Task<XElement> List(
        [FromQuery(Name = "$skip")] int skip = 0,
        [FromQuery(Name = "$top")] int top = 20,
        [FromQuery(Name = "$orderby")] string orderBy = null)
    {
        // See: https://joelverhagen.github.io/NuGetUndocs/?http#endpoint-enumerate-packages
        var package = await GetPackageAsync();

        return _builder.BuildPackages(new List<Package> { package });
    }

    public async Task<XElement> Search(
        string searchTerm,
        string targetFramework,
        bool includePrerelease = true)
    {
        searchTerm = searchTerm?.Trim('\'') ?? "";
        targetFramework = targetFramework?.Trim('\'') ?? "";
        var package = await GetPackageAsync();

        return _builder.BuildPackages(new List<Package> { package });
    }

    public async Task<XElement> Package(string id)
    {
        id = id?.Trim('\'');
        var package = await GetPackageAsync();

        return _builder.BuildPackages(new List<Package> { package });
    }

    public async Task<XElement> PackageVersion(string id, string version)
    {
        var package = await GetPackageAsync();

        return _builder.BuildPackage(package);
    }

    private async Task<Package> GetPackageAsync()
    {
        await Task.Yield();

        return new Package
        {
            Id = "Newtonsoft.Json",
            Authors = new[] { "Foo", "Bar" },
            Description = "My package description",
            Downloads = 123,
            Language = "English",
            MinClientVersion = "1.2.3",
            Published = DateTime.Now.AddDays(-1),
            Summary = "My package summary",

            IconUrl = new("https://package.test/icon"),
            LicenseUrl = new("https://package.test/license"),
            ProjectUrl = new("https://package.test/icon"),
            RepositoryUrl = new("https://package.test/icon"),

            Tags = new[] { "Tag1", "Tag2" },

            Version = new("12.0.1"),

            Dependencies = new()
            {
                new() { TargetFramework = "net45" },
                new()
                {
                    TargetFramework = "net45",
                    Id = "Dependency1"
                },
                new()
                {
                    TargetFramework = "net45",
                    Id = "Dependency2",
                    VersionRange = "1.2"
                },
            }
        };
    }
}
