#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
;#SingleInstance force
#MaxThreadsPerHotkey 2

WinActivate, Stellaris ahk_class SDL_app
Send {Esc}
Sleep, 100
Send {F1}
Sleep, 100
Send {F1}
Sleep, 100
Send {%1%}
Sleep, 300
Send {%1%}

return
ExitApp