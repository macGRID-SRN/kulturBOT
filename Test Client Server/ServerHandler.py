import socket
from Enumerators import *

class Connection:
	HOST, PORT = "127.0.0.1", 5000
	BUF_SIZE = 4096
	ROBOT_ID = 1
	
	def __init__(self):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.connect((self.HOST,self.PORT))
	
	def sendFile(self, path, ComType, FileType):
		inits = bytearray([ROBOT_ID, ComType, FileType, AdditionalInfo.NULL])
		self.sock.send(inits)
		cont = self.sock.recv(1)
		if (cont):
			with open(path, 'rb') as fd:
				buf = fd.read(Connection.BUF_SIZE)
				while(buf):
					self.sock.send(buf)
					self.sock.recv(1)
					buf = fd.read(Connection.BUF_SIZE)
				print "Done sending"
				self.sock.send(bytearray([255]))
				#if (readyForHash):
			return True
		return False

	def closeConnection(self):
		self.sock.close()
		
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