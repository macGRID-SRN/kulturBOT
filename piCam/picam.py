import io
import time
import picamera
import RPi.GPIO as GPIO

class PictureTaker(object):
    def __init__(self):
        self.camera = picamera.PiCamera()

    def takePhoto(self):
        self.camera.capture("photos/"+str(int(round(time.time() * 1000)))+'pipic.jpg')
if __name__ == "__main__":
    while True:
        pT = PictureTaker()
        pT.takePhoto()
        time.sleep(60)
    
