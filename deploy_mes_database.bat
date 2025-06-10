@echo off
chcp 65001 >nul
echo ========================================
echo MES Database Deployment Starting
echo ========================================
echo.

echo Executing database creation script...
"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 < create_mes_database.sql

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo Database deployment successful!
    echo ========================================
    echo.
    echo Verifying deployment results...
    "C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 -e "USE mes_db; SHOW TABLES;"
    echo.
    echo Verifying initial data...
    "C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 -e "USE mes_db; SELECT COUNT(*) AS user_count FROM user_info; SELECT COUNT(*) AS workshop_count FROM workshop_info; SELECT COUNT(*) AS material_count FROM material_info;"
) else (
    echo.
    echo ========================================
    echo Database deployment failed! Error code: %ERRORLEVEL%
    echo ========================================
)

echo.
echo Deployment completed!
pause
