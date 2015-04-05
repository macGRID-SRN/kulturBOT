from CorpusDictionary import *
from random import randint

class Markov:
    def __init__(self, fileName, characterLimit, chainSize):
        self.filename = fileName
        self.chainsize = chainSize
        self.corpus = CorpusDictionary(self.filename, self.chainsize)
        self.isParsed = False
        self.characterLimit = characterLimit

    def parseDictionary(self):
        self.corpus.makeDictionary()

    def getChain(self):
        if(self.isParsed == True):
            phrase = ""
            dic = self.corpus.getDic()
            keys = list(dic.keys())
            numKeys = len(keys)
            startingKeyIndex = randint(0, numKeys - 1)
            startingKey = keys[startingKeyIndex]
            character = dic[startingKey][randint(0,len(dic[startingKey]) - 1)]
            phrase += character
            while(len(phrase) < self.characterLimit - 1):
                startingKey += character
                startingKey = startingKey[1:]
                try:
                    character = dic[startingKey][randint(0,len(dic[startingKey]) - 1)]
                except:
                    startingKeyIndex = randint(0, numKeys - 1)
                    startingKey = keys[startingKeyIndex]
                    character = dic[startingKey][randint(0,len(dic[startingKey]) - 1)]
                phrase += character
            return phrase
        else:
            return "Setup not Called!" 

    def setup(self):
        self.parseDictionary()
        self.isParsed = True

    def generateText(self):
        punctuation = ['.', '!', '?'] 
        text = self.getChain()
        punctuationIndex = randint(0, 2)
        text += punctuation[punctuationIndex]
        return text.capitalize()


#if __name__=="__main__":
    # Markov(1:2:3)
    '''
    1: name of file to parse as corpus dictionary
    2: maximum length of text chain
    3. chain size (shorter => less context)
    '''
 #   markov = Markov("corpus.txt", 144, 4)
 #   markov.setup()
 #   print(markov.generateText())