# **Recipe Rumble**

## Installation Guide

1. **Clone the Repository**

Clone the repository using HTTPS or SSH:

```
git clone https://github.com/your-username/recipe-rumble.git
```

2. **Navigate to the Project Directory**

```
cd recipe-rumble
```

3. **Run the Node Server**

   1. Navigate to the API_WEB directory:
      ```
      cd api_web
      ```

   2. Create the `.env` file and add the required environment variables:
      ```
      touch .env
      ```

      Add the following variables to the `.env` file:
      ```
      DB_HOST=your_database_host
      DB_PORT=your_database_port
      DB_USERNAME=your_database_username
      DB_PASSWORD=your_database_password
      DB_DATABASE=your_database_name
      API_PORT=3000
      ```

   3. Install the dependencies and start the server:
      ```
      npm install
      npm start
      ```
      Make sure not to close the terminal window.

4. **Run the Game**

   1. Open Unity Hub.
   2. Click on the "Add" button in the top right corner of the screen.
   3. Select the "RecipeRumble" folder from the project directory.
   4. Click "Open" to open the project in Unity.
   5. In the Unity Project window, navigate to the "Scenes" folder and open the "principal menu" scene.
   6. Click the "Play" button to start the game.

The game should now be running, and you can interact with it. Make sure to follow any additional instructions or documentation provided for the game.

## Inside the event

- Currently the only event with full functionality is "Picnic" event . Once in picnic event you can see there are several buttons. The start button shows the game. The deck button cannot be used if the start button is not pressed. We also provided the arrows button which shows the 4 ingridient combination you can make in the game(this button is enabled throughout the entire game). And finally the menu button which leads you to the game menu.

- Once you start playing, there will be 3 parts, the panel closest to the keyboard is your hand. You need to drag the cards to the middle part of the game where the white containers are. This containers have limited tuns before being descarted. The first container starting from the left hand has 1 tun available and the truns inrease by one endind with the right hand container which has 4 turns available.

- A turn is defined by the pressed deck button which you can find it in the lower left corner of the screen. This button is enabled if you drag and drop a card in the containers or if you change a card from one container to another.

- The cards can only move throughout the middle white containers, but they cannot be droped in the player hand again.

- The combinations must have the 4 types of ingridients (1 protin, 1 side, 1 utensil and 1 vegetable). These combinatios can aldo have alternative ingridients so you can add more points.

- The score can be found in the upper right corner and is calculated with the addition of nutritional value written in the cards.

- You have 5 minutes to make the most combinations possible, you can consulte how much time you have left in the upper right corner.


## _Game Design Document_
**Team:** Lisette Melo Reyes, Sebastián Borjas Lizardi and Emilio Ramírez Mascarúa

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
  
### **Images**
The videogame aesthetic will be cartoon animated themed with minimalistic materials 

[Background Images](RecipieRumble/Assets/Images)

