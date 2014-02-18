@echo off

MSBuild /t:Rebuild /p:Configuration=Release

mkdir "../tmp"
mkdir "../tmp/Snowflake"
mkdir "../tmp/Media"

xcopy "Snowflake/bin/Release" "../tmp/Snowflake" /s
xcopy "Snowflake/bin/Media" "../tmp/Media" /s

cd Snowflake/bin
copy resources.cfg "../../../tmp/"

cd ../../../tmp

rar a ../Snowflake.zip *.* -r

cd ..

copy Snowflake.zip "./inetpub/wwwroot/downloads/Snowflake.zip"

rm tmp

pause
