using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CramIt.Core;

namespace Tests
{
    [TestClass]
    public class StandardRecipeItemFiltererTests
    {
        [TestMethod]
        public void CannotCompleteRareCandyRecipeUsingApricorn()
        {
            var filterer = new StandardRecipeItemFilterer(Recipes.StandardRecipes["Rare Candy"].First());
            Assert.IsFalse(filterer.CanCompleteRecipeUsingItem(Items.ItemsByName["Black Apricorn"]));
        }
    }
}
