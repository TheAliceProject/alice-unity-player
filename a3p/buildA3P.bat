@echo off

set BUILD_DIR=%cd%\build
set SOURCE_NAME=%1
set SOURCE_DIR="src\%SOURCE_NAME%"

::buildFile
pushd %SOURCE_DIR%

set target=%BUILD_DIR%\%SOURCE_NAME%.a3p

echo Building %SOURCE_DIR% to %target%...
if exist "%targetFile%" (
	del /q "%targetFile%"
)
for /d %%f in (*) do (
	echo ... Adding %%f
	7z a "%target%" -tzip "%%f" > nul
)
for %%f in (*) do (
	echo ... Adding %%f
	7z a "%target%" -tzip "%%f" > nul
)
echo Done.
popd