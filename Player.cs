using System;
using SplashKitSDK;
using System.Collections.Generic;

// Player class controls player related events
public class Player
{
    // Initialise player stats
    private int _maxHealth;
    public int _health;
    private int _maxMagic;
    public int _magic;
    private int _combat;
    private int _spell;
    private int _protect;
    public int _coins;
    public static Weapon _equippedWeapon;
    public static Helmet _equippedHelmet;
    public static Chestplate _equippedChestplate;
    public static Platelegs _equippedPlatelegs;
    public static Shield _equippedShield;
    public int _location;
    private int _rotation;
    private Floor _floor;
    public static bool[] _roomsExplored = new bool[400];

    public int MaxHealth { get { return _maxHealth; } }
    public int Health { get { return _health; } }
    public int MaxMagic {get {return _maxMagic; } }
    public int Magic { get { return _magic; } }
    public int Combat { get { return _combat; } }
    public int Spell { get { return _spell; } }
    public int Protect { get { return _protect; } }
    public int Coins { get { return _coins; } }
    public Weapon EquippedWeapon { get { return _equippedWeapon; } }
    public Helmet EquippedHelmet { get { return _equippedHelmet; } }
    public Chestplate EquippedChestplate { get { return _equippedChestplate; } }
    public Platelegs EquippedPlatelegs { get { return _equippedPlatelegs; } }
    public Shield EquippedShield { get { return _equippedShield; } }
    public int Location { get { return _location; } }
    public int Rotation { get {return _rotation; } }

    Bitmap player = new Bitmap("player", "player.png");

    // Initilalise inventory lists
    public static List<Item> _HeldItems = new List<Item>(27);

    // Constructor for Player; set the stats for the player and gives them a bronze knife
    public Player(Floor floor)
    {
        // Set stats
        _maxHealth = 100;
        _health = 100;
        _maxMagic = 100;
        _magic = 100;
        _combat = 100;
        _spell = 100;
        _protect = 100;
        _coins = 0;
        _location = 0;
        _rotation = SplashKit.Rnd(0, 4);
        _floor = floor;
        
        // Start the player with a set of items
        Weapon _newWeapon = new Weapon(0, 0, 0);
        Potion _newHealthPotion = new Potion (0, 0, 10);
        Potion _newMagicPotion = new Potion (0, 1, 10);
        _HeldItems.Add(_newWeapon);
        _HeldItems.Add(_newHealthPotion);
        _HeldItems.Add(_newMagicPotion);
        EquipWeapon(_newWeapon);
    }

    // Method for adding an item to the players inventory
    public void AddItem(Item _addItem)
    {
        if (_HeldItems.Count <= 27)
        {
            // Add the item to the inventory
            _HeldItems.Add(_addItem);
        }
        else
        {
            // Drop the weapon to the screen
            _floor.Rooms[_location]._RoomItems.Add(_addItem);
        }
    }

    // Method for equipping a player with a weapon
    public void EquipWeapon(Weapon _equipWeapon)
    {
        _equippedWeapon = _equipWeapon;
        _HeldItems.Remove(_equipWeapon);
    }

    // Method for equipping a player with a helmet
    public void EquipHelmet(Helmet _equipHelmet)
    {
        _equippedHelmet = _equipHelmet;
        _HeldItems.Remove(_equipHelmet);
    }

    // Method for equipping a player with a chestplate
    public void EquipChestplate(Chestplate _equipChestplate)
    {
        _equippedChestplate = _equipChestplate;
        _HeldItems.Remove(_equipChestplate);
    }

    // Method for equipping a player with platelegs
    public void EquipPlatelegs(Platelegs _equipPlatelegs)
    {
        _equippedPlatelegs = _equipPlatelegs;
        _HeldItems.Remove(_equipPlatelegs);
    }

    // Method for equipping a player with a shield
    public void EquipShield(Shield _equipShield)
    {
        _equippedShield = _equipShield;
        _HeldItems.Remove(_equipShield);
    }

    public bool Move(Floor floor)
    {
        // Check if the move is valid, then execute the move
        switch(_rotation)
        {
            case 0:
                if (_location > 20)
                {
                    if (floor.Rooms[_location - 20] != null)
                    {
                        _location -= 20;
                        return true;
                    }
                }
                break;
            case 1:
                if (_location < 399)
                {
                    if (floor.Rooms[_location + 1] != null && !Array.Exists(floor.rightBorder, number => number == _location))
                    {
                        _location += 1;
                        return true;
                    }
                }
                break;
            case 2:
                if (_location < 380)
                {
                    if (floor.Rooms[_location + 20] != null)
                    {
                        _location += 20;
                        return true;
                    }
                }
                break;
            case 3:
                if (_location > 1)
                {
                    if (floor.Rooms[_location - 1] != null && !Array.Exists(floor.leftBorder, number => number == _location))
                    {
                        _location -= 1;
                        return true;
                    }
                }
                break;
        }
        return false;
    }

    public void LevelUp(int health, int magic, int combat, int spell, int protect)
    {
        _maxHealth += health;
        _health += health;
        _maxMagic += magic;
        _magic += magic;
        _combat += combat;
        _spell += spell;
        _protect += protect;
    }

    public void Rotate(int direction)
    {
        switch(direction)
        {
            case 0:
                if (_rotation > 0) { _rotation -= 1; }
                else { _rotation = 3; }
                break;
            case 1:
                if (_rotation < 3) { _rotation += 1; }
                else { _rotation = 0; }
                break;
        }
    }

    // Draw the player related bitmaps
    public void Draw(Window _gameWindow)
    {
        // Draw the player on the map
        int x = _location;
        while (x >= 21)
        {
            x -= 20;
        }
        double tempY = _location;
        tempY = Math.Ceiling(tempY / 20);
        int y = Convert.ToInt32(tempY);
        int X = (x * 7) + 652;
        int Y = (y * 7) + 1;
        _gameWindow.DrawBitmap(player, X - (player.Width / 2), Y - (player.Height / 2), SplashKit.OptionRotateBmp(_rotation * 90));

        // Display the player stats
        _gameWindow.DrawText(Convert.ToString(_health), Color.White, 50, 471);
        _gameWindow.DrawText(Convert.ToString(_magic), Color.White, 50, 506);
        _gameWindow.DrawText(Convert.ToString(_combat) + "%", Color.White, 50, 540);
        _gameWindow.DrawText(Convert.ToString(_protect) + "%", Color.White, 50, 574);
        _gameWindow.DrawText(Convert.ToString(_spell) + "%", Color.White, 150, 540);
        _gameWindow.DrawText(Convert.ToString(_coins), Color.White, 150, 574);
    }
}