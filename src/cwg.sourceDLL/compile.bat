@ECHO OFF

rc cwg.sourceDLL.rc
cvtres /MACHINE:x86 /OUT:cwg.sourceDLL.o cwg.sourceDLL.res
cl.exe /O2 /D_USRDLL /D_WINDLL dllmain.cpp Source.def /MT /link /DLL /OUT:cwg.sourceDLL.dll cwg.sourceDLL.o
