import random
 
class Markov(object):
    
        def __init__(self, open_file):
          self.cache = {}
          self.open_file = open_file
          self.characters = self.file_to_words()
          self.character_count = len(self.characters)
          self.database()
         
        
        def file_to_words(self):
            self.open_file.seek(0)
            data = self.open_file.read()
            characters = list(data)
            return characters
            
        
        def triples(self):
            if len(self.characters) < 3:
                return
          
            for i in range(len(self.characters) - 2):
                yield (self.characters[i], self.characters[i+1], self.characters[i+2])
                 
        def database(self):
            for c1, c2, c3 in self.triples():
                key = (c1, c2)
                if key in self.cache:
                    self.cache[key].append(c3)    
                else:
                    self.cache[key] = [c3]
                          
        def generate_markov_text(self, size=140):
           punctuation = ["!","?","."]
           seed = random.randint(0, self.character_count-3)
           seed_character, next_character = self.characters[seed], self.characters[seed+1]
           c1, c2 = seed_character, next_character
           gen_characters = []
           notLongEnough = True;
           sentence = ''
           while(notLongEnough):
               gen_characters.append(c1)
               c1, c2 = c2, random.choice(self.cache[(c1, c2)])
               gen_characters.append(c2)
               if (len(' '.join(gen_characters)) > 140):
                   del gen_characters[-1]
                   sentence = ' '.join(gen_characters) + punctuation[random.randint(0,2)]
                   notLongEnough = False
               else:
                   continue
           #I know the below stuff is silly, TODO find a more elegent way to format it
           sentence = sentence.replace('   ', '**')
           sentence = sentence.replace(' ', '')
           sentence = sentence.replace('**', ' ')
           return sentence.capitalize()
