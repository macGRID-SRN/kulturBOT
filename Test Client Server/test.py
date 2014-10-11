import socket

class Connection:
	HOST, PORT = "127.0.0.1", 5000
	BUF_SIZE = 4096
	
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
		print data
		self.sock.send(bytearray([]))
		return self.sock.recv(1)
		
	def sendFile(self, path):
		with open(path, 'rb') as fd:
			buf = fd.read(self.BUF_SIZE)
			while(buf):
				self.sock.send(buf)
				buf = fd.read(self.BUF_SIZE)
		return True

	def closeConnection(self):
		self.sock.close()

server = Connection()
print server.sendFile("z8Z9wi8.jpg")
server.closeConnection()