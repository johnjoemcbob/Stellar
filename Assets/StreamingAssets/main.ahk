#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
;#SingleInstance force
#MaxThreadsPerHotkey 2

WinActivate, Stellaris ahk_class SDL_app
; Reset UI
Send {Esc}
Sleep, 10
Send {F1}
Sleep, 10
Send {F1}
Sleep, 10

; Center mouse
MouseMove, A_ScreenWidth / 2, A_ScreenHeight / 2, 0

; Go to home system
Send {Home}
Sleep, 500

; Galaxy view
Send {e}
Sleep, 100

; Zoom out while centered on home system
Loop, 40
{
	Send {WheelDown}
	Sleep, 5
}
Send {WheelDown}
Sleep, 300

; Tilt camera
MouseClickDrag, Right, A_ScreenWidth / 2, A_ScreenHeight / 2, A_ScreenWidth / 2, A_ScreenHeight / 2 + 300, 10
Sleep, 200

; Center y position
Send {w down}
Sleep, 200
Send {w up}
Sleep, 200

; Center mouse
MouseMove, A_ScreenWidth / 2, A_ScreenHeight / 2, 0

return
ExitApp