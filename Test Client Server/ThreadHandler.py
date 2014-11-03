import threading
import subprocess

#Queue functions to be thrown into the server thread

class ServerCommunicationsThread(threading.Thread):
    def __init__(self):
        super(ServerCommunicationsThread, self).__init__()
        self.daemon = True
        
    def run(self):
        try:
            print "run called"
            print "length of queue is: " + str(len(StaticList.queue))
            for i in range(len(StaticList.queue)):
                print "in loop"
                StaticList.queue[i].startFunction()
            StaticList.queue = []
            
        except Exception as e:
            print str(e);          

    def add(self, target, *args):
        self.activity = Func(target, *args)
        #damn pythong make all field variables in constructor static class variables..
        StaticList.queue.append(self.activity)

class StaticList:
    queue = []

class SpeechThread(threading.Thread):
    def __init__(self, text):
        super(SpeechThread, self).__init__()
        print "speech init called"
        self.text = text
    
    def run(self):
        print "Speech Started"
        subprocess.call('echo ' + self.text + '| festival --tts', shell = True)
        print "Speech Done"

#Future class to be implemented to check which threads are alive/dead
class ThreadHandler(threading.Thread):
    def __init__(self):
        self.five = 5    
    def getActiveThreads(self):
        return threading.enumerate()

class Func:
    def __init__(self, target, *args):
        self.target = target
        self._args = args

    def startFunction(self):
        print "function started"
        self.target(*self._args)
