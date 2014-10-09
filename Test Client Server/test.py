import socket

class Connection:
	HOST, PORT = "127.0.0.1", 5000
	
	def __init__(self):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.connect((Connection.HOST,Connection.PORT))

	def sendData(self, data, type=None):
		print type
		if type is None:
			self.sock.send('\00')
		else:
			self.sock.send(bytearray([type]))
		self.sock.send(data)
		self.sock.send()
		return self.sock.recv(1)
		
	def sendFile(self, path):
		sendfile = open(path, 'rb')
		return self.sendData(sendfile.read(), 1)

	def closeConnection(self):
		self.sock.close()

server = Connection()
print server.sendFile("testFile.txt")