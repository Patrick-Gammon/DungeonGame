using System;
using SplashKitSDK;
using System.Collections.Generic;

// DungeonGame controls the game functions
public class DungeonGame
{
    public bool quit = true;
    private Player _player;
    private Floor _floor;
    public int _floorNumber = 1;
    public int _storeCounter = 1;
    public int _enemiesDefeated = 0;
    public int _score = 0;
    public bool _levelUp;
    private Window _gameWindow;
    public bool mouseOccupied = false;
    bool _fightState = false;
    int doFight;
    bool _inStore = false;
    bool swingAnim = false;
    public Item mousedItem = null;
    int _damageTaken = 0;
    int _animCycle;
    int _numberLifetime;
    bool moveMonsters;

    Bitmap weaponSwing0 = new Bitmap("weaponSwing0", "weaponSwing0.png");
    Bitmap weaponSwing1 = new Bitmap("weaponSwing1", "weaponSwing1.png");
    Bitmap weaponSwing2 = new Bitmap("weaponSwing2", "weaponSwing2.png");
    Bitmap weaponSwing3 = new Bitmap("weaponSwing3", "weaponSwing3.png");
    Bitmap weaponSwing4 = new Bitmap("weaponSwing4", "weaponSwing4.png");
    Bitmap panels = new Bitmap("panels", "panels.png");
    Bitmap storeBackground = new Bitmap("storeBackground", "storeBackground.png");
    Bitmap background = new Bitmap("background", "background.png");
    Bitmap wall_left = new Bitmap("wall left", "wall-left.png");
    Bitmap wall_right = new Bitmap("wall right", "wall-right.png");
    Bitmap wall_front = new Bitmap("wall front", "wall-front.png");
    Bitmap wall_ahead = new Bitmap("wall ahead", "wall-ahead.png");
    Bitmap wall_ahead_left = new Bitmap("wall ahead left", "wall-ahead-left.png");
    Bitmap wall_ahead_right = new Bitmap("wall ahead right", "wall-ahead-right.png");
    Bitmap levelUpScreen = new Bitmap("levelUpScreen", "levelUpScreen.png");

    // Constructor for DungeonGame; creates a new player
    public DungeonGame(Window gameWindow)
    {
        // Initiliase the window
        _gameWindow = gameWindow;
        _player = NewPlayer(_floor);
        _floor = NewFloor(_player);
        _floor.Populate();
        _animCycle = 0;
        doFight = -1;
    }

    // Handles the events that occur each turn
    public void TurnEvents()
    {
        // Check if the player is standing in a monster
        if (_floor.Rooms[_player._location]._monster != null)
        {
            _fightState = true;
            moveMonsters = false;
        }
        else
        {
            _fightState = false;
        }

        // Check if the players health is above maximum
        if (_player.Health > _player.MaxHealth)
        {
            _player._health = _player.MaxHealth;
        }

        // Check if the players magic is above maximum
        if (_player.Magic > _player.MaxMagic)
        {
            _player._magic = _player.MaxMagic;
        }

        // Check if the player can level up
        if (_score >= 100)
        {
            _score -= 100;
            _levelUp = true;
        }

        // Reveal surrounding rooms
        if (_player._location > 20)
        {
            Player._roomsExplored[_player._location - 21] = true;
            Player._roomsExplored[_player._location - 20] = true;
            Player._roomsExplored[_player._location - 19] = true;
            Player._roomsExplored[_player._location - 1] = true;
        }
        if (_player._location < 379)
        {
            Player._roomsExplored[_player._location + 1] = true;
            Player._roomsExplored[_player._location + 19] = true;
            Player._roomsExplored[_player._location + 20] = true;
            Player._roomsExplored[_player._location + 21] = true;
        }
        Player._roomsExplored[_player._location] = true;

        // Check if the player is on the store
        if (_player._location == _floor._store)
        {
            _inStore = true;
        }
        else
        {
            _inStore = false;
        }

        // Reset the store counter
        if (_storeCounter > 3)
        {
            _storeCounter = 1;
        }
        
        // Check if the monsters need to be moved
        if (moveMonsters)
        {
            MonsterMove(_floor);
            moveMonsters = false;
        }
    }

