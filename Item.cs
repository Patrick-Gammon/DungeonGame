using System;
using SplashKitSDK;
using System.Collections.Generic;

public abstract class Item
{
    public int x;
    public int y;
    public int _price;
    protected string _name;
    protected string _description;
    public bool followCursor = false;

    public virtual string Name { get { return _name; } }
    public virtual string Description { get { return _description ; } }
    public virtual int Price { get { return _price; } }

    public Item(int name, int type, int modifier)
    {
        _name = "failed to load string.";
        _name = "failed to load description.";
    }

    public abstract int Use(Player _player, int index, bool doFight);

    public abstract void Draw(Window gameWindow, int x, int y);
}

// Weapon contains the framework for weapons and calculations
public class Weapon : Item
{
    int _weaponIndex;
    int _typeIndex;
    int _modifierIndex;
    int _damage;
    new private string _name;
    new private string _description;

    private string[,] weaponArray = new string[,] {{"knife", "2"}, {"dagger", "3"}, {"sword", "4"}, {"axe", "5"}, {"hammer", "3"}, {"mace", "4"}, {"flail", "5"}, {"fauchard", "3"}, {"glaive", "4"}, {"naginata", "5"}};
    private string[,] typeArray = new string[,] {{"bronze ", "1"}, {"iron ", "2"}, {"steel ", "3"}, {"silver ", "4"}, {"golden ", "5"}, {"jade ", "6"}, {"ruby ", "7"}, {"glyph ", "8"}, {"adamant ", "9"}, {"unobtainium ", "10"}};
    private string[] modifierArray = new string[] {"", "poisoned ", "scorching ", "augmented ", "strong ", "piercing ", "deft ", "accurate "};
    // Players should start with a bronze knife (with regular stats, players will hit for about 2.67HP against 0 Protection)
    // Poisoned deals bonus damage to living enemies
    // Scorching deals bonus damage to undead enemies
    // Augmented causes the weapon to deal both combat and spell damage
    // Strong causes the weapon to always hit max
    // Piercing causes the weapon to ignore some armor
    // Deft causes the attack to ignore blocks
    // Accurate causes the weapon to never miss

