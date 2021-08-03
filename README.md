#  3D Dungeon Generator (Unity)
## Info
This project is part of my bachelor thesis or research paper for my degree at [Howest](https://www.howest.be/en/programmes/bachelor/digital-arts-and-entertainment) [DAE](https://www.digitalartsandentertainment.be) (Digital Arts & Entertainment) the title of the paper is: **"Procedural generation of 3D Dungeons with interesting height variations"**, more information about the project or the paper can be found on [my website](https://danielpatyk.wixsite.com/game-dev-portfolio?lang=en).


## Abstract (from my research Paper)
In this paper, various methods are examined for their suitability for generating 3D dungeons with height variation. [...] From the methods examined, the space-testing method was selected as a suitable starting point for investigating the problems of generating 3D dungeons with height variation. [...] The method still has room for improvement, both in terms of speed and influence on the generation. [...]

## Disclaimer
The method used and its implementation served as a proof-of-concept and was implemented within two weeks, so **I do not recommend anyone to adopt the code as it is.**

## Limitations and possible improvements
### 1. Faster room placement verification
#### 1.1 Problem
Currently, testing whether a room can be placed relies on methods from unity that are physics-based. This is one of the reasons why the process is very slow. Because each time at least 1 frame has to be waited before continuing with the generation.

#### 1.2 Possible solution
Therefore I recommend to implement your own methods to check if a room can be placed, which directly access the dimensions or size of the room and does not use the physics based overlapping checks of collision boxes.

### 2. Faster room connection linking
#### 2.1 Problem
At the moment, each room checks with small triggers and box colliders whether it is already connected to an adjacent room. Since this is also physically based, it slows down the process more and more the more rooms are not yet connected everywhere.

#### 2.2 Possible solution
Therefore, the connection should be communicated in a different way. The best way would be to create a **node system** in the background that saves the connections of all rooms, this network of rooms that is created through this can then also be saved and used to quickly generate the same dungeon based on the saved network.

### 3. Limiting parameters
#### 3.1 Problem
At the moment, a dungeon can theoretically become infinitely large, because there are no restrictions in the 3 axes and also no limits on how often a room is allowed to occur at maximum.

I have changed the weighting of certain rooms by adding the same room in the same list of possible rooms multiple times, which is not an optimal solution to increase the appearance of a certain room.

#### 3.2 Possible solution
Introduce dungeon parameters that limit, for example, the size, number and percentage of rooms used. If a room is to be placed, a check would still have to be made to see whether it is still within the boundary, otherwise a dead end should be generated instead.

Also, for example, rooms that have a particularly large number of connection paths should be limited in number or frequency by either limiting the max amount that can be used or by giving it a percentage of how often they can appear compared to the rest.

### 4. Room shape
#### 4.1 Problem
At the moment, the room shape is limited to cuboids, because I only check one box collider per room to see if a room can be placed or not.

#### 4.2 Possible solution
To fix this issue this should be extended to several box colliders or custom colliders or a different way of representing a room and checking its collision. This would allow more room variation.

### 5. More speed through spactial partitioning
#### 5.1 Problem
Currently, all rooms are checked to see if they collide with the current room to be placed, even though most rooms are too far away to be relevant.

#### 5.2 Possible solution
To fix this problem, use spatial partitioning to only check rooms for collision that are in the immediate neighbourhood, this would drastically increase the speed.

### 6. Using occlusion culling
Since the number of rooms increases relatively quickly, it is worthwhile to use occlusion culling in order to work more smoothly in the editor and to get more fps in the game.

### 7. Optimisation of geometry
In order to see the rooms better from the outside, I have used models that are visible from all sides, but if, like this dungeon generator, the whole thing is designed for first person, you should of course avoid unnecessary geometry and use first person optimised models.

### 8. Missing features for the demo
- Simple camera control
- First person mode
- In-Game dungeon configuration
- Debug View Toggle

## Future of this project
As you can see from the list, there is still a lot of room for improvement and certainly more to come, so for me the project is not finished yet and I will come back to it. At the moment, however, the project is on hold.
