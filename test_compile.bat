@echo off
echo ========================================
echo MES项目编译测试脚本
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

echo 错误：未找到MSBuild.exe
echo 请安装Visual Studio 2019/2022
pause
exit /b 1

:found
echo 找到MSBuild: %MSBUILD_PATH%

echo.
echo 正在测试编译MES.Models...
"%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:quiet /nologo
if errorlevel 1 (
    echo ❌ MES.Models 编译失败
    echo 正在显示详细错误信息...
    "%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
    pause
    exit /b 1
) else (
    echo ✅ MES.Models 编译成功
)

echo.
echo 正在测试编译MES.Common...
"%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:quiet /nologo
if errorlevel 1 (
    echo ❌ MES.Common 编译失败
    echo 正在显示详细错误信息...
    "%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
    pause
    exit /b 1
) else (
    echo ✅ MES.Common 编译成功
)

echo.
echo 正在测试编译MES.DAL...
"%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:quiet /nologo
if errorlevel 1 (
    echo ❌ MES.DAL 编译失败
    echo 正在显示详细错误信息...
    "%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
    pause
    exit /b 1
) else (
    echo ✅ MES.DAL 编译成功
)

echo.
echo 正在测试编译MES.BLL...
"%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:quiet /nologo
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
echo 正在测试编译MES.UI...
"%MSBUILD_PATH%" "src\MES.UI\MES.UI.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:quiet /nologo
if errorlevel 1 (
    echo ❌ MES.UI 编译失败
    echo 正在显示详细错误信息...
    "%MSBUILD_PATH%" "src\MES.UI\MES.UI.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
    pause
    exit /b 1
) else (
    echo ✅ MES.UI 编译成功
)

echo.
echo ========================================
echo 🎉 所有项目编译测试通过！
echo ========================================

echo.
echo 检查生成的文件：
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
