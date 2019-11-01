# Socani
![Socani gameplay](https://raw.githubusercontent.com/Entalpi/Socani/master/screenshots/socani-lvl6.gif?token=AAMTLIWEPZPCUN7ZCPEYI3C5YUMTO)

Socani is a free and open source fresh take on the classic [Sokoban](https://en.wikipedia.org/wiki/Sokoban) game. 

## Create custom maps
All maps in Socani are made using the map editor [Tiled](https://www.mapeditor.org/) and are imported with a custom parser at [Level.cs](https://github.com/Entalpi/Socani/blob/master/Assets/Resources/scripts/Level.cs). Tile properties are supported and can be more or less painfree which is nice. :)

All the levels and the tileset is located in the folder [Resources/levels](https://github.com/Entalpi/Socani/tree/master/Assets/Resources/levels). Simply import the tileset and in to Tiled load a level and modify! Go nuts! 🥜

## Changes from Sokoban
- Can move multiple crates 📦
- Multiple crate color with their own goals 🥅
- Coins 💰
- Rewind mechanic as a gameplay mechanic 🕓

## Documentation
In general the source code and comments should be enough. [Here](https://lingtorp.com/2019/04/13/Tile-based-level-representation-in-Unity.html) is a blog post about Socani old level representation for some perspective. 🌞 

## Credits
Thanks to [Kenney](https://kenney.nl/assets/sokoban) for the awesome Sokoban assets.

## License
The MIT License

Copyright (c) 2019 Alexander Lingtorp, https://lingtorp.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.