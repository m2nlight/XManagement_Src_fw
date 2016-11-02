@echo off
set slnDir=XManagement
set output=%slnDir%_Src.7z
set exe7z=7za.exe
set excludeFile=exclude.txt
if exist %excludeFile% goto COMPRESS
echo bin\>%excludeFile%
echo obj\>>%excludeFile%
echo _ReSharper.%slnDir%>>%excludeFile%
echo %slnDir%.5.1.ReSharper.user>>%excludeFile%
echo %slnDir%.suo>>%excludeFile%
echo %slnDir%.sln.cache>>%excludeFile%
echo Thumbs.db>>%excludeFile%
:COMPRESS
rem del /F /S /Q /A %slnDir%\Thumbs.db
if exist %output% del %output% /F /Q
%exe7z% a -t7z -mx=9 -r %output% %slnDir%\* -xr@exclude.txt 
echo 压缩完成：%output%
echo 按任意键退出
pause>nul