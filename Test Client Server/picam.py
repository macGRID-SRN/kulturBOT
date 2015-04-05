from ThreadHandler import *
from TextToSpeechEngine import *
from RobotHandler import *
from debug import *

#add path for markov libraries
import sys
sys.path.insert(0, '/markov')

import MarkovGenerator

import random

if(not DEBUG):
	import io
	import RPi.GPIO as GPIO
	import time
	INTERVAL_SECONDS = 60

	if __name__ == "__main__":
		tts = TextToSpeechEngine()
		markov = Markov("corpus.txt", 50, 4)
		markov.setup()
		netduino = Netduino()
		count = 1
		while True:
			if(not (count % INTERVAL_SECONDS)):
				marks = markov.generateText()
				netduino.sendSentence(marks)
				tts.speak(marks)
				time.sleep(10)
			
			time.sleep(1)
			count+=1
else:   
	print "something"
