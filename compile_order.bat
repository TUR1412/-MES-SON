@echo off
echo ========================================
echo MES项目按依赖顺序编译脚本
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
echo 正在清理之前的编译输出...
if exist "src\MES.Models\bin" rmdir /s /q "src\MES.Models\bin"
if exist "src\MES.Models\obj" rmdir /s /q "src\MES.Models\obj"
if exist "src\MES.Common\bin" rmdir /s /q "src\MES.Common\bin"
if exist "src\MES.Common\obj" rmdir /s /q "src\MES.Common\obj"
if exist "src\MES.DAL\bin" rmdir /s /q "src\MES.DAL\bin"
if exist "src\MES.DAL\obj" rmdir /s /q "src\MES.DAL\obj"
if exist "src\MES.BLL\bin" rmdir /s /q "src\MES.BLL\bin"
if exist "src\MES.BLL\obj" rmdir /s /q "src\MES.BLL\obj"
if exist "src\MES.UI\bin" rmdir /s /q "src\MES.UI\bin"
if exist "src\MES.UI\obj" rmdir /s /q "src\MES.UI\obj"

echo.
echo 开始按依赖顺序编译项目...

echo.
echo [1/5] 编译 MES.Models...
"%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：MES.Models 编译失败
    pause
    exit /b 1
)
echo ✅ MES.Models 编译成功

echo.
echo [2/5] 编译 MES.Common...
"%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：MES.Common 编译失败
    pause
    exit /b 1
)
echo ✅ MES.Common 编译成功

echo.
echo [3/5] 编译 MES.DAL...
"%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：MES.DAL 编译失败
    pause
    exit /b 1
)
echo ✅ MES.DAL 编译成功

echo.
echo [4/5] 编译 MES.BLL...
"%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：MES.BLL 编译失败
    pause
    exit /b 1
)
echo ✅ MES.BLL 编译成功

echo.
echo [5/5] 编译 MES.UI...
"%MSBUILD_PATH%" "src\MES.UI\MES.UI.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal
if errorlevel 1 (
    echo 错误：MES.UI 编译失败
    pause
    exit /b 1
)
echo ✅ MES.UI 编译成功

echo.
echo ========================================
echo 🎉 所有项目编译成功！
echo ========================================
echo.
echo 可执行文件位置：src\MES.UI\bin\Debug\MES.UI.exe
echo.
pause
