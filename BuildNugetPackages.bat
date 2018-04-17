set BASE=%~dp0
set SEARCH=%BASE%src
set OUT=%BASE%nuget
mkdir %OUT%
pushd %BASE%
rd /s /q src\Threax.ModelGen\bin
rd /s /q src\Threax.ModelGen\obj
dotnet build -c Release
pushd %SEARCH%
FOR /R %SEARCH% %%I in (Release\*.nupkg) DO move /Y %%I %OUT%
popd
popd