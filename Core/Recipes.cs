using System;
using System.Collections.Generic;
using System.Linq;
using static CramIt.Core.Type;
using SR   = CramIt.Core.StandardRecipe;
using BR   = CramIt.Core.BallRecipe;
using BRAC = CramIt.Core.BallRecipeSpecificAlternativeOutcome;

namespace CramIt.Core
{
    public static class Recipes
    {
        public static IReadOnlyDictionary<string, IReadOnlyList<SR>> StandardRecipes     { get; }
        public static IReadOnlyDictionary<string, BR>                BallRecipes         { get; }
        public static IReadOnlyDictionary<string, Item>              RepeatedItemRecipes { get; }

        static Recipes()
        {
            var standardRecipes_Simple = new Dictionary<string, SR>
            {
                {"Bright Powder",    new SR(16, Bug     )},
                {"Silver Powder",    new SR(26, Bug     )},
                {"Shed Shell",       new SR(41, Bug     )},
                {"Wide Lens",        new SR(11, Dark    )},
                {"Scope Lens",       new SR(41, Dark    )},
                {"Dragon Fang",      new SR(11, Dragon  )},
                {"Dragon Scale",     new SR(36, Dragon  )},
                {"Life Orb",         new SR(41, Dragon  )},
                {"King's Rock",      new SR(51, Dragon  )},
                {"Electric Seed",    new SR( 1, Electric)},
                {"Cell Battery",     new SR(16, Electric)},
                {"Magnet",           new SR(26, Electric)},
                {"Upgrade",          new SR(36, Electric)},
                {"Light Ball",       new SR(41, Electric)},
                {"Dubious Disc",     new SR(51, Electric)},
                {"Misty Seed",       new SR(11, Fairy   )},
                {"Sachet",           new SR(26, Fairy   )},
                {"Room Service",     new SR(31, Fairy   )},
                {"Whipped Dream",    new SR(36, Fairy   )},
                {"Destiny Knot",     new SR(41, Fairy   )},
                {"Muscle Band",      new SR(16, Fighting)},
                {"Macho Brace",      new SR(36, Fighting)},
                {"Expert Belt",      new SR(46, Fighting)},
                {"Flame Orb",        new SR(11, Fire    )},
                {"Red Card",         new SR(41, Fire    )},
                {"Charcoal",         new SR(51, Fire    )},
                {"Pretty Feather",   new SR( 1, Flying  )},
                {"Sharp Beak",       new SR(11, Flying  )},
                {"Blunder Policy",   new SR(31, Flying  )},
                {"Grip Claw",        new SR(36, Flying  )},
                {"Weakness Policy",  new SR(46, Flying  )},
                {"Odd Incense",      new SR( 1, Ghost   )},
                {"Adrenaline Orb",   new SR(11, Ghost   )},
                {"Ring Target",      new SR(16, Ghost   )},
                {"Cleanse Tag",      new SR(36, Ghost   )},
                {"Spell Tag",        new SR(41, Ghost   )},
                {"Cracked Pot",      new SR(46, Ghost   )},
                {"Reaper Cloth",     new SR(51, Ghost   )},
                {"Grassy Seed",      new SR( 1, Grass   )},
                {"White Herb",       new SR(16, Grass   )},
                {"Absorb Bulb",      new SR(41, Grass   )},
                {"Light Clay",       new SR(26, Ground  )},
                {"Terrain Extender", new SR(41, Ground  )},
                {"Icy Rock",         new SR(11, Ice     )},
                {"Never-Melt Ice",   new SR(16, Ice     )},
                {"Razor Claw",       new SR(36, Ice     )},
                {"Black Sludge",     new SR( 1, Poison  )},
                {"Toxic Orb",        new SR(11, Poison  )},
                {"Smoke Ball",       new SR(31, Poison  )},
                {"Quick Powder",     new SR(41, Poison  )},
                {"Poison Barb",      new SR(46, Poison  )},
                {"Float Stone",      new SR( 1, Rock    )},
                {"Oval Stone",       new SR(11, Rock    )},
                {"Hard Stone",       new SR(16, Rock    )},
                {"Everstone",        new SR(26, Rock    )},
                {"Protector",        new SR(31, Rock    )},
                {"Rocky Helmet",     new SR(36, Rock    )},
                {"Eviolite",         new SR(51, Rock    )},
                {"Metal Powder",     new SR(16, Steel   )},
                {"Utility Umbrella", new SR(26, Steel   )},
                {"Metal Coat",       new SR(31, Steel   )},
                {"Assault Vest",     new SR(41, Steel   )},
                {"Amulet Coin",      new SR(51, Steel   )},
                {"Sea Incense",      new SR( 1, Water   )},
                {"Shell Bell",       new SR(16, Water   )},
                {"Prism Scale",      new SR(36, Water   )},
                {"Mystic Water",     new SR(41, Water   )},
                {"TR60", new SR( 1, Bug     )},
                {"TR18", new SR(11, Bug     )},
                {"TR61", new SR(36, Bug     )},
                {"TR96", new SR(51, Bug     )},
                {"TR28", new SR(61, Bug     )},
                {"TR37", new SR( 1, Dark    )},
                {"TR68", new SR(16, Dark    )},
                {"TR81", new SR(36, Dark    )},
                {"TR95", new SR(46, Dark    )},
                {"TR58", new SR(51, Dark    )},
                {"TR32", new SR(56, Dark    )},
                {"TR93", new SR(61, Dark    )},
                {"TR47", new SR( 1, Dragon  )},
                {"TR62", new SR(46, Dragon  )},
                {"TR51", new SR(56, Dragon  )},
                {"TR24", new SR(61, Dragon  )},
                {"TR80", new SR(11, Electric)},
                {"TR86", new SR(31, Electric)},
                {"TR08", new SR(56, Electric)},
                {"TR09", new SR(61, Electric)},
                {"TR92", new SR(51, Fairy   )},
                {"TR90", new SR(61, Fairy   )},
                {"TR07", new SR( 1, Fighting)},
                {"TR56", new SR(11, Fighting)},
                {"TR48", new SR(26, Fighting)},
                {"TR21", new SR(31, Fighting)},
                {"TR99", new SR(41, Fighting)},
                {"TR64", new SR(51, Fighting)},
                {"TR39", new SR(56, Fighting)},
                {"TR53", new SR(61, Fighting)},
                {"TR88", new SR( 1, Fire    )},
                {"TR41", new SR(16, Fire    )},
                {"TR02", new SR(26, Fire    )},
                {"TR36", new SR(36, Fire    )},
                {"TR15", new SR(46, Fire    )},
                {"TR55", new SR(56, Fire    )},
                {"TR43", new SR(61, Fire    )},
                {"TR89", new SR(51, Flying  )},
                {"TR66", new SR(61, Flying  )},
                {"TR33", new SR(61, Ghost   )},
                {"TR59", new SR(11, Grass   )},
                {"TR77", new SR(26, Grass   )},
                {"TR50", new SR(31, Grass   )},
                {"TR65", new SR(36, Grass   )},
                {"TR72", new SR(51, Grass   )},
                {"TR71", new SR(61, Grass   )},
                {"TR23", new SR(11, Ground  )},
                {"TR87", new SR(31, Ground  )},
                {"TR67", new SR(36, Ground  )},
                {"TR94", new SR(51, Ground  )},
                {"TR10", new SR(61, Ground  )},
                {"TR05", new SR(51, Ice     )},
                {"TR06", new SR(61, Ice     )},
                {"TR85", new SR( 1, Normal  )},
                {"TR14", new SR(11, Normal  )},
                {"TR26", new SR(16, Normal  )},
                {"TR13", new SR(21, Normal  )},
                {"TR27", new SR(26, Normal  )},
                {"TR35", new SR(31, Normal  )},
                {"TR01", new SR(36, Normal  )},
                {"TR19", new SR(41, Normal  )},
                {"TR29", new SR(46, Normal  )},
                {"TR30", new SR(51, Normal  )},
                {"TR20", new SR(56, Normal  )},
                {"TR00", new SR(61, Normal  )},
                {"TR42", new SR(66, Normal  )},
                {"TR91", new SR(16, Poison  )},
                {"TR54", new SR(26, Poison  )},
                {"TR57", new SR(36, Poison  )},
                {"TR22", new SR(51, Poison  )},
                {"TR78", new SR(56, Poison  )},
                {"TR73", new SR(61, Poison  )},
                {"TR12", new SR( 1, Psychic )},
                {"TR34", new SR(11, Psychic )},
                {"TR40", new SR(16, Psychic )},
                {"TR82", new SR(21, Psychic )},
                {"TR44", new SR(26, Psychic )},
                {"TR83", new SR(31, Psychic )},
                {"TR25", new SR(36, Psychic )},
                {"TR69", new SR(41, Psychic )},
                {"TR17", new SR(46, Psychic )},
                {"TR38", new SR(51, Psychic )},
                {"TR49", new SR(56, Psychic )},
                {"TR97", new SR(61, Psychic )},
                {"TR11", new SR(66, Psychic )},
                {"TR63", new SR(41, Rock    )},
                {"TR76", new SR(56, Rock    )},
                {"TR75", new SR(61, Rock    )},
                {"TR31", new SR( 1, Steel   )},
                {"TR46", new SR(11, Steel   )},
                {"TR52", new SR(36, Steel   )},
                {"TR79", new SR(46, Steel   )},
                {"TR70", new SR(56, Steel   )},
                {"TR74", new SR(61, Steel   )},
                {"TR04", new SR(11, Water   )},
                {"TR16", new SR(26, Water   )},
                {"TR98", new SR(31, Water   )},
                {"TR45", new SR(51, Water   )},
                {"TR84", new SR(56, Water   )},
                {"TR03", new SR(61, Water   )},
            };

            var standardRecipes_MultipleTypesSingleValue = new Dictionary<string, SR>
            {
                {"Stardust",      new SR( 1, Fairy, Ground                           )},
                {"Big Mushroom",  new SR(16, Dragon, Fairy, Flying, Ground           )},
                {"Star Piece",    new SR(26, Dark, Dragon, Ghost, Ice                )},
                {"Balm Mushroom", new SR(31, Bug, Dark, Dragon, Fire, Ghost, Ice     )},
                {"Pearl String",  new SR(46, Bug, Electric, Grass, Ground, Ice, Water)},
                {"Comet Shard",   new SR(56, Bug, Flying, Ghost, Grass, Ground, Ice  )},
                {"Rare Candy", SR.AllTypesExcept(66, Normal, Psychic)},
                {"Bottle Cap", SR.AllTypes(71)},
                {"PP Up",      SR.AllTypes(76)},
            };

            var standardRecipes_SingleTypeMultipleValues = new Dictionary<string, IReadOnlyList<SR>>
            {
                {"Air Balloon", new [] {new SR(26, Flying), new SR(41, Flying)}},
                {"Snowball",    new [] {new SR( 1, Ice   ), new SR(41, Ice   )}},
            };

            var standardRecipes_EverythingElse = new Dictionary<string, IReadOnlyList<SR>>
            {
                {"Wishing Piece",    new [] {SR.AllTypesExcept(21, Normal, Psychic), new SR(46, Rock)}},
                {"Berry Sweet",      new [] {new SR(46, new [] {Fairy}, new [] {"Clover Sweet", "Flower Sweet", "Love Sweet",   "Strawberry Sweet"})}},
                {"Clover Sweet",     new [] {new SR(46, new [] {Fairy}, new [] {"Berry Sweet",  "Flower Sweet", "Love Sweet",   "Strawberry Sweet"})}},
                {"Flower Sweet",     new [] {new SR(46, new [] {Fairy}, new [] {"Berry Sweet",  "Clover Sweet", "Love Sweet",   "Strawberry Sweet"})}},
                {"Love Sweet",       new [] {new SR(46, new [] {Fairy}, new [] {"Berry Sweet",  "Clover Sweet", "Flower Sweet", "Strawberry Sweet"})}},
                {"Ribbon Sweet",     new [] {new SR(56, new [] {Fairy}, new [] {"Star Sweet",   "Strawberry Sweet"})}},
                {"Star Sweet",       new [] {new SR(56, new [] {Fairy}, new [] {"Ribbon Sweet", "Strawberry Sweet"})}},
                {"Strawberry Sweet", new [] {
                    new SR(46, new [] {Fairy}, new [] {"Berry Sweet", "Clover Sweet", "Flower Sweet", "Love Sweet"}),
                    new SR(56, new [] {Fairy}, new [] {"Ribbon Sweet", "Star Sweet"})
                }},
            };

            StandardRecipes = standardRecipes_Simple
                .Concat(standardRecipes_MultipleTypesSingleValue)
                .Select(kvp => new KeyValuePair<string, IReadOnlyList<SR>>(kvp.Key, new [] {kvp.Value}))
                .Concat(standardRecipes_SingleTypeMultipleValues)
                .Concat(standardRecipes_EverythingElse)
                .ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyList<SR>)(new [] {kvp.Value}));

