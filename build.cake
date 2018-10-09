#addin Cake.XdtTransform
#addin "Cake.FileHelpers"
#tool "nuget:?package=GitVersion.CommandLine&prerelease"

var target = Argument("target", "");
GitVersion version;

Task("UpdateVersion")
  .Does(() => {
    version = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        UpdateAssemblyInfoFilePath = @"DesktopModules\DNNCorp\SolutionInfo.cs"
    });

    if (BuildSystem.IsLocalBuild == false) 
    {
        StartPowershellScript("Write-Output", args => {
            args.AppendQuoted($"##vso[build.updatebuildnumber]{version.FullSemVer}");
        });
    }
  });

RunTarget(target);