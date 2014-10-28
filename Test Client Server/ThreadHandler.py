import threading

#Queue functions to be thrown into the server thread

class ServerCommunicationsThread(threading.Thread):
    def __init__(self):
        self.queue = []
                
    def run(self):
        try:
            for i in range(len(self.queue) - 1):
                queue[0].startFunction()
                queue.pop(0)
        except:
            print "something went wrong";          
                
    def add(self, target, *args):
        self.activity = Func(target, *args)
        self.queue.append(self.activity)

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
        self.target(*self._args)