    // Draws the window and objects
    public void Draw()
    {
        _gameWindow.Clear(Color.White);
        _gameWindow.DrawBitmap(panels, 0, 0);

        // Draw the health
        float currentHealth = _player.Health;
        float maxHealth = _player.MaxHealth;
        float healthRatio = 150 * (currentHealth / maxHealth);
        SplashKit.FillRectangle(Color.Red, 46, 462, healthRatio, 24);

        // Draw the magic
        float currentMagic = _player.Magic;
        float maxMagic = _player.MaxMagic;
        float magicRatio = 150 * (currentMagic / maxMagic);
        SplashKit.FillRectangle(Color.Blue, 46, 497, magicRatio, 24);

        // Draw the game window
        // If the player is in the store, draw the store instead
        if (_inStore)
        {
            _gameWindow.DrawBitmap(storeBackground, 3, 3);
            List<Item> storeList = _floor.Rooms[_floor._store]._RoomItems;
            for (int i=0;i<storeList.Count;i++)
            {
                if (storeList[i] != null) { _gameWindow.DrawText("$"+ storeList[i]._price, Color.Black, storeList[i].x - 10, storeList[i].y + 40); }
            }
        }
        // Otherwise draw the dungeon
        else
        {
            _gameWindow.DrawBitmap(background, 3, 3);
            // Check to see what is in front of the player
            bool roomFront = false;
            bool roomLeft = false;
            bool roomRight = false;
            bool roomAhead = false;
            bool roomAheadLeft = false;
            bool roomAheadRight = false;
            switch(_player.Rotation)
            {

                case 0: // Up
                    if (_player._location > 20)
                    {
                        if (_floor.Rooms[_player._location - 20] != null)
                        {
                            roomFront = true;
                        }
                        if (_floor.Rooms[_player._location - 21] != null)
                        {
                            roomLeft = true;
                        }
                        if (_floor.Rooms[_player._location - 19] != null)
                        {
                            roomRight = true;
                        }
                    }
                    else
                    {
                        roomFront = true;
                    }
                    
                    if (_player._location > 40)
                    {
                        if (_floor.Rooms[_player._location - 40] != null)
                        {
                            roomAhead = true;
                        }
                        if (_floor.Rooms[_player._location - 41] != null)
                        {
                            roomAheadLeft = true;
                        }
                        if (_floor.Rooms[_player._location - 39] != null)
                        {
                            roomAheadRight = true;
                        }
                    }
                    break;
                case 1: // Right
                    if (!Array.Exists(_floor.rightBorder, number => number == _player._location))
                    {
                        if (_floor.Rooms[_player._location + 1] != null)
                        {
                            roomFront = true;
                        }
                        if (_player._location > 20)
                        {
                            if (_floor.Rooms[_player._location - 19] != null)
                            {
                                roomLeft = true;
                            }
                        }
                        if (_player._location < 379)
                        {
                            if (_floor.Rooms[_player._location + 21] != null)
                            {
                                roomRight = true;
                            }
                        }
                    }
                    else
                    {
                        roomFront = true;
                    }
                    
                    if (!Array.Exists(_floor.rightBorder, number => number == _player._location + 1))
                    {
                        if (_floor.Rooms[_player._location + 2] != null)
                        {
                            roomAhead = true;
                        }
                        if (_player._location > 20)
                        {
                            if (_floor.Rooms[_player._location - 18] != null)
                            {
                                roomAheadLeft = true;
                            }
                        }
                        if (_player._location < 378)
                        {
                            if (_floor.Rooms[_player._location + 22] != null)
                            {
                                roomAheadRight = true;
                            }
                        }
                    }
                    break;
                case 2: // Down
                    if (_player._location < 380)
                    {
                        if (_floor.Rooms[_player._location + 20] != null)
                        {
                            roomFront = true;
                        }
                        if  (_player._location < 379)
                        {
                            if (_floor.Rooms[_player._location + 21] != null)
                            {
                                roomLeft = true;
                            }
                        }
                        
                        if (_floor.Rooms[_player._location + 19] != null)
                        {
                            roomRight = true;
                        }
                    }
                    else
                    {
                        roomFront = true;
                    }
                    
                    if (_player._location < 340)
                    {
                        if (_floor.Rooms[_player._location + 40] != null)
                        {
                            roomAhead = true;
                        }
                        if (_floor.Rooms[_player._location + 41] != null)
                        {
                            roomAheadLeft = true;
                        }
                        if (_floor.Rooms[_player._location + 39] != null)
                        {
                            roomAheadRight = true;
                        }
                    }
                    break;
                case 3: // Left
                    if (!Array.Exists(_floor.leftBorder, number => number == _player._location))
                    {
                        if (_floor.Rooms[_player._location - 1] != null)
                        {
                            roomFront = true;
                        }
                        if (_player._location < 380)
                        {
                            if (_floor.Rooms[_player._location + 19] != null)
                            {
                                roomLeft = true;
                            }
                        }
                        if (_player._location > 20)
                        {
                            if (_floor.Rooms[_player._location - 21] != null)
                            {
                                roomRight = true;
                            }
                        }
                    }
                    else
                    {
                        roomFront = true;
                    }
                    
                    if (!Array.Exists(_floor.leftBorder, number => number == _player._location - 1))
                    {
                        if (_floor.Rooms[_player._location - 2] != null)
                        {
                            roomAhead = true;
                        }
                        if (_player._location < 380)
                        {
                            if (_floor.Rooms[_player._location + 18] != null)
                            {
                                roomAheadLeft = true;
                            }
                        }
                        if (_player._location > 20)
                        {
                            if (_floor.Rooms[_player._location - 22] != null)
                            {
                                roomAheadRight = true;
                            }
                        }
                        
                    }
                    break;
            }
            if (roomFront)
            {
                if (!roomAhead)
                {
                    _gameWindow.DrawBitmap(wall_ahead, 3, 3);
                }
                if (!roomAheadLeft)
                {
                    _gameWindow.DrawBitmap(wall_ahead_left, 3, 3);
                }
                if (!roomAheadRight)
                {
                    _gameWindow.DrawBitmap(wall_ahead_right, 3, 3);
                }
                if (!roomLeft)
                {
                    _gameWindow.DrawBitmap(wall_left, 3, 3);
                }
                if (!roomRight)
                {
                    _gameWindow.DrawBitmap(wall_right, 3, 3);
                }
            }
            else
            {
                _gameWindow.DrawBitmap(wall_front, 3, 3);
            }
        }

        // Draw the items in the player inventory
        foreach (Item i in Player._HeldItems)
        {
            (int x, int y) = InventoryPosition(i);
            i.Draw(_gameWindow, x, y);
        }

        // Draw the weapon in the weapon slot
        if (_player.EquippedWeapon != null)
        {
            _player.EquippedWeapon.Draw(_gameWindow, 625, 525);
        }

        // Draw the helmet in the helmet slot
        if (_player.EquippedHelmet != null)
        {
            _player.EquippedHelmet.Draw(_gameWindow, 575, 475);
        }

        // Draw the chestplate in the chestplate slot
        if (_player.EquippedChestplate != null)
        {
            _player.EquippedChestplate.Draw(_gameWindow, 575, 525);
        }

        // Draw the platelegs in the platelegs slot
        if (_player.EquippedPlatelegs != null)
        {
            _player.EquippedPlatelegs.Draw(_gameWindow, 575, 575);
        }

        // Draw the shield in the shield slot
        if (_player.EquippedShield != null)
        {
            _player.EquippedShield.Draw(_gameWindow, 525, 525);
        }
        
        // Draw the monster
        if (_floor.Rooms[_player._location]._monster != null)
        {
            _floor.Rooms[_player._location]._monster.Draw(_gameWindow, _animCycle);

            // And the monsters health bar
            float monsterHealth = _floor.Rooms[_player._location]._monster.Health;
            float monsterMaxHealth = _floor.Rooms[_player._location]._monster.MaxHealth;
            float monsterHealthRatio = 150 * (monsterHealth / monsterMaxHealth);
            SplashKit.FillRectangle(Color.Red, 250, 300, monsterHealthRatio, 24);
        }

        // Draw the items on the floor
        foreach(Item i in _floor.Rooms[_player._location]._RoomItems)
        {
            if (i.x == 0)
            {
                i.x = SplashKit.Rnd(50, 595);
            }
            if (i.y == 0)
            {
                i.y = SplashKit.Rnd(290, 400);
            }
            i.Draw(_gameWindow, i.x, i.y);
        }

        // Draw the attack
        if (swingAnim)
        {
            int _animPhase = _animCycle / 100;
            switch(_animPhase)
            {
                case 0:
                    _gameWindow.DrawBitmap(weaponSwing0, 289, 325);
                    break;
                case 1:
                    _gameWindow.DrawBitmap(weaponSwing1, 289, 325);
                    break;
                case 2:
                    _gameWindow.DrawBitmap(weaponSwing2, 289, 325);
                    break;
                case 3:
                    _gameWindow.DrawBitmap(weaponSwing3, 289, 325);
                    break;
                case 4:
                    _gameWindow.DrawBitmap(weaponSwing4, 289, 325);
                    break;
            }
        }

        // Draw the damage number
        if (_numberLifetime > 0)
        {
            if (_damageTaken != 0) { _gameWindow.DrawText(Convert.ToString(_damageTaken), Color.Red, 315, 250); }
            else { _gameWindow.DrawText("BLOCKED", Color.Blue, 300, 250); }
            _numberLifetime -= 1;
        }

        // Draw the context text
        int conTextX = 210;
        int conTextY = 587;
        if (mouseOccupied)
        {
            _gameWindow.DrawText("Item: " + mousedItem.Name, Color.White, conTextX, conTextY - 15);
            _gameWindow.DrawText(mousedItem.Description, Color.White, conTextX, conTextY);
        }
        else
        {
            Point2D mousePosition = SplashKit.MousePosition();
            int mX = Convert.ToInt32(mousePosition.X);
            int mY = Convert.ToInt32(mousePosition.Y);
            Rectangle mouseCursor = SplashKit.RectangleFrom(mX, mY, 1, 1);
            if (_floor.Rooms[_player._location]._RoomItems.Count > 0)
            {
                foreach (Item i in _floor.Rooms[_player._location]._RoomItems)
                {
                    Rectangle itemRectangle = SplashKit.RectangleFrom(i.x - 20, i.y - 20, 40, 40);
                    if (SplashKit.RectanglesIntersect(itemRectangle, mouseCursor) == true)
                    {
                        _gameWindow.DrawText("Item: " + i.Name, Color.White, conTextX, conTextY - 15);
                        _gameWindow.DrawText(i.Description, Color.White, conTextX, conTextY);
                    }
                }    
            }
            
            if (Player._HeldItems.Count > 0)
            {
                foreach (Item i in Player._HeldItems)
                {
                    Rectangle itemRectangle = SplashKit.RectangleFrom(i.x - 20, i.y - 20, 40, 40);
                    if (SplashKit.RectanglesIntersect(itemRectangle, mouseCursor) == true)
                    {
                        _gameWindow.DrawText("Item: " + i.Name, Color.White, conTextX, conTextY - 15);
                        _gameWindow.DrawText(i.Description, Color.White, conTextX, conTextY);
                    }
                }    
            }
            
            if (Player._equippedWeapon != null)
            {
                Rectangle weaponRectangle = SplashKit.RectangleFrom(Player._equippedWeapon.x - 20, Player._equippedWeapon.y - 20, 40, 40);
                if (SplashKit.RectanglesIntersect(weaponRectangle, mouseCursor) == true)
                {
                    _gameWindow.DrawText("Item: " + Player._equippedWeapon.Name, Color.White, conTextX, conTextY - 15);
                    _gameWindow.DrawText(Player._equippedWeapon.Description, Color.White, conTextX, conTextY);
                }    
            }
            
            if (Player._equippedHelmet != null)
            {
                Rectangle helmetRectangle = SplashKit.RectangleFrom(Player._equippedHelmet.x - 20, Player._equippedHelmet.y - 20, 40, 40);
                if (SplashKit.RectanglesIntersect(helmetRectangle, mouseCursor) == true)
                {
                    _gameWindow.DrawText("Item: " + Player._equippedHelmet.Name, Color.White, conTextX, conTextY - 15);
                    _gameWindow.DrawText(Player._equippedHelmet.Description, Color.White, conTextX, conTextY);
                }    
            }

            if (Player._equippedChestplate != null)
            {
                Rectangle chestplateRectangle = SplashKit.RectangleFrom(Player._equippedChestplate.x - 20, Player._equippedChestplate.y - 20, 40, 40);
                if (SplashKit.RectanglesIntersect(chestplateRectangle, mouseCursor) == true)
                {
                    _gameWindow.DrawText("Item: " + Player._equippedChestplate.Name, Color.White, conTextX, conTextY - 15);
                    _gameWindow.DrawText(Player._equippedChestplate.Description, Color.White, conTextX, conTextY);
                }    
            }
            
            if (Player._equippedPlatelegs != null)
            {
                Rectangle platelegsRectangle = SplashKit.RectangleFrom(Player._equippedPlatelegs.x - 20, Player._equippedPlatelegs.y - 20, 40, 40);
                if (SplashKit.RectanglesIntersect(platelegsRectangle, mouseCursor) == true)
                {
                    _gameWindow.DrawText("Item: " + Player._equippedPlatelegs.Name, Color.White, conTextX, conTextY - 15);
                    _gameWindow.DrawText(Player._equippedPlatelegs.Description, Color.White, conTextX, conTextY);
                }    
            }

            if (Player._equippedShield != null)
            {
                Rectangle shieldRectangle = SplashKit.RectangleFrom(Player._equippedShield.x - 20, Player._equippedShield.y - 20, 40, 40);
                if (SplashKit.RectanglesIntersect(shieldRectangle, mouseCursor) == true)
                {
                    _gameWindow.DrawText("Item: " + Player._equippedShield.Name, Color.White, conTextX, conTextY - 15);
                    _gameWindow.DrawText(Player._equippedShield.Description, Color.White, conTextX, conTextY);
                }    
            }
        }
        
        // Draw the game stats
        _gameWindow.DrawText("Floor: " + _floorNumber, Color.White, 210, 456);
        _gameWindow.DrawText("Defeated: " + _enemiesDefeated, Color.White, 210, 471);
        _gameWindow.DrawText("Experience: " + _score + "%", Color.White, 210, 486);

        // Draw the map
        _floor.Draw(_gameWindow);
        _player.Draw(_gameWindow);

        // Draw the item in the cursor
        if (mousedItem != null)
        {
            Point2D mousePosition = SplashKit.MousePosition();
            int mX = Convert.ToInt32(mousePosition.X);
            int mY = Convert.ToInt32(mousePosition.Y);
            mousedItem.Draw(_gameWindow, mX, mY);
        }

        // Draw the level up screen
        if (_levelUp)
        {
            _gameWindow.DrawBitmap(levelUpScreen, 324 - (levelUpScreen.Width / 2), 224 - (levelUpScreen.Height / 2));
            _gameWindow.DrawText("+20", Color.White, 232, 255);
            _gameWindow.DrawText("+20", Color.White, 274, 255);
            _gameWindow.DrawText("+20", Color.White, 316, 255);
            _gameWindow.DrawText("+20", Color.White, 358, 255);
            _gameWindow.DrawText("+20", Color.White, 400, 255);
        }

        _gameWindow.Refresh();
        _animCycle += 1;

        if (_animCycle > 2000)
        {
            _animCycle = 0;
            swingAnim = false;
        }
    }

