@echo off
set api_dir=/src/UpBlazor.WebApi
set ui_dir=/src/UpBlazor.WebUI

wt -d %CD%%api_dir% dotnet watch; split-pane -p "Windows PowerShell" -d %CD%%ui_dir% dotnet watch