[Sketches](https://github.com/Emilio-Ramirez/GAME_TEAM/assets/77208976/489f630f-9534-4975-afbd-b73bd34d6928)

[Assets](RecipieRumble/Assets) 


### **List of program classes**
1. Card Generation System
2. Hand of Card Management
3. User Interface (UI)
4. Card Movement and Action Mechanics
5. Card Combination System
6. Scoring and Redemption System
7. Special and Sabotage Card System
8. Game Event Management
9. Save and Load System


### **Controls**

How will the player interact with the game? Will they be able to choose the controls? What kind of in-game events are they going to be able to trigger, and how? (e.g. pressing buttons, opening doors, etc.)

### **Mechanics**

1.Event Selection:
 - Upon starting the game, the player selects an event from the available options, including a wedding, picnic, or Christmas dinner. Each event has a specific menu that includes a main dish and several secondary dishes.

2.Deck of cards:
 - Depending on the event chosen, the player receives a deck of cards designed specifically for that event. This deck includes cards of ingredients and utensils necessary to prepare the dishes of the event.
The player draws cards from his deck to form a hand of cards that he will use during the game.

3. Combination of Cards and Preparation of Dishes:
 - Players can combine ingredient cards and utensils to prepare dishes based on specific recipes provided in the game.
 - For example, to prepare a Mediterranean chicken sandwich, you will need the menus of whole wheat bread, chicken, tongs or grill, feta cheese or cherry tomatoes. Some of these ingredients or utensils will have more than 1 possible option to play on the dish so that the player can make more combinations. By using these cards together, the player will prepare the dish and earn points based on the nutritional value of the ingredients.

4. Special Cards:
 - The deck also includes special cards that can alter the flow of the game. These cards can have positive effects such as doubling the duration of cards in play, or negative effects such as blocking card combinations.
 - Example: A special "Extra Time" card grants the player additional time to complete their recipes before the game time runs out.
 - Special cards will come out randomly from the deck and will be applied automatically when they come into play.
   
5. Discard and Card Management:
 - Each card can remain in play for only a limited number of turns on the game table before being automatically discarded.
 - Every time the players are missing a card, they can select the deck to add the missing cards to their hand.

 6. Points Accumulation:
 - Points are accumulated by successfully preparing dishes. The use of ingredients with high nutritional value in a dish increases the number of points earned.
   
 7. Recipe Unlocking:
 - As players accumulate points, they can reach thresholds that allow them to unlock new recipes, which can provide more valuable ingredient combinations and the opportunity to earn more points.

9. End of the game:
The game concludes when specific victory conditions are met, such as achieving a set number of points or completing all required dishes in the event.
At the end of the game, the results and the player's total score are displayed.
## _Level Design_

---

_(Note : These sections can safely be skipped if they&#39;re not relevant, or you&#39;d rather go about it another way. For most games, at least one of them should be useful. But I&#39;ll understand if you don&#39;t want to use them. It&#39;ll only hurt my feelings a little bit.)_

### **Themes**

1. Pincnic
   1. Mood
      1. Bright, Calm, Animated
   2. Objects
      1. _Ambient_
         1. Picnic tablemoth  
         2. Chef
         3. Picnic Objects
      2. _Interactive_
         1. Cards
         2. Deck
2. Wedding
   1. Mood
      1. Happy, Colors, Animated
   2. Objects
      1. _Ambient_
         1. Wedding Decorations
         2. Chef
      2. _Interactive_
         1. Cards
         2. Deck
3. Christmas Dinner
   1. Mood
      1. Festive, Lights, Animated
   2. Objects
      1. _Ambient_
         1. Christmas Setting 
         2. Chef
      2. _Interactive_
         1. Cards
         2. Deck

_(example)_

### **Game Flow**
1. The player reviews his hand and the cards in play to understand the current state of the board, taking into account any special or sabotage cards that have been activated automatically when drawn from the deck.
2. The player chooses a card from their hand, which can be an ingredient, a utensil
3. The player places the card on the table, activating it if it has immediate effects or preparing it for future use in recipes.
4. When you draw from the deck, any special or sabotage cards are automatically activated.
5. These cards can alter the game by affecting all players' cards in play, restricting actions, or significantly changing the dynamics of the game.
6. If the player has the necessary cards on the table for a recipe from the book, he combines them.
7. This action "cooks" the dish, granting the player nutritional points based on the recipe fulfilled.
8. If a card in the player's hand is not useful at the moment, they can choose to discard it ny placing themn in the instant descard table place to potentially receive better cards on future turns.
9. Each time the player draws a card from the deck, he evaluates its immediate or potential usefulness for future turns.
10. If the drawn card is a special or sabotage card, it is activated immediately, requiring a quick adaptation of the player's strategy.
11. Verify that all actions have been completed properly before the time ends.

_(example)_

## _Development_

---

### **Abstract Classes / Components**

1. **CardBase**
   - Represents the base class for all game cards.
   - Common methods like `Draw()`, `Play()`, and `Discard()`.
2. **UIElementBase**
   - Base for all user interface elements.
   - Common methods like `Show()`, `Hide()`, and `Update()`.
3. **GameEventBase**
   - Base class for game events.
   - Common methods like `StartEvent()`, `EndEvent()`, and `UpdateEvent()`.

### **Derived Classes / Component Compositions**

1. **CardBase**
   - **IngredientCard**
     - `ProtinCard`
     - `SideCard`
     - `VegetableCard`
   - **UtensilCard**
   - **SpecialCard**
     - `BonusCard`
     - `PenaltyCard`
2. **UIElementBase**
   - **Button**
     - `StartButton`
     - `DeckButton`
     - `ArrowsButton`
     - `MenuButton`
   - **Panel**
     - `HandPanel`
     - `GamePanel`
     - `ScorePanel`
   - **Text**
     - `TimerText`
     - `ScoreText`
3. **GameEventBase**
   - **PicnicEvent**
   - **WeddingEvent**
   - **ChristmasDinnerEvent**

## _Graphics_

---

### **Style Attributes**

**Graphic Style:**
- **Color Palette:**
  - Bright and cheerful colors for events like picnic and wedding.
  - Warm and festive colors for the Christmas dinner.
- **Style:**
  - Animated and cartoonish theme.
  - Minimalistic materials.
  - Solid, thick outlines with flat hues.
  - Emphasis on smooth curves over sharp angles.

**Visual Feedback:**
- **Interaction:**
  - Glow or shadow to indicate an interactive element.
  - Visual effects like flashes or color changes to indicate correct actions or errors.

### **Graphics Needed**

1. **Characters**
   - **Chef** (various interaction animations)
   - **Interactive Cards**
     - `ProtinCard`
     - `SideCard`
     - `VegetableCard`
     - `UtensilCard`
2. **Ambient Objects**
   - **Picnic Setting**
     - Picnic table
     - Picnic cloth
   - **Wedding Setting**
     - Wedding decorations
   - **Christmas Setting**
     - Christmas setting
3. **User Interface**
   - Buttons (start, deck, arrows, menu)
   - Panels (hand, game, score)
   - Texts (timer, score)

## _Sounds/Music_

---

### **Style Attributes**

**Music:**
- **Instruments:**
  - Use of cheerful instruments like guitar, ukulele for the picnic.
  - Orchestral sounds and bells for the wedding.
  - Festive melodies and choirs for the Christmas dinner.
- **Genre:**
  - Lively and cheerful.
  - Festive and exciting.
- **Sound Effects:**
  - Exaggerated for actions (e.g., bell sounds when completing a recipe).
  - Realistic and short for basic interactions (e.g., click sound when pressing a button).

### **Sounds Needed**

1. **Effects**
   - Soft footsteps (picnic floor)
   - Festive sounds (wedding and Christmas floors)
   - Card interaction sounds
   - Success and failure sounds for recipe combinations
2. **Feedback**
   - Happy chime (when completing a recipe)
   - Sad chime (when discarding a card)
   - Timer sound (countdown)
   - Score sound (when updating the score)

### **Music Needed**

1. **Tracks**
   - Lively and cheerful melody for the picnic.
   - Exciting and festive music for the wedding.
   - Festive and Christmas melody for the Christmas dinner.

## _Schedule_

---

1. **Development of Base Classes**
   - **CardBase**
     - Development of derived classes (IngredientCard, UtensilCard, SpecialCard).
   - **UIElementBase**
     - Development of buttons, panels, and texts.
   - **GameEventBase**
     - Development of specific events (PicnicEvent, WeddingEvent, ChristmasDinnerEvent).

2. **Development of Application State**
   - **Game World**
     - Implementation of game logic.
   - **Menu World**
     - Implementation of menu logic.

3. **Development of Player and Basic Block Classes**
   - Physics and collisions.

4. **Find Smooth Controls/Physics Systems**
   - Adjustment and testing of controls.

5. **Development of Other Derived Classes**
   - **Blocks**
     - Animated, falling, breaking.
   - **Enemies**
     - Implementation of enemies if necessary.

6. **Level Design**
   - Introduction of movement and jumping mechanics.
   - Introduction of card combination mechanics.

7. **Sound Design**
   - Creation and implementation of sound effects.

8. **Music Design**
   - Composition and implementation of music tracks.
