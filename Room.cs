using System;
using SplashKitSDK;
using System.Collections.Generic;

public abstract class Room
{
    DungeonGame _dungeonGame;
    public List<Item> _RoomItems = new List<Item>();
    public Monster _monster;
    public int _position;   // Used by rooms to check the position of the room in the Rooms array
    protected int x;        // The x position of the room on the map
    protected int y;        // The y position of the room on the map

    // Load the room bitmaps
    protected Bitmap all = new Bitmap("all", "room-all.png");
    protected Bitmap north = new Bitmap("north", "room-n.png");
    protected Bitmap west = new Bitmap("west", "room-w.png");
    protected Bitmap east = new Bitmap("east", "room-e.png");
    protected Bitmap south = new Bitmap("south", "room-s.png");
    protected Bitmap north_west = new Bitmap("north west", "room-nw.png");
    protected Bitmap north_east = new Bitmap("north east", "room-ne.png");
    protected Bitmap north_south = new Bitmap("north south", "room-ns.png");
    protected Bitmap west_east = new Bitmap("west east", "room-we.png");
    protected Bitmap west_south = new Bitmap("west south", "room-sw.png");
    protected Bitmap east_south = new Bitmap("east south", "room-se.png");
    protected Bitmap north_west_east = new Bitmap("north west east", "room-nwe.png");
    protected Bitmap north_west_south = new Bitmap("north west south", "room-nsw.png");
    protected Bitmap north_east_south = new Bitmap("north east south", "room-nse.png");
    protected Bitmap west_east_south = new Bitmap("west east south", "room-swe.png");
    protected Bitmap none = new Bitmap("none", "room-null.png");
    protected Bitmap _monsterImage = new Bitmap("monster", "monster.png");
    protected Bitmap _itemImage = new Bitmap("weapon", "weapon.png");

    public Room(int position, DungeonGame dungeonGame)
    {
        _dungeonGame = dungeonGame;
        _RoomItems.Clear();
        _position = position;

        // Find the x position
        // Since the x position is a multiple of 20
        // We keep on removing 20 until the position is on the first line
        x = position;
        while (x >= 21)
        {
            x -= 20;
        }
        // Find the y position
        // This is found by dividing the location by 20 and rounding up
        double tempY = position;
        tempY = Math.Ceiling(tempY / 20);
        y = Convert.ToInt32(tempY);
    }

    public abstract void Draw(Window _gameWindow, string type);

    public void AddMonster()
    {
        // Add a scaling monster
        // Each floor, monsters gain a flat 10% more power (up to roughly floor 80, where the stats hit max)
        double scaling = _dungeonGame._floorNumber * 0.1;
        // Starting at floor 0, each monster has 10 hp and 5 in all other stats
        double baseHealth = 10;
        baseHealth = baseHealth + (baseHealth * scaling);
        double baseCombat = 100;
        baseCombat = baseCombat + (baseCombat * scaling);
        double baseSpell = 100;
        baseSpell = baseSpell + (baseSpell * scaling);
        double baseProtect = 100;
        baseProtect = baseProtect + (baseProtect * scaling);
        // In addition, each enemy has a 10% chance to spawn as a colored variant
        // Blue enemies have 1.25 * protect, and red enemies have 1.25 * combat
        int type = SplashKit.Rnd(0, 9);
        switch(type)
        {
            case 0: // Blue
                type = 2;
                baseProtect = baseProtect * 1.25;
                break;
            case 1: // Red
                type = 1;
                baseCombat = baseCombat * 1.25;
                break;
            default: // Regular
                type = 0;
                break;
        }
        // Each monster has a random chance of being 0.75 - 1.1 times stronger in each stat
        int health = SplashKit.Rnd(Convert.ToInt32(75 * baseHealth), Convert.ToInt32(110 * baseHealth)) / 100;
        int combat = SplashKit.Rnd(Convert.ToInt32(75 * baseCombat), Convert.ToInt32(110 * baseCombat)) / 100;
        int spell = SplashKit.Rnd(Convert.ToInt32(75 * baseSpell), Convert.ToInt32(110 * baseSpell)) / 100;
        int protect = SplashKit.Rnd(Convert.ToInt32(75 * baseProtect), Convert.ToInt32(110 * baseProtect)) / 100;
        
        _monster = new Monster(health, combat, spell, protect, type);
    }
    
    public void AddWeapon()
    {
        // Add a scaling weapon
        // Every 10 floors, a new type of weapon will spawn
        int weaponName = SplashKit.Rnd(1, 10);
        int floorNumber = (_dungeonGame._floorNumber) / 10;
        if (floorNumber > 10)
        {
            floorNumber = 10;
        }
        int weaponType = SplashKit.Rnd(0, floorNumber + 1);
        Weapon weapon = new Weapon(weaponName, weaponType, 0);
        weapon._price = weapon.Damage * 5;
        _RoomItems.Add(weapon);
    }

    public void AddHelmet()
    {
        // Add a scaling helmet
        // Every 10 floors, a new type of helmet will spawn
        int floorNumber = (_dungeonGame._floorNumber) / 10;
        if (floorNumber > 10)
        {
            floorNumber = 10;
        }
        int helmetType = SplashKit.Rnd(0, floorNumber + 1);
        Helmet helmet = new Helmet(0, helmetType, 0);
        helmet._price = helmet.Armor * 10;
        _RoomItems.Add(helmet);
    }