    // Load all the bitmaps
    Bitmap bronze_knife = new Bitmap("bronze knife", "bronze-knife.png");
    Bitmap bronze_dagger = new Bitmap("bronze dagger", "bronze-dagger.png");
    Bitmap bronze_sword = new Bitmap("bronze sword", "bronze-sword.png");
    Bitmap bronze_axe = new Bitmap("bronze axe", "bronze-axe.png");
    Bitmap bronze_hammer = new Bitmap("bronze hammer", "bronze-hammer.png");
    Bitmap bronze_mace = new Bitmap("bronze mace", "bronze-mace.png");
    Bitmap bronze_flail = new Bitmap("bronze flail", "bronze-flail.png");
    Bitmap bronze_fauchard = new Bitmap("bronze fauchard", "bronze-fauchard.png");
    Bitmap bronze_glaive = new Bitmap("bronze glaive", "bronze-glaive.png");
    Bitmap bronze_naginata = new Bitmap("bronze naginata", "bronze-naginata.png");
    Bitmap iron_knife = new Bitmap("iron knife", "iron-knife.png");
    Bitmap iron_dagger = new Bitmap("iron dagger", "iron-dagger.png");
    Bitmap iron_sword = new Bitmap("iron sword", "iron-sword.png");
    Bitmap iron_axe = new Bitmap("iron axe", "iron-axe.png");
    Bitmap iron_hammer = new Bitmap("iron hammer", "iron-hammer.png");
    Bitmap iron_mace = new Bitmap("iron mace", "iron-mace.png");
    Bitmap iron_flail = new Bitmap("iron flail", "iron-flail.png");
    Bitmap iron_fauchard = new Bitmap("iron fauchard", "iron-fauchard.png");
    Bitmap iron_glaive = new Bitmap("iron glaive", "iron-glaive.png");
    Bitmap iron_naginata = new Bitmap("iron naginata", "iron-naginata.png");
    Bitmap steel_knife = new Bitmap("steel knife", "steel-knife.png");
    Bitmap steel_dagger = new Bitmap("steel dagger", "steel-dagger.png");
    Bitmap steel_sword = new Bitmap("steel sword", "steel-sword.png");
    Bitmap steel_axe = new Bitmap("steel axe", "steel-axe.png");
    Bitmap steel_hammer = new Bitmap("steel hammer", "steel-hammer.png");
    Bitmap steel_mace = new Bitmap("steel mace", "steel-mace.png");
    Bitmap steel_flail = new Bitmap("steel flail", "steel-flail.png");
    Bitmap steel_fauchard = new Bitmap("steel fauchard", "steel-fauchard.png");
    Bitmap steel_glaive = new Bitmap("steel glaive", "steel-glaive.png");
    Bitmap steel_naginata = new Bitmap("steel naginata", "steel-naginata.png");
    Bitmap silver_knife = new Bitmap("silver knife", "silver-knife.png");
    Bitmap silver_dagger = new Bitmap("silver dagger", "silver-dagger.png");
    Bitmap silver_sword = new Bitmap("silver sword", "silver-sword.png");
    Bitmap silver_axe = new Bitmap("silver axe", "silver-axe.png");
    Bitmap silver_hammer = new Bitmap("silver hammer", "silver-hammer.png");
    Bitmap silver_mace = new Bitmap("silver mace", "silver-mace.png");
    Bitmap silver_flail = new Bitmap("silver flail", "silver-flail.png");
    Bitmap silver_fauchard = new Bitmap("silver fauchard", "silver-fauchard.png");
    Bitmap silver_glaive = new Bitmap("silver glaive", "silver-glaive.png");
    Bitmap silver_naginata = new Bitmap("silver naginata", "silver-naginata.png");
    Bitmap golden_knife = new Bitmap("golden knife", "golden-knife.png");
    Bitmap golden_dagger = new Bitmap("golden dagger", "golden-dagger.png");
    Bitmap golden_sword = new Bitmap("golden sword", "golden-sword.png");
    Bitmap golden_axe = new Bitmap("golden axe", "golden-axe.png");
    Bitmap golden_hammer = new Bitmap("golden hammer", "golden-hammer.png");
    Bitmap golden_mace = new Bitmap("golden mace", "golden-mace.png");
    Bitmap golden_flail = new Bitmap("golden flail", "golden-flail.png");
    Bitmap golden_fauchard = new Bitmap("golden fauchard", "golden-fauchard.png");
    Bitmap golden_glaive = new Bitmap("golden glaive", "golden-glaive.png");
    Bitmap golden_naginata = new Bitmap("golden naginata", "golden-naginata.png");
    Bitmap jade_knife = new Bitmap("jade knife", "jade-knife.png");
    Bitmap jade_dagger = new Bitmap("jade dagger", "jade-dagger.png");
    Bitmap jade_sword = new Bitmap("jade sword", "jade-sword.png");
    Bitmap jade_axe = new Bitmap("jade axe", "jade-axe.png");
    Bitmap jade_hammer = new Bitmap("jade hammer", "jade-hammer.png");
    Bitmap jade_mace = new Bitmap("jade mace", "jade-mace.png");
    Bitmap jade_flail = new Bitmap("jade flail", "jade-flail.png");
    Bitmap jade_fauchard = new Bitmap("jade fauchard", "jade-fauchard.png");
    Bitmap jade_glaive = new Bitmap("jade glaive", "jade-glaive.png");
    Bitmap jade_naginata = new Bitmap("jade naginata", "jade-naginata.png");
    Bitmap ruby_knife = new Bitmap("ruby knife", "ruby-knife.png");
    Bitmap ruby_dagger = new Bitmap("ruby dagger", "ruby-dagger.png");
    Bitmap ruby_sword = new Bitmap("ruby sword", "ruby-sword.png");
    Bitmap ruby_axe = new Bitmap("ruby axe", "ruby-axe.png");
    Bitmap ruby_hammer = new Bitmap("ruby hammer", "ruby-hammer.png");
    Bitmap ruby_mace = new Bitmap("ruby mace", "ruby-mace.png");
    Bitmap ruby_flail = new Bitmap("ruby flail", "ruby-flail.png");
    Bitmap ruby_fauchard = new Bitmap("ruby fauchard", "ruby-fauchard.png");
    Bitmap ruby_glaive = new Bitmap("ruby glaive", "ruby-glaive.png");
    Bitmap ruby_naginata = new Bitmap("ruby naginata", "ruby-naginata.png");
    Bitmap glyph_knife = new Bitmap("glyph knife", "glyph-knife.png");
    Bitmap glyph_dagger = new Bitmap("glyph dagger", "glyph-dagger.png");
    Bitmap glyph_sword = new Bitmap("glyph sword", "glyph-sword.png");
    Bitmap glyph_axe = new Bitmap("glyph axe", "glyph-axe.png");
    Bitmap glyph_hammer = new Bitmap("glyph hammer", "glyph-hammer.png");
    Bitmap glyph_mace = new Bitmap("glyph mace", "glyph-mace.png");
    Bitmap glyph_flail = new Bitmap("glyph flail", "glyph-flail.png");
    Bitmap glyph_fauchard = new Bitmap("glyph fauchard", "glyph-fauchard.png");
    Bitmap glyph_glaive = new Bitmap("glyph glaive", "glyph-glaive.png");
    Bitmap glyph_naginata = new Bitmap("glyph naginata", "glyph-naginata.png");
    Bitmap adamant_knife = new Bitmap("adamant knife", "adamant-knife.png");
    Bitmap adamant_dagger = new Bitmap("adamant dagger", "adamant-dagger.png");
    Bitmap adamant_sword = new Bitmap("adamant sword", "adamant-sword.png");
    Bitmap adamant_axe = new Bitmap("adamant axe", "adamant-axe.png");
    Bitmap adamant_hammer = new Bitmap("adamant hammer", "adamant-hammer.png");
    Bitmap adamant_mace = new Bitmap("adamant mace", "adamant-mace.png");
    Bitmap adamant_flail = new Bitmap("adamant flail", "adamant-flail.png");
    Bitmap adamant_fauchard = new Bitmap("adamant fauchard", "adamant-fauchard.png");
    Bitmap adamant_glaive = new Bitmap("adamant glaive", "adamant-glaive.png");
    Bitmap adamant_naginata = new Bitmap("adamant naginata", "adamant-naginata.png");
    Bitmap unobtainium_knife = new Bitmap("unobtainium knife", "unobtainium-knife.png");
    Bitmap unobtainium_dagger = new Bitmap("unobtainium dagger", "unobtainium-dagger.png");
    Bitmap unobtainium_sword = new Bitmap("unobtainium sword", "unobtainium-sword.png");
    Bitmap unobtainium_axe = new Bitmap("unobtainium axe", "unobtainium-axe.png");
    Bitmap unobtainium_hammer = new Bitmap("unobtainium hammer", "unobtainium-hammer.png");
    Bitmap unobtainium_mace = new Bitmap("unobtainium mace", "unobtainium-mace.png");
    Bitmap unobtainium_flail = new Bitmap("unobtainium flail", "unobtainium-flail.png");
    Bitmap unobtainium_fauchard = new Bitmap("unobtainium fauchard", "unobtainium-fauchard.png");
    Bitmap unobtainium_glaive = new Bitmap("unobtainium glaive", "unobtainium-glaive.png");
    Bitmap unobtainium_naginata = new Bitmap("unobtainium naginata", "unobtainium-naginata.png");

