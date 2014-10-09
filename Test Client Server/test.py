import socket

HOST, PORT = "127.0.0.1", 5000

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

sock.connect((HOST, PORT))

sock.send("Hello World")
data = sock.recv(1024)
sock.close()
print repr(data)
