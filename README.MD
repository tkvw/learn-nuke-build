# Learn nuke build 

First initialize a git repository and commit the empty directory and switch to the `develop` branch: 
```
mkdir learn-nuke-build
cd learn-nuke-build
git init
git commit --allow-empty -m "Initial commit"
git checkout -b develop 
```
We use the develop branch to follow `gitflow` guidelines. 

First create a new solution project: 
```
dotnet new sln --name "learn-nuke-build"
```

Make sure nuke is installed: 
```
dotnet tool install Nuke.GlobalTool --global
```

and setup nuke by entering: 
```
nuke :setup
```
Simply accept the defaults (this repo uses `/src` as sources folder). 

## Create a library project

```
dotnet new classlib --output src/SimpleApi --name SimpleApi
```

Add project to the solution: 
```
dotnet sln .\learn-nuke-build.sln add .\src\SimpleApi\SimpleApi.csproj
```

Now it's time to open the project in `Rider`/`Visual Studio`. Add some content to the `SimpleApi` library and let's run the build command.

```powershell
nuke 
``` 

This results in an error: 
```
UKE Global Tool version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)
PowerShell Desktop version 5.1.18362.628
Microsoft (R) .NET Core SDK version 3.1.101
Creating directory '~\src\github.com\tkvw\learn-nuke-build\.tmp'...

███╗   ██╗██╗   ██╗██╗  ██╗███████╗
████╗  ██║██║   ██║██║ ██╔╝██╔════╝
██╔██╗ ██║██║   ██║█████╔╝ █████╗
██║╚██╗██║██║   ██║██╔═██╗ ██╔══╝
██║ ╚████║╚██████╔╝██║  ██╗███████╗
╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝

NUKE Execution Engine version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)

> "C:\Program Files\Git\cmd\git.exe" rev-parse --abbrev-ref HEAD
Assertion failed: Could not parse remote URL for 'origin'.
   at Nuke.Common.ControlFlow.Fail(String text)
   at Nuke.Common.ControlFlow.Assert(Boolean condition, String text)
   at Nuke.Common.Git.GitRepository.FromLocalDirectory(String directory, String branch, String remote)
   at Nuke.Common.Git.GitRepositoryAttribute.GetValue(MemberInfo member, Object instance)
   at Nuke.Common.Execution.InjectionUtility.InjectValuesInternal[T](T instance, IEnumerable`1 tuples)
   at Nuke.Common.Execution.InjectionUtility.InjectValues[T](T instance, Func`2 filter)
   at Nuke.Common.Execution.BuildManager.Execute[T](Expression`1[] defaultTargetExpressions)

Repeating warnings and errors:
Assertion failed: Could not parse remote URL for 'origin'.

═══════════════════════════════════════
Target             Status      Duration
───────────────────────────────────────
Restore            NotRun          0:00
Compile            NotRun          0:00
───────────────────────────────────────
Total                              0:00
═══════════════════════════════════════

Build failed on 10/03/2020 09:50:45. (?°?°)?? ???
```

So apparently Nuke needs a remote called `origin`. This is probably a bug, but for now simply add a remote to the repo: 

```powershell
git remote add origin git@github.com:tkvw/learn-nuke-build.git 
```

Try to build again: 
```powershell
nuke
```
Outputs something like: 
```
NUKE Global Tool version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)
PowerShell Desktop version 5.1.18362.628
Microsoft (R) .NET Core SDK version 3.1.101

███╗   ██╗██╗   ██╗██╗  ██╗███████╗
████╗  ██║██║   ██║██║ ██╔╝██╔════╝
██╔██╗ ██║██║   ██║█████╔╝ █████╗
██║╚██╗██║██║   ██║██╔═██╗ ██╔══╝
██║ ╚████║╚██████╔╝██║  ██╗███████╗
╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝

NUKE Execution Engine version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)

> "C:\Program Files\Git\cmd\git.exe" rev-parse --abbrev-ref HEAD
> "C:\Program Files\dotnet\dotnet.exe" ~\.nuget\packages\gitversion.tool\5.1.1\tools\netcoreapp3.0\any\GitVersion.dll

╬════════════
║ Restore
╬═══

> "C:\Program Files\dotnet\dotnet.exe" restore ~\src\github.com\tkvw\learn-nuke-build\learn-nuke-build.sln
  Restore completed in 227,99 ms for ~\src\github.com\tkvw\learn-nuke-build\src\SimpleApi\SimpleApi.csproj.

╬════════════
║ Compile
╬═══

> "C:\Program Files\dotnet\dotnet.exe" build ~\src\github.com\tkvw\learn-nuke-build\learn-nuke-build.sln --configuration Debug --no-restore /property:AssemblyVersion=0.1.0.0 /property:FileVersion=0.1.0.0 /property:InformationalVersion=0.1.0-build-libr
ary-project.1+3.Branch.features-build-library-project.Sha.12e96c2a595fabcef866d344e0b23e80cf14a063
Microsoft (R) Build Engine version 16.4.0+e901037fe for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  SimpleApi -> ~\src\github.com\tkvw\learn-nuke-build\src\SimpleApi\bin\Debug\netstandard2.0\SimpleApi.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.68
═══════════════════════════════════════
Target             Status      Duration
───────────────────────────────────────
Restore            Executed        0:01
Compile            Executed        0:02
───────────────────────────────────────
Total                              0:03
═══════════════════════════════════════

Build succeeded on 10/03/2020 10:06:15. \(^?^)/

```
The build was successful! But how did it know what to run? If we checkout the [Build.cs](_build/Build.cs), there's a line: 
```c#
public static int Main () => Execute<Build>(x => x.Compile);
``` 

The main tells to run the `Compile` task by default. What if we want to run something else or use a different configuration? 
Let's checkout the help of nuke: 
```powershell
nuke --help
```
Outputs: 
```
NUKE Global Tool version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)
PowerShell Desktop version 5.1.18362.628
Microsoft (R) .NET Core SDK version 3.1.101

███╗   ██╗██╗   ██╗██╗  ██╗███████╗
████╗  ██║██║   ██║██║ ██╔╝██╔════╝
██╔██╗ ██║██║   ██║█████╔╝ █████╗
██║╚██╗██║██║   ██║██╔═██╗ ██╔══╝
██║ ╚████║╚██████╔╝██║  ██╗███████╗
╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝

NUKE Execution Engine version 0.24.4 (Windows,.NETCoreApp,Version=v2.1)

Targets (with their direct dependencies):

  Clean
  Restore
  Compile (default)    -> Restore

Parameters:

  --configuration      Configuration to build - Default is 'Debug' (local) or
                       'Release' (server).

  --continue           Indicates to continue a previously failed build attempt.
  --help               Shows the help text for this build assembly.
  --host               Host for execution. Default is 'automatic'.
  --target             List of targets to be executed. Default is 'Compile'.
  --no-logo            Disables displaying the NUKE logo.
  --plan               Shows the execution plan (HTML).
  --root               Root directory during build execution.
  --skip               List of targets to be skipped. Empty list skips all
                       dependencies.
  --verbosity          Logging verbosity during build execution. Default is
                       'Normal'.

```
The interesting part is that running on the server environment (ci) the configuration 
is automatically set to `Release`, but what if we want to run `Release` locally? Simply run: 
```powershell
nuke --configuration Release
```
