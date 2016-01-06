from markov import *
from haiku import *
import time

markov = Markov(open('data/corpus.txt','r'))

def main():
    for i in range(200000):
        firstLine = getNSyllableLine(5)
        print firstLine
        secondLine = getNSyllableLine(7)
        print secondLine
        thirdLine = getNSyllableLine(5)
        print thirdLine
        print "That was the " + str(i) + "th iteration"
        with open("generated/" + str(int(time.time())) + ".txt", "w") as f:
            f.write(firstLine + "\n" + secondLine + "\n" + thirdLine)

def getNSyllableLine(n):
    while True:
        for i in range(n*3):
            text = markov.generate_markov_text(i)
            sylCount = numSyllables(text)
            if sylCount < n:
                continue
            if sylCount == n:
                return text
            if sylCount > n:
                break
            #Catch special error case
            if sylCount == -1:
                break

if __name__=="__main__":
    main()
