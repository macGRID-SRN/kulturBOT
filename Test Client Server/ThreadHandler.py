import threading

    class ThreadHandler(threading.Thread):
        def __init__(self):
            threading.Thread.__init__(self)
            
        def run(self):
            self.target(*self._args)
            
        def sendToThread(self, target *args):
            self.start()
            
                
