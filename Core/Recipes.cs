// Copyright 2020 Philip Eve
//
// This file is part of CRAM IT!.
//
// CRAM IT! is free software: you can redistribute it and/or modify it under the terms of the
// GNU Affero General Public License, version 3, as published by the Free Software Foundation.
//
// CRAM IT! is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License along with this
// program. If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using SR   = CramIt.Core.StandardRecipe;
using BR   = CramIt.Core.BallRecipe;
using BRAC = CramIt.Core.BallRecipeSpecificAlternativeOutcome;
using static CramIt.Core.Type;

namespace CramIt.Core
{
    public static class Recipes
    {
        public static IReadOnlyDictionary<string, IReadOnlyList<SR>> StandardRecipes             { get; }
        public static IReadOnlyDictionary<string, SR>                TRRecipes                   { get; }
        public static IReadOnlyDictionary<string, IReadOnlyList<SR>> StandardRecipesExcludingTRs { get; }

        public static IReadOnlyDictionary<string, BR>   BallRecipes         { get; }
        public static IReadOnlyDictionary<string, Item> RepeatedItemRecipes { get; }

        static Recipes()
        {
            var standardRecipes_Simple = new []
            {
                SimpleRecipes("Bright Powder",    16, Bug     ),
                SimpleRecipes("Silver Powder",    26, Bug     ),
                SimpleRecipes("Shed Shell",       41, Bug     ),
                SimpleRecipes("Wide Lens",        11, Dark    ),
                SimpleRecipes("Scope Lens",       41, Dark    ),
                SimpleRecipes("Dragon Fang",      11, Dragon  ),
                SimpleRecipes("Dragon Scale",     36, Dragon  ),
                SimpleRecipes("Life Orb",         41, Dragon  ),
                SimpleRecipes("King's Rock",      51, Dragon  ),
                SimpleRecipes("Electric Seed",     1, Electric),
                SimpleRecipes("Cell Battery",     16, Electric),
                SimpleRecipes("Magnet",           26, Electric),
                SimpleRecipes("Upgrade",          36, Electric),
                SimpleRecipes("Light Ball",       41, Electric),
                SimpleRecipes("Dubious Disc",     51, Electric),
                SimpleRecipes("Misty Seed",       11, Fairy   ),
                SimpleRecipes("Sachet",           26, Fairy   ),
                SimpleRecipes("Room Service",     31, Fairy   ),
                SimpleRecipes("Whipped Dream",    36, Fairy   ),
                SimpleRecipes("Destiny Knot",     41, Fairy   ),
                SimpleRecipes("Muscle Band",      16, Fighting),
                SimpleRecipes("Macho Brace",      36, Fighting),
                SimpleRecipes("Expert Belt",      46, Fighting),
                SimpleRecipes("Flame Orb",        11, Fire    ),
                SimpleRecipes("Red Card",         41, Fire    ),
                SimpleRecipes("Charcoal",         51, Fire    ),
                SimpleRecipes("Pretty Feather",    1, Flying  ),
                SimpleRecipes("Sharp Beak",       11, Flying  ),
                SimpleRecipes("Blunder Policy",   31, Flying  ),
                SimpleRecipes("Grip Claw",        36, Flying  ),
                SimpleRecipes("Weakness Policy",  46, Flying  ),
                SimpleRecipes("Odd Incense",       1, Ghost   ),
                SimpleRecipes("Adrenaline Orb",   11, Ghost   ),
                SimpleRecipes("Ring Target",      16, Ghost   ),
                SimpleRecipes("Cleanse Tag",      36, Ghost   ),
                SimpleRecipes("Spell Tag",        41, Ghost   ),
                SimpleRecipes("Cracked Pot",      46, Ghost   ),
                SimpleRecipes("Reaper Cloth",     51, Ghost   ),
                SimpleRecipes("Grassy Seed",       1, Grass   ),
                SimpleRecipes("White Herb",       16, Grass   ),
                SimpleRecipes("Absorb Bulb",      41, Grass   ),
                SimpleRecipes("Light Clay",       26, Ground  ),
                SimpleRecipes("Terrain Extender", 41, Ground  ),
                SimpleRecipes("Icy Rock",         11, Ice     ),
                SimpleRecipes("Never-Melt Ice",   16, Ice     ),
                SimpleRecipes("Razor Claw",       36, Ice     ),
                SimpleRecipes("Black Sludge",      1, Poison  ),
                SimpleRecipes("Toxic Orb",        11, Poison  ),
                SimpleRecipes("Smoke Ball",       31, Poison  ),
                SimpleRecipes("Quick Powder",     41, Poison  ),
                SimpleRecipes("Poison Barb",      46, Poison  ),
                SimpleRecipes("Float Stone",       1, Rock    ),
                SimpleRecipes("Oval Stone",       11, Rock    ),
                SimpleRecipes("Hard Stone",       16, Rock    ),
                SimpleRecipes("Everstone",        26, Rock    ),
                SimpleRecipes("Protector",        31, Rock    ),
                SimpleRecipes("Rocky Helmet",     36, Rock    ),
                SimpleRecipes("Eviolite",         51, Rock    ),
                SimpleRecipes("Metal Powder",     16, Steel   ),
                SimpleRecipes("Utility Umbrella", 26, Steel   ),
                SimpleRecipes("Metal Coat",       31, Steel   ),
                SimpleRecipes("Assault Vest",     41, Steel   ),
                SimpleRecipes("Amulet Coin",      51, Steel   ),
                SimpleRecipes("Sea Incense",       1, Water   ),
                SimpleRecipes("Shell Bell",       16, Water   ),
                SimpleRecipes("Prism Scale",      36, Water   ),
                SimpleRecipes("Mystic Water",     41, Water   ),
                SimpleRecipes("TR60",  1, Bug     ),
                SimpleRecipes("TR18", 11, Bug     ),
                SimpleRecipes("TR61", 36, Bug     ),
                SimpleRecipes("TR96", 51, Bug     ),
                SimpleRecipes("TR28", 61, Bug     ),
                SimpleRecipes("TR37",  1, Dark    ),
                SimpleRecipes("TR68", 16, Dark    ),
                SimpleRecipes("TR81", 36, Dark    ),
                SimpleRecipes("TR95", 46, Dark    ),
                SimpleRecipes("TR58", 51, Dark    ),
                SimpleRecipes("TR32", 56, Dark    ),
                SimpleRecipes("TR93", 61, Dark    ),
                SimpleRecipes("TR47",  1, Dragon  ),
                SimpleRecipes("TR62", 46, Dragon  ),
                SimpleRecipes("TR51", 56, Dragon  ),
                SimpleRecipes("TR24", 61, Dragon  ),
                SimpleRecipes("TR80", 11, Electric),
                SimpleRecipes("TR86", 31, Electric),
                SimpleRecipes("TR08", 56, Electric),
                SimpleRecipes("TR09", 61, Electric),
                SimpleRecipes("TR92", 51, Fairy   ),
                SimpleRecipes("TR90", 61, Fairy   ),
                SimpleRecipes("TR07",  1, Fighting),
                SimpleRecipes("TR56", 11, Fighting),
                SimpleRecipes("TR48", 26, Fighting),
                SimpleRecipes("TR21", 31, Fighting),
                SimpleRecipes("TR99", 41, Fighting),
                SimpleRecipes("TR64", 51, Fighting),
                SimpleRecipes("TR39", 56, Fighting),
                SimpleRecipes("TR53", 61, Fighting),
                SimpleRecipes("TR88",  1, Fire    ),
                SimpleRecipes("TR41", 16, Fire    ),
                SimpleRecipes("TR02", 26, Fire    ),
                SimpleRecipes("TR36", 36, Fire    ),
                SimpleRecipes("TR15", 46, Fire    ),
                SimpleRecipes("TR55", 56, Fire    ),
                SimpleRecipes("TR43", 61, Fire    ),
                SimpleRecipes("TR89", 51, Flying  ),
                SimpleRecipes("TR66", 61, Flying  ),
                SimpleRecipes("TR33", 61, Ghost   ),
                SimpleRecipes("TR59", 11, Grass   ),
                SimpleRecipes("TR77", 26, Grass   ),
                SimpleRecipes("TR50", 31, Grass   ),
                SimpleRecipes("TR65", 36, Grass   ),
                SimpleRecipes("TR72", 51, Grass   ),
                SimpleRecipes("TR71", 61, Grass   ),
                SimpleRecipes("TR23", 11, Ground  ),
                SimpleRecipes("TR87", 31, Ground  ),
                SimpleRecipes("TR67", 36, Ground  ),
                SimpleRecipes("TR94", 51, Ground  ),
                SimpleRecipes("TR10", 61, Ground  ),
                SimpleRecipes("TR05", 51, Ice     ),
                SimpleRecipes("TR06", 61, Ice     ),
                SimpleRecipes("TR85",  1, Normal  ),
                SimpleRecipes("TR14", 11, Normal  ),
                SimpleRecipes("TR26", 16, Normal  ),
                SimpleRecipes("TR13", 21, Normal  ),
                SimpleRecipes("TR27", 26, Normal  ),
                SimpleRecipes("TR35", 31, Normal  ),
                SimpleRecipes("TR01", 36, Normal  ),
                SimpleRecipes("TR19", 41, Normal  ),
                SimpleRecipes("TR29", 46, Normal  ),
                SimpleRecipes("TR30", 51, Normal  ),
                SimpleRecipes("TR20", 56, Normal  ),
                SimpleRecipes("TR00", 61, Normal  ),
                SimpleRecipes("TR42", 66, Normal  ),
                SimpleRecipes("TR91", 16, Poison  ),
                SimpleRecipes("TR54", 26, Poison  ),
                SimpleRecipes("TR57", 36, Poison  ),
                SimpleRecipes("TR22", 51, Poison  ),
                SimpleRecipes("TR78", 56, Poison  ),
                SimpleRecipes("TR73", 61, Poison  ),
                SimpleRecipes("TR12",  1, Psychic ),
                SimpleRecipes("TR34", 11, Psychic ),
                SimpleRecipes("TR40", 16, Psychic ),
                SimpleRecipes("TR82", 21, Psychic ),
                SimpleRecipes("TR44", 26, Psychic ),
                SimpleRecipes("TR83", 31, Psychic ),
                SimpleRecipes("TR25", 36, Psychic ),
                SimpleRecipes("TR69", 41, Psychic ),
                SimpleRecipes("TR17", 46, Psychic ),
                SimpleRecipes("TR38", 51, Psychic ),
                SimpleRecipes("TR49", 56, Psychic ),
                SimpleRecipes("TR97", 61, Psychic ),
                SimpleRecipes("TR11", 66, Psychic ),
                SimpleRecipes("TR63", 41, Rock    ),
                SimpleRecipes("TR76", 56, Rock    ),
                SimpleRecipes("TR75", 61, Rock    ),
                SimpleRecipes("TR31",  1, Steel   ),
                SimpleRecipes("TR46", 11, Steel   ),
                SimpleRecipes("TR52", 36, Steel   ),
                SimpleRecipes("TR79", 46, Steel   ),
                SimpleRecipes("TR70", 56, Steel   ),
                SimpleRecipes("TR74", 61, Steel   ),
                SimpleRecipes("TR04", 11, Water   ),
                SimpleRecipes("TR16", 26, Water   ),
                SimpleRecipes("TR98", 31, Water   ),
                SimpleRecipes("TR45", 51, Water   ),
                SimpleRecipes("TR84", 56, Water   ),
                SimpleRecipes("TR03", 61, Water   ),
            };

            static KeyValuePair<string, SR> SimpleRecipes(string itemName, int minimumTotalValue, Type type)
                => new KeyValuePair<string, SR>(itemName, new SR(itemName, minimumTotalValue, type));

            var standardRecipes_MultipleTypesSingleValue = new Dictionary<string, SR>
            {
                {"Stardust",      new SR("Stardust",       1, Fairy, Ground                           )},
                {"Big Mushroom",  new SR("Big Mushroom",  16, Dragon, Fairy, Flying, Ground           )},
                {"Star Piece",    new SR("Star Piece",    26, Dark, Dragon, Ghost, Ice                )},
                {"Balm Mushroom", new SR("Balm Mushroom", 31, Bug, Dark, Dragon, Fire, Ghost, Ice     )},
                {"Pearl String",  new SR("Pearl String",  46, Bug, Electric, Grass, Ground, Ice, Water)},
                {"Comet Shard",   new SR("Comet Shard",   56, Bug, Flying, Ghost, Grass, Ground, Ice  )},
                {"Rare Candy", SR.AllTypesExcept("Rare Candy", 66, Normal, Psychic)},
                {"Bottle Cap", SR.AllTypes("Bottle Cap", 71)},
                {"PP Up",      SR.AllTypes("PP Up",      76)},
            };

            var standardRecipes_SingleTypeMultipleValues = new Dictionary<string, IReadOnlyList<SR>>
            {
                {"Air Balloon", new [] {new SR("Air Balloon", 26, Flying), new SR("Air Balloon", 41, Flying)}},
                {"Snowball",    new [] {new SR("Snowball",     1, Ice   ), new SR("Snowball",    41, Ice   )}},
            };

            var standardRecipes_EverythingElse = new Dictionary<string, IReadOnlyList<SR>>
            {
                {"Wishing Piece", new [] {SR.AllTypesExcept("Wishing Piece", 21, Normal, Psychic), new SR("Wishing Piece", 46, Rock)}},
                {"Berry Sweet",  SimpleSweetRecipes("Berry",  46, "Clover", "Flower", "Love",   "Strawberry")},
                {"Clover Sweet", SimpleSweetRecipes("Clover", 46, "Berry",  "Flower", "Love",   "Strawberry")},
                {"Flower Sweet", SimpleSweetRecipes("Flower", 46, "Berry",  "Clover", "Love",   "Strawberry")},
                {"Love Sweet",   SimpleSweetRecipes("Love",   46, "Berry",  "Clover", "Flower", "Strawberry")},
                {"Ribbon Sweet", SimpleSweetRecipes("Ribbon", 56, "Star",   "Strawberry")},
                {"Star Sweet",   SimpleSweetRecipes("Star",   56, "Ribbon", "Strawberry")},
                {"Strawberry Sweet", new [] {
                    new SR("Strawberry Sweet", 46, new [] {Fairy}, new [] {"Berry Sweet", "Clover Sweet", "Flower Sweet", "Love Sweet"}),
                    new SR("Strawberry Sweet", 56, new [] {Fairy}, new [] {"Ribbon Sweet", "Star Sweet"})
                }},
            };

            static SR[] SimpleSweetRecipes(string sweetTypeName, int minimumTotalValue, params string[] otherPossibleSweetTypeNames)
                => new [] {new SR($"{sweetTypeName} Sweet", minimumTotalValue, new [] {Fairy}, otherPossibleSweetTypeNames.Select(name => $"{name} Sweet"))};

            TRRecipes = standardRecipes_Simple.Where(recipe => recipe.Value.Item.IsTR).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            StandardRecipesExcludingTRs = standardRecipes_Simple
                .Where (recipe => ! recipe.Value.Item.IsTR)
                .Concat(standardRecipes_MultipleTypesSingleValue)
                .Select(kvp => new KeyValuePair<string, IReadOnlyList<SR>>(kvp.Key, new [] {kvp.Value}))
                .Concat(standardRecipes_SingleTypeMultipleValues)
                .Concat(standardRecipes_EverythingElse)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            StandardRecipes = StandardRecipesExcludingTRs
                .Concat(TRRecipes.Select(kvp => new KeyValuePair<string, IReadOnlyList<SR>>(kvp.Key, new [] {kvp.Value})))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

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
                247 + 247 + 10,
                new [] {new BRAC("Pok\u00e9 Ball", 247), new BRAC("Great Ball", 247), new BRAC("Sport Ball", 1)}
            );
            var BR_SportBall = new BR(
                apricornColours_All,
                1,
                247 + 247 + 10,
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

            static IEnumerable<BRAC> AlternativeOutcomes_247(string b247, string b10)
            {
                yield return new BRAC($"Pok\u00e9 Ball", 247);
                yield return new BRAC($"Great Ball",     247);
                yield return new BRAC($"{b247} Ball",    247);
                yield return new BRAC($"{b10} Ball",      10);
                yield return new BRAC($"Safari Ball",      1);
                yield return new BRAC($"Sport Ball",       1);
            }

            static IEnumerable<BRAC> AlternativeOutcomes_10(string b247a, string b247b)
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
