import threading

#Queue functions to be thrown into the server thread

class ServerCommunicationsThread(threading.Thread):
    def __init__(self):
		self.queue = []
		
    def run(self):
        for i in range(len(self.queue) - 1):
	    queue[0].startFunction()
	    queue.pop(0)
		
    def sendToThread(self, target, *args):
	    self.activity = Func(target, *args)
                
    def add(self):
            self.queue.append(self.activity)

class Func(object):
    def __init__(self, target, *args):
        self.target = target
        self._args = *args

    def startFunction(self):
        self.target(*self._args)
