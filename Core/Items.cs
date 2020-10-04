﻿using System;
using System.Collections.Generic;
using System.Linq;
using static CramIt.Core.Type;

namespace CramIt.Core
{
    public static class Items
    {
        public static IReadOnlyList<Item>               ItemList    { get; }
        public static IReadOnlyDictionary<string, Item> ItemsByName { get; }

        public static IEnumerable<Item> InputItems()
            => ItemList.Where(item => item.CanBeInput);

        public static IEnumerable<Item> InputItems(InputItemOptions options)
        {
            var inputItems = InputItems();

            if ( ! options.IncludeIrreplaceableItems)
            {
                inputItems = inputItems.Where(item => ! item.IsIrreplaceable);
            }
            if (options.CombineGroupsOfSimilarItems)
            {
                inputItems = inputItems.Where(item => item.IsGroupRepresentative);
            }

            return inputItems;
        }

        public static IEnumerable<InputItemWithViabilityCategoryInformation> InputItems(
            InputItemOptions options, StandardRecipeGroupItemFilterer standardRecipeGroupItemFilterer)
            => InputItems(options).Select(item => new InputItemWithViabilityCategoryInformation(item, standardRecipeGroupItemFilterer.BestViabilityCategory(item)));

        static Items()
        {
            ItemList = new List<Item>
            {
                Item.Input("Black Apricorn",    0,  1, Dark,      0, false, true ),
                Item.Input("Yellow Apricorn",   0,  2, Electric,  0, false, true ),
                Item.Input("Pink Apricorn",     0,  3, Fairy,     0, false, true ),
                Item.Input("Red Apricorn",      0,  4, Fire,      0, false, true ),
                Item.Input("Green Apricorn",    0,  5, Grass,     0, false, true ),
                Item.Input("White Apricorn",    0,  6, Normal,    0, false, true ),
                Item.Input("Blue Apricorn",     0,  7, Water,     0, false, true ),
                Item.Input("Honey",             0,  8, Bug,       1, false, true ),
                Item.Input("Pecha Berry",       0,  9, Electric,  1, false, true ),
                Item.Input("Pixie Plate",       0, 10, Fairy,     1, false, true ),
                Item.Input("Leppa Berry",       0, 11, Fighting,  1, false, true ),
                Item.Input("Cheri Berry",       0, 12, Fire,      1, false, true ),
                Item.Input("Lum Berry",         0, 13, Flying,    1, false, true ),
                Item.Input("Odd Incense",       0, 14, Ghost,     1, false, true ),
                Item.Input("Rawst Berry",       0, 15, Grass,     1, false, true ),
                Item.Input("Rose Incense",      0, 16, Grass,     1, false, true ),
                Item.Input("Tiny Mushroom",     0, 17, Grass,     1, false, true ),
                Item.Input("Persim Berry",      0, 18, Ground,    1, false, true ),
                Item.Input("Aspear Berry",      0, 19, Ice,       1, false, true ),
                Item.Input("Full Incense",      1,  0, Normal,    1, false, true ),
                Item.Input("Luck Incense",      1,  1, Normal,    1, false, true ),
                Item.Input("Max Repel",         1,  2, Poison,    1, false, true ),
                Item.Input("Oran Berry",        1,  3, Poison,    1, false, true ),
                Item.Input("Repel",             1,  4, Poison,    1, false, true ),
                Item.Input("Super Repel",       1,  5, Poison,    1, false, true ),
                Item.Input("Exp. Candy XS",     1,  6, Psychic,   1, false, true ),
                Item.Input("Lax Incense",       1,  7, Psychic,   1, false, true ),
                Item.Input("Pure Incense",      1,  8, Psychic,   1, false, true ),
                Item.Input("Sitrus Berry",      1,  9, Psychic,   1, false, true ),
                Item.Input("Rock Incense",      1, 10, Rock,      1, false, true ),
                Item.Input("Rusted Shield",     1, 11, Steel,     1, true,  true ),
                Item.Input("Rusted Sword",      1, 12, Steel,     1, true,  true ),
                Item.Input("Chesto Berry",      1, 13, Water,     1, false, true ),
                Item.Input("Sea Incense",       1, 14, Water,     1, false, true ),
                Item.Input("Wave Incense",      1, 15, Water,     1, false, true ),
                Item.Input("Tanga Berry",       1, 16, Bug,       2, false, true ),
                Item.Input("Colbur Berry",      1, 17, Dark,      2, false, true ),
                Item.Input("Iapapa Berry",      1, 18, Dark,      2, false, true ),
                Item.Input("Aguav Berry",       1, 19, Dragon,    2, false, true ),
                Item.Input("Haban Berry",       2,  0, Dragon,    2, false, true ),
                Item.Input("Wacan Berry",       2,  1, Electric,  2, false, true ),
                Item.Input("Roseli Berry",      2,  2, Fairy,     2, false, true ),
                Item.Input("Calcium",           2,  3, Fighting,  2, false, true ),
                Item.Input("Carbos",            2,  4, Fighting,  2, false, true ),
                Item.Input("Chople Berry",      2,  5, Fighting,  2, false, true ),
                Item.Input("HP Up",             2,  6, Fighting,  2, false, true ),
                Item.Input("Iron",              2,  7, Fighting,  2, false, true ),
                Item.Input("Protein",           2,  8, Fighting,  2, false, true ),
                Item.Input("Zinc",              2,  9, Fighting,  2, false, true ),
                Item.Input("Figy Berry",        2, 10, Fire,      2, false, true ),
                Item.Input("Occa Berry",        2, 11, Fire,      2, false, true ),
                Item.Input("Coba Berry",        2, 12, Flying,    2, false, true ),
                Item.Input("Kasib Berry",       2, 13, Ghost,     2, false, true ),
                Item.Input("Mago Berry",        2, 14, Ghost,     2, false, true ),
                Item.Input("Galarica Twig",     2, 15, Grass,     2, false, true ),
                Item.Input("Rindo Berry",       2, 16, Grass,     2, false, true ),
                Item.Input("Shuca Berry",       2, 17, Ground,    2, false, true ),
                Item.Input("Yache Berry",       2, 18, Ice,       2, false, true ),
                Item.Input("Chilan Berry",      2, 19, Normal,    2, false, true ),
                Item.Input("Kebia Berry",       3,  0, Poison,    2, false, true ),
                Item.Input("Exp. Candy S",      3,  1, Psychic,   2, false, true ),
                Item.Input("Payapa Berry",      3,  2, Psychic,   2, false, true ),
                Item.Input("Charti Berry",      3,  3, Rock,      2, false, true ),
                Item.Input("Wiki Berry",        3,  4, Rock,      2, false, true ),
                Item.Input("Babiri Berry",      3,  5, Steel,     2, false, true ),
                Item.Input("Passho Berry",      3,  6, Water,     2, false, true ),
                Item.Input("Maranga Berry",     3,  7, Dark,      3, false, true ),
                Item.Input("Rowap Berry",       3,  8, Dark,      3, false, true ),
                Item.Input("Dynamax Candy",     3,  9, Dragon,    3, false, true ),
                Item.Input("Jaboca Berry",      3, 10, Dragon,    3, false, true ),
                Item.Input("Thunder Stone",     3, 11, Electric,  3, false, true ),
                Item.Input("Kee Berry",         3, 12, Fairy,     3, false, true ),
                Item.Input("Moon Stone",        3, 13, Fairy,     3, false, true ),
                Item.Input("Shiny Stone",       3, 14, Fairy,     3, false, true ),
                Item.Input("Kelpsy Berry",      3, 15, Fighting,  3, false, true ),
                Item.Input("Fire Stone",        3, 16, Fire,      3, false, true ),
                Item.Input("Sun Stone",         3, 17, Fire,      3, false, true ),
                Item.Input("Clever Feather",    3, 18, Flying,    3, false, false),
                Item.Input("Genius Feather",    3, 19, Flying,    3, false, false),
                Item.Input("Grepa Berry",       4,  0, Flying,    3, false, true ),
                Item.Input("Health Feather",    4,  1, Flying,    3, false, true ),
                Item.Input("Muscle Feather",    4,  2, Flying,    3, false, false),
                Item.Input("Pretty Feather",    4,  3, Flying,    3, false, true ),
                Item.Input("Resist Feather",    4,  4, Flying,    3, false, false),
                Item.Input("Swift Feather",     4,  5, Flying,    3, false, false),
                Item.Input("Dusk Stone",        4,  6, Ghost,     3, false, true ),
                Item.Input("Leaf Stone",        4,  7, Grass,     3, false, true ),
                Item.Input("Hondew Berry",      4,  8, Ground,    3, false, true ),
                Item.Input("Ice Stone",         4,  9, Ice,       3, false, true ),
                Item.Input("Pomeg Berry",       4, 10, Ice,       3, false, true ),
                Item.Input("Qualot Berry",      4, 11, Poison,    3, false, true ),
                Item.Input("Dawn Stone",        4, 12, Psychic,   3, false, true ),
                Item.Input("Exp. Candy M",      4, 13, Psychic,   3, false, true ),
                Item.Input("Tamato Berry",      4, 14, Psychic,   3, false, true ),
                Item.Input("Pearl",             4, 15, Water,     3, false, true ),
                Item.Input("Water Stone",       4, 16, Water,     3, false, true ),
                Item.Input("Binding Band",      4, 17, Dark,      4, false, true ),
                Item.Input("Electric Seed",     4, 18, Electric,  4, false, true ),
                Item.Input("Misty Seed",        4, 19, Fairy,     4, false, true ),
                Item.Input("Absorb Bulb",       5,  0, Grass,     4, false, true ),
                Item.Input("Big Root",          5,  1, Grass,     4, false, true ),
                Item.Input("Grassy Seed",       5,  2, Grass,     4, false, true ),
                Item.Input("Luminous Moss",     5,  3, Grass,     4, false, true ),
                Item.Input("Stardust",          5,  4, Ground,    4, false, true ),
                Item.Input("Snowball",          5,  5, Ice,       4, false, true ),
                Item.Input("Normal Gem",        5,  6, Normal,    4, false, true ),
                Item.Input("Exp. Candy L",      5,  7, Psychic,   4, false, true ),
                Item.Input("Psychic Seed",      5,  8, Psychic,   4, false, true ),
                Item.Input("Float Stone",       5,  9, Rock,      4, false, true ),
                Item.Input("Enigma Berry",      5, 10, Bug,       5, false, true ),
                Item.Input("Shed Shell",        5, 11, Bug,       5, false, true ),
                Item.Input("Silver Powder",     5, 12, Bug,       5, false, true ),
                Item.Input("Fossilized Drake",  5, 13, Dragon,    5, false, true ),
                Item.Input("Fossilized Bird",   5, 14, Electric,  5, false, true ),
                Item.Input("Lansat Berry",      5, 15, Flying,    5, false, true ),
                Item.Input("Sharp Beak",        5, 16, Flying,    5, false, true ),
                Item.Input("Custap Berry",      5, 17, Ghost,     5, false, true ),
                Item.Input("Power Herb",        5, 18, Grass,     5, false, true ),
                Item.Input("Sticky Barb",       5, 19, Grass,     5, false, true ),
                Item.Input("Fossilized Dino",   6,  0, Ice,       5, false, true ),
                Item.Input("Safety Goggles",    6,  1, Normal,    5, false, true ),
                Item.Input("Black Sludge",      6,  2, Poison,    5, false, true ),
                Item.Input("Exp. Candy XL",     6,  3, Psychic,   5, false, true ),
                Item.Input("Starf Berry",       6,  4, Psychic,   5, false, true ),
                Item.Input("Wise Glasses",      6,  5, Psychic,   5, false, true ),
                Item.Input("Lagging Tail",      6,  6, Rock,      5, false, true ),
                Item.Input("Micle Berry",       6,  7, Rock,      5, false, true ),
                Item.Input("Fossilized Fish",   6,  8, Water,     5, false, true ),
                Item.Input("Mystic Water",      6,  9, Water,     5, false, true ),
                Item.Input("Scope Lens",        6, 10, Dark,      6, false, true ),
                Item.Input("Focus Band",        6, 11, Fighting,  6, false, true ),
                Item.Input("Focus Sash",        6, 12, Fighting,  6, false, true ),
                Item.Input("Protective Pads",   6, 13, Fighting,  6, false, true ),
                Item.Input("Salac Berry",       6, 14, Fighting,  6, false, true ),
                Item.Input("Heat Rock",         6, 15, Fire,      6, false, true ),
                Item.Input("Adrenaline Orb",    6, 16, Ghost,     6, false, true ),
                Item.Input("Liechi Berry",      6, 17, Grass,     6, false, true ),
                Item.Input("Apicot Berry",      6, 18, Ground,    6, false, true ),
                Item.Input("Rare Bone",         6, 19, Ground,    6, false, true ),
                Item.Input("Terrain Extender",  7,  0, Ground,    6, false, true ),
                Item.Input("Ganlon Berry",      7,  1, Ice,       6, false, true ),
                Item.Input("Icy Rock",          7,  2, Ice,       6, false, true ),
                Item.Input("Petaya Berry",      7,  3, Poison,    6, false, true ),
                Item.Input("Oval Stone",        7,  4, Rock,      6, false, true ),
                Item.Input("Smooth Rock",       7,  5, Rock,      6, false, true ),
                Item.Input("Big Pearl",         7,  6, Water,     6, false, true ),
                Item.Input("Damp Rock",         7,  7, Water,     6, false, true ),
                Item.Input("Throat Spray",      7,  8, Water,     6, false, true ),
                Item.Input("Black Glasses",     7,  9, Dark,      7, false, true ),
                Item.Input("Ring Target",       7, 10, Dark,      7, false, true ),
                Item.Input("Cell Battery",      7, 11, Electric,  7, false, true ),
                Item.Input("Bright Powder",     7, 12, Fairy,     7, false, true ),
                Item.Input("Black Belt",        7, 13, Fighting,  7, false, true ),
                Item.Input("Muscle Band",       7, 14, Fighting,  7, false, true ),
                Item.Input("Flame Orb",         7, 15, Fire,      7, false, true ),
                Item.Input("Big Mushroom",      7, 16, Grass,     7, false, true ),
                Item.Input("Mental Herb",       7, 17, Grass,     7, false, true ),
                Item.Input("Miracle Seed",      7, 18, Grass,     7, false, true ),
                Item.Input("White Herb",        7, 19, Grass,     7, false, true ),
                Item.Input("Never-Melt Ice",    8,  0, Ice,       7, false, true ),
                Item.Input("Toxic Orb",         8,  1, Poison,    7, false, true ),
                Item.Input("Hard Stone",        8,  2, Rock,      7, false, true ),
                Item.Input("Metal Powder",      8,  3, Steel,     7, false, true ),
                Item.Input("Soothe Bell",       8,  4, Steel,     7, false, true ),
                Item.Input("Shell Bell",        8,  5, Water,     7, false, true ),
                Item.Input("Red Card",          8,  6, Fire,      8, false, true ),
                Item.Input("Adamant Mint",      8,  7, Grass,     8, false, false),
                Item.Input("Brave Mint",        8,  7, Grass,     8, false, false),
                Item.Input("Lonely Mint",       8,  7, Grass,     8, false, false),
                Item.Input("Naughty Mint",      8,  7, Grass,     8, false, false),
                Item.Input("Bold Mint",         8,  8, Grass,     8, false, false),
                Item.Input("Impish Mint",       8,  8, Grass,     8, false, false),
                Item.Input("Lax Mint",          8,  8, Grass,     8, false, false),
                Item.Input("Relaxed Mint",      8,  8, Grass,     8, false, false),
                Item.Input("Calm Mint",         8,  9, Grass,     8, false, false),
                Item.Input("Careful Mint",      8,  9, Grass,     8, false, false),
                Item.Input("Gentle Mint",       8,  9, Grass,     8, false, false),
                Item.Input("Sassy Mint",        8,  9, Grass,     8, false, false),
                Item.Input("Hasty Mint",        8, 10, Grass,     8, false, false),
                Item.Input("Jolly Mint",        8, 10, Grass,     8, false, false),
                Item.Input("Naive Mint",        8, 10, Grass,     8, false, false),
                Item.Input("Timid Mint",        8, 10, Grass,     8, false, false),
                Item.Input("Mild Mint",         8, 11, Grass,     8, false, false),
                Item.Input("Modest Mint",       8, 11, Grass,     8, false, false),
                Item.Input("Quiet Mint",        8, 11, Grass,     8, false, false),
                Item.Input("Rash Mint",         8, 11, Grass,     8, false, false),
                Item.Input("Serious Mint",      8, 12, Grass,     8, false, true ),
                Item.Input("Heavy-Duty Boots",  8, 13, Ground,    8, false, true ),
                Item.Input("Nugget",            8, 14, Ground,    8, false, true ),
                Item.Input("Eject Button",      8, 15, Steel,     8, false, true ),
                Item.Input("Eject Pack",        8, 16, Steel,     8, false, true ),
                Item.Input("Iron Ball",         8, 17, Steel,     8, false, true ),
                Item.Input("Dragon Fang",       8, 18, Dragon,    9, false, true ),
                Item.Input("Dragon Scale",      8, 19, Dragon,    9, false, true ),
                Item.Input("Leek",              9,  0, Grass,     9, false, true ),
                Item.Input("Thick Club",        9,  1, Ground,    9, false, true ),
                Item.Input("Galarica Cuff",     9,  2, Poison,    9, false, true ),
                Item.Input("Twisted Spoon",     9,  3, Psychic,   9, false, true ),
                Item.Input("Life Orb",          9,  4, Dragon,   10, false, true ),
                Item.Input("Light Ball",        9,  5, Electric, 10, false, true ),
                Item.Input("Magnet",            9,  6, Electric, 10, false, true ),
                Item.Input("Sachet",            9,  7, Fairy,    10, false, true ),
                Item.Input("Choice Band",       9,  8, Fighting, 10, false, true ),
                Item.Input("Power Anklet",      9,  9, Fighting, 10, false, true ),
                Item.Input("Power Band",        9, 10, Fighting, 10, false, true ),
                Item.Input("Power Belt",        9, 11, Fighting, 10, false, true ),
                Item.Input("Power Bracer",      9, 12, Fighting, 10, false, true ),
                Item.Input("Power Weight",      9, 13, Fighting, 10, false, true ),
                Item.Input("Air Balloon",       9, 14, Flying,   10, false, true ),
                Item.Input("Utility Umbrella",  9, 15, Flying,   10, false, true ),
                Item.Input("Leftovers",         9, 16, Grass,    10, false, true ),
                Item.Input("Soft Sand",         9, 17, Ground,   10, false, true ),
                Item.Input("Choice Scarf",      9, 18, Normal,   10, false, true ),
                Item.Input("Lucky Egg",         9, 19, Normal,   10, false, true ),
                Item.Input("Choice Specs",     10,  0, Psychic,  10, false, true ),
                Item.Input("Light Clay",       10,  1, Psychic,  10, false, true ),
                Item.Input("Power Lens",       10,  2, Psychic,  10, false, true ),
                Item.Input("Everstone",        10,  3, Rock,     10, false, true ),
                Item.Input("Star Piece",       10,  4, Rock,     10, false, true ),
                Item.Input("Assault Vest",     10,  5, Steel,    10, false, true ),
                Item.Input("Wide Lens",        10,  6, Dark,     11, false, true ),
                Item.Input("Zoom Lens",        10,  7, Dark,     11, false, true ),
                Item.Input("Silk Scarf",       10,  8, Normal,   11, false, true ),
                Item.Input("Smoke Ball",       10,  9, Poison,   11, false, true ),
                Item.Input("Metronome",        10, 10, Steel,    11, false, true ),
                Item.Input("Blunder Policy",   10, 11, Dark,     12, false, true ),
                Item.Input("Cleanse Tag",      10, 12, Ghost,    12, false, true ),
                Item.Input("Balm Mushroom",    10, 13, Grass,    12, false, true ),
                Item.Input("Room Service",     10, 14, Psychic,  12, false, true ),
                Item.Input("Protector",        10, 15, Rock,     12, false, true ),
                Item.Input("Metal Coat",       10, 16, Steel,    12, false, true ),
                Item.Input("Grip Claw",        10, 17, Bug,      13, false, true ),
                Item.Input("Razor Claw",       10, 18, Dark,     13, false, true ),
                Item.Input("Whipped Dream",    10, 19, Fairy,    13, false, true ),
                Item.Input("Macho Brace",      11,  0, Fighting, 13, false, true ),
                Item.Input("Spell Tag",        11,  1, Ghost,    13, false, true ),
                Item.Input("Quick Claw",       11,  2, Normal,   13, false, true ),
                Item.Input("Rocky Helmet",     11,  3, Rock,     13, false, true ),
                Item.Input("Prism Scale",      11,  4, Water,    13, false, true ),
                Item.Input("Armorite Ore",     11,  5, Fighting, 14, false, true ),
                Item.Input("Quick Powder",     11,  6, Normal,   14, false, true ),
                Item.Input("Wishing Piece",    11,  7, Poison,   14, false, true ),
                Item.Input("Destiny Knot",     11,  8, Psychic,  14, false, true ),
                Item.Input("Weakness Policy",  11,  9, Dark,     15, false, true ),
                Item.Input("Berry Sweet",      11, 10, Fairy,    15, false, true ),
                Item.Input("Clover Sweet",     11, 11, Fairy,    15, false, true ),
                Item.Input("Flower Sweet",     11, 12, Fairy,    15, false, true ),
                Item.Input("Love Sweet",       11, 13, Fairy,    15, false, true ),
                Item.Input("Ribbon Sweet",     11, 14, Fairy,    15, false, true ),
                Item.Input("Star Sweet",       11, 15, Fairy,    15, false, true ),
                Item.Input("Strawberry Sweet", 11, 16, Fairy,    15, false, true ),
                Item.Input("Sweet Apple",      11, 17, Grass,    15, false, true ),
                Item.Input("Tart Apple",       11, 18, Grass,    15, false, true ),
                Item.Input("Cracked Pot",      11, 19, Ground,   15, false, true ),
                Item.Input("Upgrade",          12,  0, Normal,   15, false, true ),
                Item.Input("Pearl String",     12,  1, Water,    15, false, true ),
                Item.Input("Dubious Disc",     12,  2, Dark,     16, false, true ),
                Item.Input("Eviolite",         12,  3, Dragon,   16, false, true ),
                Item.Input("Expert Belt",      12,  4, Fighting, 16, false, true ),
                Item.Input("Charcoal",         12,  5, Fire,     16, false, true ),
                Item.Input("Reaper Cloth",     12,  6, Ghost,    16, false, true ),
                Item.Input("Amulet Coin",      12,  7, Normal,   16, true,  true ),
                Item.Input("Poison Barb",      12,  8, Poison,   16, false, true ),
                Item.Input("King's Rock",      12,  9, Steel,    16, false, true ),
                Item.Input("Comet Shard",      12, 10, Ice,      17, false, true ),
                Item.Input("Chipped Pot",      12, 11, Ground,   18, false, true ),
                Item.Input("Big Nugget",       12, 12, Ground,   19, false, true ),
                Item.Input("PP Up",            12, 13, Normal,   19, false, true ),
                Item.Input("Bug Memory",       12, 14, Bug,      20, true,  true ),
                Item.Input("Dark Memory",      12, 15, Dark,     20, true,  true ),
                Item.Input("Dragon Memory",    12, 16, Dragon,   20, true,  true ),
                Item.Input("Electric Memory",  12, 17, Electric, 20, true,  true ),
                Item.Input("Fairy Memory",     12, 18, Fairy,    20, true,  true ),
                Item.Input("Fighting Memory",  12, 19, Fighting, 20, true,  true ),
                Item.Input("Fire Memory",      13,  0, Fire,     20, true,  true ),
                Item.Input("Flying Memory",    13,  1, Flying,   20, true,  true ),
                Item.Input("Ghost Memory",     13,  2, Ghost,    20, true,  true ),
                Item.Input("Grass Memory",     13,  3, Grass,    20, true,  true ),
                Item.Input("Ground Memory",    13,  4, Ground,   20, true,  true ),
                Item.Input("Ice Memory",       13,  5, Ice,      20, true,  true ),
                Item.Input("Ability Capsule",  13,  6, Normal,   20, false, true ),
                Item.Input("PP Max",           13,  7, Normal,   20, false, true ),
                Item.Input("Poison Memory",    13,  8, Poison,   20, true,  true ),
                Item.Input("Psychic Memory",   13,  9, Psychic,  20, true,  true ),
                Item.Input("Rare Candy",       13, 10, Psychic,  20, false, true ),
                Item.Input("Rock Memory",      13, 11, Rock,     20, true,  true ),
                Item.Input("Bottle Cap",       13, 12, Steel,    20, false, true ),
                Item.Input("Gold Bottle Cap",  13, 13, Steel,    20, false, true ),
                Item.Input("Steel Memory",     13, 14, Steel,    20, true,  true ),
                Item.Input("Water Memory",     13, 15, Water,    20, true,  true ),
                Item.TR(18, "Leech Life",      13, 16, Bug     ),
                Item.TR(28, "Megahorn",        13, 16, Bug     ),
                Item.TR(60, "X-Scissor",       13, 16, Bug     ),
                Item.TR(61, "Bug Buzz",        13, 16, Bug     ),
                Item.TR(96, "Pollen Puff",     13, 16, Bug     ),
                Item.TR(32, "Crunch",          13, 17, Dark    ),
                Item.TR(37, "Taunt",           13, 17, Dark    ),
                Item.TR(58, "Dark Pulse",      13, 17, Dark    ),
                Item.TR(68, "Nasty Plot",      13, 17, Dark    ),
                Item.TR(81, "Foul Play",       13, 17, Dark    ),
                Item.TR(93, "Darkest Lariat",  13, 17, Dark    ),
                Item.TR(95, "Throat Chop",     13, 17, Dark    ),
                Item.TR(24, "Outrage",         13, 18, Dragon  ),
                Item.TR(47, "Dragon Claw",     13, 18, Dragon  ),
                Item.TR(51, "Dragon Dance",    13, 18, Dragon  ),
                Item.TR(62, "Dragon Pulse",    13, 18, Dragon  ),
                Item.TR( 8, "Thunderbolt",     13, 19, Electric),
                Item.TR( 9, "Thunder",         13, 19, Electric),
                Item.TR(80, "Electro Ball",    13, 19, Electric),
                Item.TR(86, "Wild Charge",     13, 19, Electric),
                Item.TR(90, "Play Rough",      14,  0, Fairy   ),
                Item.TR(92, "Dazzling Gleam",  14,  0, Fairy   ),
                Item.TR( 7, "Low Kick",        14,  1, Fighting),
                Item.TR(21, "Reversal",        14,  1, Fighting),
                Item.TR(39, "Superpower",      14,  1, Fighting),
                Item.TR(48, "Bulk Up",         14,  1, Fighting),
                Item.TR(53, "Close Combat",    14,  1, Fighting),
                Item.TR(56, "Aura Sphere",     14,  1, Fighting),
                Item.TR(64, "Focus Blast",     14,  1, Fighting),
                Item.TR(99, "Body Press",      14,  1, Fighting),
                Item.TR( 2, "Flamethrower",    14,  2, Fire    ),
                Item.TR(15, "Fire Blast",      14,  2, Fire    ),
                Item.TR(36, "Heat Wave",       14,  2, Fire    ),
                Item.TR(41, "Blaze Kick",      14,  2, Fire    ),
                Item.TR(43, "Overheat",        14,  2, Fire    ),
                Item.TR(55, "Flare Blitz",     14,  2, Fire    ),
                Item.TR(88, "Heat Crash",      14,  2, Fire    ),
                Item.TR(66, "Brave Bird",      14,  3, Flying  ),
                Item.TR(89, "Hurricane",       14,  3, Flying  ),
                Item.TR(33, "Shadow Ball",     14,  4, Ghost   ),
                Item.TR(50, "Leaf Blade",      14,  5, Grass   ),
                Item.TR(59, "Seed Bomb",       14,  5, Grass   ),
                Item.TR(65, "Energy Ball",     14,  5, Grass   ),
                Item.TR(71, "Leaf Storm",      14,  5, Grass   ),
                Item.TR(72, "Power Whip",      14,  5, Grass   ),
                Item.TR(77, "Grass Knot",      14,  5, Grass   ),
                Item.TR(10, "Earthquake",      14,  6, Ground  ),
                Item.TR(23, "Spikes",          14,  6, Ground  ),
                Item.TR(67, "Earth Power",     14,  6, Ground  ),
                Item.TR(87, "Drill Run",       14,  6, Ground  ),
                Item.TR(94, "High Horsepower", 14,  6, Ground  ),
                Item.TR( 5, "Ice Beam",        14,  7, Ice     ),
                Item.TR( 6, "Blizzard",        14,  7, Ice     ),
                Item.TR( 0, "Swords Dance",    14,  8, Normal  ),
                Item.TR( 1, "Body Slam",       14,  8, Normal  ),
                Item.TR(13, "Focus Energy",    14,  8, Normal  ),
                Item.TR(14, "Metronome",       14,  8, Normal  ),
                Item.TR(19, "Tri Attack",      14,  8, Normal  ),
                Item.TR(20, "Substitute",      14,  8, Normal  ),
                Item.TR(26, "Endure",          14,  8, Normal  ),
                Item.TR(27, "Sleep Talk",      14,  8, Normal  ),
                Item.TR(29, "Baton Pass",      14,  8, Normal  ),
                Item.TR(30, "Encore",          14,  8, Normal  ),
                Item.TR(35, "Uproar",          14,  8, Normal  ),
                Item.TR(42, "Hyper Voice",     14,  8, Normal  ),
                Item.TR(85, "Work Up",         14,  8, Normal  ),
                Item.TR(22, "Sludge Bomb",     14,  9, Poison  ),
                Item.TR(54, "Toxic Spikes",    14,  9, Poison  ),
                Item.TR(57, "Poison Jab",      14,  9, Poison  ),
                Item.TR(73, "Gunk Shot",       14,  9, Poison  ),
                Item.TR(78, "Sludge Wave",     14,  9, Poison  ),
                Item.TR(91, "Venom Drench",    14,  9, Poison  ),
                Item.TR(11, "Psychic",         14, 10, Psychic ),
                Item.TR(12, "Agility",         14, 10, Psychic ),
                Item.TR(17, "Amnesia",         14, 10, Psychic ),
                Item.TR(25, "Psyshock",        14, 10, Psychic ),
                Item.TR(34, "Future Sight",    14, 10, Psychic ),
                Item.TR(38, "Trick",           14, 10, Psychic ),
                Item.TR(40, "Skill Swap",      14, 10, Psychic ),
                Item.TR(44, "Cosmic Power",    14, 10, Psychic ),
                Item.TR(49, "Calm Mind",       14, 10, Psychic ),
                Item.TR(69, "Zen Headbutt",    14, 10, Psychic ),
                Item.TR(82, "Stored Power",    14, 10, Psychic ),
                Item.TR(83, "Ally Switch",     14, 10, Psychic ),
                Item.TR(97, "Psychic Fangs",   14, 10, Psychic ),
                Item.TR(63, "Power Gem",       14, 11, Rock    ),
                Item.TR(75, "Stone Edge",      14, 11, Rock    ),
                Item.TR(76, "Stealth Rock",    14, 11, Rock    ),
                Item.TR(31, "Iron Tail",       14, 12, Steel   ),
                Item.TR(46, "Iron Defense",    14, 12, Steel   ),
                Item.TR(52, "Gyro Ball",       14, 12, Steel   ),
                Item.TR(70, "Flash Cannon",    14, 12, Steel   ),
                Item.TR(74, "Iron Head",       14, 12, Steel   ),
                Item.TR(79, "Heavy Slam",      14, 12, Steel   ),
                Item.TR( 3, "Hydro Pump",      14, 13, Water   ),
                Item.TR( 4, "Surf",            14, 13, Water   ),
                Item.TR(16, "Waterfall",       14, 13, Water   ),
                Item.TR(45, "Muddy Water",     14, 13, Water   ),
                Item.TR(84, "Scald",           14, 13, Water   ),
                Item.TR(98, "Liquidation",     14, 13, Water   ),
                Item.Ball("Dive Ball",      14, 14),
                Item.Ball("Dusk Ball",      14, 15),
                Item.Ball("Fast Ball",      14, 16),
                Item.Ball("Friend Ball",    14, 17),
                Item.Ball("Great Ball",     14, 18),
                Item.Ball("Heal Ball",      14, 19),
                Item.Ball("Heavy Ball",     15,  0),
                Item.Ball("Level Ball",     15,  1),
                Item.Ball("Love Ball",      15,  2),
                Item.Ball("Lure Ball",      15,  3),
                Item.Ball("Luxury Ball",    15,  4),
                Item.Ball("Moon Ball",      15,  5),
                Item.Ball("Nest Ball",      15,  6),
                Item.Ball("Net Ball",       15,  7),
                Item.Ball("Pok\u00e9 Ball", 15,  8),
                Item.Ball("Premier Ball",   15,  9),
                Item.Ball("Quick Ball",     15, 10),
                Item.Ball("Repeat Ball",    15, 11),
                Item.Ball("Safari Ball",    15, 12),
                Item.Ball("Sport Ball",     15, 13),
                Item.Ball("Ultra Ball",     15, 14),
            };

            ItemsByName = ItemList.ToDictionary(item => item.Name, item => item, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
