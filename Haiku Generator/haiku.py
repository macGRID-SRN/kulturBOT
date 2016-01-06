import nltk
from nltk.corpus import cmudict
import curses
from curses.ascii import isdigit

english_stopwords = set(nltk.corpus.stopwords.words('english'))
non_english_stopwords = set(nltk.corpus.stopwords.words())

'''
This method returns the number of syllyables in a string. It takes
in an arbitrary sized string (of non numeric characters)
'''
def numSyllables(text):
    import re
    text = text.lower()
    if filter(str.isdigit, str(text)):
        return 0
    words = nltk.wordpunct_tokenize(re.sub('[^a-zA-Z_ ]', '',text))
    sylCount = 0
    wordCount = 0
    d = cmudict.dict()
    for word in words:
        if word not in d:
            return -1
        if word in ["our", "fire", "hour"]:
            return 1
        sylCount += len([norm(y[-1]) for y in d[word][0] if isdigit(norm(y[-1]))])
    return sylCount

'''
Converts unicode to python string
'''
def norm(text):
    return text.encode('utf-8')
