@ ECHO off
cls
"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" Build.proj /m /p:ConfigurationName=Release /p:Archive=true /p:VisualStudioVersion=14.0%*
pause