    private static List<Weapon> _Weapons = new List<Weapon>();

    public int Damage { get { return _damage; } }
    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    // Constructor for Weapon; identifies the damage and name of the weapon
    public Weapon(int weapon, int type, int modifier) : base(weapon, type, modifier)
    {
        // Initialise fields
        _weaponIndex = weapon;
        _typeIndex = type;
        _modifierIndex = modifier;

        // Create weapon damage from fields
        int _weaponInt = Convert.ToInt32(weaponArray[_weaponIndex, 1]);
        int _typeInt = Convert.ToInt32(typeArray[_typeIndex, 1]);
        _damage = _weaponInt * _typeInt;

        // Create weapon name from fields
        string _weaponString = weaponArray[_weaponIndex, 0];
        string _typeString = typeArray[_typeIndex, 0];
        string _modifierString = modifierArray[_modifierIndex];
        _name = String.Concat(_modifierString, _typeString, _weaponString);

        // Create weapon description
        _description = ("A " + _weaponString + ". Has " + _damage + " power.");
    }

    // Equips the weapon
    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            // Also check if an item is already in the weapon slot
            if (Player._equippedWeapon != null)
            {
                Player._HeldItems.Add(Player._equippedWeapon);
            }
            _player.EquipWeapon((Weapon)Player._HeldItems[index]);
        }
        else    // Equipped from the hand
        {
            if (Player._equippedWeapon != null)
            {
                Player._HeldItems.Add(Player._equippedWeapon);
            }
            _player.EquipWeapon(this);
        }
        return -1;   // Always returns -1 since equipping a weapon doesn't use a turn
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _weaponImage = bronze_knife;
        switch(_weaponIndex)
        {
            case 0:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_knife; break;
                    case 1:
                        _weaponImage = iron_knife; break;
                    case 2:
                        _weaponImage = steel_knife; break;
                    case 3:
                        _weaponImage = silver_knife; break;
                    case 4:
                        _weaponImage = golden_knife; break;
                    case 5:
                        _weaponImage = jade_knife; break;
                    case 6:
                        _weaponImage = ruby_knife; break;
                    case 7:
                        _weaponImage = glyph_knife; break;
                    case 8:
                        _weaponImage = adamant_knife; break;
                    case 9:
                        _weaponImage = unobtainium_knife; break;
                }
                break;
            case 1:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_dagger; break;
                    case 1:
                        _weaponImage = iron_dagger; break;
                    case 2:
                        _weaponImage = steel_dagger; break;
                    case 3:
                        _weaponImage = silver_dagger; break;
                    case 4:
                        _weaponImage = golden_dagger; break;
                    case 5:
                        _weaponImage = jade_dagger; break;
                    case 6:
                        _weaponImage = ruby_dagger; break;
                    case 7:
                        _weaponImage = glyph_dagger; break;
                    case 8:
                        _weaponImage = adamant_dagger; break;
                    case 9:
                        _weaponImage = unobtainium_dagger; break;
                }
                break;
            case 2:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_sword; break;
                    case 1:
                        _weaponImage = iron_sword; break;
                    case 2:
                        _weaponImage = steel_sword; break;
                    case 3:
                        _weaponImage = silver_sword; break;
                    case 4:
                        _weaponImage = golden_sword; break;
                    case 5:
                        _weaponImage = jade_sword; break;
                    case 6:
                        _weaponImage = ruby_sword; break;
                    case 7:
                        _weaponImage = glyph_sword; break;
                    case 8:
                        _weaponImage = adamant_sword; break;
                    case 9:
                        _weaponImage = unobtainium_sword; break;
                }
                break;
            case 3:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_axe; break;
                    case 1:
                        _weaponImage = iron_axe; break;
                    case 2:
                        _weaponImage = steel_axe; break;
                    case 3:
                        _weaponImage = silver_axe; break;
                    case 4:
                        _weaponImage = golden_axe; break;
                    case 5:
                        _weaponImage = jade_axe; break;
                    case 6:
                        _weaponImage = ruby_axe; break;
                    case 7:
                        _weaponImage = glyph_axe; break;
                    case 8:
                        _weaponImage = adamant_axe; break;
                    case 9:
                        _weaponImage = unobtainium_axe; break;
                }
                break;
            case 4:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_hammer; break;
                    case 1:
                        _weaponImage = iron_hammer; break;
                    case 2:
                        _weaponImage = steel_hammer; break;
                    case 3:
                        _weaponImage = silver_hammer; break;
                    case 4:
                        _weaponImage = golden_hammer; break;
                    case 5:
                        _weaponImage = jade_hammer; break;
                    case 6:
                        _weaponImage = ruby_hammer; break;
                    case 7:
                        _weaponImage = glyph_hammer; break;
                    case 8:
                        _weaponImage = adamant_hammer; break;
                    case 9:
                        _weaponImage = unobtainium_hammer; break;
                }
                break;
            case 5:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_mace; break;
                    case 1:
                        _weaponImage = iron_mace; break;
                    case 2:
                        _weaponImage = steel_mace; break;
                    case 3:
                        _weaponImage = silver_mace; break;
                    case 4:
                        _weaponImage = golden_mace; break;
                    case 5:
                        _weaponImage = jade_mace; break;
                    case 6:
                        _weaponImage = ruby_mace; break;
                    case 7:
                        _weaponImage = glyph_mace; break;
                    case 8:
                        _weaponImage = adamant_mace; break;
                    case 9:
                        _weaponImage = unobtainium_mace; break;
                }
                break;
            case 6:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_flail; break;
                    case 1:
                        _weaponImage = iron_flail; break;
                    case 2:
                        _weaponImage = steel_flail; break;
                    case 3:
                        _weaponImage = silver_flail; break;
                    case 4:
                        _weaponImage = golden_flail; break;
                    case 5:
                        _weaponImage = jade_flail; break;
                    case 6:
                        _weaponImage = ruby_flail; break;
                    case 7:
                        _weaponImage = glyph_flail; break;
                    case 8:
                        _weaponImage = adamant_flail; break;
                    case 9:
                        _weaponImage = unobtainium_flail; break;
                }
                break;
            case 7:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_fauchard; break;
                    case 1:
                        _weaponImage = iron_fauchard; break;
                    case 2:
                        _weaponImage = steel_fauchard; break;
                    case 3:
                        _weaponImage = silver_fauchard; break;
                    case 4:
                        _weaponImage = golden_fauchard; break;
                    case 5:
                        _weaponImage = jade_fauchard; break;
                    case 6:
                        _weaponImage = ruby_fauchard; break;
                    case 7:
                        _weaponImage = glyph_fauchard; break;
                    case 8:
                        _weaponImage = adamant_fauchard; break;
                    case 9:
                        _weaponImage = unobtainium_fauchard; break;
                }
                break;
            case 8:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_glaive; break;
                    case 1:
                        _weaponImage = iron_glaive; break;
                    case 2:
                        _weaponImage = steel_glaive; break;
                    case 3:
                        _weaponImage = silver_glaive; break;
                    case 4:
                        _weaponImage = golden_glaive; break;
                    case 5:
                        _weaponImage = jade_glaive; break;
                    case 6:
                        _weaponImage = ruby_glaive; break;
                    case 7:
                        _weaponImage = glyph_glaive; break;
                    case 8:
                        _weaponImage = adamant_glaive; break;
                    case 9:
                        _weaponImage = unobtainium_glaive; break;
                }
                break;
            case 9:
                switch(_typeIndex)
                {
                    case 0:
                        _weaponImage = bronze_naginata; break;
                    case 1:
                        _weaponImage = iron_naginata; break;
                    case 2:
                        _weaponImage = steel_naginata; break;
                    case 3:
                        _weaponImage = silver_naginata; break;
                    case 4:
                        _weaponImage = golden_naginata; break;
                    case 5:
                        _weaponImage = jade_naginata; break;
                    case 6:
                        _weaponImage = ruby_naginata; break;
                    case 7:
                        _weaponImage = glyph_naginata; break;
                    case 8:
                        _weaponImage = adamant_naginata; break;
                    case 9:
                        _weaponImage = unobtainium_naginata; break;
                }
                break;
        }

        _gameWindow.DrawBitmap(_weaponImage, x - (_weaponImage.Width / 2), y - (_weaponImage.Height / 2));
    }
}

