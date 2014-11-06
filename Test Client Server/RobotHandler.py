import serial

class Robot:
	
	def __init__(self):
		self.serialport = serial.Serial("/dev/ttyAMA0", 57600, timeout=0.5)
		self.serialport.write("\x80")
		
	def demo(self):
		self.serialport.write("\x88\x00")
		
	def charge(self):
		self.serialport.write("\x88\x01")
		
	def stop(self):
		self.serialport.write("\x88\xFF");
		
	def isCharging(self):
		serialport.write("\x8E\x15")
		return ord(serialport.read(size=1))