from sys import platform as _platform

DEBUG = True
if(_platform != "win32" and _platform != "darwin"):
        DEBUG = False

#Override the OS check
#DEBUG = False