public class Helmet: Item
{
    int _typeIndex;
    int _armor;
    new private string _name;

    private string[,] typeArray = new string[,] {{"bronze ", "1"}, {"iron ", "2"}, {"steel ", "3"}, {"silver ", "4"}, {"golden ", "5"}, {"jade ", "6"}, {"ruby ", "7"}, {"glyph ", "8"}, {"adamant ", "9"}, {"unobtainium ", "10"}};

    // Load all the bitmaps
    Bitmap bronze_helmet = new Bitmap("bronze helmet", "bronze-helmet.png");
    Bitmap iron_helmet = new Bitmap("iron helmet", "iron-helmet.png");
    Bitmap steel_helmet = new Bitmap("steel helmet", "steel-helmet.png");
    Bitmap silver_helmet = new Bitmap("silver helmet", "silver-helmet.png");
    Bitmap golden_helmet = new Bitmap("golden helmet", "golden-helmet.png");
    Bitmap jade_helmet = new Bitmap("jade helmet", "jade-helmet.png");
    Bitmap ruby_helmet = new Bitmap("ruby helmet", "ruby-helmet.png");
    Bitmap glyph_helmet = new Bitmap("glyph helmet", "glyph-helmet.png");
    Bitmap adamant_helmet = new Bitmap("adamant helmet", "adamant-helmet.png");
    Bitmap unobtainium_helmet = new Bitmap("unobtainium helmet", "unobtainium-helmet.png");

