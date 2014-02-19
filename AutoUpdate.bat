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

cd ..
7za a /r Snowflake.zip ./tmp/*.*

copy Snowflake.zip "./inetpub/wwwroot/downloads/Snowflake.zip"

rd tmp /Q /S

pause