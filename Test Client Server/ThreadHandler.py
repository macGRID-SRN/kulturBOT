import threading

#Queue functions to be thrown into the server thread

class ServerCommunicationsThread(threading.Thread):
    def __init__(self):
        super(ServerCommunicationsThread, self).__init__()
        self.queue = []
        self.daemon = True
                
    def run(self):
        try:
            for i in range(len(self.queue)):
                self.queue[i].startFunction()
            self.queue = []
            
        except Exception as e:
            print str(e);          
                
    def add(self, target, *args):
        self.activity = Func(target, *args)
        self.queue.append(self.activity)
        print "Activity added"

#Future class to be implemented to check which threads are alive/dead
class ThreadHandler(threading.Thread):
    def __init__(self):
        self.five = 5    
    def getActiveThreads(self):
        return threading.enumerate()

class Func(object):
    def __init__(self, target, *args):
        self.target = target
        self._args = args

    def startFunction(self):
        print "function started"
        self.target(*self._args)
