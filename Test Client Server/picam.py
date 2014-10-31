from ServerHandler import *
from ThreadHandler import *

if(not DEBUG):
        import io
        import time
        import picamera
        import RPi.GPIO as GPIO
		from time import sleep
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
                                self.fileNameQueue.append(fileName)
                                for file in fileNameQueue:
                                        self.sct.add(sendJPG, file)
                                self.fileNameQueue = []
                        #removed delay so that function can be called from outside

                #Flag to check if picture is being taken
                def isTakingPicture(self):
                        return self.takingPicture

        if __name__ == "__main__":
                pT = PictureTaker()
                while True:
                        if(not(pT.isTakingPicture())):
                                pT.takePhotoJPG()

                        
else:   
        #test jpg file being sent!
        sendJPG("z8Z9wi8.jpg")
