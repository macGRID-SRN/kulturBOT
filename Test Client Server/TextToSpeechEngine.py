from ThreadHandler import *


class TextToSpeechEngine(object):
    def __init__(self):
        self.st = SpeechThread("")
    
    def speak(self, text):
        if(self.st.is_alive()):
            #Decide here if speech events get overwritten or queued
            self.st = SpeechThread(text)
            self.st.start()
        else:
            self.st = SpeechThread(text)
            self.st.start()
        