            var apricornColours_All                     = new [] {"Black", "Blue", "Green", "Pink", "Red", "White", "Yellow"};
            var apricornColours_ContributingToUltraBall = new [] {                 "Green", "Pink", "Red", "White", "Yellow"};

            var BR_PokeBall  = new BR(
                apricornColours_All,
                247,
                247 + 247 + 10,
                new [] {new BRAC("Great Ball", 247), new BRAC("Safari Ball", 1), new BRAC("Sport Ball", 1)}
            );
            var BR_GreatBall = new BR(
                apricornColours_All,
                247,
                247 + 247 + 10,
                new [] {new BRAC("Pok\u00e9 Ball", 247), new BRAC("Safari Ball", 1), new BRAC("Sport Ball", 1)}
            );
            var BR_UltraBall = new BR(
                apricornColours_ContributingToUltraBall,
                247,
                247 + 10,
                new [] {new BRAC("Pok\u00e9 Ball", 247), new BRAC("Great Ball", 247), new BRAC("Safari Ball", 1), new BRAC("Sport Ball", 1)}
            );

            var BR_SafariBall = new BR(
                apricornColours_All,
                1,
                247 + 247 + 10 + 1,
                new [] {new BRAC("Pok\u00e9 Ball", 247), new BRAC("Great Ball", 247), new BRAC("Sport Ball", 1)}
            );
            var BR_SportBall = new BR(
                apricornColours_All,
                1,
                247 + 247 + 10 + 1,
                new [] {new BRAC("Pok\u00e9 Ball", 247), new BRAC("Great Ball", 247), new BRAC("Safari Ball", 1)}
            );

