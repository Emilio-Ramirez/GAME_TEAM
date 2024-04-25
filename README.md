# **Recipe Rumble**

## _Game Design Document_

---

"Recipe Rumble" is a trademark of "Instituto tecnológico y de estudios superiores de Monterrey". The game and all its associated content, including but not limited to text, graphics, logos, images, audio clips, digital downloads, data compilations, and software, are the exclusive property of "Instituto tecnológico y de estudios superiores de Monterrey" and are protected by international copyright laws.

No part of this game may be reproduced, distributed, or transmitted in any form or by any means, including photocopying, recording, or other electronic or mechanical methods, without the prior written permission of "Instituto tecnológico y de estudios superiores de Monterrey", except in the case of brief quotations embodied in critical reviews and certain other non-commercial uses permitted by copyright law.

For permission requests, please write to the attention of "Copyright Permissions" at the address below:

"Instituto tecnológico y de estudios superiores de Monterrey"

México

## _Index_

1. [Index](#index)
2. [Game Design](#game-design)
   1. [Summary](#summary)
   2. [Gameplay](#gameplay)
   3. [Mindset](#mindset)
3. [Technical](#technical)
   1. [Screens](#screens)
   2. [Controls](#controls)
   3. [Mechanics](#mechanics)
4. [Level Design](#level-design)
   1. [Themes](#themes)
      1. Ambience
      2. Objects
         1. Ambient
         2. Interactive
      3. Challenges
   2. [Game Flow](#game-flow)
5. [Development](#development)
   1. [Abstract Classes](#abstract-classes--components)
   2. [Derived Classes](#derived-classes--component-compositions)
6. [Graphics](#graphics)
   1. [Style Attributes](#style-attributes)
   2. [Graphics Needed](#graphics-needed)
7. [Sounds/Music](#soundsmusic)
   1. [Style Attributes](#style-attributes-1)
   2. [Sounds Needed](#sounds-needed)
   3. [Music Needed](#music-needed)
8. [Schedule](#schedule)

## _Game Design_

### **Summary**

The game begins with players selecting an event, such as a wedding, a picnic, or a Christmas dinner. Each event features a main dish along with secondary dishes. Players aim to prepare as many dishes as possible, focusing on either the main dish or the secondary ones. However, crafting the main dish is more intricate and yields higher points. Throughout the game, players maintain a hand of four cards. These cards can be combined to create dishes by mixing ingredients or pairing ingredients with utensils to execute preparation methods. Alongside ingredient and utensil cards, there are also special cards that introduce bonuses or penalties, influencing the gameplay dynamics. Each card has a limited number of turns it can remain on the table based on its placement. Once the turns are exhausted, the cards are discarded.

### **Gameplay**

The goal of the game is for the player to score the maximum number of points by creating dishes within the chosen event. The player must strategically combine ingredient and utensil cards to create both secondary dishes and the more complex main dish.

Obstacles:

- Limited hand size: The player can only have 4 cards in their hand at any given time, restricting their options and requiring careful planning.

- Card expiration: Each card placed on the table has a limited number of turns before it expires and is discarded, adding a time pressure element to the gameplay.

- Recipe complexity: Creating the main dish is more challenging than secondary dishes, as it likely requires more specific combinations of ingredients and utensils.
  Tactics:

- Hand management: Players should carefully consider which cards to keep in their hand and which to play, ensuring they have the necessary components to create dishes efficiently.

- Prioritization: Players must decide whether to focus on creating multiple secondary dishes or investing more time and resources into completing the main dish for higher points.

- Card placement: Strategically placing cards on the table can maximize their lifespan, allowing players more time to use them in recipes before they expire.

- Adaptability: As cards expire and new ones are drawn, players must adapt their strategy to make the most of the available ingredients and utensils.


### **Mindset**

The Recipe Rumble card game should challenge players to think critically and creatively while under time pressure. By strategically combining ingredients and utensils within a limited hand size, players must adapt to the changing game state and make quick decisions to create unique dishes. The complexity of recipes and card expirations should create a sense of urgency and satisfaction upon completion. Engaging artwork, balanced difficulty, rewarding sound effects, and a well-designed scoring system will provoke feelings of accomplishment and encourage continued play. The game should ultimately leave players feeling challenged yet satisfied with their strategic decisions and culinary creations.

## _Technical_

---

### **Screens**

1. Title Screen
   1. Start
   2. Recipies
2. Event Select
   1. Wedding
   2. Picnic
   3. Christmas
3. Board
   1. Deck
   2. Objective Dishes
   3. Hand
   4. Cooking Assembly
5. End of Game
   1. Home
   2. Restart
   3. Select event
6. Recipie book
   1. My recipies
   2. Purchase
   3. View Details of recipie

### **Controls**

How will the player interact with the game? Will they be able to choose the controls? What kind of in-game events are they going to be able to trigger, and how? (e.g. pressing buttons, opening doors, etc.)

### **Mechanics**

Are there any interesting mechanics? If so, how are you going to accomplish them? Physics, algorithms, etc.

## _Level Design_

---

_(Note : These sections can safely be skipped if they&#39;re not relevant, or you&#39;d rather go about it another way. For most games, at least one of them should be useful. But I&#39;ll understand if you don&#39;t want to use them. It&#39;ll only hurt my feelings a little bit.)_

### **Themes**

1. Forest
   1. Mood
      1. Dark, calm, foreboding
   2. Objects
      1. _Ambient_
         1. Fireflies
         2. Beams of moonlight
         3. Tall grass
      2. _Interactive_
         1. Wolves
         2. Goblins
         3. Rocks
2. Castle
   1. Mood
      1. Dangerous, tense, active
   2. Objects
      1. _Ambient_
         1. Rodents
         2. Torches
         3. Suits of armor
      2. _Interactive_
         1. Guards
         2. Giant rats
         3. Chests

_(example)_

### **Game Flow**

1. Player starts in forest
2. Pond to the left, must move right
3. To the right is a hill, player jumps to traverse it (&quot;jump&quot; taught)
4. Player encounters castle - door&#39;s shut and locked
5. There&#39;s a window within jump height, and a rock on the ground
6. Player picks up rock and throws at glass (&quot;throw&quot; taught)
7. … etc.

_(example)_

## _Development_

---

### **Abstract Classes / Components**

1. BasePhysics
   1. BasePlayer
   2. BaseEnemy
   3. BaseObject
2. BaseObstacle
3. BaseInteractable

_(example)_

### **Derived Classes / Component Compositions**

1. BasePlayer
   1. PlayerMain
   2. PlayerUnlockable
2. BaseEnemy
   1. EnemyWolf
   2. EnemyGoblin
   3. EnemyGuard (may drop key)
   4. EnemyGiantRat
   5. EnemyPrisoner
3. BaseObject
   1. ObjectRock (pick-up-able, throwable)
   2. ObjectChest (pick-up-able, throwable, spits gold coins with key)
   3. ObjectGoldCoin (cha-ching!)
   4. ObjectKey (pick-up-able, throwable)
4. BaseObstacle
   1. ObstacleWindow (destroyed with rock)
   2. ObstacleWall
   3. ObstacleGate (watches to see if certain buttons are pressed)
5. BaseInteractable
   1. InteractableButton

_(example)_

## _Graphics_

---

### **Style Attributes**

What kinds of colors will you be using? Do you have a limited palette to work with? A post-processed HSV map/image? Consistency is key for immersion.

What kind of graphic style are you going for? Cartoony? Pixel-y? Cute? How, specifically? Solid, thick outlines with flat hues? Non-black outlines with limited tints/shades? Emphasize smooth curvatures over sharp angles? Describe a set of general rules depicting your style here.

Well-designed feedback, both good (e.g. leveling up) and bad (e.g. being hit), are great for teaching the player how to play through trial and error, instead of scripting a lengthy tutorial. What kind of visual feedback are you going to use to let the player know they&#39;re interacting with something? That they \*can\* interact with something?

### **Graphics Needed**

1. Characters
   1. Human-like
      1. Goblin (idle, walking, throwing)
      2. Guard (idle, walking, stabbing)
      3. Prisoner (walking, running)
   2. Other
      1. Wolf (idle, walking, running)
      2. Giant Rat (idle, scurrying)
2. Blocks
   1. Dirt
   2. Dirt/Grass
   3. Stone Block
   4. Stone Bricks
   5. Tiled Floor
   6. Weathered Stone Block
   7. Weathered Stone Bricks
3. Ambient
   1. Tall Grass
   2. Rodent (idle, scurrying)
   3. Torch
   4. Armored Suit
   5. Chains (matching Weathered Stone Bricks)
   6. Blood stains (matching Weathered Stone Bricks)
4. Other
   1. Chest
   2. Door (matching Stone Bricks)
   3. Gate
   4. Button (matching Weathered Stone Bricks)

_(example)_

## _Sounds/Music_

---

### **Style Attributes**

Again, consistency is key. Define that consistency here. What kind of instruments do you want to use in your music? Any particular tempo, key? Influences, genre? Mood?

Stylistically, what kind of sound effects are you looking for? Do you want to exaggerate actions with lengthy, cartoony sounds (e.g. mario&#39;s jump), or use just enough to let the player know something happened (e.g. mega man&#39;s landing)? Going for realism? You can use the music style as a bit of a reference too.

Remember, auditory feedback should stand out from the music and other sound effects so the player hears it well. Volume, panning, and frequency/pitch are all important aspects to consider in both music _and_ sounds - so plan accordingly!

### **Sounds Needed**

1. Effects
   1. Soft Footsteps (dirt floor)
   2. Sharper Footsteps (stone floor)
   3. Soft Landing (low vertical velocity)
   4. Hard Landing (high vertical velocity)
   5. Glass Breaking
   6. Chest Opening
   7. Door Opening
2. Feedback
   1. Relieved &quot;Ahhhh!&quot; (health)
   2. Shocked &quot;Ooomph!&quot; (attacked)
   3. Happy chime (extra life)
   4. Sad chime (died)

_(example)_

### **Music Needed**

1. Slow-paced, nerve-racking &quot;forest&quot; track
2. Exciting &quot;castle&quot; track
3. Creepy, slow &quot;dungeon&quot; track
4. Happy ending credits track
5. Rick Astley&#39;s hit #1 single &quot;Never Gonna Give You Up&quot;

_(example)_

## _Schedule_

---

_(define the main activities and the expected dates when they should be finished. This is only a reference, and can change as the project is developed)_

1. develop base classes
   1. base entity
      1. base player
      2. base enemy
      3. base block
2. base app state
   1. game world
   2. menu world
3. develop player and basic block classes
   1. physics / collisions
4. find some smooth controls/physics
5. develop other derived classes
   1. blocks
      1. moving
      2. falling
      3. breaking
      4. cloud
   2. enemies
      1. soldier
      2. rat
      3. etc.
6. design levels
   1. introduce motion/jumping
   2. introduce throwing
   3. mind the pacing, let the player play between lessons
7. design sounds
8. design music

_(example)_
