import serial
import binascii

class Netduino:
	
	def __init__(self):
		self.serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=3.5)
		#self.serialport.write("\x80")
		
	def demo(self):
		self.serialport.write("\x88\x00")
		
	def sendSentence(self, sentence):
		b = sentence.encode('utf-8')
		self.serialport.write("\x81")
		self.serialport.write(bytes([len(b)]))
		self.serialport.write(b)