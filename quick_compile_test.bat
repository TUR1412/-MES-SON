@echo off
echo 快速编译测试...

:: 尝试查找MSBuild
set "MSBUILD_PATH="

:: 查找Visual Studio 2022的MSBuild
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

:: 查找Visual Studio 2019的MSBuild
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

:: 查找Build Tools的MSBuild
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\*\BuildTools\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

echo 错误：未找到MSBuild.exe
echo 请安装Visual Studio 2019/2022
pause
exit /b 1

:found
echo 找到MSBuild: %MSBUILD_PATH%

echo.
echo 正在测试编译MES.Models...
"%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if errorlevel 1 (
    echo ❌ MES.Models 编译失败
    pause
    exit /b 1
) else (
    echo ✅ MES.Models 编译成功
)

echo.
echo 正在测试编译MES.Common...
"%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if errorlevel 1 (
    echo ❌ MES.Common 编译失败
    pause
    exit /b 1
) else (
    echo ✅ MES.Common 编译成功
)

echo.
echo 正在测试编译MES.DAL...
"%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if errorlevel 1 (
    echo ❌ MES.DAL 编译失败
    pause
    exit /b 1
) else (
    echo ✅ MES.DAL 编译成功
)

echo.
echo 正在测试编译MES.BLL...
"%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if errorlevel 1 (
    echo ❌ MES.BLL 编译失败
    echo 正在显示详细错误信息...
    "%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
    pause
    exit /b 1
) else (
    echo ✅ MES.BLL 编译成功
)

echo.
echo ✅ 所有核心项目编译成功！
pause