            BallRecipes = new Dictionary<string, BR>
            {
                {"Pok\u00e9 Ball", BR_PokeBall },
                {"Great Ball",     BR_GreatBall},
                {"Ultra Ball",     BR_UltraBall},
                {"Dive Ball",    new BR("Blue",   247, 0, AlternativeOutcomes_247("Net",    "Lure"   ))},
                {"Dusk Ball",    new BR("Black",  247, 0, AlternativeOutcomes_247("Luxury", "Heavy"  ))},
                {"Heal Ball",    new BR("Pink",   247, 0, AlternativeOutcomes_247("Ultra",  "Love"   ))},
                {"Luxury Ball",  new BR("Black",  247, 0, AlternativeOutcomes_247("Dusk",   "Heavy"  ))},
                {"Nest Ball",    new BR("Green",  247, 0, AlternativeOutcomes_247("Ultra",  "Friend" ))},
                {"Net Ball",     new BR("Blue",   247, 0, AlternativeOutcomes_247("Dive",   "Lure"   ))},
                {"Premier Ball", new BR("White",  247, 0, AlternativeOutcomes_247("Ultra",  "Fast"   ))},
                {"Quick Ball",   new BR("Yellow", 247, 0, AlternativeOutcomes_247("Ultra",  "Moon"   ))},
                {"Repeat Ball",  new BR("Red",    247, 0, AlternativeOutcomes_247("Ultra",  "Level"  ))},
                {"Fast Ball",    new BR("White",   10, 0, AlternativeOutcomes_10 ("Ultra",  "Premier"))},
                {"Friend Ball",  new BR("Green",   10, 0, AlternativeOutcomes_10 ("Ultra",  "Nest"   ))},
                {"Heavy Ball",   new BR("Black",   10, 0, AlternativeOutcomes_10 ("Dusk",   "Luxury" ))},
                {"Level Ball",   new BR("Red",     10, 0, AlternativeOutcomes_10 ("Ultra",  "Repeat" ))},
                {"Love Ball",    new BR("Pink",    10, 0, AlternativeOutcomes_10 ("Ultra",  "Heal"   ))},
                {"Lure Ball",    new BR("Blue",    10, 0, AlternativeOutcomes_10 ("Dive",   "Net"    ))},
                {"Moon Ball",    new BR("Yellow",  10, 0, AlternativeOutcomes_10 ("Ultra",  "Quick"  ))},
                {"Safari Ball", BR_SafariBall},
                {"Sport Ball",  BR_SportBall },
            };

