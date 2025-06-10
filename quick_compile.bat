@echo off
chcp 65001 >nul
echo ========================================
echo MES项目快速编译工具
echo ========================================
echo.

echo [1] 查找编译工具...

REM 查找MSBuild
set "MSBUILD_PATH="
if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD_PATH=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    echo ✅ 找到Visual Studio 2022 Professional MSBuild
) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    echo ✅ 找到Visual Studio 2022 Community MSBuild
) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    echo ✅ 找到Visual Studio 2019 Professional MSBuild
) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    echo ✅ 找到Visual Studio 2019 Community MSBuild
) else (
    where msbuild >nul 2>&1
    if %errorlevel% equ 0 (
        set "MSBUILD_PATH=msbuild"
        echo ✅ 找到系统PATH中的MSBuild
    ) else (
        echo ❌ 未找到MSBuild工具
        echo.
        echo 请安装以下任一工具：
        echo • Visual Studio 2019/2022 (Community/Professional)
        echo • Build Tools for Visual Studio
        echo • .NET Framework SDK
        echo.
        echo 或者在Visual Studio中手动编译项目
        goto :manual_compile
    )
)

echo 使用MSBuild路径: %MSBUILD_PATH%
echo.

echo [2] 清理编译输出...
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
echo ✅ 清理完成

echo.
echo [3] 按依赖顺序编译项目...

echo 编译 MES.Models...
"%MSBUILD_PATH%" "src\MES.Models\MES.Models.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if %errorlevel% neq 0 (
    echo ❌ MES.Models 编译失败
    goto :compile_error
) else (
    echo ✅ MES.Models 编译成功
)

echo 编译 MES.Common...
"%MSBUILD_PATH%" "src\MES.Common\MES.Common.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if %errorlevel% neq 0 (
    echo ❌ MES.Common 编译失败
    goto :compile_error
) else (
    echo ✅ MES.Common 编译成功
)

echo 编译 MES.DAL...
"%MSBUILD_PATH%" "src\MES.DAL\MES.DAL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if %errorlevel% neq 0 (
    echo ❌ MES.DAL 编译失败
    goto :compile_error
) else (
    echo ✅ MES.DAL 编译成功
)

echo 编译 MES.BLL...
"%MSBUILD_PATH%" "src\MES.BLL\MES.BLL.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if %errorlevel% neq 0 (
    echo ❌ MES.BLL 编译失败
    goto :compile_error
) else (
    echo ✅ MES.BLL 编译成功
)

echo 编译 MES.UI...
"%MSBUILD_PATH%" "src\MES.UI\MES.UI.csproj" /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
if %errorlevel% neq 0 (
    echo ❌ MES.UI 编译失败
    goto :compile_error
) else (
    echo ✅ MES.UI 编译成功
)

echo.
echo ========================================
echo 🎉 编译成功！
echo ========================================
echo.
echo 可执行文件位置: src\MES.UI\bin\Debug\MES.UI.exe
echo.
echo 下一步：
echo 1. 运行 test_database_connection.bat 初始化数据库
echo 2. 启动 MES.UI.exe 测试应用程序
echo 3. 使用"数据库诊断"工具验证数据库连接
echo.
goto :end

:compile_error
echo.
echo ❌ 编译失败！
echo 请在Visual Studio中查看详细错误信息
goto :end

:manual_compile
echo.
echo 手动编译指南：
echo 1. 打开Visual Studio
echo 2. 打开 MES.sln 解决方案文件
echo 3. 选择"生成" → "重新生成解决方案"
echo 4. 确保编译成功后运行程序
echo.

:end
pause
