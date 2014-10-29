import socket
import hashlib
import array
from itertools import izip
from Enumerators import *
from debug import *

class Connection:
	HOST, PORT = "192.168.1.5", 5000
	BUF_SIZE = 4096
	ROBOT_ID = 1
	
	if(DEBUG):
		HOST = "127.0.0.1"
	
	def __init__(self):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.connect((self.HOST,self.PORT))
	
	def sendFile(self, path, ComType, FileType):
		inits = bytearray([self.ROBOT_ID, ComType, FileType, AdditionalInfo.NULL])
		self.sock.send(inits)
		cont = self.sock.recv(1)
		if (cont):
			#at this point we can confirm the init packet has been received by server.
			self.sock.send(self.getMD5OfFile(path))
			cont = self.sock.recv(1)
			if (cont):
				with open(path, 'rb') as fd:
					buf = fd.read(Connection.BUF_SIZE)
					while(buf):
						self.sock.send(buf)
						self.sock.recv(1)
						buf = fd.read(Connection.BUF_SIZE)
					print "Done sending file!"
					self.sendConfirmPacket()
					#if (readyForHash):
				return True
		return False

	def sendConfirmPacket(self):
		self.sock.send(bytearray([255]))
		
	def closeConnection(self):
		self.sock.close()
		
	def getMD5OfFile(self, path):
		hasher = hashlib.md5()
		with open(path, 'rb') as afile:
			buf = afile.read(self.BUF_SIZE)
			count = 0
			while (buf):
				count += len(buf)
				print len(buf), count
				hasher.update(buf)
				buf = afile.read(self.BUF_SIZE)
		for i in bytearray.fromhex(hasher.hexdigest()):
			print i
		return bytearray.fromhex(hasher.hexdigest())
		
#Onto functions that can't be put ABOVE THE CLASS BECAUSE STUPID REASONS PYTHON...
def sendImage(path, ImageType):
	server = Connection()
	server.sendFile(path, ComType.ImageSend, ImageType)
	server.closeConnection()
	print "Successfully sent image " + path + " to the server"

def sendJPG(path):
	sendImage(path, ImageType.JPG)

def sendPNG(path):
	sendImage(path, ImageType.PNG)
	
def sendBMP(path):
	sendImage(path, ImageType.BITMAP)
	
