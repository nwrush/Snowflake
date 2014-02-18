@echo off

:: MSBuild /t:Rebuild /p:Configuration=Release

mkdir "../tmp"
mkdir "../tmp/Snowflake"
mkdir "../tmp/Media"

xcopy "Snowflake/bin/Release" "../tmp/Snowflake"
xcopy "Snowflake/bin/Media" "../tmp/Media"
copy "Snowflake/bin/*.cfg" "../tmp/*.cfg"

pause