    private static List<Helmet> _Helmets = new List<Helmet>();

    public int Armor { get { return _armor; } }
    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    // Constructor for Helmet; identifies the armor and name of the helmet
    public Helmet(int helmet, int type, int modifier) : base(helmet, type, modifier)
    {
        // Initialise fields
        _typeIndex = type;

        // Create helmet defense from fields
        _armor = Convert.ToInt32(typeArray[_typeIndex, 1]);

        // Create helmet name from fields
        _name = String.Concat(typeArray[_typeIndex, 0], " helmet");
        
        // Create helmet description from fields
        _description = ("A helmet. Has " + _armor + " armor.");
    }

    // Equips the helmet
    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            // Also check if an item is already in the helmet slot
            if (Player._equippedHelmet != null)
            {
                Player._HeldItems.Add(Player._equippedHelmet);
            }
            _player.EquipHelmet((Helmet)Player._HeldItems[index]);
        }
        else    // Equipped from the hand
        {
            if (Player._equippedHelmet != null)
            {
                Player._HeldItems.Add(Player._equippedHelmet);
            }
            _player.EquipHelmet(this);
        }
        return -1;   // Always returns -1 since equipping a helmet doesn't use a turn
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _helmetImage = bronze_helmet;
        switch(_typeIndex)
        {
            case 0:
                _helmetImage = bronze_helmet; break;
            case 1:
                _helmetImage = iron_helmet; break;
            case 2:
                _helmetImage = steel_helmet; break;
            case 3:
                _helmetImage = silver_helmet; break;
            case 4:
                _helmetImage = golden_helmet; break;
            case 5:
                _helmetImage = jade_helmet; break;
            case 6:
                _helmetImage = ruby_helmet; break;
            case 7:
                _helmetImage = glyph_helmet; break;
            case 8:
                _helmetImage = adamant_helmet; break;
            case 9:
                _helmetImage = unobtainium_helmet; break;
        }
        _gameWindow.DrawBitmap(_helmetImage, x - (_helmetImage.Width / 2), y - (_helmetImage.Height / 2));
    }
}

public class Chestplate: Item
{
    int _typeIndex;
    int _armor;
    new private string _name;

    private string[,] typeArray = new string[,] {{"bronze ", "1"}, {"iron ", "2"}, {"steel ", "3"}, {"silver ", "4"}, {"golden ", "5"}, {"jade ", "6"}, {"ruby ", "7"}, {"glyph ", "8"}, {"adamant ", "9"}, {"unobtainium ", "10"}};

    // Load all the bitmaps
    Bitmap bronze_chestplate = new Bitmap("bronze chestplate", "bronze-chestplate.png");
    Bitmap iron_chestplate = new Bitmap("iron chestplate", "iron-chestplate.png");
    Bitmap steel_chestplate = new Bitmap("steel chestplate", "steel-chestplate.png");
    Bitmap silver_chestplate = new Bitmap("silver chestplate", "silver-chestplate.png");
    Bitmap golden_chestplate = new Bitmap("golden chestplate", "golden-chestplate.png");
    Bitmap jade_chestplate = new Bitmap("jade chestplate", "jade-chestplate.png");
    Bitmap ruby_chestplate = new Bitmap("ruby chestplate", "ruby-chestplate.png");
    Bitmap glyph_chestplate = new Bitmap("glyph chestplate", "glyph-chestplate.png");
    Bitmap adamant_chestplate = new Bitmap("adamant chestplate", "adamant-chestplate.png");
    Bitmap unobtainium_chestplate = new Bitmap("unobtainium chestplate", "unobtainium-chestplate.png");

