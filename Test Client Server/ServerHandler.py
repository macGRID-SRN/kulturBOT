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
		print "Connecting"
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.connect((self.HOST,self.PORT))
		print "Connected"

	def sendFile(self, path, ComType, FileType):
		inits = bytearray([self.ROBOT_ID, ComType, FileType, AdditionalInfo.NULL])
		self.sock.send(inits)
		cont = self.sock.recv(1)
		if (cont):
			#at this point we can confirm the init packet has been received by server.
			self.sock.send(self.getMD5OfFile(path))
			cont = self.sock.recv(1)
			if (cont):
				count = 0;
				while(True):
					with open(path, 'rb') as fd:
						buf = fd.read(Connection.BUF_SIZE)
						while(buf):
							self.sock.send(buf)
							self.sock.recv(1)
							buf = fd.read(Connection.BUF_SIZE)
						print "Done sending file!"
						self.sendConfirmPacket()
						count += 1
					if(self.sock.recv(1)):
						return True
					
					print "Send failed! Attempt:", count
					if(count >=5):
						print "Max send tries reached"
						return False
		return False
	
	def getStrings(self, ComType, UpdateType, ActionType):
		inits = bytearray([self.ROBOT_ID, ComType, UpdateType, ActionType])
		self.sock.send(inits)
		cont = self.sock.recv(1)
		if (cont):
			numStrings = self.sock.recv(1)
			stringy = ""
			done = False
			while(not done):
				temp = self.sock.recv(self.BUF_SIZE)
				if(len(temp) == 1 and ord(temp) == 255):
					done = True
				stringy+=temp
			return stringy.split('\n')
		else:
			return False
	
	def sendConfirmPacket(self):
		self.sock.send(bytearray([255]))
		
	def closeConnection(self):
		self.sock.close()
		
	def getMD5OfFile(self, path):
		hasher = hashlib.md5()
		with open(path, 'rb') as afile:
			buf = afile.read(self.BUF_SIZE)
			while (buf):
				hasher.update(buf)
				buf = afile.read(self.BUF_SIZE)
		return bytearray.fromhex(hasher.hexdigest())

class RecentTweets():
	Tweets = []
	
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
	
def getRecentTweets():
	print "Getting recent tweets"
	server = Connection()
	RecentTweets.Tweets = server.getStrings(ComType.RequestUpdate, UpdateType.GetInformation, ActionType.RecentTweets)
	server.closeConnection()
	