            IEnumerable<BRAC> AlternativeOutcomes_247(string b247, string b10)
            {
                yield return new BRAC($"Pok\u00e9 Ball", 247);
                yield return new BRAC($"Great Ball",     247);
                yield return new BRAC($"{b247} Ball",    247);
                yield return new BRAC($"{b10} Ball",      10);
                yield return new BRAC($"Safari Ball",      1);
                yield return new BRAC($"Sport Ball",       1);
            }

            IEnumerable<BRAC> AlternativeOutcomes_10(string b247a, string b247b)
            {
                yield return new BRAC($"Pok\u00e9 Ball", 247);
                yield return new BRAC($"Great Ball",     247);
                yield return new BRAC($"{b247a} Ball",   247);
                yield return new BRAC($"{b247b} Ball",   247);
                yield return new BRAC($"Safari Ball",      1);
                yield return new BRAC($"Sport Ball",       1);
            }

            RepeatedItemRecipes = new Dictionary<string, Item>
            {
                {"Ability Capsule", Items.ItemsByName["Rare Candy"   ]},
                {"Balm Mushroom",   Items.ItemsByName["Big Mushroom" ]},
                {"Big Mushroom",    Items.ItemsByName["Tiny Mushroom"]},
                {"Big Nugget",      Items.ItemsByName["Nugget"       ]},
                {"Big Pearl",       Items.ItemsByName["Pearl"        ]},
                {"Comet Shard",     Items.ItemsByName["Star Piece"   ]},
                {"Gold Bottle Cap", Items.ItemsByName["Bottle Cap"   ]},
                {"Pearl String",    Items.ItemsByName["Big Pearl"    ]},
                {"PP Up",           Items.ItemsByName["Armorite Ore" ]},
                {"Star Piece",      Items.ItemsByName["Stardust"     ]},
            };
        }
    }
}
