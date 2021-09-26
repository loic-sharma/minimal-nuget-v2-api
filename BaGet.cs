using NuGet.Versioning;

namespace NuGetV2;

public interface IUrlGenerator
{
    string GetServiceIndexV2Url();
    string GetServiceIndexV3Url();
    string GetPackageDownloadUrl(string id, NuGetVersion version);

    // New
    string GetPackageDownloadUrl(Package package);
    string GetPackageV2Url(Package package);
}

// Below are types copied from BaGet.

// TODO: Delete
public class UrlGenerator : IUrlGenerator
{
    public string GetPackageDownloadUrl(string id, NuGetVersion version) => $"https://localhost/{id}/{version}.nupkg";
    public string GetPackageDownloadUrl(Package package) => GetPackageDownloadUrl(package.Id, package.Version);

    // These are new
    public string GetPackageV2Url(Package package) =>
        $"https://localhost/api/v2/FindPackageById(Id=\'{package.Id}\',Version=\'{package.NormalizedVersionString}\')";

    public string GetServiceIndexV2Url() => "https://localhost/api/v2";
    public string GetServiceIndexV3Url() => "https://localhost/v3/index.json";
}

// See NuGetGallery's: https://github.com/NuGet/NuGetGallery/blob/master/src/NuGetGallery.Core/Entities/Package.cs
public class Package
{
    public int Key { get; set; }

    public string Id { get; set; }

    public NuGetVersion Version
    {
        get
        {
            // Favor the original version string as it contains more information.
            // Packages uploaded with older versions of BaGet may not have the original version string.
            return NuGetVersion.Parse(
                OriginalVersionString != null
                    ? OriginalVersionString
                    : NormalizedVersionString);
        }

        set
        {
            NormalizedVersionString = value.ToNormalizedString().ToLowerInvariant();
            OriginalVersionString = value.OriginalVersion;
        }
    }

    public string[] Authors { get; set; }
    public string Description { get; set; }
    public long Downloads { get; set; }
    public bool HasReadme { get; set; }
    public bool HasEmbeddedIcon { get; set; }
    public bool IsPrerelease { get; set; }
    public string ReleaseNotes { get; set; }
    public string Language { get; set; }
    public bool Listed { get; set; }
    public string MinClientVersion { get; set; }
    public DateTime Published { get; set; }
    public bool RequireLicenseAcceptance { get; set; }
    public SemVerLevel SemVerLevel { get; set; }
    public string Summary { get; set; }
    public string Title { get; set; }

    public Uri IconUrl { get; set; }
    public Uri LicenseUrl { get; set; }
    public Uri ProjectUrl { get; set; }

    public Uri RepositoryUrl { get; set; }
    public string RepositoryType { get; set; }

    public string[] Tags { get; set; }

    /// <summary>
    /// Used for optimistic concurrency.
    /// </summary>
    public byte[] RowVersion { get; set; }

    public List<PackageDependency> Dependencies { get; set; }
    public List<PackageType> PackageTypes { get; set; }
    public List<TargetFramework> TargetFrameworks { get; set; }

    public string NormalizedVersionString { get; set; }
    public string OriginalVersionString { get; set; }


    public string IconUrlString => IconUrl?.AbsoluteUri ?? string.Empty;
    public string LicenseUrlString => LicenseUrl?.AbsoluteUri ?? string.Empty;
    public string ProjectUrlString => ProjectUrl?.AbsoluteUri ?? string.Empty;
    public string RepositoryUrlString => RepositoryUrl?.AbsoluteUri ?? string.Empty;
}

// See NuGetGallery.Core's: https://github.com/NuGet/NuGetGallery/blob/master/src/NuGetGallery.Core/Entities/PackageDependency.cs
public class PackageDependency
{
    public int Key { get; set; }

    /// <summary>
    /// The dependency's package ID. Null if this is a dependency group without any dependencies.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The dependency's package version. Null if this is a dependency group without any dependencies.
    /// </summary>
    public string VersionRange { get; set; }

    public string TargetFramework { get; set; }

    public Package Package { get; set; }
}

// See NuGetGallery.Core's: https://github.com/NuGet/NuGetGallery/blob/master/src/NuGetGallery.Core/Entities/PackageType.cs
public class PackageType
{
    public int Key { get; set; }

    public string Name { get; set; }
    public string Version { get; set; }

    public Package Package { get; set; }
}

public class TargetFramework
{
    public int Key { get; set; }

    public string Moniker { get; set; }

    public Package Package { get; set; }
}

public enum SemVerLevel
{
    /// <summary>
    /// Either an invalid semantic version or a semantic version v1.0.0
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A valid semantic version v2.0.0
    /// </summary>
    SemVer2 = 2
}
