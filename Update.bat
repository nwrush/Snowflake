@echo off

git pull

MSBuild /t:Rebuild /p:Configuration=Release

mkdir "../tmp"
mkdir "../tmp/Snowflake"
mkdir "../tmp/Media"

xcopy "Snowflake/bin/Release" "../tmp/Snowflake" /s
xcopy "Snowflake/bin/Media" "../tmp/Media" /s

cd Snowflake/bin
copy resources.cfg "../../../tmp/"

cd C:\tmp

mklink "Snowflake" "./Snowflake/Snowflake.exe"

zip -r "../Snowflake.zip" Media Snowflake *.*

cd ..

copy Snowflake.zip "./inetpub/wwwroot/downloads/Snowflake.zip"

rd tmp /Q /S

pause