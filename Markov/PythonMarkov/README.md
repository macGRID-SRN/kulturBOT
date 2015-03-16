# Pseudo random text generation #

This document outlines the use of MarkovGenerator.py in generating pseudo random text
***

## Steps
-   Import the module to your python program:
```
from MarkovGenerator.py import * 
```

- Create a Markov object:
```
 markov = Markov(pathToCorpusFile, characterLimit, chainSize ) 
```

pathToCorpusFile is the relative path to the corpus file

characterLimit is the maximum allowable character count of the final pseudo-random phrase (ex/ for twitter may want to set it to 144), this does not guarentee a minimum, only a maximum.

chainSize decides the chain size to use, smaller -> less context, bigger -> more context. Play around with it to find best results

- call setup to parse corpuse with:
```
markov.setup()
```


- Finally, to generate the text call:
```
markov.generateText()
``` 
