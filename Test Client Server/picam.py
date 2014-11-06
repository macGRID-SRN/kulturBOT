from ServerHandler import *
from ThreadHandler import *
from TextToSpeechEngine import *
import random


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
		import serial
		serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=0.5)
		serialport.write("\x80");
		serialport.write("\x88\x00");
		
		time.sleep(5);
		serialport.write("\x88\xFF");
		
		pT = PictureTaker()
		count = 1
		while True:
			if(not (count % (PICTURE_INTERVAL_SECONDS - 20))):
				tts.speak(RecentTweets.Tweets[random.randint(0,len(RecentTweets.Tweets) - 1)])
			if(not (count % PICTURE_INTERVAL_SECONDS)):
				if(not(pT.isTakingPicture())):
						pT.takePhotoJPG()
			time.sleep(1)
			count+=1
else:   
	#test jpg file being sent!
	#sendJPG("test.jpg")
	getRecentTweets()
