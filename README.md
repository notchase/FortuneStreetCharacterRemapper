This program is designed for making Custom Characters in Fortune Street / Boom Street.

Custom Characters have to be placed over an existing "Base Character". 

Once a Custom Characters has been made, this program will quickly replace the "Base Character".



An Example of creating the Custom Character Ganon from SSBB over "Base Character" Mario in Fortune Street


-Extract Super Smash Brothers Brawl and Fortune Street


-Load "SSBB/files/fighter/ganon/FitGanon00.pac" into BrawlCrate

  -Export:
  
    -Model Data [0]
    
    -Texture Data [0]
    

-Load "SSBB/files/fighter/ganon/FitGanonMotionEtc.pac" into BrawlCrate

  -Export: 
  
    -FitGanonMotion
    

-Make a copy of "FS/chara/ch_nt_mro.brres" named "Ganon_over_mro.brres"


-Load "Ganon_over_mro.brres" into BrawlCrate

  -import Textures: select all from previous
  
    -Game will run if old textures are left
    
  -replace ch_nt_mro.mdl0 with FitGanon00.mdl0
  
  -select ch_nt_mro
  
     -change G3D Node version from 9 to 11
     
  -Replace animations one at a time
  
    -(example: ch_nt_mro_buys.chr0 -> AppealHi.chr0)
    
    -Change G3D Node version from 4 to 5
    
    -Opening up the original FitGanon00.pac and FitGanonMotionEtc.pac in BRRESViewer can be helpful to chose a good animation 
    
  -save Ganon_over_mro.brres
  

-Open FortuneStreet.iso in WiiScrubber or some other tool

  -Replace "FS/chara/ch_nt_mro.brres" with "Ganon_over_mro.brres"
  

-Boot the game




Using FSCharacterRemapper

-Load an Existing FS Custom Character .brres file

-Select a new "Base Character" from Character Slot drop down menu

-Save as a new .brres file

-Open FortuneStreet.iso in WiiScrubber or some other tool

-Replace "FS/chara/[Base].brres" with "[Custom_over_Base].brres"
