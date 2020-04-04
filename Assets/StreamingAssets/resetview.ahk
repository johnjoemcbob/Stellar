#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
;#SingleInstance force
#MaxThreadsPerHotkey 2

WinActivate, Stellaris ahk_class SDL_app

; Go to home system
Send {Home}
Sleep, 100

; Galaxy view
Send {e}

; Zoom out while centered on home system
Loop, 30
{
	Send {WheelDown}
	Sleep, 10
}
Sleep, 100

; Tilt camera
;MouseMove, A_ScreenWidth / 2, A_ScreenHeight / 2, 0
Click Down Right
SendEvent {Click right 1088, 800, down}{click right 1000, 1080, up}
;MouseMove, A_ScreenWidth / 2, A_ScreenHeight / 2 + 300, 0
Click Up Right
;Sleep, 200

Send {WheelUp} ; For tiny galaxies!

; Send goto command to ensure perfect position
; Open console
Send ``
Sleep, 100
Send {Text}goto 0 0
Send {Enter}
Sleep, 100
Send ``

; Hide UI
;Send ^{F9}

return
ExitApp