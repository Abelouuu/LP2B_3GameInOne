# UFOAttack

## 1. Game Overview and Features

**UFOAttack** is a 2D shooting game in which the player controls a spaceship.  
The main objective is to score as many points as possible by destroying enemies that appear on screen.

The player must survive as long as possible while avoiding enemy projectiles and collecting the different bonuses available during the game.

### Player Objective

The goal of the game is to destroy as many enemies as possible in order to achieve the highest score.

Each enemy gives a certain number of points when defeated. The number of points depends on the enemy’s strength and difficulty: the more dangerous or resistant an enemy is, the more points it gives.

### Controls

| Action | Key |
|---|---|
| Move the player | Arrow keys |
| Main shot | Space bar |
| Use mega bomb | E key |

### Health System and Interface

At the beginning of the game, the player has **10 health points**.

The interface displays important information during the game:

- the player’s health points in the top-left corner;
- the number of available mega bombs at the top-center of the screen;
- the player’s score in the top-right corner.

At the start of the game, a countdown is displayed so that the player has time to get ready before the game actually begins.

### Enemies

The game contains **six different types of enemies**.

Each enemy type has its own behavior, number of health points and point value. Some enemies move across the screen, while others shoot projectiles, use missiles, move in a specific way or directly target the player.

This variety makes the game more dynamic because the player has to adapt their strategy depending on the enemies they face.

### Projectiles and Weapons

The game contains three main types of projectiles.

#### Standard Projectile

The standard projectile is the basic weapon of the game.  
It deals **1 damage point**.

It is used by the player and also by some weaker enemies.

#### Missile

The missile is stronger than the standard projectile.  
It deals **2 damage points**.

There is also a special type of missile, created as a *nested prefab* from the standard missile. This missile can track an enemy for a short time before locking onto its target and then continuing in a straight line.

#### Laser

The laser is the most powerful projectile in the game.  
It deals **3 damage points**.

However, its damage is only active when the laser is in its thick phase, which makes it work differently from the other projectiles.

### Bonuses

Bonuses appear randomly during the game.  
They spawn at more or less regular intervals and move from right to left.

There are five types of bonuses:

| Bonus | Effect |
|---|---|
| Speed boost | Temporarily increases the player’s movement speed |
| Fire rate boost | Temporarily increases the player’s shooting rate |
| Health boost | Restores 3 health points |
| Shield | Creates a shield around the player, protecting them before gradually weakening and being destroyed |
| Mega bomb | Adds a bomb that can be used with the E key and destroys almost all enemies on the map |

Bonuses appear randomly, so it may take more or less time to obtain a specific bonus.

### End of the Game

When the player has no health points left, the game ends and a **Game Over** menu appears.

This menu displays:

- the final score;
- the number of enemies killed for each enemy type.

From this menu, the player can restart the game or return to the main menu.

---

## 2. Additional Features Compared to the Initial Requirements

Several additional features were added to improve the gameplay and make the game more complete.

### Additional Bonuses

Some bonuses were not included in the initial requirements.

The following bonuses were added:

- fire rate boost;
- speed boost;
- shield.

These additions make the gameplay more varied and give the player more options to survive or become more efficient.

### Specific Behavior of the Blue Enemy

The blue enemy has a special shooting pattern.

It fires two missiles: one goes upward and the other goes downward. After a short delay, these missiles explode and each one creates two smaller projectiles.

This behavior makes the blue enemy more dangerous because its shots cover a larger area and force the player to pay more attention to their movements.

### Modified Boss Behavior

The boss behavior was improved to make it harder to defeat.

Instead of moving in a simple way, the boss teleports from point to point on the map. This makes its position less predictable and makes it more difficult for the player to hit it.

The boss also aims at the player before attacking. It has two types of attacks:

1. **Projectile burst**  
   The boss fires several projectiles toward the player, with slight variations in direction.

2. **Laser shot**  
   The boss fires a laser toward the player.

The attack is chosen randomly. However, the projectile burst is more likely to be used than the laser attack.

### Improved Game Over Screen

The Game Over screen does not only display the final score.

It also shows the number of enemies killed for each enemy type. This allows the player to get a more detailed summary of their game and better understand their performance.