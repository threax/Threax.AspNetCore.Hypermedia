set BASE=%~dp0
set SEARCH=%BASE%src
set OUT=%BASE%nuget
rmdir %OUT%
mkdir %OUT%
pushd %SEARCH%
FOR /R %BASE% %%I in (*BuildNuget.bat) DO call %%I 
FOR /R %SEARCH% %%I in (Release\*.nupkg) DO copy %%I %OUT%
popd