    private static List<Chestplate> _Chestplates = new List<Chestplate>();

    public int Armor { get { return _armor; } }
    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    // Constructor for Chestplate; identifies the armor and name of the chestplate
    public Chestplate(int chestplate, int type, int modifier) : base(chestplate, type, modifier)
    {
        // Initialise fields
        _typeIndex = type;

        // Create chestplate defense from fields
        _armor = Convert.ToInt32(typeArray[_typeIndex, 1]);

        // Create chestplate name from fields
        _name = String.Concat(typeArray[_typeIndex, 0], " chestplate");
        
        // Create chestplate description from fields
        _description = ("A chestplate. Has " + _armor + " armor.");
    }

    // Equips the chestplate
    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            // Also check if an item is already in the chestplate slot
            if (Player._equippedChestplate != null)
            {
                Player._HeldItems.Add(Player._equippedChestplate);
            }
            _player.EquipChestplate((Chestplate)Player._HeldItems[index]);
        }
        else    // Equipped from the hand
        {
            if (Player._equippedChestplate != null)
            {
                Player._HeldItems.Add(Player._equippedChestplate);
            }
            _player.EquipChestplate(this);
        }
        return -1;   // Always returns -1 since equipping a chestplate doesn't use a turn
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _chestplateImage = bronze_chestplate;
        switch(_typeIndex)
        {
            case 0:
                _chestplateImage = bronze_chestplate; break;
            case 1:
                _chestplateImage = iron_chestplate; break;
            case 2:
                _chestplateImage = steel_chestplate; break;
            case 3:
                _chestplateImage = silver_chestplate; break;
            case 4:
                _chestplateImage = golden_chestplate; break;
            case 5:
                _chestplateImage = jade_chestplate; break;
            case 6:
                _chestplateImage = ruby_chestplate; break;
            case 7:
                _chestplateImage = glyph_chestplate; break;
            case 8:
                _chestplateImage = adamant_chestplate; break;
            case 9:
                _chestplateImage = unobtainium_chestplate; break;
        }
        _gameWindow.DrawBitmap(_chestplateImage, x - (_chestplateImage.Width / 2), y - (_chestplateImage.Height / 2));
    }
}

public class Platelegs: Item
{
    int _typeIndex;
    int _armor;
    new private string _name;

    private string[,] typeArray = new string[,] {{"bronze ", "1"}, {"iron ", "2"}, {"steel ", "3"}, {"silver ", "4"}, {"golden ", "5"}, {"jade ", "6"}, {"ruby ", "7"}, {"glyph ", "8"}, {"adamant ", "9"}, {"unobtainium ", "10"}};

    // Load all the bitmaps
    Bitmap bronze_platelegs = new Bitmap("bronze platelegs", "bronze-platelegs.png");
    Bitmap iron_platelegs = new Bitmap("iron platelegs", "iron-platelegs.png");
    Bitmap steel_platelegs = new Bitmap("steel platelegs", "steel-platelegs.png");
    Bitmap silver_platelegs = new Bitmap("silver platelegs", "silver-platelegs.png");
    Bitmap golden_platelegs = new Bitmap("golden platelegs", "golden-platelegs.png");
    Bitmap jade_platelegs = new Bitmap("jade platelegs", "jade-platelegs.png");
    Bitmap ruby_platelegs = new Bitmap("ruby platelegs", "ruby-platelegs.png");
    Bitmap glyph_platelegs = new Bitmap("glyph platelegs", "glyph-platelegs.png");
    Bitmap adamant_platelegs = new Bitmap("adamant platelegs", "adamant-platelegs.png");
    Bitmap unobtainium_platelegs = new Bitmap("unobtainium platelegs", "unobtainium-platelegs.png");

    private static List<Platelegs> _Platelegs = new List<Platelegs>();

    public int Armor { get { return _armor; } }
    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    // Constructor for Platelegs; identifies the armor and name of the platelegs
    public Platelegs(int platelegs, int type, int modifier) : base(platelegs, type, modifier)
    {
        // Initialise fields
        _typeIndex = type;

        // Create platelegs defense from fields
        _armor = Convert.ToInt32(typeArray[_typeIndex, 1]);

        // Create platelegs name from fields
        _name = String.Concat(typeArray[_typeIndex, 0], " platelegs");
        
        // Create platelegs description from fields
        _description = ("Platelegs. Has " + _armor + " armor.");
    }

