from sys import platform as _platform

DEBUG = True
if(_platform != "win32"):
        DEBUG = False

#Override the OS check
#DEBUG = False