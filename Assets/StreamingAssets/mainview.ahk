#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
;#SingleInstance force
#MaxThreadsPerHotkey 2

ControlSend,, {Control Down}{F9}{Control Up}, Stellaris
Sleep, 10
ControlSend,, {Control Down}{F9}{Control Up}, Stellaris

return
ExitApp