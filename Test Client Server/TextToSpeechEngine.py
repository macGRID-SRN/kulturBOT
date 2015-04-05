from ThreadHandler import *

class TextToSpeechEngine:
    def __init__(self):
        print "tts init called"
    def speak(self, text):
       #Decide here if speech events get overwritten or queued
        st = SpeechThread(text)
        st.start()
'''       
if __name__ == "__main__":
    tts = TextToSpeechEngine()
    tts.speak("test text")
'''