    // Equips the platelegs
    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            // Also check if an item is already in the platelegs slot
            if (Player._equippedPlatelegs != null)
            {
                Player._HeldItems.Add(Player._equippedPlatelegs);
            }
            _player.EquipPlatelegs((Platelegs)Player._HeldItems[index]);
        }
        else    // Equipped from the hand
        {
            if (Player._equippedPlatelegs != null)
            {
                Player._HeldItems.Add(Player._equippedPlatelegs);
            }
            _player.EquipPlatelegs(this);
        }
        return -1;   // Always returns -1 since equipping a platelegs doesn't use a turn
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _platelegsImage = bronze_platelegs;
        switch(_typeIndex)
        {
            case 0:
                _platelegsImage = bronze_platelegs; break;
            case 1:
                _platelegsImage = iron_platelegs; break;
            case 2:
                _platelegsImage = steel_platelegs; break;
            case 3:
                _platelegsImage = silver_platelegs; break;
            case 4:
                _platelegsImage = golden_platelegs; break;
            case 5:
                _platelegsImage = jade_platelegs; break;
            case 6:
                _platelegsImage = ruby_platelegs; break;
            case 7:
                _platelegsImage = glyph_platelegs; break;
            case 8:
                _platelegsImage = adamant_platelegs; break;
            case 9:
                _platelegsImage = unobtainium_platelegs; break;
        }
        _gameWindow.DrawBitmap(_platelegsImage, x - (_platelegsImage.Width / 2), y - (_platelegsImage.Height / 2));
    }
}

public class Shield: Item
{
    int _typeIndex;
    int _armor;
    new private string _name;

    private string[,] typeArray = new string[,] {{"bronze ", "1"}, {"iron ", "2"}, {"steel ", "3"}, {"silver ", "4"}, {"golden ", "5"}, {"jade ", "6"}, {"ruby ", "7"}, {"glyph ", "8"}, {"adamant ", "9"}, {"unobtainium ", "10"}};

    // Load all the bitmaps
    Bitmap bronze_shield = new Bitmap("bronze shield", "bronze-shield.png");
    Bitmap iron_shield = new Bitmap("iron shield", "iron-shield.png");
    Bitmap steel_shield = new Bitmap("steel shield", "steel-shield.png");
    Bitmap silver_shield = new Bitmap("silver shield", "silver-shield.png");
    Bitmap golden_shield = new Bitmap("golden shield", "golden-shield.png");
    Bitmap jade_shield = new Bitmap("jade shield", "jade-shield.png");
    Bitmap ruby_shield = new Bitmap("ruby shield", "ruby-shield.png");
    Bitmap glyph_shield = new Bitmap("glyph shield", "glyph-shield.png");
    Bitmap adamant_shield = new Bitmap("adamant shield", "adamant-shield.png");
    Bitmap unobtainium_shield = new Bitmap("unobtainium shield", "unobtainium-shield.png");

    private static List<Shield> _Shields = new List<Shield>();

    public int Armor { get { return _armor; } }
    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    // Constructor for Shield; identifies the armor and name of the shield
    public Shield(int shield, int type, int modifier) : base(shield, type, modifier)
    {
        // Initialise fields
        _typeIndex = type;

        // Create shield defense from fields
        _armor = Convert.ToInt32(typeArray[_typeIndex, 1]);

        // Create shield name from fields
        _name = String.Concat(typeArray[_typeIndex, 0], " shield");
        
        // Create shield description from fields
        _description = ("A shield. Has " + _armor + " armor.");
    }

    // Equips the shield
    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            // Also check if an item is already in the shield slot
            if (Player._equippedShield != null)
            {
                Player._HeldItems.Add(Player._equippedShield);
            }
            _player.EquipShield((Shield)Player._HeldItems[index]);
        }
        else    // Equipped from the hand
        {
            if (Player._equippedShield != null)
            {
                Player._HeldItems.Add(Player._equippedShield);
            }
            _player.EquipShield(this);
        }
        return -1;   // Always returns -1 since equipping a shield doesn't use a turn
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _shieldImage = bronze_shield;
        switch(_typeIndex)
        {
            case 0:
                _shieldImage = bronze_shield; break;
            case 1:
                _shieldImage = iron_shield; break;
            case 2:
                _shieldImage = steel_shield; break;
            case 3:
                _shieldImage = silver_shield; break;
            case 4:
                _shieldImage = golden_shield; break;
            case 5:
                _shieldImage = jade_shield; break;
            case 6:
                _shieldImage = ruby_shield; break;
            case 7:
                _shieldImage = glyph_shield; break;
            case 8:
                _shieldImage = adamant_shield; break;
            case 9:
                _shieldImage = unobtainium_shield; break;
        }
        _gameWindow.DrawBitmap(_shieldImage, x - (_shieldImage.Width / 2), y - (_shieldImage.Height / 2));
    }
}

// Handles the Spell functions
public class Spell : Item
{
    new private string _name;
    int _damage;
    int _spellIndex;
    int _typeIndex;

