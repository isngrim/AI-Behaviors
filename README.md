# AI-Behaviors
Collection of my AI Practice,still a WIP

TODO:
  -Clean up code
  
  -have the ai at least change stopping distance's for combat and patrol
  
  -fix ai aiming bug/figure out what the bug is

Discription of the scripts and their uses (and some ToDos so i dont forget to do it)

AiGunAim.cs:
this script goes on the Camera GameObject,it will point the camera object at its target,simulating a player aiming

Health.cs:
this script is pretty obvious,goes on the parent game object (ToDo: to deal with errors the health script needs to set the game object to inactive instead of destroying it,or i need to find a better way to handle ai target acquisition)

PossesionTestAi.cs:
This is the main Behaviour tree.(ToDo: have the script find the relevent scripts on its own,this should simplify set-up,also remove the references to PossessionTest,should just name the ai TestAi or BasicAi)

Possesion.cs: 
this script lets you swap bodies with an ai,its a part of the prototype this ai was made for and not relevent to the AI functionality,currently doesnt work,need tp change some script refernces to match the new ai systems 

Shoot.cs:
shoots things

SubTreeFactory.cs:
Factory pattern to add new branches to the Behaviour Tree,should make it easier to read the tree.

