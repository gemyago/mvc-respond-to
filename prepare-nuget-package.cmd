@echo off

md release

pushd RespondTo
echo Creating package...
nuget pack RespondTo.csproj -Build -Prop Configuration=Release -OutputDirectory %~dp0release
popd