    public void AddChestplate()
    {
        // Add a scaling chestplate
        // Every 10 floors, a new type of chestplate will spawn
        int floorNumber = (_dungeonGame._floorNumber) / 10;
        if (floorNumber > 10)
        {
            floorNumber = 10;
        }
        int chestplateType = SplashKit.Rnd(0, floorNumber + 1);
        Chestplate chestplate = new Chestplate(0, chestplateType, 0);
        chestplate._price = chestplate.Armor * 10;
        _RoomItems.Add(chestplate);
    }

    public void AddPlatelegs()
    {
        // Add a scaling platelegs
        // Every 10 floors, a new type of platelegs will spawn
        int floorNumber = (_dungeonGame._floorNumber) / 10;
        if (floorNumber > 10)
        {
            floorNumber = 10;
        }
        int platelegsType = SplashKit.Rnd(0, floorNumber + 1);
        Platelegs platelegs = new Platelegs(0, platelegsType, 0);
        platelegs._price = platelegs.Armor * 10;
        _RoomItems.Add(platelegs);
    }

    public void AddShield()
    {
        // Add a scaling shield
        // Every 10 floors, a new type of shield will spawn
        int floorNumber = (_dungeonGame._floorNumber) / 10;
        if (floorNumber > 10)
        {
            floorNumber = 10;
        }
        int shieldType = SplashKit.Rnd(0, floorNumber + 1);
        Shield shield = new Shield(0, shieldType, 0);
        shield._price = shield.Armor * 10;
        _RoomItems.Add(shield);
    }

    public void AddSpell()
    {
        // Add a scaling spell
        // Every 20 floors, a new type of spell will spawn
        int spellName = SplashKit.Rnd(0, 4);
        int floorNumber = (_dungeonGame._floorNumber) / 20;
        if (floorNumber > 5)
        {
            floorNumber = 5;
        }
        int spellType = SplashKit.Rnd(0, floorNumber + 1);
        Spell spell = new Spell(spellName, spellType, 0);
        spell._price = spell.Damage * 2;
        _RoomItems.Add(spell);
    }
    public void AddHealthPotion()
    {
        Potion potion = new Potion(0, 0, 10);
        // Set price value
        int floorNumber = _dungeonGame._floorNumber;
        potion._price = 2 * floorNumber;
        _RoomItems.Add(potion);
    }
    public void AddPotion()
    {
        Potion potion = new Potion(0, SplashKit.Rnd(0, 2), 10);
        // Set price value
        int floorNumber = _dungeonGame._floorNumber;
        potion._price = 2 * floorNumber;
        _RoomItems.Add(potion);
    }
    public void AddRandom()
    {
        switch(SplashKit.Rnd(0, 7))
        {
            case 0: // Weapon
                AddWeapon();
                break;
            case 1: // Helmet
                AddHelmet();
                break;
            case 2: // Chestplate
                AddChestplate();
                break;
            case 3: // Platelegs
                AddPlatelegs();
                break;
            case 4: // Shield
                AddShield();
                break;
            case 5: // Spell
                AddSpell();
                break;
            case 6: // Potion
                AddPotion();
                break;
        }
    }
}

public class Generic : Room
{
    public Generic(int _position, DungeonGame dungeonGame) : base(_position,dungeonGame)
    {
        
    }

    public override void Draw(Window _gameWindow, string type)
    {
        int X = (x * 7) + 652;
        int Y = (y * 7) + 1;
        Bitmap _roomImage = all;
        switch(type)
        {
            case "":
                _roomImage = none;
                break;
            case "n":
                _roomImage = north;
                break;
            case "w":
                _roomImage = west;
                break;
            case "e":
                _roomImage = east;
                break;
            case "s":
                _roomImage = south;
                break;
            case "nw":
                _roomImage = north_west;
                break;
            case "ne":
                _roomImage = north_east;
                break;
            case "ns":
                _roomImage = north_south;
                break;
            case "we":
                _roomImage = west_east;
                break;
            case "ws":
                _roomImage = west_south;
                break;
            case "es":
                _roomImage = east_south;
                break;
            case "nwe":
                _roomImage = north_west_east;
                break;
            case "nws":
                _roomImage = north_west_south;
                break;
            case "nes":
                _roomImage = north_east_south;
                break;
            case "wes":
                _roomImage = west_east_south;
                break;
            case "nwes":
                _roomImage = all;
                break;
        }
        _gameWindow.DrawBitmap(_roomImage, X - (_roomImage.Width / 2), Y - (_roomImage.Height / 2));
        if (_monster != null)
        {
            _gameWindow.DrawBitmap(_monsterImage, X - (_monsterImage.Width / 2), Y - (_monsterImage.Height / 2));
        }
        if (_RoomItems.Count >= 1)
        {
            _gameWindow.DrawBitmap(_itemImage, X - (_itemImage.Width / 2), Y - (_itemImage.Height / 2));
        }
    }
}

public class Stairs : Room
{
    public Stairs(int _position, DungeonGame dungeonGame) : base(_position,dungeonGame)
    {
        
    }

    public override void Draw(Window _gameWindow, string type)
    {
        int X = (x * 7) + 652;
        int Y = (y * 7) + 1;
        Bitmap stairs = new Bitmap("stairs", "stairs.png");
        _gameWindow.DrawBitmap(stairs, X - (stairs.Width / 2), Y - (stairs.Height / 2));
    }
}

public class Store : Room
{
    public Store(int _position, DungeonGame dungeonGame) : base(_position,dungeonGame)
    {
        
    }

    public override void Draw(Window _gameWindow, string type)
    {
        int X = (x * 7) + 652;
        int Y = (y * 7) + 1;
        Bitmap store = new Bitmap("store", "store.png");
        _gameWindow.DrawBitmap(store, X - (store.Width / 2), Y - (store.Height / 2));
    }
}