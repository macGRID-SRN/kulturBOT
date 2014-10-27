from ServerHandler import *
from ThreadHandler import *
import platform

if(platform.system() == "Linux"):
        DEBUG = False

else:
        DEBUG = True

if(not DEBUG):
	import io
	import time
	import picamera
	import RPi.GPIO as GPIO

	PICTURE_INTERVAL_SECONDS = 60

	class PictureTaker(object):
		def __init__(self):
			self.camera = null
			
		def takePhotoJPG(self):
			self.camera = picamera.PiCamera()
			fileName = "photos/"+str(int(round(time.time() * 1000)))+'pipic.jpg'
			self.camera.capture(fileName)
			self.jpg_callback(fileName)

		def jpg_callback(self,fileName):
			self.camera.close()
			ThreadHandler.sendToThread(sendJPG,fileName)
			time.sleep(PICTURE_INTERVAL_SECONDS)

	if __name__ == "__main__":
		pT = PictureTaker()
		while True:
			pT.takePhotoJPG()
			
else:	
	#test jpg file being sent!
	ThreadHandler.sendToThreadsendJPG("z8Z9wi8.jpg")
