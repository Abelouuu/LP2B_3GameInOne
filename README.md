LP2B project realized by Abel Haller and Abdelrahim Yakhlef

Link to download the asset package: https://we.tl/t-TE8ZcUDFAwTaXRwJ
this link expires on June 27, 2026, at 7:00 p.m.

# Mini-Games Project 3 Game In 1

This project contains three mini-games developed in Unity.
Each game has its own gameplay mechanics, additional features, and technical improvements.

The goal of this README is not to fully explain the basic rules of each game, but rather to highlight the main systems and improvements implemented during development.

---

# 1. UFOAttack

## Quick Overview

**UFOAttack** is a 2D shooting game in which the player controls a spaceship and must survive while destroying enemies.
The player can move with the arrow keys, shoot with the space bar, and use a mega bomb with the `E` key.

The game includes several enemy types, different projectile categories, bonuses, a score system, and a Game Over screen showing the final performance.

## Main Added Features

### Multiple Enemy Behaviors

The game includes several enemy types, each with a specific behavior, amount of health, and score value.

Some enemies move in simple patterns, while others use more complex attacks such as missiles, lasers, teleportation, or direct targeting of the player.

### Improved Blue Enemy Attack

The blue enemy has a custom attack pattern.
It fires two missiles: one travels upward and the other downward. After a short delay, both missiles explode and generate smaller projectiles.

This creates a wider danger zone and forces the player to react more carefully.

### Advanced Boss-Type Enemy

A stronger enemy type was added with a more complex behavior than standard enemies.
This enemy can teleport between different positions, aim at the player, and randomly choose between different attacks.

Its attacks include:

* projectile bursts aimed at the player;
* laser attacks;
* unpredictable positioning through teleportation.

### Recurring Boss Event

A special boss phase was added to make the game more dynamic.

At regular intervals during (1 min) the game:

* the normal enemy spawn is paused;
* enemies currently on screen evacuate the play area;
* the game transitions into a boss phase;
* a powerful event boss appears;
* after the boss is defeated, the normal game resumes.

This makes the game feel less repetitive and creates important moments during the run.

### Boss Music Transition

During the boss event, the normal background music smoothly transitions into a dedicated boss music track.

When the boss is defeated, the game smoothly returns to the normal music.
This reinforces the feeling of entering and leaving a special game phase.

### Boss Health Bar

A boss health bar was added to the interface.

It includes:

* the boss name displayed above the bar;
* a slider showing the remaining boss health;
* a fade-in animation when the boss appears;
* a fade-out animation when the boss is defeated.

This makes the boss event clearer and more impactful for the player.

### Bonus System

Several bonuses can appear during the game and move from right to left.

The available bonuses are:

* speed boost;
* fire rate boost;
* health bonus;
* shield;
* mega bomb.

Some of these bonuses were added beyond the initial requirements, especially the speed boost, fire rate boost, and shield.

### Mega Bomb

The player can collect mega bombs and activate them using the `E` key.
The mega bomb destroys most enemies currently present on the map, giving the player a powerful emergency option.

For boss enemies, the bomb does not necessarily kill them instantly but can deal significant damage instead.

### Detailed Game Over Screen

The Game Over screen displays more than just the final score.
It also shows how many enemies of each type were defeated.

This gives the player a clearer summary of their performance.

---

# 2. CatchBoy

## Quick Overview

**CatchBoy** is an apple-catching mini-game.
The player moves horizontally and tries to catch falling fruits within a limited game duration.

The game was expanded with new fruit types, a controlled randomization system, and improved feedback.

## Main Added Features

### Three Fruit Types

The game includes three different fruit types:

* **Red Apple**: standard fruit, gives `+1` point;
* **Rotten Apple**: negative fruit, gives a `-2` point penalty;
* **Golden Apple**: rare bonus fruit, gives `+2` points.

The score cannot go below zero, which prevents frustrating negative scores.

### Virtual Bag System

A “virtual bag” system was implemented to control randomness.

Instead of spawning fruits with pure random probability, the spawner creates a bag of 10 fruits with a fixed distribution:

* 7 red apples;
* 2 rotten apples;
* 1 golden apple.

The bag is then shuffled, and fruits are spawned one by one.
When the bag is empty, a new one is generated.

This prevents unfair random streaks, such as too many rotten apples in a row or no golden apples for a long time.

### Balanced Probability System

The fruit distribution is controlled as follows:

* **70%** red apples;
* **20%** rotten apples;
* **10%** golden apples.

This keeps the game balanced while still allowing variety during gameplay.

### Audio Feedback

Different audio feedback was added depending on the collected fruit.

Positive fruits use the standard collection sound, while rotten apples use a different sound at a lower volume to avoid being too aggressive for the player.

### Lightweight UI Using TextMeshPro

The game uses world-space `TextMeshPro` components to display information such as score and timer.

This avoids relying on heavier Canvas structures and keeps the interface simple and efficient.

### Persistent Score for Game Over

The final score is stored and transferred to the Game Over scene using `PlayerPrefs`.

This allows the Game Over screen to display the player’s final result after the gameplay scene ends.

---

# 3. Brick Breaker

## Quick Overview

**Brick Breaker** is a classic brick-breaking game where the player controls a paddle and destroys bricks using a ball.

The game includes power-ups, hazards, improved ball physics, score tracking, and end-game feedback.

## Main Added Features

### Drop System

When bricks are destroyed, they can drop special items.
These items are divided into bonuses and maluses.

The system uses 2D trigger physics to detect item collection accurately.

### Power-Ups

Two positive items were added:

* **Blue Star**: temporarily increases the paddle speed and changes its visual appearance;
* **Heart Item**: restores one life, up to a maximum of 3 lives.

These bonuses make the gameplay more dynamic and give the player opportunities to recover or gain an advantage.

### Hazards

Two negative items were added:

* **Red Skull**: instantly removes one life and resets the ball;
* **Black Skull**: temporarily slows down the paddle and changes its visual appearance.

These hazards add risk and force the player to react quickly.

### Anti-Stacking Malus System

A safeguard was added to prevent unfair difficulty spikes.

If the paddle is already affected by the Black Skull slow effect, the drop system prevents additional Black Skulls from appearing until the effect ends.

This prevents the player from being repeatedly punished by the same malus.

### Angular Reflection System

The ball reflection is not random.

The bounce angle depends on where the ball hits the paddle:

* hitting the center creates a more vertical bounce;
* hitting the edges creates a sharper horizontal angle.

This rewards precise paddle positioning and gives the player more control over the ball trajectory.

### Reactive UI

The game includes UI elements that update in real time, including:

* current score;
* remaining lives;
* final score on victory or defeat screens.

This improves clarity during gameplay and gives immediate feedback to the player.

### Victory and Game Over Feedback

When the player wins or loses, the game stops the normal gameplay sounds and plays a dedicated audio effect for the final state.

This makes the end of the game clearer and more polished.

---

# Conclusion

The three mini-games were improved with additional gameplay systems, better feedback, and more polished mechanics.

The main improvements include:

* new enemy and boss behaviors in **UFOAttack**;
* controlled random generation and fruit variety in **CatchBoy**;
* power-ups, hazards, and improved ball physics in **Brick Breaker**.

These additions make the games more complete, more balanced, and more enjoyable to play.