    // Handles the inputs
    public void HandleInput()
    {
        SplashKit.ProcessEvents();
        
        // Check for a left click
        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            // If the player is levelling up
            if (_levelUp)
            {
                // Check to see if the cursor is inside a level incrementer
                Point2D mousePosition = SplashKit.MousePosition();
                int mX = Convert.ToInt32(mousePosition.X);
                int mY = Convert.ToInt32(mousePosition.Y);
                if (mY > 248 && mY < 267)
                {
                    if (mX > 226 && mX < 260)
                    {
                        _player.LevelUp(20, 0, 0, 0, 0);
                        _levelUp = false;
                    }
                    else if (mX > 267 && mX < 301)
                    {
                        _player.LevelUp(0, 20, 0, 0, 0);
                        _levelUp = false;
                    }
                    else if (mX > 308 && mX < 342)
                    {
                        _player.LevelUp(0, 0, 20, 0, 0);
                        _levelUp = false;
                    }
                    else if (mX > 349 && mX < 383)
                    {
                        _player.LevelUp(0, 0, 0, 20, 0);
                        _levelUp = false;
                    }
                    else if (mX > 390 && mX < 424)
                    {
                        _player.LevelUp(0, 0, 0, 0, 20);
                        _levelUp = false;
                    }
                }
            }
            else
            {
                // If there is something in the cursor
                if (mouseOccupied == true)
                {
                    // Check if the position is valid
                    // Need to find if the item is inside an inventory slot
                    if (mousedItem.x > 655 && mousedItem.x < 695)
                    {
                        if (mousedItem.y > 154 && mousedItem.y < 194)
                        {
                            if (Player._HeldItems.Count < 0) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(0, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 205 && mousedItem.y < 245)
                        {
                            if (Player._HeldItems.Count < 3) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(3, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 256 && mousedItem.y < 296)
                        {
                            if (Player._HeldItems.Count < 6) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(6, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 307 && mousedItem.y < 347)
                        {
                            if (Player._HeldItems.Count < 9) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(9, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 358 && mousedItem.y < 398)
                        {
                            if (Player._HeldItems.Count < 12) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(12, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 409 && mousedItem.y < 449)
                        {
                            if (Player._HeldItems.Count < 15) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(15, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 460 && mousedItem.y < 500)
                        {
                            if (Player._HeldItems.Count < 18) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(18, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 511 && mousedItem.y < 551)
                        {
                            if (Player._HeldItems.Count < 21) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(21, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 562 && mousedItem.y < 602)
                        {
                            if (Player._HeldItems.Count < 24) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(24, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                    }
                    else if (mousedItem.x > 706 && mousedItem.x < 746)
                    {
                        if (mousedItem.y > 154 && mousedItem.y < 194)
                        {
                            if (Player._HeldItems.Count < 1) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(1, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 205 && mousedItem.y < 245)
                        {
                            if (Player._HeldItems.Count < 4) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(4, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 256 && mousedItem.y < 296)
                        {
                            if (Player._HeldItems.Count < 7) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(7, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 307 && mousedItem.y < 347)
                        {
                            if (Player._HeldItems.Count < 10) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(10, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 358 && mousedItem.y < 398)
                        {
                            if (Player._HeldItems.Count < 13) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(13, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 409 && mousedItem.y < 449)
                        {
                            if (Player._HeldItems.Count < 16) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(16, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 460 && mousedItem.y < 500)
                        {
                            if (Player._HeldItems.Count < 19) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(19, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 511 && mousedItem.y < 551)
                        {
                            if (Player._HeldItems.Count < 22) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(22, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 562 && mousedItem.y < 602)
                        {
                            if (Player._HeldItems.Count < 25) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(25, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                    }
                    else if (mousedItem.x > 757 && mousedItem.x < 797)
                    {
                        if (mousedItem.y > 154 && mousedItem.y < 194)
                        {
                            if (Player._HeldItems.Count < 2) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(2, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 205 && mousedItem.y < 245)
                        {
                            if (Player._HeldItems.Count < 5) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(5, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 256 && mousedItem.y < 296)
                        {
                            if (Player._HeldItems.Count < 8) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(8, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 307 && mousedItem.y < 347)
                        {
                            if (Player._HeldItems.Count < 11) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(11, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 358 && mousedItem.y < 398)
                        {
                            if (Player._HeldItems.Count < 14) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(14, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 409 && mousedItem.y < 449)
                        {
                            if (Player._HeldItems.Count < 17) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(17, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 460 && mousedItem.y < 500)
                        {
                            if (Player._HeldItems.Count < 20) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(20, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 511 && mousedItem.y < 551)
                        {
                            if (Player._HeldItems.Count < 23) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(23, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else if (mousedItem.y > 562 && mousedItem.y < 602)
                        {
                            if (Player._HeldItems.Count < 26) { Player._HeldItems.Add(mousedItem); }
                            else { Player._HeldItems.Insert(26, mousedItem); }
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                    }
                    // Also check the weapon slot
                    else if (mousedItem.x > 604 && mousedItem.x < 644 && mousedItem.y > 504 && mousedItem.y < 544 && mousedItem is Weapon)
                    {
                        if (Player._equippedWeapon == null) 
                        {
                            Player._equippedWeapon = (Weapon)mousedItem;
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else // Swap the weapons
                        {
                            Weapon temp = (Weapon)mousedItem;
                            mousedItem = Player._equippedWeapon;
                            Player._equippedWeapon = temp;
                        }
                    }
                    // Also check the helmet slot
                    else if (mousedItem.x > 555 && mousedItem.x < 595 && mousedItem.y > 455 && mousedItem.y < 495 && mousedItem is Helmet)
                    {
                        if (Player._equippedHelmet == null) 
                        {
                            Player._equippedHelmet = (Helmet)mousedItem;
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else // Swap the helmets
                        {
                            Helmet temp = (Helmet)mousedItem;
                            mousedItem = Player._equippedHelmet;
                            Player._equippedHelmet = temp;
                        }
                    }
                    // Also check the chestplate slot
                    else if (mousedItem.x > 555 && mousedItem.x < 595 && mousedItem.y > 505 && mousedItem.y < 545 && mousedItem is Chestplate)
                    {
                        if (Player._equippedChestplate == null) 
                        {
                            Player._equippedChestplate = (Chestplate)mousedItem;
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else // Swap the chestplates
                        {
                            Chestplate temp = (Chestplate)mousedItem;
                            mousedItem = Player._equippedChestplate;
                            Player._equippedChestplate = temp;
                        }
                    }
                    // Also check the platelegs slot
                    else if (mousedItem.x > 555 && mousedItem.x < 595 && mousedItem.y > 555 && mousedItem.y < 595 && mousedItem is Platelegs)
                    {
                        if (Player._equippedPlatelegs == null) 
                        {
                            Player._equippedPlatelegs = (Platelegs)mousedItem;
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else // Swap the platelegs
                        {
                            Platelegs temp = (Platelegs)mousedItem;
                            mousedItem = Player._equippedPlatelegs;
                            Player._equippedPlatelegs = temp;
                        }
                    }
                    // Also check the shield slot
                    else if (mousedItem.x > 505 && mousedItem.x < 545 && mousedItem.y > 505 && mousedItem.y < 545 && mousedItem is Shield)
                    {
                        if (Player._equippedShield == null) 
                        {
                            Player._equippedShield = (Shield)mousedItem;
                            mousedItem.followCursor = false;
                            mousedItem = null;
                            mouseOccupied = false;
                        }
                        else // Swap the shields
                        {
                            Shield temp = (Shield)mousedItem;
                            mousedItem = Player._equippedShield;
                            Player._equippedShield = temp;
                        }
                    }
                    // Also check the shop sell
                    else if (mousedItem.x < 750 && mousedItem.y < 450 && _inStore)
                    {
                        _player._coins += mousedItem._price;
                        mousedItem.followCursor = false;
                        mousedItem = null;
                        mouseOccupied = false;
                    }
                }
                else
                {
                    List<Item> RemoveItems = new List<Item>();
                    // Check the context
                    Point2D mousePosition = SplashKit.MousePosition();
                    int mX = Convert.ToInt32(mousePosition.X);
                    int mY = Convert.ToInt32(mousePosition.Y);
                    foreach (Item i in Player._HeldItems)
                    {
                        (int x, int y) = InventoryPosition(i);
                        Rectangle itemImage = SplashKit.RectangleFrom(x - 20, y - 20, 40, 40);
                        Rectangle mouseCursor = SplashKit.RectangleFrom(mX, mY, 1, 1);
                        if (SplashKit.RectanglesIntersect(itemImage, mouseCursor) == true)
                        {
                            i.followCursor = true;
                            mouseOccupied = true;
                            mousedItem = i;
                            RemoveItems.Add(i);
                        }
                    }

                    // Also check the weapon slot
                    if (mX > 604 && mX < 644 && mY > 504 && mY < 544 && Player._equippedWeapon != null)
                    {
                        mousedItem = Player._equippedWeapon;
                        mousedItem.followCursor = true;
                        mouseOccupied = true;
                        Player._equippedWeapon = null;
                    }

                    // Also check the helmet slot
                    if (mX > 555 && mX < 595 && mY > 455 && mY < 495 && Player._equippedHelmet != null)
                    {
                        mousedItem = Player._equippedHelmet;
                        mousedItem.followCursor = true;
                        mouseOccupied = true;
                        Player._equippedHelmet = null;
                    }

                    // Also check the chestplate slot
                    if (mX > 555 && mX < 595 && mY > 505 && mY < 525 && Player._equippedChestplate != null)
                    {
                        mousedItem = Player._equippedChestplate;
                        mousedItem.followCursor = true;
                        mouseOccupied = true;
                        Player._equippedChestplate = null;
                    }

                    // Also check the platelegs slot
                    if (mX > 555 && mX < 595 && mY > 555 && mY < 595 && Player._equippedPlatelegs != null)
                    {
                        mousedItem = Player._equippedPlatelegs;
                        mousedItem.followCursor = true;
                        mouseOccupied = true;
                        Player._equippedPlatelegs = null;
                    }

                    // Also check the shield slot
                    if (mX > 505 && mX < 545 && mY > 505 && mY < 545 && Player._equippedShield != null)
                    {
                        mousedItem = Player._equippedShield;
                        mousedItem.followCursor = true;
                        mouseOccupied = true;
                        Player._equippedShield = null;
                    }

                    foreach (Item i in RemoveItems)
                    {
                        Player._HeldItems.Remove(i);
                    }
                    RemoveItems.Clear();

                    foreach (Item i in _floor.Rooms[_player._location]._RoomItems)
                    {
                        Rectangle itemImage = SplashKit.RectangleFrom(i.x - 20, i.y - 20, 40, 40);
                        Rectangle mouseCursor = SplashKit.RectangleFrom(mX, mY, 1, 1);
                        if (SplashKit.RectanglesIntersect(itemImage, mouseCursor) == true)
                        {
                            if (_inStore)
                            {
                                if (_player.Coins >= i.Price)
                                {
                                    _player._coins -= i.Price;
                                    i.followCursor = true;
                                    mouseOccupied = true;
                                    mousedItem = i;
                                    RemoveItems.Add(i);
                                }    
                            }
                            else
                            {
                                i.followCursor = true;
                                mouseOccupied = true;
                                mousedItem = i;
                                RemoveItems.Add(i); 
                            }
                        }
                    }
                    foreach (Item i in RemoveItems)
                    {
                        _floor.Rooms[_player._location]._RoomItems.Remove(i);
                    }
                    
                }    
            }
            
        }
        if (SplashKit.MouseClicked(MouseButton.RightButton))
        {
            // Check if the player is levelling up
            if (_levelUp)
            {
                
            }
            else
            {
                // Check if an item is being held
                if (mouseOccupied == true)
                {
                    // Use the held item
                    doFight = mousedItem.Use(_player, -1, _fightState);
                    if (mousedItem.GetType().Equals(typeof(Spell)))
                    {
                        
                    }
                    else
                    {
                        mousedItem.followCursor = false;
                        mousedItem = null;
                        mouseOccupied = false;
                    }
                }
                else
                {
                    // Check if the right click was on an item
                    Point2D mousePosition = SplashKit.MousePosition();
                    int mX = Convert.ToInt32(mousePosition.X);
                    int mY = Convert.ToInt32(mousePosition.Y);
                    if (mX > 655 && mX < 695)
                    {
                        if (mY > 154 && mY < 194)       // Slot 0
                        {
                            if (Player._HeldItems.Count > 0)
                            {
                                doFight = Player._HeldItems[0].Use(_player, 0, _fightState);
                            }
                        }
                        else if (mY > 205 && mY < 245)  // Slot 3
                        {
                            if (Player._HeldItems.Count > 3)
                            {
                                doFight = Player._HeldItems[3].Use(_player, 3, _fightState);
                            }
                        }
                        else if (mY > 256 && mY < 296)  // Slot 6
                        {
                            if (Player._HeldItems.Count > 6)
                            {
                                doFight = Player._HeldItems[6].Use(_player, 6, _fightState);
                            }
                        }
                        else if (mY > 307 && mY < 347)  // Slot 9
                        {
                            if (Player._HeldItems.Count > 9)
                            {
                                doFight = Player._HeldItems[9].Use(_player, 9, _fightState);
                            }
                        }
                        else if (mY > 358 && mY < 398)  // Slot 12
                        {
                            if (Player._HeldItems.Count > 12)
                            {
                                doFight = Player._HeldItems[12].Use(_player, 12, _fightState);
                            }
                            }
                        else if (mY > 409 && mY < 449)  // Slot 15
                        {
                            if (Player._HeldItems.Count > 15)
                            {
                                doFight = Player._HeldItems[15].Use(_player, 15, _fightState);
                            }
                        }
                        else if (mY > 460 && mY < 500)  // Slot 18
                        {
                            if (Player._HeldItems.Count > 18)
                            {
                                doFight = Player._HeldItems[18].Use(_player, 18, _fightState);
                            }
                        }
                        else if (mY > 511 && mY < 551)  // Slot 21
                        {
                            if (Player._HeldItems.Count > 21)
                            {
                                doFight = Player._HeldItems[21].Use(_player, 21, _fightState);
                            }
                        }
                        else if (mY > 562 && mY < 602)  // Slot 24
                        {
                            if (Player._HeldItems.Count > 24)
                            {
                                doFight = Player._HeldItems[24].Use(_player, 24, _fightState);
                            }
                        }
                    }
                    else if (mX > 706 && mX < 746)
                    {
                        if (mY > 154 && mY < 194)       // Slot 1
                        {
                            if (Player._HeldItems.Count > 1)
                            {
                                doFight = Player._HeldItems[1].Use(_player, 1, _fightState);
                            }
                        }
                        else if (mY > 205 && mY < 245)  // Slot 4
                        {
                            if (Player._HeldItems.Count > 4)
                            {
                                doFight = Player._HeldItems[4].Use(_player, 4, _fightState);
                            }
                        }
                        else if (mY > 256 && mY < 296)  // Slot 7
                        {
                            if (Player._HeldItems.Count > 7)
                            {
                                doFight = Player._HeldItems[7].Use(_player, 7, _fightState);
                            }
                        }
                        else if (mY > 307 && mY < 347)  // Slot 10
                        {
                            if (Player._HeldItems.Count > 10)
                            {
                                doFight = Player._HeldItems[10].Use(_player, 10, _fightState);
                            }
                        }
                        else if (mY > 358 && mY < 398)  // Slot 13
                        {
                            if (Player._HeldItems.Count > 13)
                            {
                                doFight = Player._HeldItems[13].Use(_player, 13, _fightState);
                            }
                        }
                        else if (mY > 409 && mY < 449)  // Slot 16
                        {
                            if (Player._HeldItems.Count > 16)
                            {
                                doFight = Player._HeldItems[16].Use(_player, 16, _fightState);
                            }
                        }
                        else if (mY > 460 && mY < 500)  // Slot 19
                        {
                            if (Player._HeldItems.Count > 19)
                            {
                                doFight = Player._HeldItems[19].Use(_player, 19, _fightState);
                            }
                        }
                        else if (mY > 511 && mY < 551)  // Slot 22
                        {
                            if (Player._HeldItems.Count > 22)
                            {
                                doFight = Player._HeldItems[22].Use(_player, 22, _fightState);
                            }
                        }
                        else if (mY > 562 && mY < 602)  // Slot 25
                        {
                            if (Player._HeldItems.Count > 25)
                            {
                                doFight = Player._HeldItems[25].Use(_player, 25, _fightState);
                            }
                        }
                    }
                    else if (mX > 757 && mX < 797)
                    {
                        if (mY > 154 && mY < 194)       // Slot 2
                        {
                            if (Player._HeldItems.Count > 2)
                            {
                                doFight = Player._HeldItems[2].Use(_player, 2, _fightState);
                            }
                        }
                        else if (mY > 205 && mY < 245)  // Slot 5
                        {
                            if (Player._HeldItems.Count > 5)
                            {
                                doFight = Player._HeldItems[5].Use(_player, 5, _fightState);
                            }
                        }
                        else if (mY > 256 && mY < 296)  // Slot 8
                        {
                            if (Player._HeldItems.Count > 8)
                            {
                                doFight = Player._HeldItems[8].Use(_player, 8, _fightState);
                            }
                        }
                        else if (mY > 307 && mY < 347)  // Slot 11
                        {
                            if (Player._HeldItems.Count > 11)
                            {
                                doFight = Player._HeldItems[11].Use(_player, 11, _fightState);
                            }
                        }   
                        else if (mY > 358 && mY < 398)  // Slot 14
                        {
                            if (Player._HeldItems.Count > 14)
                            {
                                doFight = Player._HeldItems[14].Use(_player, 14, _fightState);
                            }
                        }
                        else if (mY > 409 && mY < 449)  // Slot 17
                        {
                            if (Player._HeldItems.Count > 17)
                            {
                                doFight = Player._HeldItems[17].Use(_player, 17, _fightState);
                            }
                        }
                        else if (mY > 460 && mY < 500)  // Slot 20
                        {
                            if (Player._HeldItems.Count > 20)
                            {
                                doFight = Player._HeldItems[20].Use(_player, 20, _fightState);
                            }
                        }
                        else if (mY > 511 && mY < 551)  // Slot 23
                        {
                            if (Player._HeldItems.Count > 23)
                            {
                                doFight = Player._HeldItems[23].Use(_player, 23, _fightState);
                            }
                        }
                        else if (mY > 562 && mY < 602)  // Slot 26
                        {
                            if (Player._HeldItems.Count > 26)
                            {
                                doFight = Player._HeldItems[26].Use(_player, 26, _fightState);
                            }
                        }
                    }

                    // Also check the weapon slot
                    else if (mX > 604 && mX < 644 && mY > 504 && mY < 544 && Player._equippedWeapon != null)
                    {
                        // Send the weapon back to the inventory
                        Player._HeldItems.Add(Player._equippedWeapon);
                        Player._equippedWeapon = null;
                    }

                    // Also check the helmet slot
                    else if (mX > 555 && mX < 595 && mY > 455 && mY < 459 && Player._equippedHelmet != null)
                    {
                        // Send the helmet back to the inventory
                        Player._HeldItems.Add(Player._equippedHelmet);
                        Player._equippedHelmet = null;
                    }
                    // Also check the chestplate slot
                    else if (mX > 555 && mX < 595 && mY > 505 && mY < 525 && Player._equippedChestplate != null)
                    {
                        // Send the chestplate back to the inventory
                        Player._HeldItems.Add(Player._equippedChestplate);
                        Player._equippedChestplate = null;
                    }
                    // Also check the platelegs slot
                    else if (mX > 555 && mX < 595 && mY > 455 && mY < 459 && Player._equippedPlatelegs != null)
                    {
                        // Send the platelegs back to the inventory
                        Player._HeldItems.Add(Player._equippedPlatelegs);
                        Player._equippedPlatelegs = null;
                    }
                    // Also check the shield slot
                    else if (mX > 505 && mX < 545 && mY > 505 && mY < 545 && Player._equippedShield != null)
                    {
                        // Send the shield back to the inventory
                        Player._HeldItems.Add(Player._equippedShield);
                        Player._equippedShield = null;
                    }
                }
            }
            
        }
        // Check for up key
        if (SplashKit.KeyTyped(KeyCode.UpKey) && !_fightState && !_levelUp)
        {
            moveMonsters = _player.Move(_floor);
        }

        // Check for left key
        if (SplashKit.KeyTyped(KeyCode.LeftKey) && !_fightState && !_levelUp)
        {
            _player.Rotate(0);
        }

        // Check for right key
        if (SplashKit.KeyTyped(KeyCode.RightKey) && !_fightState && !_levelUp)
        {
            _player.Rotate(1);
        }

        // Check for space key
        if (SplashKit.KeyTyped(KeyCode.SpaceKey) && _fightState && !_levelUp)
        {
            FightMonster(_floor.Rooms[_player._location]._monster);
        }
        else if (SplashKit.KeyTyped(KeyCode.SpaceKey) && _player._location == _floor._stairs)
        {
            // Generate a new floor
            _floor = new Floor(_player, this);
            _floorNumber += 1;
            _storeCounter += 1;
            _score += 20;
            _floor.Populate();
        }

        // Check for escape key
        if (SplashKit.KeyDown(KeyCode.EscapeKey)) {quit = false; }

        if (doFight != -1)
        {
            SpellMonster(_floor.Rooms[_player._location]._monster, doFight);
        }
        doFight = -1;
    }

    // Creates a new floor
    public Floor NewFloor(Player _player)
    {
        Floor floor = new Floor(_player, this);
        return floor;
    }

    // Creates a new player
    public Player NewPlayer(Floor _floor)
    {
        Player player = new Player(_floor);
        return player;
    }

    // Returns the room at the players current location
    public Room GetRoom(Floor _floor)
    {
        return _floor.Rooms[_player.Location];
    }

    public void FightMonster(Monster _monster)
    {
        int damage;
        damage = CalculateDamage(_player.Combat, _player.EquippedWeapon, _monster.Protect, _monster.EquippedHelmet, _monster.EquippedChestplate, _monster.EquippedPlatelegs, _monster.EquippedShield);
        _monster._health = _monster._health - damage;
        
        _damageTaken = -damage;
        _numberLifetime = 1000;
        if (damage > 0)
        {
            _animCycle = 0;
            swingAnim = true;
        }

        if (_monster.Health <= 0)
        {
            _player._coins += _monster.Coins;
            _floor.Rooms[_player._location]._monster = null;
            _enemiesDefeated += 1;
            _score += 10;
        }
        else
        {
            MonsterAttack(_monster);
        }
        if (_player.Health <= 0)
        {
            quit = false;
        }
    }

    public void SpellMonster(Monster _monster, int _spellPower)
    {
        int damage;
        damage = CalculateSpell(_player.Spell, _spellPower, _monster.Protect, _monster.EquippedHelmet, _monster.EquippedChestplate, _monster.EquippedPlatelegs, _monster.EquippedShield);
        _monster._health = _monster._health - damage;
        
        _damageTaken = -damage;
        _player._magic -= _spellPower / 2;
        _numberLifetime = 3000;
        if (damage > 0)
        {
            _animCycle = 0;
            swingAnim = true;
        }

        if (_monster.Health <= 0)
        {
            _floor.Rooms[_player._location]._monster = null;
            _enemiesDefeated += 1;
            _score += 10;
        }
        else
        {
            MonsterAttack(_monster);
        }
        if (_player.Health <= 0)
        {
            quit = false;
        }
    }

    public void MonsterAttack(Monster _monster)
    {
        int damage = CalculateDamage(_monster.Combat, _monster.EquippedWeapon, _player.Protect, _player.EquippedHelmet, _player.EquippedChestplate, _player.EquippedPlatelegs, _player.EquippedShield);
        _player._health = _player._health - damage;
    }

    public int CalculateDamage(int combat, Weapon weapon, int protect, Helmet helmet, Chestplate chestplate, Platelegs platelegs, Shield shield)
    {
        double attack;
        double armor = 0;
        if (weapon == null)
        {
            attack = 1;
        }
        else
        {
            attack = Convert.ToDouble(weapon.Damage);
        }

        if (helmet != null)
        {
            armor += Convert.ToDouble(helmet.Armor);
        }
        if (chestplate != null)
        {
            armor += Convert.ToDouble(chestplate.Armor);
        }
        if (platelegs != null)
        {
            armor += Convert.ToDouble(platelegs.Armor);
        }
        if (shield != null)
        {
            armor += Convert.ToDouble(shield.Armor);
        }
        double strength = (combat * attack) / 100;          // Damage dealt = weapon damage * weapon proficiency%
        double defense = (armor * protect) / 100;         // Damage reduced = armor strength * armor proficiency%
        double damage = strength - defense;    // Final damage = damage dealt - damage reduced, minimum of 1
        int multiplier = SplashKit.Rnd(75, 110);
        int actualDamage = Convert.ToInt32(Math.Round(damage * multiplier / 100));    // Actual damage ranges from 75% to 110% of the recieved damage
        if (actualDamage <= 0)
        {
            actualDamage = 0;
        }
        return actualDamage;
    }

    public int CalculateSpell(int spell, int spellPower, int protect, Helmet helmet, Chestplate chestplate, Platelegs platelegs, Shield shield)
    {
        double armor = 0;

        if (helmet != null)
        {
            armor += Convert.ToDouble(helmet.Armor);
        }
        if (chestplate != null)
        {
            armor += Convert.ToDouble(chestplate.Armor);
        }
        if (platelegs != null)
        {
            armor += Convert.ToDouble(platelegs.Armor);
        }
        if (shield != null)
        {
            armor += Convert.ToDouble(shield.Armor);
        }
        double strength = (spell * spellPower) / 100;
        double defense = (armor * protect) / 100;         // Max defense of 0.8 (translates to 80% damage reduction)
        double damage = strength - defense;    // Max damage of 100 (20 with max defense), min 1 (0.2 with max defense)
        int multiplier = SplashKit.Rnd(75, 110);
        int actualDamage = Convert.ToInt32(Math.Round(damage * multiplier / 100));    // Actual damage ranges from 0 to 110 depending on damage
        if (actualDamage <=0 )
        {
            actualDamage = 0;
        }
        return actualDamage;
    }

    // Handles monster movement on the floor
    public void MonsterMove(Floor floor)
    {
        bool skipEast = false;
        int skipSouth = -1;
        foreach (Room r in floor.Rooms)
        {
            if (skipEast)
            {
                // The monster just moved east, no need to check for the monster movement again
                skipEast = false;
            }
            else if (skipSouth == 0)
            {
                // The monster just moved south, no need to check for monster movement again
            }
            // If the room exists
            else if (r != null)
            {
                // If a monster is in a room
                if (floor.Rooms[r._position]._monster != null)
                {
                    // Pick a random number between 1 and 4
                    int monsterDirection = SplashKit.Rnd(0, 4);
                    switch(monsterDirection)
                    {
                        case 0: // North
                            if (floor.Rooms[r._position - 20] != null)
                            {
                                if (floor.Rooms[r._position - 20]._monster == null)
                                {
                                    // Move the monster north
                                    floor.Rooms[r._position - 20]._monster = floor.Rooms[r._position]._monster;
                                    floor.Rooms[r._position]._monster = null;
                                }
                                
                            }
                            break;
                        case 1: // West
                            if (floor.Rooms[r._position - 1] != null)
                            {
                                if (floor.Rooms[r._position - 1]._monster == null)
                                {
                                    // Move the monster west
                                    floor.Rooms[r._position - 1]._monster = floor.Rooms[r._position]._monster;
                                    floor.Rooms[r._position]._monster = null;
                                }
                                
                            }
                            break;
                        case 2: // East
                            if (floor.Rooms[r._position + 1] != null)
                            {
                                if (floor.Rooms[r._position + 1]._monster == null)
                                {
                                    // Move the monster east
                                    floor.Rooms[r._position + 1]._monster = floor.Rooms[r._position]._monster;
                                    floor.Rooms[r._position]._monster = null;
                                    skipEast = true;    // Skips the next room
                                }
                                
                            }
                            break;
                        case 3: // South
                            if (floor.Rooms[r._position + 20] != null)
                            {
                                if (floor.Rooms[r._position + 20]._monster == null)
                                {
                                    // Move the monster south
                                    floor.Rooms[r._position + 20]._monster = floor.Rooms[r._position]._monster;
                                    floor.Rooms[r._position]._monster = null;
                                    skipSouth = 20;     // Skips the room south
                                }
                                
                            }
                            break;
                    }
                
                }
                
            }
            skipSouth -= 1;
        }
    }

    public (int, int) InventoryPosition(Item i)
    {
        int position = Player._HeldItems.IndexOf(i);
        int x = 0;
        int y = 0;
        switch(position)
        {
            case 0:
                x = 675; y = 174; break;
            case 1:
                x = 726; y = 174; break;
            case 2:
                x = 777; y = 174; break;
            case 3:
                x = 675; y = 224; break;
            case 4:
                x = 726; y = 224; break;
            case 5:
                x = 777; y = 224; break;
            case 6:
                x = 675; y = 274; break;
            case 7:
                x = 726; y = 274; break;
            case 8:
                x = 777; y = 274; break;
            case 9:
                x = 675; y = 324; break;
            case 10:
                x = 726; y = 324; break;
            case 11:
                x = 777; y = 324; break;
            case 12:
                x = 675; y = 374; break;
            case 13:
                x = 726; y = 374; break;
            case 14:
                x = 777; y = 374; break;
            case 15:
                x = 675; y = 424; break;
            case 16:
                x = 726; y = 424; break;
            case 17:
                x = 777; y = 424; break;
            case 18:
                x = 675; y = 474; break;
            case 19:
                x = 726; y = 474; break;
            case 20:
                x = 777; y = 474; break;
            case 21:
                x = 675; y = 524; break;
             case 22:
                x = 726; y = 524; break;
            case 23:
                x = 777; y = 524; break;
            case 24:
                x = 675; y = 574; break;
            case 25:
                x = 726; y = 574; break;
            case 26:
                x = 777; y = 574; break;
        }
        return (x, y);
    }
}