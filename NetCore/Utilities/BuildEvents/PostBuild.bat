rem About the Script
rem Written by: Yufei Liu
rem Email: feilfly@gmail.com
rem Date: 21 Oct 2015
rem -----------------------------------------------------------------------------------
@echo off
rem Call example 1: "D:\a\b\"PostBuild.bat "$(ProjectDir)" "$(TargetPath)" $(ConfigurationName)

set solutionName=%1
set projectName=%2
set projectPath=%3
set dllPath=%4
set buildConfig=%5

set projectPath=%projectPath:\"=\\"%

set targetScriptPath=%~dp0\Scripts\

if "%solutionName%"=="FrameworkExtKit" (
    echo ------------------------------ Post-build event started [%buildConfig%] -------------------
    echo.
    echo Project Path: %projectPath%
    echo DLL Path: %dllPath%
    echo.


    if "%buildConfig%"=="Release" GOTO :releaseBuild
    if "%buildConfig%"=="Debug" GOTO :debugBuild

    :end
    echo.
    echo ------------------------------ Post-build event ended -------------------
    echo.
    echo.
    exit
) ELSE (
    echo !!The project %projectName% "(%solutionName%)" is not built within solution FrameworkExtKit, do not increase build version
)
exit

:releaseBuild
    PowerShell.exe -ExecutionPolicy ByPass -File %targetScriptPath%\UpdateVersionInfo.ps1 -projectName %projectName% -projectPath %projectPath% -targetDLLFullPath %dllPath% -buildConfig %buildConfig%
    if %errorlevel% neq 0 Exit %errorlevel%
    GOTO :end


:debugBuild
    GOTO :end
