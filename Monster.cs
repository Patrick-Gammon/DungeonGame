using System;
using SplashKitSDK;
using System.Collections.Generic;

public class Monster
{
    // Initialise enemy stats
    private int _maxHealth;
    public int _health;
    private int _combat;
    private int _spell;
    private int _protect;
    private int _type;
    private int _coins;
    private Weapon _equippedWeapon;
    private Helmet _equippedHelmet;
    private Chestplate _equippedChestplate;
    private Platelegs _equippedPlatelegs;
    private Shield _equippedShield;

    public int MaxHealth { get { return _maxHealth; } }
    public int Health { get { return _health; } } 
    public int Combat { get { return _combat; } }
    public int Spell { get { return _spell; } }
    public int Protect { get { return _protect; } }
    public int Coins { get { return _coins; } }
    public Weapon EquippedWeapon { get { return _equippedWeapon; } }
    public Helmet EquippedHelmet { get { return _equippedHelmet; } }
    public Chestplate EquippedChestplate { get { return _equippedChestplate; } }
    public Platelegs EquippedPlatelegs { get { return _equippedPlatelegs; } }
    public Shield EquippedShield { get { return _equippedShield; } }

    Bitmap slime = new Bitmap("slime", "slime.png");
    Bitmap slimeAlt = new Bitmap("slimeAlt", "slimeAlt.png");
    Bitmap slimeRed = new Bitmap("slimeRed", "slimeRed.png");
    Bitmap slimeRedAlt = new Bitmap("slimeRedAlt", "slimeRedAlt.png");
    Bitmap slimeBlue = new Bitmap("slimeBlue", "slimeBlue.png");
    Bitmap slimeBlueAlt = new Bitmap("slimeBlueAlt", "slimeBlueAlt.png");

    public Monster(int maxHealth, int combat, int spell, int protect, int type)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
        _combat = combat;
        _spell = spell;
        _protect = protect;
        _type = type;
        _equippedWeapon = null;
        _equippedHelmet = null;
        _equippedChestplate = null;
        _equippedPlatelegs = null;
        _equippedShield = null;
        _coins = (_maxHealth + _combat + _protect) / 10;
    }

    public void Draw(Window _gameWindow, int animCycle)
    {
        switch(_type)
        {
            case 0:
                if (animCycle <= 1000)
                {
                    _gameWindow.DrawBitmap(slime, 325 - (slime.Width / 2), 375 - (slime.Height / 2));
                }
                else
                {
                    _gameWindow.DrawBitmap(slimeAlt, 325 - (slime.Width / 2), 368 - (slime.Height / 2));
                }
                break;
            case 1:
                if (animCycle <= 1000)
                {
                    _gameWindow.DrawBitmap(slimeBlue, 325 - (slimeBlue.Width / 2), 375 - (slimeBlue.Height / 2));
                }
                else
                {
                    _gameWindow.DrawBitmap(slimeBlueAlt, 325 - (slimeBlue.Width / 2), 368 - (slimeBlue.Height / 2));
                }
                break;
            case 2:
                if (animCycle <= 1000)
                {
                    _gameWindow.DrawBitmap(slimeRed, 325 - (slimeRed.Width / 2), 375 - (slimeRed.Height / 2));
                }
                else
                {
                    _gameWindow.DrawBitmap(slimeRedAlt, 325 - (slimeRed.Width / 2), 368 - (slimeRed.Height / 2));
                }
                break;
        }
    }
}