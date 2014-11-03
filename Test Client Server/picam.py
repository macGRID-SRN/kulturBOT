from ServerHandler import *
from ThreadHandler import *

if(not DEBUG):
	import io
	import time
	import picamera
	import RPi.GPIO as GPIO
	import time
	PICTURE_INTERVAL_SECONDS = 60

	class PictureTaker(object):
		def __init__(self):
			self.takingPicture = False
			self.camera = None
			self.sct = ServerCommunicationsThread()
			self.fileNameQueue = []
				
		def takePhotoJPG(self):
			self.takingPicture = True
			self.camera = picamera.PiCamera()
			fileName = "photos/"+str(int(round(time.time() * 1000)))+'pipic.jpg'
			self.camera.capture(fileName)
			self.jpg_callback(fileName)

		def jpg_callback(self,fileName):
			self.camera.close()
			self.takingPicture = False
			if(self.sct.is_alive()):
					self.fileNameQueue.append(fileName)
			else:
				sct = ServerCommunicationsThread()
				self.fileNameQueue.append(fileName)
				for file in self.fileNameQueue:
						print file
						self.sct.add(sendJPG, file)
				sct.start()
				self.fileNameQueue = []
				#removed delay so that function can be called from outside

		#Flag to check if picture is being taken
		def isTakingPicture(self):
			return self.takingPicture

	if __name__ == "__main__":
		tts = TextToSpeechEngine()
		
		getRecentTweets()
		pT = PictureTaker()
		count = 0
		while True:
			tts.speak(RecentTweets.Tweets[count])
			if(not(pT.isTakingPicture())):
					pT.takePhotoJPG()
			time.sleep(60)
			count+=1
else:   
	#test jpg file being sent!
	#sendJPG("test.jpg")
	getRecentTweets()
