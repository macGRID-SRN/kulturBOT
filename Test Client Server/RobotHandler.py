import serial
import binascii

class Netduino:
	
	def __init__(self):
		self.serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=3.5, parity=serial.PARITY_NONE, stopbits=serial.STOPBITS_NON)
		#self.serialport.write("\x80")
		
	def demo(self):
		self.serialport.write("\x88\x00")
		
	def sendSentence(self, sentence):
		b = sentence.encode('utf-8')
		print sentence, len(sentence), len(b)
		self.serialport.write(bytearray([129, len(b)]))
		netOk = self.serialport.read(1)
		#self.serialport.write()
		if netOk == 128:
			print "confirm packet received"
			self.serialport.write(b)