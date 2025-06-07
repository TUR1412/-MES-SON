@echo off
echo ========================================
echo MES项目强制重新编译脚本
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

echo 错误：未找到MSBuild.exe
echo 请安装Visual Studio 2019/2022或Visual Studio Build Tools
pause
exit /b 1

:found
echo 找到MSBuild: %MSBUILD_PATH%

echo.
echo 正在彻底清理所有编译输出和缓存...

:: 清理所有bin和obj目录
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s /q "%%d"

:: 清理Visual Studio缓存
if exist ".vs" rd /s /q ".vs"

:: 清理NuGet包缓存（如果存在packages目录）
if exist "packages" rd /s /q "packages"

echo ✅ 清理完成

echo.
echo 正在还原NuGet包...
nuget restore MES.sln
if errorlevel 1 (
    echo 警告：NuGet包还原失败，继续编译...
)

echo.
echo 开始强制重新编译所有项目...

echo.
echo [1/5] 强制编译 MES.Models...
"%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /verbosity:normal
if errorlevel 1 (
    echo ❌ 错误：MES.Models 编译失败
    pause
    exit /b 1
)
echo ✅ MES.Models 编译成功

echo.
echo [2/5] 强制编译 MES.Common...
"%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /verbosity:normal
if errorlevel 1 (
    echo ❌ 错误：MES.Common 编译失败
    pause
    exit /b 1
)
echo ✅ MES.Common 编译成功

echo.
echo [3/5] 强制编译 MES.DAL...
"%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /verbosity:normal
if errorlevel 1 (
    echo ❌ 错误：MES.DAL 编译失败
    pause
    exit /b 1
)
echo ✅ MES.DAL 编译成功

echo.
echo [4/5] 强制编译 MES.BLL...
"%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /verbosity:normal
if errorlevel 1 (
    echo ❌ 错误：MES.BLL 编译失败
    pause
    exit /b 1
)
echo ✅ MES.BLL 编译成功

echo.
echo [5/5] 强制编译 MES.UI...
"%MSBUILD_PATH%" "src\MES.UI\MES.UI.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild /verbosity:normal
if errorlevel 1 (
    echo ❌ 错误：MES.UI 编译失败
    pause
    exit /b 1
)
echo ✅ MES.UI 编译成功

echo.
echo ========================================
echo 🎉 强制重新编译完成！
echo ========================================
echo.
echo 可执行文件位置：src\MES.UI\bin\Debug\MES.UI.exe
echo.
echo 检查编译输出：
if exist "src\MES.Models\bin\Debug\MES.Models.dll" (
    echo ✅ MES.Models.dll 已生成
) else (
    echo ❌ MES.Models.dll 未找到
)

if exist "src\MES.Common\bin\Debug\MES.Common.dll" (
    echo ✅ MES.Common.dll 已生成
) else (
    echo ❌ MES.Common.dll 未找到
)

if exist "src\MES.DAL\bin\Debug\MES.DAL.dll" (
    echo ✅ MES.DAL.dll 已生成
) else (
    echo ❌ MES.DAL.dll 未找到
)

if exist "src\MES.BLL\bin\Debug\MES.BLL.dll" (
    echo ✅ MES.BLL.dll 已生成
) else (
    echo ❌ MES.BLL.dll 未找到
)

if exist "src\MES.UI\bin\Debug\MES.UI.exe" (
    echo ✅ MES.UI.exe 已生成
) else (
    echo ❌ MES.UI.exe 未找到
)

echo.
pause
