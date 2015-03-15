from CorpusDictionary import *

class Markov:
    def __init__(self, fileName, chainSize):
        self.filename = fileName
        self.chainsize = chainSize
        self.corpus = CorpusDictionary(self.filename, self.chainsize)
        self.isParsed = False

    def parseDictionary(self):
        print("______Creating Dictionary_______")
        self.corpus.makeDictionary()
        self.corpus.toString()

    def getChain(self):
        if(self.isParsed == True):
            return "Markov chain here"
        else:
            return "Setup not Called!" 

    def setup(self):
        self.parseDictionary()
        self.isParsed = True

if __name__=="__main__":
    markov = Markov("corpus.txt", 4)
    markov.setup()