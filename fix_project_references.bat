@echo off
echo 修复项目引用中的XML标签问题...

echo.
echo 正在修复 MES.DAL.csproj...
powershell -Command "Start-Sleep -Seconds 2; (Get-Content 'src/MES.DAL/MES.DAL.csproj') -replace '<n>', '<Name>' -replace '</n>', '</Name>' | Set-Content 'src/MES.DAL/MES.DAL.csproj'"

echo.
echo 正在修复 MES.BLL.csproj...
powershell -Command "Start-Sleep -Seconds 2; (Get-Content 'src/MES.BLL/MES.BLL.csproj') -replace '<n>', '<Name>' -replace '</n>', '</Name>' | Set-Content 'src/MES.BLL/MES.BLL.csproj'"

echo.
echo 正在修复 MES.Models.csproj...
powershell -Command "Start-Sleep -Seconds 2; (Get-Content 'src/MES.Models/MES.Models.csproj') -replace '<n>', '<Name>' -replace '</n>', '</Name>' | Set-Content 'src/MES.Models/MES.Models.csproj'"

echo.
echo 正在修复 MES.UI.csproj...
powershell -Command "Start-Sleep -Seconds 2; (Get-Content 'src/MES.UI/MES.UI.csproj') -replace '<n>', '<Name>' -replace '</n>', '</Name>' | Set-Content 'src/MES.UI/MES.UI.csproj'"

echo.
echo 修复完成！
pause
