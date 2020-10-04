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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CramIt.Core;

namespace Tests
{
    [TestClass]
    public class StandardRecipeGroupItemFiltererTests
    {
        private static readonly InputItemOptions _inputItemOptions = new InputItemOptions {IncludeIrreplaceableItems = true};

        private static IEnumerable<Item> ItemNamesToItems(params string[] itemNames)
            => itemNames.Select(itemName => Items.ItemsByName[itemName]);

        [TestMethod]
        public void CanCompleteRecipeUsingAnApricornIfAndOnlyIfMinValueIsLessThan61()
        {
            var namesOfMinValue61OrAboveRecipeItems = new []
            {
                "Bottle Cap", "PP Up", "Rare Candy",
                "TR00", "TR06", "TR10", "TR24", "TR33", "TR43", "TR66", "TR73", "TR75", "TR93",
                "TR03", "TR09", "TR11", "TR28", "TR42", "TR53", "TR71", "TR74", "TR90", "TR97",
            };
            var apricornColourNames = new [] {"Black", "Blue", "Green", "Pink", "Red", "White", "Yellow"};

            foreach ((string recipeItemName, IReadOnlyList<StandardRecipe> recipesList) in Recipes.StandardRecipes)
            {
                bool canCompleteRecipe_Expected = ! namesOfMinValue61OrAboveRecipeItems.Contains(recipeItemName);
                var filtererGroup = new StandardRecipeGroupItemFilterer(recipesList, _inputItemOptions);

                foreach (string colourName in apricornColourNames)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[$"{colourName} Apricorn"]);
                    Assert.AreEqual(canCompleteRecipe_Expected, canCompleteRecipe_Actual, $"[{recipeItemName}, {colourName} Apricorn]");
                }
            }
        }

        [TestMethod]
        public void CannotCompleteMaxValue10RecipesUsingValue11OrAboveInputItems()
        {
            var namesOfMaxValue10RecipeItems = new []
            {
                "Black Sludge", "Electric Seed", "Float Stone", "Grassy Seed", "Odd Incense", "Pretty Feather", "Sea Incense", "Stardust",
                "TR07", "TR12", "TR31", "TR37", "TR47", "TR60", "TR85", "TR88",
            };
            var namesOfValue11OrAboveInputItems = new []
            {
                "Ability Capsule", "Charcoal",     "Dragon Memory",   "Fire Memory",     "Ice Memory",    "PP Max",         "Reaper Cloth", "Strawberry Sweet",
                "Amulet Coin",     "Chipped Pot",  "Dubious Disc",    "Flower Sweet",    "King's Rock",   "PP Up",          "Ribbon Sweet", "Sweet Apple",
                "Armorite Ore",    "Clover Sweet", "Electric Memory", "Flying Memory",   "Love Sweet",    "Prism Scale",    "Rock Memory",  "Tart Apple",
                "Berry Sweet",     "Comet Shard",  "Eviolite",        "Ghost Memory",    "Macho Brace",   "Psychic Memory", "Rocky Helmet", "Upgrade",
                "Big Nugget",      "Cracked Pot",  "Expert Belt",     "Gold Bottle Cap", "Pearl String",  "Quick Claw",     "Spell Tag",    "Water Memory",
                "Bottle Cap",      "Dark Memory",  "Fairy Memory",    "Grass Memory",    "Poison Barb",   "Quick Powder",   "Star Sweet",   "Weakness Policy",
                "Bug Memory",      "Destiny Knot", "Fighting Memory", "Ground Memory",   "Poison Memory", "Rare Candy",     "Steel Memory", "Wishing Piece",
            };

            foreach (string recipeItemName in namesOfMaxValue10RecipeItems)
            {
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName], _inputItemOptions);

                foreach (string inputItemName in namesOfValue11OrAboveInputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[inputItemName]);
                    Assert.IsFalse(canCompleteRecipe_Actual, $"[{recipeItemName}, {inputItemName}]");
                }
            }
        }

        [TestMethod]
        public void CannotCompleteValue11To15RecipesUsingValue16OrAboveInputItems()
        {
            var namesOfValue11To15RecipeItems = new []
            {
                "Adrenaline Orb", "Dragon Fang", "Flame Orb", "Icy Rock", "Misty Seed", "Oval Stone", "Sharp Beak", "Toxic Orb", "Wide Lens",
                "TR04", "TR14", "TR18", "TR23", "TR34", "TR46", "TR56", "TR59", "TR80",
            };
            var namesOfValue16OrAboveInputItems = new []
            {
                "Ability Capsule", "Bug Memory",  "Dark Memory",     "Fairy Memory",    "Ghost Memory",    "Ice Memory",  "Poison Barb",    "Reaper Cloth",
                "Amulet Coin",     "Charcoal",    "Dragon Memory",   "Fighting Memory", "Gold Bottle Cap", "King's Rock", "Poison Memory",  "Rock Memory",
                "Big Nugget",      "Chipped Pot", "Electric Memory", "Fire Memory",     "Grass Memory",    "PP Max",      "Psychic Memory", "Steel Memory",
                "Bottle Cap",      "Comet Shard", "Expert Belt",     "Flying Memory",   "Ground Memory",   "PP Up",       "Rare Candy",     "Water Memory",
            };

            foreach (string recipeItemName in namesOfValue11To15RecipeItems)
            {
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName], _inputItemOptions);

                foreach (string inputItemName in namesOfValue16OrAboveInputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[inputItemName]);
                    Assert.IsFalse(canCompleteRecipe_Actual, $"[{recipeItemName}, {inputItemName}]");
                }
            }
        }

        [TestMethod]
        public void CanCompleteValue16To20RecipesUsingAnyValue20InputItemIfAnApricornExistsOfTheAppropriateType()
        {
            var namesOfValue16To20RecipeItemsForWhichAnApricornExistsOfAnAppropriateType = new []
            {
                "Big Mushroom", "Cell Battery", "Shell Bell", "White Herb",
                "TR26", "TR41", "TR68",
            };
            var namesOfValue20InputItems = new []
            {
                "Ability Capsule", "Bottle Cap",      "Gold Bottle Cap", "PP Max",        "Rare Candy",
                "Bug Memory",      "Electric Memory", "Fire Memory",     "Grass Memory",  "Poison Memory",  "Steel Memory",
                "Dark Memory",     "Fairy Memory",    "Flying Memory",   "Ground Memory", "Psychic Memory", "Water Memory",
                "Dragon Memory",   "Fighting Memory", "Ghost Memory",    "Ice Memory",    "Rock Memory",
            };

            foreach (string recipeItemName in namesOfValue16To20RecipeItemsForWhichAnApricornExistsOfAnAppropriateType)
            {
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName], _inputItemOptions);

                foreach (string inputItemName in namesOfValue20InputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[inputItemName]);
                    Assert.IsTrue(canCompleteRecipe_Actual, $"[{recipeItemName}, {inputItemName}]");
                }
            }
        }

        [TestMethod]
        public void CanCompleteValue16To20RecipesUsingCertainValue20InputItemsEvenWhenNoApricornExistsOfTheAppropriateType()
        {
            var namesOfValue16To20RecipeItemsForWhichNoApricornExistsOfAnAppropriateTypeMappedToNamesOfValue20InputItemsWhichCanCompleteThoseRecipes =
                new Dictionary<string, IReadOnlyList<string>>
                {
                    {"Bright Powder",  new [] {"Bug Memory"                                   }},
                    {"Hard Stone",     new [] {"Rock Memory"                                  }},
                    {"Metal Powder",   new [] {"Steel Memory", "Bottle Cap", "Gold Bottle Cap"}},
                    {"Muscle Band",    new [] {"Fighting Memory"                              }},
                    {"Never-Melt Ice", new [] {"Ice Memory"                                   }},
                    {"Ring Target",    new [] {"Ghost Memory"                                 }},
                    {"TR40",           new [] {"Psychic Memory", "Rare Candy"                 }},
                    {"TR91",           new [] {"Poison Memory"                                }},
                };
            var namesOfValue20InputItems = new []
            {
                "Ability Capsule", "Bottle Cap",      "Gold Bottle Cap", "PP Max",        "Rare Candy",
                "Bug Memory",      "Electric Memory", "Fire Memory",     "Grass Memory",  "Poison Memory",  "Steel Memory",
                "Dark Memory",     "Fairy Memory",    "Flying Memory",   "Ground Memory", "Psychic Memory", "Water Memory",
                "Dragon Memory",   "Fighting Memory", "Ghost Memory",    "Ice Memory",    "Rock Memory",
            };

            foreach ((string recipeItemName, IReadOnlyList<string> compatibleValue20InputItems) in
                namesOfValue16To20RecipeItemsForWhichNoApricornExistsOfAnAppropriateTypeMappedToNamesOfValue20InputItemsWhichCanCompleteThoseRecipes)
            {
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName], _inputItemOptions);

                foreach (string inputItemName in namesOfValue20InputItems)
                {
                    bool canCompleteRecipe_Expected = compatibleValue20InputItems.Contains(inputItemName);
                    bool canCompleteRecipe_Actual   = filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[inputItemName]);
                    Assert.AreEqual(canCompleteRecipe_Expected, canCompleteRecipe_Actual, $"[{recipeItemName}, {inputItemName}]");
                }
            }
        }

        [TestMethod]
        public void GenericExample_3AlreadyChosenInputs()
        {
            var alreadyChosenInputItems_IncludingAFightingTypeItem    = ItemNamesToItems("Rare Candy", "Black Apricorn", "Armorite Ore");
            var alreadyChosenInputItems_NotIncludingAFightingTypeItem = ItemNamesToItems("Rare Candy", "Black Apricorn", "Destiny Knot");

            var namesOfInputItemsThatCanCompleteTheRecipe_AlreadyChosenInputsIncludeAFightingTypeItem = new []
            {
                "Absorb Bulb",    "Coba Berry",    "Fire Stone",       "Haban Berry",    "Lansat Berry",   "Passho Berry",    "Roseli Berry",   "Sticky Barb",
                "Adrenaline Orb", "Colbur Berry",  "Float Stone",      "Health Feather", "Leaf Stone",     "Payapa Berry",    "Rowap Berry",    "Sun Stone",
                "Aguav Berry",    "Custap Berry",  "Focus Band",       "Heat Rock",      "Liechi Berry",   "Pearl",           "Safety Goggles", "Swift Feather",
                "Apicot Berry",   "Damp Rock",     "Focus Sash",       "Hondew Berry",   "Luminous Moss",  "Petaya Berry",    "Salac Berry",    "Tamato Berry",
                "Babiri Berry",   "Dawn Stone",    "Fossilized Bird",  "Iapapa Berry",   "Mago Berry",     "Pomeg Berry",     "Scope Lens",     "Tanga Berry",
                "Big Pearl",      "Dusk Stone",    "Fossilized Dino",  "Ice Stone",      "Maranga Berry",  "Power Herb",      "Sharp Beak",     "Terrain Extender",
                "Big Root",       "Dynamax Candy", "Fossilized Drake", "Icy Rock",       "Micle Berry",    "Pretty Feather",  "Shed Shell",     "Throat Spray",
                "Binding Band",   "Electric Seed", "Fossilized Fish",  "Iron",           "Misty Seed",     "Protective Pads", "Shiny Stone",    "Thunder Stone",
                "Black Sludge",   "Enigma Berry",  "Galarica Twig",    "Jaboca Berry",   "Moon Stone",     "Protein",         "Shuca Berry",    "Wacan Berry",
                "Calcium",        "Exp. Candy L",  "Ganlon Berry",     "Kasib Berry",    "Muscle Feather", "Psychic Seed",    "Silver Powder",  "Water Stone",
                "Carbos",         "Exp. Candy M",  "Genius Feather",   "Kebia Berry",    "Mystic Water",   "Qualot Berry",    "Smooth Rock",    "Wiki Berry",
                "Charti Berry",   "Exp. Candy S",  "Grassy Seed",      "Kee Berry",      "Normal Gem",     "Rare Bone",       "Snowball",       "Wise Glasses",
                "Chilan Berry",   "Exp. Candy XL", "Grepa Berry",      "Kelpsy Berry",   "Occa Berry",     "Resist Feather",  "Stardust",       "Yache Berry",
                "Chople Berry",   "Figy Berry",    "HP Up",            "Lagging Tail",   "Oval Stone",     "Rindo Berry",     "Starf Berry",    "Zinc",
                "Clever Feather",
            };
            var namesOfInputItemsThatCanCompleteTheRecipe_AlreadyChosenInputsDoNotIncludeAFightingTypeItem = new []
            {
                "Calcium", "Chople Berry", "Focus Sash", "Iron",         "Protective Pads", "Protein", "Salac Berry", "Zinc",
                "Carbos",  "Focus Band",   "HP Up",      "Kelpsy Berry",
            };

            var filtererGroup_AlreadyChosenInputItemsIncludeAFightingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Macho Brace"], _inputItemOptions, alreadyChosenInputItems_IncludingAFightingTypeItem);
            var filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Macho Brace"], _inputItemOptions, alreadyChosenInputItems_NotIncludingAFightingTypeItem);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFightingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteTheRecipe_AlreadyChosenInputsIncludeAFightingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFightingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsIncludeAFightingTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFightingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFightingTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items include a Fighting-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteTheRecipe_AlreadyChosenInputsDoNotIncludeAFightingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFightingTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items do not include a Fighting-type item]"
                );
            }
        }

        [TestMethod]
        public void CanCompleteTheBigMushroomRecipeUsingInputItemsOfSeveralTypes()
        {
            var alreadyChosenInputItems = ItemNamesToItems("Wide Lens", "Black Apricorn", "Black Apricorn");
            var namesOfInputItemsThatCanCompleteTheRecipe = new []
            {
                "Apicot Berry",  "Dragon Fang",  "Fossilized Drake", "Lansat Berry", "Rare Bone", "Sharp Beak", "Terrain Extender", "Thick Club",
                "Bright Powder", "Dragon Scale", "Heavy-Duty Boots", "Nugget",
            };
            var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Big Mushroom"], _inputItemOptions, alreadyChosenInputItems);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_Expected = namesOfInputItemsThatCanCompleteTheRecipe.Contains(item.Name);
                bool canCompleteRecipe_Actual   = filtererGroup.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(canCompleteRecipe_Expected, canCompleteRecipe_Actual, $"[{item.Name}]");
            }
        }

        [TestMethod]
        public void CanCompleteTheCometShardUsingInputItemsOfSeveralTypes()
        {
            var alreadyChosenInputItems = ItemNamesToItems("Rare Candy", "Rare Candy", "Weakness Policy");
            var namesOfInputItemsThatCanCompleteTheRecipe = new []
            {
                "Absorb Bulb",    "Dusk Stone",      "Grepa Berry",    "Lansat Berry",   "Odd Incense",    "Rawst Berry",    "Shed Shell",    "Sticky Barb",
                "Aspear Berry",   "Enigma Berry",    "Health Feather", "Leaf Stone",     "Persim Berry",   "Resist Feather", "Shuca Berry",   "Swift Feather",
                "Big Root",       "Fossilized Dino", "Hondew Berry",   "Lum Berry",      "Pomeg Berry",    "Rindo Berry",    "Silver Powder", "Tanga Berry",
                "Clever Feather", "Galarica Twig",   "Honey",          "Luminous Moss",  "Power Herb",     "Rose Incense",   "Snowball",      "Tiny Mushroom",
                "Coba Berry",     "Genius Feather",  "Ice Stone",      "Mago Berry",     "Pretty Feather", "Sharp Beak",     "Stardust",      "Yache Berry",
                "Custap Berry",   "Grassy Seed",     "Kasib Berry",    "Muscle Feather",
            };
            var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Comet Shard"], _inputItemOptions, alreadyChosenInputItems);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_Expected = namesOfInputItemsThatCanCompleteTheRecipe.Contains(item.Name);
                bool canCompleteRecipe_Actual   = filtererGroup.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(canCompleteRecipe_Expected, canCompleteRecipe_Actual, $"[{item.Name}]");
            }
        }

        [TestMethod]
        public void CannotCompleteThePPUpRecipeWithARareCandyWhenThe3AlreadyChosenInputsAreAllRareCandies()
        {
            var alreadyChosenInputItems = ItemNamesToItems("Rare Candy", "Rare Candy", "Rare Candy");
            var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["PP Up"], _inputItemOptions, alreadyChosenInputItems);
            Assert.IsFalse(filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName["Rare Candy"]));
        }

        [TestMethod]
        public void Given2AlreadyChosenInputsNeitherOfWhichIsWaterTypeAndHavingATotalValueOf33CanCompleteTheMysticWaterRecipeUsingAnApricornIfAndOnlyIfItIsABlueApricorn()
        {
            var alreadyChosenInputItems = ItemNamesToItems("Rare Candy", "Razor Claw");
            var apricornColourNames     = new [] {"Black", "Blue", "Green", "Pink", "Red", "White", "Yellow"};

            var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Mystic Water"], _inputItemOptions, alreadyChosenInputItems);

            foreach (string colourName in apricornColourNames)
            {
                bool canCompleteRecipe_Expected = colourName == "Blue";
                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAWaterTypeItem_Actual =
                    filtererGroup.ItemIsViableForAnyRecipe(Items.ItemsByName[$"{colourName} Apricorn"]);
                Assert.AreEqual(
                    canCompleteRecipe_Expected, canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAWaterTypeItem_Actual, $"[{colourName} Apricorn]");
            }
        }

        [TestMethod]
        public void CompletionOfAnAirBalloonRecipeWhenTheTotalValueOf3AlreadyChosenInputsIs25()
        {
            var alreadyChosenInputItems_IncludingAFlyingTypeItem    = ItemNamesToItems("Rare Candy", "Wacan Berry", "Swift Feather");
            var alreadyChosenInputItems_NotIncludingAFlyingTypeItem = ItemNamesToItems("Rare Candy", "Wacan Berry", "Dusk Stone"   );

            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAFlyingTypeItem = new []
            {
                "Ability Capsule", "Chople Berry",    "Exp. Candy XS",    "Grepa Berry",    "Leaf Stone",     "Passho Berry",   "Repel",          "Stardust",
                "Absorb Bulb",     "Clever Feather",  "Expert Belt",      "Ground Memory",  "Leppa Berry",    "Payapa Berry",   "Resist Feather", "Starf Berry",
                "Aguav Berry",     "Coba Berry",      "Fairy Memory",     "HP Up",          "Luck Incense",   "Pearl",          "Rindo Berry",    "Steel Memory",
                "Amulet Coin",     "Colbur Berry",    "Fighting Memory",  "Haban Berry",    "Lum Berry",      "Pecha Berry",    "Rock Incense",   "Sticky Barb",
                "Aspear Berry",    "Comet Shard",     "Figy Berry",       "Health Feather", "Luminous Moss",  "Persim Berry",   "Rock Memory",    "Sun Stone",
                "Babiri Berry",    "Custap Berry",    "Fire Memory",      "Hondew Berry",   "Mago Berry",     "Pixie Plate",    "Rose Incense",   "Super Repel",
                "Big Nugget",      "Dark Memory",     "Fire Stone",       "Honey",          "Maranga Berry",  "Poison Barb",    "Roseli Berry",   "Swift Feather",
                "Big Root",        "Dawn Stone",      "Float Stone",      "Iapapa Berry",   "Max Repel",      "Poison Memory",  "Rowap Berry",    "Tamato Berry",
                "Binding Band",    "Dragon Memory",   "Flying Memory",    "Ice Memory",     "Micle Berry",    "Pomeg Berry",    "Rusted Shield",  "Tanga Berry",
                "Black Sludge",    "Dubious Disc",    "Fossilized Bird",  "Ice Stone",      "Misty Seed",     "Power Herb",     "Rusted Sword",   "Thunder Stone",
                "Bottle Cap",      "Dusk Stone",      "Fossilized Dino",  "Iron",           "Moon Stone",     "Pretty Feather", "Safety Goggles", "Tiny Mushroom",
                "Bug Memory",      "Dynamax Candy",   "Fossilized Drake", "Jaboca Berry",   "Muscle Feather", "Protein",        "Sea Incense",    "Wacan Berry",
                "Calcium",         "Electric Memory", "Fossilized Fish",  "Kasib Berry",    "Mystic Water",   "Psychic Memory", "Sharp Beak",     "Water Memory",
                "Carbos",          "Electric Seed",   "Full Incense",     "Kebia Berry",    "Normal Gem",     "Psychic Seed",   "Shed Shell",     "Water Stone",
                "Charcoal",        "Enigma Berry",    "Galarica Twig",    "Kee Berry",      "Occa Berry",     "Pure Incense",   "Shiny Stone",    "Wave Incense",
                "Charti Berry",    "Eviolite",        "Genius Feather",   "Kelpsy Berry",   "Odd Incense",    "Qualot Berry",   "Shuca Berry",    "Wiki Berry",
                "Cheri Berry",     "Exp. Candy L",    "Ghost Memory",     "King's Rock",    "Oran Berry",     "Rare Candy",     "Silver Powder",  "Wise Glasses",
                "Chesto Berry",    "Exp. Candy M",    "Gold Bottle Cap",  "Lagging Tail",   "PP Max",         "Rawst Berry",    "Sitrus Berry",   "Yache Berry",
                "Chilan Berry",    "Exp. Candy S",    "Grass Memory",     "Lansat Berry",   "PP Up",          "Reaper Cloth",   "Snowball",       "Zinc",
                "Chipped Pot",     "Exp. Candy XL",   "Grassy Seed",      "Lax Incense",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem = new []
            {
                "Clever Feather", "Flying Memory",  "Grepa Berry",    "Lansat Berry", "Muscle Feather", "Resist Feather", "Sharp Beak", "Swift Feather",
                "Coba Berry",     "Genius Feather", "Health Feather", "Lum Berry",    "Pretty Feather",
            };

            var filtererGroup_AlreadyChosenInputItemsIncludeAFlyingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Air Balloon"], _inputItemOptions, alreadyChosenInputItems_IncludingAFlyingTypeItem);
            var filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Air Balloon"], _inputItemOptions, alreadyChosenInputItems_NotIncludingAFlyingTypeItem);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFlyingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAFlyingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFlyingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsIncludeAFlyingTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFlyingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFlyingTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items include a Flying-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFlyingTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items do not include a Flying-type item]"
                );
            }
        }

        [TestMethod]
        public void CompletionOfASnowballRecipeWhenTheTotalValueOf2AlreadyChosenInputsIs9()
        {
            var alreadyChosenInputItems_IncludingAnIceTypeItem    = ItemNamesToItems("Heat Rock", "Ice Stone" );
            var alreadyChosenInputItems_NotIncludingAnIceTypeItem = ItemNamesToItems("Heat Rock", "Dusk Stone");

            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAnIceTypeItem = new []
            {
                "Ability Capsule", "Cheri Berry",     "Exp. Candy XS",   "Ground Memory", "Oran Berry",    "Psychic Memory", "Rock Memory",      "Super Repel",
                "Amulet Coin",     "Chesto Berry",    "Expert Belt",     "Honey",         "PP Max",        "Pure Incense",   "Rocky Helmet",     "Sweet Apple",
                "Armorite Ore",    "Chipped Pot",     "Fairy Memory",    "Ice Memory",    "PP Up",         "Quick Claw",     "Room Service",     "Tart Apple",
                "Aspear Berry",    "Cleanse Tag",     "Fighting Memory", "King's Rock",   "Pearl String",  "Quick Powder",   "Rose Incense",     "Tiny Mushroom",
                "Balm Mushroom",   "Clover Sweet",    "Fire Memory",     "Lax Incense",   "Pecha Berry",   "Rare Candy",     "Rusted Shield",    "Upgrade",
                "Berry Sweet",     "Comet Shard",     "Flower Sweet",    "Leppa Berry",   "Persim Berry",  "Rawst Berry",    "Rusted Sword",     "Water Memory",
                "Big Nugget",      "Cracked Pot",     "Flying Memory",   "Love Sweet",    "Pink Apricorn", "Razor Claw",     "Sea Incense",      "Wave Incense",
                "Black Apricorn",  "Dark Memory",     "Full Incense",    "Luck Incense",  "Pixie Plate",   "Reaper Cloth",   "Sitrus Berry",     "Weakness Policy",
                "Blue Apricorn",   "Destiny Knot",    "Ghost Memory",    "Lum Berry",     "Poison Barb",   "Red Apricorn",   "Spell Tag",        "Whipped Dream",
                "Blunder Policy",  "Dragon Memory",   "Gold Bottle Cap", "Macho Brace",   "Poison Memory", "Repel",          "Star Sweet",       "White Apricorn",
                "Bottle Cap",      "Dubious Disc",    "Grass Memory",    "Max Repel",     "Prism Scale",   "Ribbon Sweet",   "Steel Memory",     "Wishing Piece",
                "Bug Memory",      "Electric Memory", "Green Apricorn",  "Metal Coat",    "Protector",     "Rock Incense",   "Strawberry Sweet", "Yellow Apricorn",
                "Charcoal",        "Eviolite",        "Grip Claw",       "Odd Incense",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem = new []
            {
                "Amulet Coin",    "Blue Apricorn",  "Cracked Pot",    "Grip Claw",   "Pearl String",  "Quick Powder", "Room Service",     "Upgrade",
                "Armorite Ore",   "Blunder Policy", "Destiny Knot",   "Ice Memory",  "Pink Apricorn", "Razor Claw",   "Spell Tag",        "Weakness Policy",
                "Aspear Berry",   "Charcoal",       "Dubious Disc",   "King's Rock", "Poison Barb",   "Reaper Cloth", "Star Sweet",       "Whipped Dream",
                "Balm Mushroom",  "Chipped Pot",    "Eviolite",       "Love Sweet",  "Prism Scale",   "Red Apricorn", "Strawberry Sweet", "White Apricorn",
                "Berry Sweet",    "Cleanse Tag",    "Expert Belt",    "Macho Brace", "Protector",     "Ribbon Sweet", "Sweet Apple",      "Wishing Piece",
                "Big Nugget",     "Clover Sweet",   "Flower Sweet",   "Metal Coat",  "Quick Claw",    "Rocky Helmet", "Tart Apple",       "Yellow Apricorn",
                "Black Apricorn", "Comet Shard",    "Green Apricorn", "PP Up",
            };

            var filtererGroup_AlreadyChosenInputItemsIncludeAnIceTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Snowball"], _inputItemOptions, alreadyChosenInputItems_IncludingAnIceTypeItem);
            var filtererGroup_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Snowball"], _inputItemOptions, alreadyChosenInputItems_NotIncludingAnIceTypeItem);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAnIceTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAnIceTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAnIceTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsIncludeAnIceTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAnIceTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAnIceTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items include an Ice-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAnIceTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items do not include an Ice-type item]"
                );
            }
        }

        [TestMethod]
        public void CompletionOfAStrawberrySweetRecipeWhenTheTotalValueOf3AlreadyChosenInputsIs50()
        {
            var alreadyChosenInputItems_IncludingAFairyTypeItem    = ItemNamesToItems("Rare Candy", "Rare Candy", "Sachet"    );
            var alreadyChosenInputItems_NotIncludingAFairyTypeItem = ItemNamesToItems("Rare Candy", "Rare Candy", "Light Clay");

            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAFairyTypeItem = new []
            {
                "Adamant Mint",   "Bright Powder", "Flame Orb",        "Impish Mint",  "Magnet",         "Petaya Berry",    "Red Apricorn", "Soothe Bell",
                "Adrenaline Orb", "Calm Mint",     "Focus Band",       "Iron Ball",    "Mental Herb",    "Pink Apricorn",   "Red Card",     "Star Piece",
                "Air Balloon",    "Careful Mint",  "Focus Sash",       "Jolly Mint",   "Metal Powder",   "Power Anklet",    "Relaxed Mint", "Terrain Extender",
                "Apicot Berry",   "Cell Battery",  "Galarica Cuff",    "Lax Mint",     "Mild Mint",      "Power Band",      "Ring Target",  "Thick Club",
                "Assault Vest",   "Choice Band",   "Ganlon Berry",     "Leek",         "Miracle Seed",   "Power Belt",      "Sachet",       "Throat Spray",
                "Big Mushroom",   "Choice Scarf",  "Gentle Mint",      "Leftovers",    "Modest Mint",    "Power Bracer",    "Salac Berry",  "Timid Mint",
                "Big Pearl",      "Choice Specs",  "Green Apricorn",   "Liechi Berry", "Muscle Band",    "Power Lens",      "Sassy Mint",   "Toxic Orb",
                "Black Apricorn", "Damp Rock",     "Hard Stone",       "Life Orb",     "Naive Mint",     "Power Weight",    "Scope Lens",   "Twisted Spoon",
                "Black Belt",     "Dragon Fang",   "Hasty Mint",       "Light Ball",   "Naughty Mint",   "Protective Pads", "Serious Mint", "Utility Umbrella",
                "Black Glasses",  "Dragon Scale",  "Heat Rock",        "Light Clay",   "Never-Melt Ice", "Quiet Mint",      "Shell Bell",   "White Apricorn",
                "Blue Apricorn",  "Eject Button",  "Heavy-Duty Boots", "Lonely Mint",  "Nugget",         "Rare Bone",       "Smooth Rock",  "White Herb",
                "Bold Mint",      "Eject Pack",    "Icy Rock",         "Lucky Egg",    "Oval Stone",     "Rash Mint",       "Soft Sand",    "Yellow Apricorn",
                "Brave Mint",     "Everstone",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem = new [] {"Pink Apricorn", "Bright Powder", "Sachet"};

            var filtererGroup_AlreadyChosenInputItemsIncludeAFairyTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Strawberry Sweet"], _inputItemOptions, alreadyChosenInputItems_IncludingAFairyTypeItem);
            var filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Strawberry Sweet"], _inputItemOptions, alreadyChosenInputItems_NotIncludingAFairyTypeItem);

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFairyTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsIncludeAFairyTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsIncludeAFairyTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsIncludeAFairyTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFairyTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsIncludeAFairyTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items include a Fairy-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem.ItemIsViableForAnyRecipe(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputItemsDoNotIncludeAFairyTypeItem_Actual,
                    $"[{item.Name}, already-chosen input items do not include a Fairy-type item]"
                );
            }
        }

        [TestMethod]
        public void CompletionOfAWishingPieceRecipeWhenTheTotalValueOf2Or3AlreadyChosenInputsIs21()
        {
            var alreadyChosenInputs_2Items_IncludingARockTypeItem =
                ItemNamesToItems("Rare Candy", "Rock Incense");
            var alreadyChosenInputs_2Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                ItemNamesToItems("Rare Candy", "Oran Berry");
            var alreadyChosenInputs_2Items_ConsistingOfPsychicTypeItemsOnly =
                ItemNamesToItems("Rare Candy", "Exp. Candy XS");
            var alreadyChosenInputs_3Items_IncludingARockTypeItem =
                ItemNamesToItems("Rare Candy", "Rock Incense", "White Apricorn");
            var alreadyChosenInputs_3Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                ItemNamesToItems("Rare Candy", "Exp. Candy XS", "Blue Apricorn");
            var alreadyChosenInputs_3Items_ConsistingOfPsychicTypeItemsOnly =
                ItemNamesToItems("Light Clay", "Light Clay", "Exp. Candy XS");

            var namesOfInputItemsThatCannotCompleteOneOfTheRecipes_2InputItemsAlreadyChosen_NotIncludingARockTypeItem = new []
            {
                "Air Balloon",  "Choice Specs", "Light Clay", "Metronome",    "Power Belt",   "Power Weight", "Smoke Ball",       "Wide Lens",
                "Assault Vest", "Leftovers",    "Lucky Egg",  "Power Anklet", "Power Bracer", "Sachet",       "Soft Sand",        "Wishing Piece",
                "Choice Band",  "Life Orb",     "Magnet",     "Power Band",   "Power Lens",   "Silk Scarf",   "Utility Umbrella", "Zoom Lens",
                "Choice Scarf", "Light Ball",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType = new []
            {
                "Absorb Bulb",    "Chople Berry",   "Float Stone",    "Iron",          "Max Repel",      "Pixie Plate",    "Rose Incense",  "Swift Feather",
                "Aguav Berry",    "Clever Feather", "Full Incense",   "Jaboca Berry",  "Misty Seed",     "Pomeg Berry",    "Roseli Berry",  "Tamato Berry",
                "Aspear Berry",   "Coba Berry",     "Galarica Twig",  "Kasib Berry",   "Moon Stone",     "Pretty Feather", "Rowap Berry",   "Tanga Berry",
                "Babiri Berry",   "Colbur Berry",   "Genius Feather", "Kebia Berry",   "Muscle Feather", "Protein",        "Rusted Shield", "Thunder Stone",
                "Big Root",       "Dawn Stone",     "Grassy Seed",    "Kee Berry",     "Normal Gem",     "Psychic Seed",   "Rusted Sword",  "Tiny Mushroom",
                "Binding Band",   "Dusk Stone",     "Green Apricorn", "Kelpsy Berry",  "Occa Berry",     "Pure Incense",   "Sea Incense",   "Wacan Berry",
                "Black Apricorn", "Dynamax Candy",  "Grepa Berry",    "Lax Incense",   "Odd Incense",    "Qualot Berry",   "Shiny Stone",   "Water Stone",
                "Blue Apricorn",  "Electric Seed",  "HP Up",          "Leaf Stone",    "Oran Berry",     "Rawst Berry",    "Shuca Berry",   "Wave Incense",
                "Calcium",        "Exp. Candy L",   "Haban Berry",    "Leppa Berry",   "Passho Berry",   "Red Apricorn",   "Sitrus Berry",  "White Apricorn",
                "Carbos",         "Exp. Candy M",   "Health Feather", "Luck Incense",  "Payapa Berry",   "Repel",          "Snowball",      "Wiki Berry",
                "Charti Berry",   "Exp. Candy S",   "Hondew Berry",   "Lum Berry",     "Pearl",          "Resist Feather", "Stardust",      "Yache Berry",
                "Cheri Berry",    "Exp. Candy XS",  "Honey",          "Luminous Moss", "Pecha Berry",    "Rindo Berry",    "Sun Stone",     "Yellow Apricorn",
                "Chesto Berry",   "Figy Berry",     "Iapapa Berry",   "Mago Berry",    "Persim Berry",   "Rock Incense",   "Super Repel",   "Zinc",
                "Chilan Berry",   "Fire Stone",     "Ice Stone",      "Maranga Berry", "Pink Apricorn",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly = new []
            {
                "Absorb Bulb",    "Chesto Berry",   "Genius Feather", "Jaboca Berry",  "Misty Seed",     "Pomeg Berry",    "Roseli Berry",  "Swift Feather",
                "Aguav Berry",    "Chople Berry",   "Grassy Seed",    "Kasib Berry",   "Moon Stone",     "Pretty Feather", "Rowap Berry",   "Tanga Berry",
                "Aspear Berry",   "Clever Feather", "Green Apricorn", "Kebia Berry",   "Muscle Feather", "Protein",        "Rusted Shield", "Thunder Stone",
                "Babiri Berry",   "Coba Berry",     "Grepa Berry",    "Kee Berry",     "Occa Berry",     "Qualot Berry",   "Rusted Sword",  "Tiny Mushroom",
                "Big Root",       "Colbur Berry",   "HP Up",          "Kelpsy Berry",  "Odd Incense",    "Rawst Berry",    "Sea Incense",   "Wacan Berry",
                "Binding Band",   "Dusk Stone",     "Haban Berry",    "Leaf Stone",    "Oran Berry",     "Red Apricorn",   "Shiny Stone",   "Water Stone",
                "Black Apricorn", "Dynamax Candy",  "Health Feather", "Leppa Berry",   "Passho Berry",   "Repel",          "Shuca Berry",   "Wave Incense",
                "Blue Apricorn",  "Electric Seed",  "Hondew Berry",   "Lum Berry",     "Pearl",          "Resist Feather", "Snowball",      "Wiki Berry",
                "Calcium",        "Figy Berry",     "Honey",          "Luminous Moss", "Pecha Berry",    "Rindo Berry",    "Stardust",      "Yache Berry",
                "Carbos",         "Fire Stone",     "Iapapa Berry",   "Mago Berry",    "Persim Berry",   "Rock Incense",   "Sun Stone",     "Yellow Apricorn",
                "Charti Berry",   "Float Stone",    "Ice Stone",      "Maranga Berry", "Pink Apricorn",  "Rose Incense",   "Super Repel",   "Zinc",
                "Cheri Berry",    "Galarica Twig",  "Iron",           "Max Repel",     "Pixie Plate",
            };

            var filtererGroup_2InputItemsAlreadyChosen_IncludingARockTypeItem =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_IncludingARockTypeItem
                );
            var filtererGroup_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                );
            var filtererGroup_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_ConsistingOfPsychicTypeItemsOnly
                );
            var filtererGroup_3InputItemsAlreadyChosen_IncludingARockTypeItem =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_IncludingARockTypeItem
                );
            var filtererGroup_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                );
            var filtererGroup_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_ConsistingOfPsychicTypeItemsOnly
                );

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem =
                    ! namesOfInputItemsThatCannotCompleteOneOfTheRecipes_2InputItemsAlreadyChosen_NotIncludingARockTypeItem.Contains(item.Name);
                bool canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .Contains(item.Name);
                bool canCompleteRecipe_Expected_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .Contains(item.Name);

                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_IncludingARockTypeItem =
                    filtererGroup_2InputItemsAlreadyChosen_IncludingARockTypeItem
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    filtererGroup_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    filtererGroup_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_IncludingARockTypeItem =
                    filtererGroup_3InputItemsAlreadyChosen_IncludingARockTypeItem
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    filtererGroup_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    filtererGroup_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .ItemIsViableForAnyRecipe(item);

                Assert.AreNotEqual(
                    item.Name == "Wishing Piece",
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_IncludingARockTypeItem,
                    $"[{item.Name}, 2 already-chosen input items including a Rock-type item]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem,
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    $"[{item.Name}, 2 already-chosen input items not including a Rock-type item but including an item that is neither Normal-type nor Psychic-type]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem,
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    $"[{item.Name}, 2 already-chosen input items consisting of Psychic-type items only]"
                );

                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_IncludingARockTypeItem,
                    $"[{item.Name}, 3 already-chosen input items including a Rock-type item]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    $"[{item.Name}, 3 already-chosen input items not including a Rock-type item but including an item that is neither Normal-type nor Psychic-type]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    $"[{item.Name}, 3 already-chosen input items consisting of Psychic-type items only]"
                );
            }
        }

        [TestMethod]
        public void CompletionOfAWishingPieceRecipeWhenTheTotalValueOf2Or3AlreadyChosenInputsIs25()
        {
            var alreadyChosenInputs_2Items_IncludingARockTypeItem =
                ItemNamesToItems("Rare Candy", "Lagging Tail");
            var alreadyChosenInputs_2Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                ItemNamesToItems("Rare Candy", "Black Sludge");
            var alreadyChosenInputs_2Items_ConsistingOfPsychicTypeItemsOnly =
                ItemNamesToItems("Rare Candy", "Exp. Candy XL");
            var alreadyChosenInputs_3Items_IncludingARockTypeItem =
                ItemNamesToItems("Rare Candy", "Lagging Tail", "White Apricorn");
            var alreadyChosenInputs_3Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                ItemNamesToItems("Rare Candy", "Exp. Candy XL", "Blue Apricorn");
            var alreadyChosenInputs_3Items_ConsistingOfPsychicTypeItemsOnly =
                ItemNamesToItems("Rare Candy", "Exp. Candy S", "Exp. Candy M");

            var namesOfInputItemsThatCannotCompleteOneOfTheRecipes_2InputItemsAlreadyChosen_NotIncludingARockTypeItem = new []
            {
                "Adrenaline Orb", "Black Glasses", "Focus Band",   "Icy Rock",     "Miracle Seed",   "Protective Pads", "Scope Lens",       "Throat Spray",
                "Apicot Berry",   "Bright Powder", "Focus Sash",   "Liechi Berry", "Muscle Band",    "Rare Bone",       "Shell Bell",       "Toxic Orb",
                "Big Mushroom",   "Cell Battery",  "Ganlon Berry", "Mental Herb",  "Never-Melt Ice", "Ring Target",     "Soothe Bell",      "White Herb",
                "Big Pearl",      "Damp Rock",     "Heat Rock",    "Metal Powder", "Petaya Berry",   "Salac Berry",     "Terrain Extender", "Wishing Piece",
                "Black Belt",     "Flame Orb",
            };
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                new [] {"Black Apricorn", "Blue Apricorn", "Green Apricorn", "Pink Apricorn", "Red Apricorn", "White Apricorn", "Yellow Apricorn"};
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                new [] {"Black Apricorn", "Blue Apricorn", "Green Apricorn", "Pink Apricorn", "Red Apricorn", "Yellow Apricorn"};

            var filtererGroup_2InputItemsAlreadyChosen_IncludingARockTypeItem =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_IncludingARockTypeItem
                );
            var filtererGroup_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                );
            var filtererGroup_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_2Items_ConsistingOfPsychicTypeItemsOnly
                );
            var filtererGroup_3InputItemsAlreadyChosen_IncludingARockTypeItem =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_IncludingARockTypeItem
                );
            var filtererGroup_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                );
            var filtererGroup_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                new StandardRecipeGroupItemFilterer(
                    Recipes.StandardRecipes["Wishing Piece"],
                    _inputItemOptions,
                    alreadyChosenInputs_3Items_ConsistingOfPsychicTypeItemsOnly
                );

            foreach (var item in Items.InputItems())
            {
                bool canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem =
                    ! namesOfInputItemsThatCannotCompleteOneOfTheRecipes_2InputItemsAlreadyChosen_NotIncludingARockTypeItem.Contains(item.Name);
                bool canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .Contains(item.Name);
                bool canCompleteRecipe_Expected_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .Contains(item.Name);

                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_IncludingARockTypeItem =
                    filtererGroup_2InputItemsAlreadyChosen_IncludingARockTypeItem
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    filtererGroup_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    filtererGroup_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_IncludingARockTypeItem =
                    filtererGroup_3InputItemsAlreadyChosen_IncludingARockTypeItem
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType =
                    filtererGroup_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType
                        .ItemIsViableForAnyRecipe(item);
                bool canCompleteRecipe_Actual_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly =
                    filtererGroup_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly
                        .ItemIsViableForAnyRecipe(item);

                Assert.AreNotEqual(
                    item.Name == "Wishing Piece",
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_IncludingARockTypeItem,
                    $"[{item.Name}, 2 already-chosen input items including a Rock-type item]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem,
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    $"[{item.Name}, 2 already-chosen input items not including a Rock-type item but including an item that is neither Normal-type nor Psychic-type]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_2InputItemsAlreadyChosen_NotIncludingARockTypeItem,
                    canCompleteRecipe_Actual_2InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    $"[{item.Name}, 2 already-chosen input items consisting of Psychic-type items only]"
                );

                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_IncludingARockTypeItem,
                    $"[{item.Name}, 3 already-chosen input items including a Rock-type item]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_IncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_NotIncludingARockTypeItemButIncludingAnItemThatIsNeitherNormalTypeNorPsychicType,
                    $"[{item.Name}, 3 already-chosen input items not including a Rock-type item but including an item that is neither Normal-type nor Psychic-type]"
                );
                Assert.AreEqual(
                    canCompleteRecipe_Expected_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    canCompleteRecipe_Actual_3InputItemsAlreadyChosen_ConsistingOfPsychicTypeItemsOnly,
                    $"[{item.Name}, 3 already-chosen input items consisting of Psychic-type items only]"
                );
            }
        }
    }
}
