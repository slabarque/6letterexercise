# 6 letter words
There's a file in the root of the repository, input.txt, that contains words of varying lengths (1 to 6 characters).

Your objective is to show all combinations of those words that together form a word of 6 characters. That combination must also be present in input.txt  
E.g.:
``` 
foobar  
fo  
obar
```
should result in the ouput: 
```
fo+obar=foobar
```

Do this twice: the first solution should focus on performance, the second on extensibility.
You can use whatever programming language you want.

Treat this exercise as if you were writing production code; think TDD, SOLID, clean code and avoid primitive obsession.  
For the extensibility part, be mindful of changing requirements like a different maximum combination length, or a different source of the input data.