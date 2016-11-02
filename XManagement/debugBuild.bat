%windir%\microsoft.net\framework\v3.5\msbuild /property:Configuration=Debug XManagement.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE