using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using System.Collections.Generic;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;

[AzurePipelines(AzurePipelinesImage.WindowsLatest,
    AutoGenerate = false,
    InvokedTargets = new[] { nameof(Pack) },
    ImportSecrets = new[] { nameof(NuGetApiKey) })]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("NuGet API key")]
    [Secret]
    readonly string NuGetApiKey;
    [Parameter("NuGet Source")]
    readonly string NuGetPackageSource = "nuget.org";

    AbsolutePath SrcDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath NugetConfig => RootDirectory / "nuget.config";

    IEnumerable<AbsolutePath> SrcProjects => SrcDirectory.GlobFiles("**/*.csproj");
    IEnumerable<AbsolutePath> TestsProjects => TestsDirectory.GlobFiles("**/*.csproj");

    Target Clean => _ => _
        .Executes(() =>
        {
            SrcDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
            ArtifactsDirectory.DeleteDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            SrcProjects.ForEach(project =>
            {
                DotNetRestore(s => s.SetProjectFile(project));
            });
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            SrcProjects.ForEach(project =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            TestsProjects.ForEach(project =>
            {
                DotNetTest(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .EnableNoBuild());
            });
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            SrcProjects.ForEach(project =>
            {
                DotNetPack(s => s
                    .SetProject(project)
                    .SetOutputDirectory(ArtifactsDirectory)
                    .SetIncludeSymbols(false) // NOTE: false
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .DisableIncludeSymbols()
                    .EnableNoBuild());
            });
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => NuGetApiKey)
        .Executes(() =>
        {
            IReadOnlyCollection<AbsolutePath> packages = ArtifactsDirectory.GlobFiles("*.nupkg");

            packages.ForEach(package =>
            {
                NuGetPush(s => s
                    .SetConfigFile(NugetConfig)
                    .SetTargetPath(package)
                    .SetSource(NuGetPackageSource)
                    .SetApiKey(NuGetApiKey)
                    .SetProcessToolPath(RootDirectory / ".nuget" / "nuget.exe")
                    .SetProcessArgumentConfigurator(_ => _
                        .Add("-SkipDuplicate")));
            });
        });
}
