from ThreadHandler import *
from TextToSpeechEngine import *
from RobotHandler import *
from MarkovGenerator import *
from debug import *

import random

if(not DEBUG):
	import io
	import RPi.GPIO as GPIO
	import time
	INTERVAL_SECONDS = 60

	if __name__ == "__main__":
		tts = TextToSpeechEngine()
		markov = Markov("corpus.txt", 240, 6)
		markov.setup()
		netduino = Netduino()
		count = 1
		while True:
			if((count % INTERVAL_SECONDS) == 1):
				marks = markov.generateText()
				netduino.sendSentence(marks)
				#tts.speak(marks)
				time.sleep(5)
			
			time.sleep(1)
			count+=1
else:   
	print "something"
