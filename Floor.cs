using System;
using SplashKitSDK;
using System.Collections.Generic;

public class Floor
{
    DungeonGame _dungeonGame;
    public Room[] Rooms = new Room[400];
    public int[] leftBorder = {1, 21, 41, 61, 81, 101, 121, 141, 161, 181, 201, 221, 241, 261, 281, 301, 321, 341, 361, 381};
    public int[] rightBorder = {20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400};

    bool check;
    public int _playerStart;
    private int _stairStart;
    public int _stairs;
    public int _storeStart = -1;
    public int _store;
    public int[] _otherRooms = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    private int[] _emptyRooms = {0, 0, 0, 0, 0, 0, 0};

    public Floor(Player _player, DungeonGame dungeonGame)
    {
        // Reset other variables
        for (int i=0; i<400;i++)
        {
            Player._roomsExplored[i] = false;
        }
        _dungeonGame = dungeonGame;

        // Choose a random quadrant for the player to start in
        _playerStart = SplashKit.Rnd(1, 17);

        // Choose a random quadrant away from the player for the stairs to generate
        do
        {    
            _stairStart = SplashKit.Rnd(1, 17);
            
            int difference = _playerStart - _stairStart;
            check = true;
            switch(difference)
            {
                case -5:
                case -4:
                case -3:
                case -1:
                case 0:
                case 1:
                case 3:
                case 4:
                case 5:
                    check = false;
                    break;
            }
        } while (check == false);

        int quadrantsLeft = 8;
        if (_dungeonGame._storeCounter == 3)
        {
            // Generate a shop on this floor
            quadrantsLeft -= 1;
            // Choose a random quadrant away from the player for the store to generate
            do
            {    
                _storeStart = SplashKit.Rnd(1, 17);
                check = true;
                if (_storeStart != _stairStart)
                {
                    int difference = _playerStart - _storeStart;
                    switch(difference)
                    {
                        case -5:
                        case -4:
                        case -3:
                        case -1:
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                        case 5:
                            check = false;
                            break;
                    }
                }
            } while (check == false);
        }
        // Choose 8 more quadrants
        
        int newRoom;
        do
        {
            quadrantsLeft -= 1;
            newRoom = SplashKit.Rnd(1, 17);
            if (newRoom == _playerStart)
            {
                quadrantsLeft += 1;
            }
            else if (newRoom == _stairStart)
            {
                quadrantsLeft += 1;
            }
            else if (newRoom == _storeStart)
            {
                quadrantsLeft += 1;
            }
            else
            {
                bool equals = false;
                foreach(int i in _otherRooms)
                {
                    if (newRoom == i)
                    {
                        equals = true;
                    }
                }
                if (equals)
                {
                    quadrantsLeft += 1;
                }
                else
                {
                    _otherRooms[quadrantsLeft] = newRoom;
                }
            }
        } while (quadrantsLeft > 0);

        // Check which quadrants are empty
        quadrantsLeft = 0;
        Comparison<int> compare;
        compare = (value1, value2) => value1.CompareTo(_otherRooms[value2]);
        for (int i=0;i<17;i++)
        {
            bool check = true;
            if (i == _playerStart || i == _stairStart || i == _storeStart)
            {
                check = false;
            }
            else
            {
                for (int j=1;j<10;j++)
                {
                    if (compare(i, j) == 0)
                    {
                        check = false;
                    }
                }
            }

            if (check)
            {
                if (quadrantsLeft <= 6)
                {
                    _emptyRooms[quadrantsLeft] = i;
                    quadrantsLeft += 1;
                }
            }
        }

        // Generate the + shape in each quadrant
        int cyclesLeft = 18;
        do
        {
            // Set generate to the quadrant to be created
            int generate;
            int rando;
            bool[] moreRooms = {false, false, false, false};
            if (cyclesLeft > 11)
            {
                generate = _emptyRooms[cyclesLeft - 12];
            }
            else
            {
                if (cyclesLeft == 11)
                {
                    generate = _playerStart;
                    // Add a random amount of rooms to the generation of this quadrant
                    // Choose where the rooms are in the quadrant
                    for (int i=0; i<4; i++)
                    {
                        rando = SplashKit.Rnd(1, 4);
                        if (rando != 3)
                        {
                            moreRooms[i] = true;
                        }
                    } 
                }
                else if (cyclesLeft == 10)
                {
                    generate = _stairStart;
                    for (int i=0; i<4; i++)
                    {
                        moreRooms[i] = true;
                    } 
                }
                else if (cyclesLeft == 9 && _storeStart != -1)
                {
                    generate = _storeStart;
                    for (int i=0; i<4; i++)
                    {
                        moreRooms[i] = true;
                    } 
                }
                else
                {
                    generate = _otherRooms[cyclesLeft];
                    // Add a random amount of rooms to the generation of this quadrant
                    // Choose where the rooms are in the quadrant
                    for (int i=0; i<4; i++)
                    {
                        rando = SplashKit.Rnd(1, 4);
                        if (rando != 3)
                        {
                            moreRooms[i] = true;
                        }
                    }  
                }   
            }
            

            // Create the rooms
            switch(generate)
            {
                // Row 1:
                case 1:
                    if (moreRooms[0]) { Rooms[22] = NewRoom(22); }
                    if (cyclesLeft <= 11) { Rooms[23] = NewRoom(23); }
                    if (moreRooms[1]) { Rooms[24] = NewRoom(24); }
                    if (cyclesLeft <= 11) { Rooms[42] = NewRoom(42); }
                    if (cyclesLeft == 10) { Rooms[43] = new Stairs(43, _dungeonGame); _stairs = 43; }
                    else if (cyclesLeft == 9) { Rooms[43] = new Store(43, _dungeonGame); _store = 43; }
                    else { Rooms[43] = NewRoom(43); }
                    Rooms[44] = NewRoom(44);
                    Rooms[45] = NewRoom(45);
                    if (moreRooms[2]) { Rooms[62] = NewRoom(62); }
                    Rooms[63] = NewRoom(63);
                    if (moreRooms[3]) { Rooms[64] = NewRoom(64); }
                    Rooms[83] = NewRoom(83);
                    break;
                case 2:
                    if (moreRooms[0]) { Rooms[27] = NewRoom(27); }
                    if (cyclesLeft <= 11) { Rooms[28] = NewRoom(28); }
                    if (moreRooms[1]) { Rooms[29] = NewRoom(29); }
                    Rooms[46] = NewRoom(46);
                    Rooms[47] = NewRoom(47);
                    if (cyclesLeft == 10) { Rooms[48] = new Stairs(48, _dungeonGame); _stairs = 48; }
                    else if (cyclesLeft == 9) { Rooms[48] = new Store(48, _dungeonGame); _store = 48; }
                    else { Rooms[48] = NewRoom(48); }
                    Rooms[49] = NewRoom(49);
                    Rooms[50] = NewRoom(50);
                    if (moreRooms[2]) { Rooms[67] = NewRoom(67); }
                    Rooms[68] = NewRoom(68);
                    if (moreRooms[3]) { Rooms[69] = NewRoom(69); }
                    Rooms[88] = NewRoom(88);
                    break;
                case 3:
                    if (moreRooms[0]) { Rooms[32] = NewRoom(32); }
                    if (cyclesLeft <= 11) { Rooms[33] = NewRoom(33); }
                    if (moreRooms[1]) { Rooms[34] = NewRoom(34); }
                    Rooms[51] = NewRoom(51);
                    Rooms[52] = NewRoom(52);
                    if (cyclesLeft == 10) { Rooms[53] = new Stairs(53, _dungeonGame); _stairs = 53; }
                    else if (cyclesLeft == 9) { Rooms[53] = new Store(53, _dungeonGame); _store = 53; }
                    else { Rooms[53] = NewRoom(53); }
                    Rooms[54] = NewRoom(54);
                    Rooms[55] = NewRoom(55);
                    if (moreRooms[2]) { Rooms[72] = NewRoom(72); }
                    Rooms[73] = NewRoom(73);
                    if (moreRooms[3]) { Rooms[74] = NewRoom(74); }
                    Rooms[93] = NewRoom(93);
                    break;
                case 4:
                    if (moreRooms[0]) { Rooms[37] = NewRoom(37); }
                    if (cyclesLeft <= 11) { Rooms[38] = NewRoom(38); }
                    if (moreRooms[1]) { Rooms[39] = NewRoom(39); }
                    Rooms[56] = NewRoom(56);
                    Rooms[57] = NewRoom(57);
                    if (cyclesLeft == 10) { Rooms[58] = new Stairs(58, _dungeonGame); _stairs = 58; }
                    else if (cyclesLeft == 9) { Rooms[58] = new Store(58, _dungeonGame); _store = 58; }
                    else { Rooms[58] = NewRoom(58); }
                    if (cyclesLeft <= 11) { Rooms[59] = NewRoom(59); }
                    if (moreRooms[2]) { Rooms[77] = NewRoom(77); }
                    Rooms[78] = NewRoom(78);
                    if (moreRooms[3]) { Rooms[79] = NewRoom(79); }
                    Rooms[98] = NewRoom(98);
                    break;
                // Row 2:
                case 5:
                    Rooms[103] = NewRoom(103);
                    if (moreRooms[0]) { Rooms[122] = NewRoom(122); }
                    Rooms[123] = NewRoom(123);
                    if (moreRooms[1]) { Rooms[124] = NewRoom(124); }
                    if (cyclesLeft <= 11) { Rooms[142] = NewRoom(142); }
                    if (cyclesLeft == 10) { Rooms[143] = new Stairs(143, _dungeonGame); _stairs = 143; }
                    else if (cyclesLeft == 9) { Rooms[143] = new Store(143, _dungeonGame); _store = 143; }
                    else { Rooms[143] = NewRoom(143); }
                    Rooms[144] = NewRoom(144);
                    Rooms[145] = NewRoom(145);
                    if (moreRooms[2]) { Rooms[162] = NewRoom(162); }
                    Rooms[163] = NewRoom(163);
                    if (moreRooms[3]) { Rooms[164] = NewRoom(164); }
                    Rooms[183] = NewRoom(183);
                    break;
                case 6:
                    Rooms[108] = NewRoom(108);
                    if (moreRooms[0]) { Rooms[127] = NewRoom(127); }
                    Rooms[128] = NewRoom(128);
                    if (moreRooms[1]) { Rooms[129] = NewRoom(129); }
                    Rooms[146] = NewRoom(146);
                    Rooms[147] = NewRoom(147);
                    if (cyclesLeft == 10) { Rooms[148] = new Stairs(148, _dungeonGame); _stairs = 148; }
                    else if (cyclesLeft == 9) { Rooms[148] = new Store(148, _dungeonGame); _store = 148; }
                    else { Rooms[148] = NewRoom(148); }
                    Rooms[149] = NewRoom(149);
                    Rooms[150] = NewRoom(150);
                    if (moreRooms[2]) { Rooms[167] = NewRoom(167); }
                    Rooms[168] = NewRoom(168);
                    if (moreRooms[3]) { Rooms[169] = NewRoom(169); }
                    Rooms[188] = NewRoom(188);
                    break;
                case 7:
                    Rooms[113] = NewRoom(113);
                    if (moreRooms[0]) { Rooms[132] = NewRoom(132); }
                    Rooms[133] = NewRoom(133);
                    if (moreRooms[1]) { Rooms[134] = NewRoom(134); }
                    Rooms[151] = NewRoom(151);
                    Rooms[152] = NewRoom(152);
                    if (cyclesLeft == 10) { Rooms[153] = new Stairs(153, _dungeonGame); _stairs = 153; }
                    else if (cyclesLeft == 9) { Rooms[153] = new Store(153, _dungeonGame); _store = 153; }
                    else { Rooms[153] = NewRoom(153); }
                    Rooms[154] = NewRoom(154);
                    Rooms[155] = NewRoom(155);
                    if (moreRooms[2]) { Rooms[172] = NewRoom(172); }
                    Rooms[173] = NewRoom(173);
                    if (moreRooms[3]) { Rooms[174] = NewRoom(174); }
                    Rooms[193] = NewRoom(193);
                    break;
                case 8:
                    Rooms[118] = NewRoom(118);
                    if (moreRooms[0]) { Rooms[137] = NewRoom(137); }
                    Rooms[138] = NewRoom(138);
                    if (moreRooms[1]) { Rooms[139] = NewRoom(139); }
                    Rooms[156] = NewRoom(156);
                    Rooms[157] = NewRoom(157);
                    if (cyclesLeft == 10) { Rooms[158] = new Stairs(158, _dungeonGame); _stairs = 158; }
                    else if (cyclesLeft == 9) { Rooms[158] = new Store(158, _dungeonGame); _store = 158; }
                    else { Rooms[158] = NewRoom(158); }
                    if (cyclesLeft <= 11) { Rooms[159] = NewRoom(159); }
                    if (moreRooms[2]) { Rooms[177] = NewRoom(177); }
                    Rooms[178] = NewRoom(178);
                    if (moreRooms[3]) { Rooms[179] = NewRoom(179); }
                    Rooms[198] = NewRoom(198);
                    break;
                // Row 3:
                case 9:
                    Rooms[203] = NewRoom(203);
                    if (moreRooms[0]) { Rooms[222] = NewRoom(222); }
                    Rooms[223] = NewRoom(223);
                    if (moreRooms[1]) { Rooms[224] = NewRoom(224); }
                    if (cyclesLeft <= 11) { Rooms[242] = NewRoom(242); }
                    if (cyclesLeft == 10) { Rooms[243] = new Stairs(243, _dungeonGame); _stairs = 243; }
                    else if (cyclesLeft == 9) { Rooms[243] = new Store(243, _dungeonGame); _store = 243; }
                    else { Rooms[243] = NewRoom(243); }
                    Rooms[244] = NewRoom(244);
                    Rooms[245] = NewRoom(245);
                    if (moreRooms[2]) { Rooms[262] = NewRoom(262); }
                    Rooms[263] = NewRoom(263);
                    if (moreRooms[3]) { Rooms[264] = NewRoom(264); }
                    Rooms[283] = NewRoom(283);
                    break;
                case 10:
                    Rooms[208] = NewRoom(208);
                    if (moreRooms[0]) { Rooms[227] = NewRoom(227); }
                    Rooms[228] = NewRoom(228);
                    if (moreRooms[1]) { Rooms[229] = NewRoom(229); }
                    Rooms[246] = NewRoom(246);
                    Rooms[247] = NewRoom(247);
                    if (cyclesLeft == 10) { Rooms[248] = new Stairs(248, _dungeonGame); _stairs = 248; }
                    else if (cyclesLeft == 9) { Rooms[248] = new Store(248, _dungeonGame); _store = 248; }
                    else { Rooms[248] = NewRoom(248); }
                    Rooms[249] = NewRoom(249);
                    Rooms[250] = NewRoom(250);
                    if (moreRooms[2]) { Rooms[267] = NewRoom(267); }
                    Rooms[268] = NewRoom(268);
                    if (moreRooms[3]) { Rooms[269] = NewRoom(269); }
                    Rooms[288] = NewRoom(288);
                    break;
                case 11:
                    Rooms[213] = NewRoom(213);
                    if (moreRooms[0]) { Rooms[232] = NewRoom(232); }
                    Rooms[233] = NewRoom(233);
                    if (moreRooms[1]) { Rooms[234] = NewRoom(234); }
                    Rooms[251] = NewRoom(251);
                    Rooms[252] = NewRoom(252);
                    if (cyclesLeft == 10) { Rooms[253] = new Stairs(253, _dungeonGame); _stairs = 253; }
                    else if (cyclesLeft == 9) { Rooms[253] = new Store(253, _dungeonGame); _store = 253; }
                    else { Rooms[253] = NewRoom(253); }
                    Rooms[254] = NewRoom(254);
                    Rooms[255] = NewRoom(255);
                    if (moreRooms[2]) { Rooms[272] = NewRoom(272); }
                    Rooms[273] = NewRoom(273);
                    if (moreRooms[3]) { Rooms[274] = NewRoom(274); }
                    Rooms[293] = NewRoom(293);
                    break;
                case 12:
                    Rooms[218] = NewRoom(218);
                    if (moreRooms[0]) { Rooms[237] = NewRoom(237); }
                    Rooms[238] = NewRoom(238);
                    if (moreRooms[1]) { Rooms[239] = NewRoom(239); }
                    Rooms[256] = NewRoom(256);
                    Rooms[257] = NewRoom(257);
                    if (cyclesLeft == 10) { Rooms[258] = new Stairs(258, _dungeonGame); _stairs = 258; }
                    else if (cyclesLeft == 9) { Rooms[258] = new Store(258, _dungeonGame); _store = 258; }
                    else { Rooms[258] = NewRoom(258); }
                    if (cyclesLeft <= 11) { Rooms[259] = NewRoom(259); }
                    if (moreRooms[2]) { Rooms[277] = NewRoom(277); }
                    Rooms[278] = NewRoom(278); 
                    if (moreRooms[3]) { Rooms[279] = NewRoom(279); }
                    Rooms[298] = NewRoom(298);
                    break;
                // Row 4:
                case 13:
                    Rooms[303] = NewRoom(303);
                    if (moreRooms[0]) { Rooms[322] = NewRoom(322); }
                    Rooms[323] = NewRoom(323);
                    if (moreRooms[1]) { Rooms[324] = NewRoom(324); }
                    if (cyclesLeft <= 11) { Rooms[342] = NewRoom(342); }
                    if (cyclesLeft == 10) { Rooms[343] = new Stairs(343, _dungeonGame); _stairs = 343; }
                    else if (cyclesLeft == 9) { Rooms[343] = new Store(343, _dungeonGame); _store = 343; }
                    else { Rooms[343] = NewRoom(343); }
                    Rooms[344] = NewRoom(344);
                    Rooms[345] = NewRoom(345);
                    if (moreRooms[2]) { Rooms[362] = NewRoom(362); }
                    if (cyclesLeft <= 11) { Rooms[363] = NewRoom(363); }
                    if (moreRooms[3]) { Rooms[364] = NewRoom(364); }
                    break;
                case 14:
                    Rooms[308] = NewRoom(308);
                    if (moreRooms[0]) { Rooms[327] = NewRoom(327); }
                    Rooms[328] = NewRoom(328);
                    if (moreRooms[1]) { Rooms[329] = NewRoom(329); }
                    Rooms[346] = NewRoom(346);
                    Rooms[347] = NewRoom(347);
                    if (cyclesLeft == 10) { Rooms[348] = new Stairs(348, _dungeonGame); _stairs = 348; }
                    else if (cyclesLeft == 9) { Rooms[348] = new Store(348, _dungeonGame); _store = 348; }
                    else { Rooms[348] = NewRoom(348); }
                    Rooms[349] = NewRoom(349);
                    Rooms[350] = NewRoom(350);
                    if (moreRooms[2]) { Rooms[367] = NewRoom(367); }
                    if (cyclesLeft <= 11) { Rooms[368] = NewRoom(368); }
                    if (moreRooms[3]) { Rooms[369] = NewRoom(369); }
                    break;
                case 15:
                    Rooms[313] = NewRoom(313);
                    if (moreRooms[0]) { Rooms[332] = NewRoom(332); }
                    Rooms[333] = NewRoom(333);
                    if (moreRooms[1]) { Rooms[334] = NewRoom(334); }
                    Rooms[351] = NewRoom(351);
                    Rooms[352] = NewRoom(352);
                    if (cyclesLeft == 10) { Rooms[353] = new Stairs(353, _dungeonGame); _stairs = 353; }
                    else if (cyclesLeft == 9) { Rooms[353] = new Store(353, _dungeonGame); _store = 353; }
                    else { Rooms[353] = NewRoom(353); }
                    Rooms[354] = NewRoom(354);
                    Rooms[355] = NewRoom(355);
                    if (moreRooms[2]) { Rooms[372] = NewRoom(372); }
                    if (cyclesLeft <= 11) { Rooms[373] = NewRoom(373); }
                    if (moreRooms[3]) { Rooms[374] = NewRoom(374); }
                    break;
                case 16:
                    Rooms[318] = NewRoom(318);
                    if (moreRooms[0]) { Rooms[337] = NewRoom(337); }
                    Rooms[338] = NewRoom(338);
                    if (moreRooms[1]) { Rooms[339] = NewRoom(339); }
                    Rooms[356] = NewRoom(356);
                    Rooms[357] = NewRoom(357);
                    if (cyclesLeft == 10) { Rooms[358] = new Stairs(358, _dungeonGame); _stairs = 358; }
                    else if (cyclesLeft == 9) { Rooms[358] = new Store(358, _dungeonGame); _store = 358; }
                    else { Rooms[358] = NewRoom(358); }
                    if (cyclesLeft <= 11) { Rooms[359] = NewRoom(359); }
                    if (moreRooms[2]) { Rooms[377] = NewRoom(377); }
                    if (cyclesLeft <= 11) { Rooms[378] = NewRoom(378); }
                    if (moreRooms[3]) { Rooms[379] = NewRoom(379); }
                    break;
            }
            cyclesLeft -= 1;
        } while (cyclesLeft > 0);

        switch(_playerStart)
        {
            case 1:
                _player._location = 43;
                break;
            case 2:
                _player._location = 48;
                break;
            case 3:
                _player._location = 53;
                break;
            case 4:
                _player._location = 58;
                break;
            case 5:
                _player._location = 143;
                break;
            case 6:
                _player._location = 148;
                break;
            case 7:
                _player._location = 153;
                break;
            case 8:
                _player._location = 158;
                break;
            case 9:
                _player._location = 243;
                break;
            case 10:
                _player._location = 248;
                break;
            case 11:
                _player._location = 253;
                break;
            case 12:
                _player._location = 258;
                break;
            case 13:
                _player._location = 343;
                break;
            case 14:
                _player._location = 348;
                break;
            case 15:
                _player._location = 353;
                break;
            case 16:
                _player._location = 358;
                break;
        }
    }