    Bitmap magic = new Bitmap("magic", "magic.png");
    Bitmap icy = new Bitmap("icy", "icy.png");
    Bitmap flame = new Bitmap("flame", "flame.png");
    Bitmap ethereal = new Bitmap("ethereal", "ethereal.png");

    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }
    public int Damage { get { return _damage; } } 

    private string[,] spellArray = new string[,] {{"magic ", "2"}, {"icy ", "3"}, {"flame ", "4"}, {"ethereal ", "5"}};
    private string[,] typeArray = new string[,] {{"stream", "5"}, {"shock", "6"}, {"ball", "7"}, {"bolt", "8"}, {"blast", "9"}, {"explosion", "10"}};

    public Spell(int spell, int type, int modifier) : base(spell, type, modifier)
    {
        // Initialise fields
        _spellIndex = spell;
        _typeIndex = type;

        // Create spell damage from fields
        int _spellInt = Convert.ToInt32(spellArray[_spellIndex, 1]);
        int _typeInt = Convert.ToInt32(typeArray[_typeIndex, 1]);
        _damage = _spellInt * _typeInt;

        // Create spell name from fields
        string _spellString = spellArray[_spellIndex, 0];
        string _typeString = typeArray[_typeIndex, 0];
        _name = String.Concat(_spellString, _typeString);

        // Create spell description
        _description = ("A " + _spellString + _typeString + " with power " + _damage + ".");
    }

    public override int Use(Player _player, int index, bool doFight)
    {
        if (doFight)    // Only run if the player is in combat
        {
            return _damage;
        }
        else
        {
            return -1;
        }
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        Bitmap _spellImage = magic;
        switch(_spellIndex)
        {
            case 0:
                _spellImage = magic; break;
            case 1:
                _spellImage = icy; break;
            case 2:
                _spellImage = flame; break;
            case 3:
                _spellImage = ethereal; break;
        }
        _gameWindow.DrawBitmap(_spellImage, x - (_spellImage.Width / 2), y - (_spellImage.Height / 2));
    }
}

// Used for restoring Health and Magic
public class Potion : Item
{
    new private string _name;
    int _type;
    int _amount;
    Bitmap healthPotion = new Bitmap("health potion", "health-potion.png");
    Bitmap magicPotion = new Bitmap("magic potion", "magic-potion.png");

    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    public Potion(int name, int type, int modifier) : base(name, type, modifier)
    {
        _type = type;
        _amount = modifier;

        // Create a name and description for the potion
        switch(_type)
        {
            case 0:
                _name = "health potion";
                _description = ("Restores " + _amount + "% total health.");
                break;
            case 1:
                _name = "magic potion";
                _description = ("Restores " + _amount + "% total magic.");
                break;
        }
    }

    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != -1)
        {
            if (_type == 0) // Health
            {
                _player._health += (_player.MaxHealth * _amount) / 100;
                Player._HeldItems.RemoveAt(index);
            }
            if (_type == 1) // Magic
            {
                _player._magic += (_player.MaxMagic * _amount) / 100;
                Player._HeldItems.RemoveAt(index);
            }
        }
        else    // Use from the cursor
        {
            if (_type == 0) // Health
            {
                _player._health += (_player.MaxHealth * _amount) / 100;
            }
            if (_type == 1) // Magic
            {
                _player._magic += (_player.MaxMagic * _amount) / 100;
            }
        }
        return -1;
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        switch(_type)
        {
            case 0:
                _gameWindow.DrawBitmap(healthPotion, x - (healthPotion.Width / 2), y - (healthPotion.Height / 2));
                break;
            case 1:
                _gameWindow.DrawBitmap(magicPotion, x - (magicPotion.Width / 2), y - (magicPotion.Height / 2));
                break;
        }
    }
}

// Used for leveling up stats
public class Spirit : Item
{
    new private string _name;
    Bitmap spirit = new Bitmap("spirit", "spirit.png");

    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    public Spirit(int name, int type, int amount) : base(name, type, amount)
    {
        _name = "spirit";
        _description = "Permanently boosts your stats.";
    }

    public override int Use(Player _player, int index, bool doFight)
    {
        if (index != 1)
        {
            Player._HeldItems.RemoveAt(index);
        }
        else    // Use from the cursor
        {
            
        }
        _player.LevelUp(100, 100, 150, 150, 150);
        return -1;
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        _gameWindow.DrawBitmap(spirit, x - (spirit.Width / 2), y - (spirit.Height / 2));
    }
}

// Single-use throwing item
public class Throwable : Item
{
    new private string _name;

    public override string Name { get { return _name; } }
    public override string Description { get { return _description; } }
    public override int Price { get { return _price; } }

    public Throwable(int name, int type, int modifier) : base(name, type, modifier)
    {
        _name = "throwable";
        _description = "Can be thrown at monsters.";
    }
    
    public override int Use(Player _player, int index, bool doFight)
    {
        return -1;
    }

    public override void Draw(Window _gameWindow, int X, int Y)
    {
        x = X;
        y = Y;
        //_gameWindow.DrawBitmap(healthPotion, x - (healthPotion.Width / 2), y - (healthPotion.Height / 2));
    }
}