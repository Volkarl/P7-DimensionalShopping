#!/bin/bash
#download and install chocolate (package manager)
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
#install stuff with chocolate
choco install 7zip.install
choco install python
#do path choco does path?
choco install pip

#use pip(package manager) to install selenium
python -m pip -U selenium


$LocalTempDir = $env:TEMP; $ChromeInstaller = "ChromeInstaller.exe"; (new-object    System.Net.WebClient).DownloadFile('http://dl.google.com/chrome/install/375.126/chrome_installer.exe', "$LocalTempDir\$ChromeInstaller"); & "$LocalTempDir\$ChromeInstaller" /silent /install; $Process2Monitor =  "ChromeInstaller"; Do { $ProcessesFound = Get-Process | ?{$Process2Monitor -contains $_.Name} | Select-Object -ExpandProperty Name; If ($ProcessesFound) { "Still running: $($ProcessesFound -join ', ')" | Write-Host; Start-Sleep -Seconds 2 } else { rm "$LocalTempDir\$ChromeInstaller" -ErrorAction SilentlyContinue -Verbose } } Until (!$ProcessesFound)

bitsadmin /addfile thisissomejobname https://chromedriver.storage.googleapis.com/2.42/chromedriver_win32.zip C:\chromedriver.zip

#unzip chromedriver
New-Item -ItemType directory -Path C:\webdriver
sz x -o $C:\webdriver\chromedriver.exe $chromedriver.zip -r

#do path for chromedriver
set PATH=%PATH%;C:\webdriver

