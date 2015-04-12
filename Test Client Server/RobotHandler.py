import serial
import binascii

class Netduino:
	
	def __init__(self):
		#self.serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=3.5, parity=serial.PARITY_NONE, stopbits=serial.STOPBITS_ONE)
		#self.serialport.write("\x80")
		print "Netduino init called"
		
	def demo(self):
		self.serialport.write("\x88\x00")
		
	def sendSentence(self, sentence):
		self.serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=3.5, parity=serial.PARITY_NONE, stopbits=serial.STOPBITS_ONE)
		b = sentence.encode('utf-8')
		print sentence, len(sentence), len(b)
		self.serialport.write(bytearray(chr(1) + chr(len(b)) + b + bytes(255- len(b) - 2)))
		numOk = self.serialport.read(1)
		print ord(numOk)
		self.serialport.close()
		if ord(numOk) == 128:
			print "confirm packet received"
		else:
			print "something bad happened"