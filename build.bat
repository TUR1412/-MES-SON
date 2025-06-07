@echo off
echo ========================================
echo MES项目本地构建脚本
echo ========================================

echo 正在查找MSBuild...

:: 尝试查找Visual Studio 2022的MSBuild
set "MSBUILD_PATH="
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

:: 尝试查找Visual Studio 2019的MSBuild
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

:: 尝试查找Build Tools的MSBuild
for /f "delims=" %%i in ('dir "C:\Program Files*\Microsoft Visual Studio\*\BuildTools\MSBuild\Current\Bin\MSBuild.exe" /s /b 2^>nul') do (
    set "MSBUILD_PATH=%%i"
    goto :found
)

echo 警告：未找到MSBuild.exe
echo 这可能是因为：
echo 1. 未安装Visual Studio或Visual Studio Build Tools
echo 2. 当前环境为CI/CD环境（如GitHub Actions）
echo.
echo 在GitHub Actions环境中，MSBuild会自动配置
echo 本地开发需要安装Visual Studio 2019/2022或Build Tools
echo.
echo 构建脚本已准备就绪，可以推送到GitHub进行测试
pause
exit /b 0

:found
echo 找到MSBuild: %MSBUILD_PATH%

echo.
echo 正在还原NuGet包...
nuget restore MES.sln
if errorlevel 1 (
    echo 错误：NuGet包还原失败
    pause
    exit /b 1
)

echo.
echo 正在构建解决方案...
"%MSBUILD_PATH%" MES.sln /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：构建失败
    pause
    exit /b 1
)

echo.
echo ✅ 构建成功完成！
pause