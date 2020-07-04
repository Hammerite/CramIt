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
                var filtererGroup = new StandardRecipeGroupItemFilterer(recipesList);

                foreach (string colourName in apricornColourNames)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.CanCompleteAnyRecipeUsingItem(Items.ItemsByName[$"{colourName} Apricorn"]);
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
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName]);

                foreach (string inputItemName in namesOfValue11OrAboveInputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.CanCompleteAnyRecipeUsingItem(Items.ItemsByName[inputItemName]);
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
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName]);

                foreach (string inputItemName in namesOfValue16OrAboveInputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.CanCompleteAnyRecipeUsingItem(Items.ItemsByName[inputItemName]);
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
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName]);

                foreach (string inputItemName in namesOfValue20InputItems)
                {
                    bool canCompleteRecipe_Actual = filtererGroup.CanCompleteAnyRecipeUsingItem(Items.ItemsByName[inputItemName]);
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
                var filtererGroup = new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes[recipeItemName]);

                foreach (string inputItemName in namesOfValue20InputItems)
                {
                    bool canCompleteRecipe_Expected = compatibleValue20InputItems.Contains(inputItemName);
                    bool canCompleteRecipe_Actual   = filtererGroup.CanCompleteAnyRecipeUsingItem(Items.ItemsByName[inputItemName]);
                    Assert.AreEqual(canCompleteRecipe_Expected, canCompleteRecipe_Actual, $"[{recipeItemName}, {inputItemName}]");
                }
            }
        }

        [TestMethod]
        public void CanCompleteEitherAirBalloonRecipeWhenTotalValueOf3AlreadyChosenInputsIs25()
        {
            var alreadyChosenInputs_IncludingAFlyingTypeItem =
                new [] {"Rare Candy", "Wacan Berry", "Swift Feather"}.Select(itemName => Items.ItemsByName[itemName]);
            var alreadyChosenInputs_NotIncludingAFlyingTypeItem =
                new [] {"Rare Candy", "Wacan Berry", "Dusk Stone"}.Select(itemName => Items.ItemsByName[itemName]);

            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsIncludeAFlyingTypeItem = new []
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
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem = new []
            {
                "Clever Feather", "Flying Memory",  "Grepa Berry",    "Lansat Berry", "Muscle Feather", "Resist Feather", "Sharp Beak", "Swift Feather",
                "Coba Berry",     "Genius Feather", "Health Feather", "Lum Berry",    "Pretty Feather",
            };

            var filtererGroup_AlreadyChosenInputsIncludeAFlyingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Air Balloon"], alreadyChosenInputs_IncludingAFlyingTypeItem);
            var filtererGroup_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Air Balloon"], alreadyChosenInputs_NotIncludingAFlyingTypeItem);

            foreach (var item in Items.InputItems)
            {
                bool canCompleteRecipe_AlreadyChosenInputsIncludeAFlyingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsIncludeAFlyingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputsIncludeAFlyingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputsIncludeAFlyingTypeItem.CanCompleteAnyRecipeUsingItem(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputsIncludeAFlyingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputsIncludeAFlyingTypeItem_Actual,
                    $"[{item.Name}, already chosen inputs include a Flying-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem.CanCompleteAnyRecipeUsingItem(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAFlyingTypeItem_Actual,
                    $"[{item.Name}, already chosen inputs do not include a Flying-type item]"
                );
            }
        }

        [TestMethod]
        public void CanCompleteEitherSnowballRecipeWhenTotalValueOf2AlreadyChosenInputsIs9()
        {
            var alreadyChosenInputs_IncludingAnIceTypeItem    = new [] {"Heat Rock", "Ice Stone" }.Select(itemName => Items.ItemsByName[itemName]);
            var alreadyChosenInputs_NotIncludingAnIceTypeItem = new [] {"Heat Rock", "Dusk Stone"}.Select(itemName => Items.ItemsByName[itemName]);

            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsIncludeAnIceTypeItem = new []
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
            var namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsDoNotIncludeAnIceTypeItem = new []
            {
                "Ability Capsule", "Bottle Cap",   "Destiny Knot",    "Flower Sweet",    "King's Rock",   "Poison Memory",  "Red Apricorn",     "Sweet Apple",
                "Amulet Coin",     "Bug Memory",   "Dragon Memory",   "Flying Memory",   "Love Sweet",    "Prism Scale",    "Ribbon Sweet",     "Tart Apple",
                "Armorite Ore",    "Charcoal",     "Dubious Disc",    "Ghost Memory",    "Macho Brace",   "Protector",      "Rock Memory",      "Upgrade",
                "Aspear Berry",    "Chipped Pot",  "Electric Memory", "Gold Bottle Cap", "Metal Coat",    "Psychic Memory", "Rocky Helmet",     "Water Memory",
                "Balm Mushroom",   "Cleanse Tag",  "Eviolite",        "Grass Memory",    "PP Max",        "Quick Claw",     "Room Service",     "Weakness Policy",
                "Berry Sweet",     "Clover Sweet", "Expert Belt",     "Green Apricorn",  "PP Up",         "Quick Powder",   "Spell Tag",        "Whipped Dream",
                "Big Nugget",      "Comet Shard",  "Fairy Memory",    "Grip Claw",       "Pearl String",  "Rare Candy",     "Star Sweet",       "White Apricorn",
                "Black Apricorn",  "Cracked Pot",  "Fighting Memory", "Ground Memory",   "Pink Apricorn", "Razor Claw",     "Steel Memory",     "Wishing Piece",
                "Blue Apricorn",   "Dark Memory",  "Fire Memory",     "Ice Memory",      "Poison Barb",   "Reaper Cloth",   "Strawberry Sweet", "Yellow Apricorn",
                "Blunder Policy",
            };

            var filtererGroup_AlreadyChosenInputsIncludeAnIceTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Snowball"], alreadyChosenInputs_IncludingAnIceTypeItem);
            var filtererGroup_AlreadyChosenInputsDoNotIncludeAnIceTypeItem =
                new StandardRecipeGroupItemFilterer(Recipes.StandardRecipes["Snowball"], alreadyChosenInputs_NotIncludingAnIceTypeItem);

            foreach (var item in Items.InputItems)
            {
                bool canCompleteRecipe_AlreadyChosenInputsIncludeAnIceTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsIncludeAnIceTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputsIncludeAnIceTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputsIncludeAnIceTypeItem.CanCompleteAnyRecipeUsingItem(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputsIncludeAnIceTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputsIncludeAnIceTypeItem_Actual,
                    $"[{item.Name}, already chosen inputs include an Ice-type item]"
                );

                bool canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAnIceTypeItem_Expected =
                    namesOfInputItemsThatCanCompleteOneOfTheRecipes_AlreadyChosenInputsDoNotIncludeAnIceTypeItem.Contains(item.Name);
                bool canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAnIceTypeItem_Actual =
                    filtererGroup_AlreadyChosenInputsDoNotIncludeAnIceTypeItem.CanCompleteAnyRecipeUsingItem(item);
                Assert.AreEqual(
                    canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAnIceTypeItem_Expected,
                    canCompleteRecipe_AlreadyChosenInputsDoNotIncludeAnIceTypeItem_Actual,
                    $"[{item.Name}, already chosen inputs do not include an Ice-type item]"
                );
            }
        }
    }
}
