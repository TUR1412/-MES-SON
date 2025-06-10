@echo off
chcp 65001 >nul
echo ========================================
echo Testing MES Database Connection
echo ========================================
echo.

echo Testing connection with root user...
"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 -e "SELECT 'Connection successful!' AS status, VERSION() AS mysql_version, DATABASE() AS current_db;"

echo.
echo Checking mes_db database...
"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 -e "USE mes_db; SELECT 'mes_db connected!' AS status;"

echo.
echo Checking table structure...
"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" -u root -pQwe.123 -e "USE mes_db; DESCRIBE material_info;"

echo.
echo Connection test completed!
pause
