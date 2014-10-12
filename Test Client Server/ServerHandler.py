import socket
from Enumerators import *

class Connection:
	HOST, PORT = "127.0.0.1", 5000
	BUF_SIZE = 4096
	
	def __init__(self):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.connect((self.HOST,self.PORT))

	def sendFile(self, path):
		with open(path, 'rb') as fd:
			buf = fd.read(Connection.BUF_SIZE)
			while(buf):
				self.sock.send(buf)
				buf = fd.read(Connection.BUF_SIZE)
		return True

	def closeConnection(self):
		self.sock.close()
		
#Onto functions that can't be put ABOVE THE CLASS BECAUSE STUPID REASONS PYTHON...
def sendImage(path):
	server = Connection()
	server.sendFile(path)
	server.closeConnection()
	print "Successfully sent file " + path + " to the server"
	
sendImage("z8Z9wi8.jpg")