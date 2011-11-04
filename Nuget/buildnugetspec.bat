set PKG=FluentMetadata
mkdir lib\net40

msbuild ..\%PKG%\%PKG%.csproj /p:Configuration=Release

copy ..\%PKG%\Bin\Release\%PKG%.dll lib\net40

subwcrev .. %PKG%.nuspec.src %PKG%.nuspec
