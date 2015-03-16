class CorpusDictionary:
    def __init__(self, filePath, numChar):
        self.corpus = ""
        with open(filePath, 'r') as myFile:
            self.corpus = myFile.read().replace('\n', '')
        self.cleanCorpus()
        self.numChar = numChar
        self.keyCollection = []
        self.dic = {}

    def cleanCorpus(self):
        self.corpus = self.corpus.replace("\r", "")
        #self.corpus = self.corpus.replace("\n", "")

    def makeDictionary(self):
        for i in range(0, len(self.corpus) - self.numChar):
            tempArray = ""
            for j in range(0, self.numChar):
                tempArray += self.corpus[i + j]
            self.dic.setdefault(tempArray, [])
            self.dic[tempArray].append(self.corpus[i + self.numChar])

    def getDic(self):
        return self.dic

    def toString(self):
        for key in self.dic:
            print(self.dic[key])

