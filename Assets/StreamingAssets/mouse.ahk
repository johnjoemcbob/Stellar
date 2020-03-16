#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
;#SingleInstance force
#MaxThreadsPerHotkey 2

MouseMove, %1%, %2%, 0
WinActivate, Stellaris ahk_class SDL_app
StringLen, Length, 3
if ( Length > 1 )
{
	Click
}

return
ExitApp