@echo off
cd /d "%~dp0"
"C:\Users\Michi\AppData\Local\Gource\gource.exe" . --seconds-per-day 0.35 --auto-skip-seconds 1 --hide filenames,mouse
pause
