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
  })
  .DoesForEach(GetFiles("**/*.dnn"), (file) => 
  { 
    var transformFile = File(System.IO.Path.GetTempFileName());
    FileAppendText(transformFile, $@"<?xml version=""1.0""?>
<dotnetnuke xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <packages>
    <package version=""{version.Major.ToString("00")}.{version.Minor.ToString("00")}.{version.Patch.ToString("00")}"" xdt:Transform=""SetAttributes(version)"">
    </package>
  </packages>
</dotnetnuke>");
    XdtTransformConfig(file, transformFile, file);
});;

RunTarget(target);