    // Fills the floor with a specified amount of resources
    public void Populate()
    {
        int toAdd;
        // Add 2-4 monsters
        toAdd = SplashKit.Rnd(2, 5);
        // Add the monsters to random rooms
        while (toAdd > 0)
        {
            int roomAdd = SplashKit.Rnd(1, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddMonster();
                    toAdd -= 1;    
                }
            }
        }

        // Roll for 50% chance to add a weapon
        toAdd = SplashKit.Rnd(0, 2);
        // If 1 is rolled
        while (toAdd == 1)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddWeapon();
                    toAdd = 0;    
                }
                
            }
        }

        // Roll for 20% chance to add a helmet
        toAdd = SplashKit.Rnd(0, 5);
        // If 1 is rolled
        while (toAdd == 1)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if(roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddHelmet();
                    toAdd = 0;    
                }
                
            }
        }

        // Roll for 20% chance to add a chestplate
        toAdd = SplashKit.Rnd(0, 5);
        // If 1 is rolled
        while (toAdd == 1)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddChestplate();
                    toAdd = 0;    
                }
                
            }
        }

        // Roll for 20% chance to add a platelegs
        toAdd = SplashKit.Rnd(0, 5);
        // If 1 is rolled
        while (toAdd == 1)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddPlatelegs();
                    toAdd = 0;    
                }
                
            }
        }

        // Roll for 20% chance to add a shield
        toAdd = SplashKit.Rnd(0, 5);
        // If 1 is rolled
        while (toAdd == 1)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddShield();
                    toAdd = 0;
                }
                
            }
        }

        // Roll for 80% chance to add a potion
        toAdd = SplashKit.Rnd(0, 5);
        // If 0 is not rolled
        while (toAdd != 0)
        {
            int roomAdd = SplashKit.Rnd(0, 400);
            if (Rooms[roomAdd] != null)
            {
                if (roomAdd != _store && roomAdd != _stairs)
                {
                    Rooms[roomAdd].AddPotion();
                    toAdd = 0;
                }
            }
        }

        if (_store != 0)
        {
            // Add a healing potion, weapon, piece of armor and two random items to the shop
            Rooms[_store].AddHealthPotion();
            Rooms[_store].AddWeapon();
            int randomArmor = SplashKit.Rnd(0, 4);
            switch (randomArmor)
            {
                case 0: // Helmet
                    Rooms[_store].AddHelmet();
                    break;
                case 1: // Chestplate
                    Rooms[_store].AddChestplate();
                    break;
                case 2: // Platelegs
                    Rooms[_store].AddPlatelegs();
                    break;
                case 3: // Shield
                    Rooms[_store].AddShield();
                    break;
            }
            Rooms[_store].AddRandom();
            Rooms[_store].AddRandom();
            // Sort the items
            foreach (Item i in Rooms[_store]._RoomItems)
            {
                switch(Rooms[_store]._RoomItems.IndexOf(i))
                {
                    case 0:
                        i.x = 150;
                        i.y = 105;
                        break;
                    case 1:
                        i.x = 319;
                        i.y = 105;
                        break;
                    case 2:
                        i.x = 488;
                        i.y = 105;
                        break;
                    case 3:
                        i.x = 150;
                        i.y = 210;
                        break;
                    case 4:
                        i.x = 319;
                        i.y = 210;
                        break;
                }
            }    
        }
        
    }

    // Draws the floor, i.e. each room
    public void Draw(Window _gameWindow)
    {
        // Parse each room
        foreach (Room r in Rooms)
        {
            if (r != null)
            {
                if (Player._roomsExplored[r._position] == true)
                {
                        
                    // Check what type of room it is
                    // This is found by checking the bounding rooms around it, and the borders
                    string bounds = "";
                    if (r._position < 20) // North
                    {
                        bounds += "n";
                    }
                    else if (Rooms[r._position - 20] != null) // North
                    {
                        bounds += "n";
                    }
                            
                    if (r._position < 1) // North
                    {
                        bounds += "n";
                    }
                    else if (Rooms[r._position - 1] != null || Array.Exists(leftBorder, number => number == r._position)) // West
                    {
                        bounds += "w";
                    }

                    if (r._position > 399) // East
                    {
                        bounds += "e";
                    }
                    else if (Rooms[r._position + 1] != null || Array.Exists(rightBorder, number => number == r._position)) // East
                    {
                        bounds += "e";
                    }

                    if (r._position > 380) // South
                    {
                        bounds += "s";
                    }
                    else if (Rooms[r._position + 20] != null) // South
                    {
                        bounds += "s";
                    }

                    // Draw the room
                    r.Draw(_gameWindow, bounds);
                }
            }
            
        }
    }

    // Gets a new room
    private Room NewRoom(int position)
    {
        Generic _room = new Generic(position, _dungeonGame);
        return _room;
    }
}