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
