DEBUG = TRUE
#DEBUG = FALSE

if(not DEBUG)
	import io
	import time
	import picamera
	import RPi.GPIO as GPIO

	PICTURE_INTERVAL_SECONDS = 60

	class PictureTaker(object):
		def __init__(self):
			self.camera = picamera.PiCamera()

		def takePhoto(self):
			fileName = "photos/"+str(int(round(time.time() * 1000)))+'pipic.jpg'
			self.camera.capture(fileName)
			return fileName

	if __name__ == "__main__":
		while True:
			pT = PictureTaker()
			pT.takePhoto()
			time.sleep(PICTURE_INTERVAL_SECONDS)
			#Does the camera have to be turned off here? - does it remain on?
else
	print ""