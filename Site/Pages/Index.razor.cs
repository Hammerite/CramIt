using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Components;
using CramIt.Core;

namespace CramIt.Site.Pages
{
    public class IndexBase: ComponentBase
    {
        public const int NumberOfItemsPerBatch = 4;

        protected Mode Mode { get; set; } = Mode.SelectingDesiredOutput;

        protected Item TargetItem { get; set; }
        protected IReadOnlyList<StandardRecipe> TargetRecipes { get; set; }

        protected Item[] InputItemSlots { get; } = new Item[NumberOfItemsPerBatch];
        private IEnumerable<Item> AlreadyChosenInputItems
            => InputItemSlots.Where(slot => ! (slot is null));
        private bool AnyFreeInputItemSlots
            => InputItemSlots.Any(slot => slot is null);
        private int FirstFreeInputItemSlotIndex
            => AnyFreeInputItemSlots ? Enumerable.Range(0, NumberOfItemsPerBatch).First(i => InputItemSlots[i] is null) : -1;

        protected StandardRecipeGroupItemFilterer StandardRecipeGroupItemFilterer { get; set; }

        protected void Restart()
        {
            Mode = Mode.SelectingDesiredOutput;
        }

        protected void RecipeClicked(IReadOnlyList<StandardRecipe> recipeList)
        {
            Debug.Assert(Mode == Mode.SelectingDesiredOutput);

            if (recipeList is null)
            {
                throw new ArgumentNullException(nameof(recipeList));
            }
            if (recipeList.Any(r => r is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(recipeList));
            }

            Mode          = Mode.SelectingInputs;
            TargetItem    = recipeList.First().Item;
            TargetRecipes = recipeList;

            Array.Clear(InputItemSlots, 0, NumberOfItemsPerBatch);
            StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes);
        }

        protected void InputItemClicked(Item inputItem)
        {
            Debug.Assert(Mode == Mode.SelectingInputs);

            if (inputItem is null)
            {
                throw new ArgumentNullException(nameof(inputItem));
            }

            Debug.Assert(inputItem.CanBeInput);
            Debug.Assert(AnyFreeInputItemSlots);

            InputItemSlots[FirstFreeInputItemSlotIndex] = inputItem;

            if (AnyFreeInputItemSlots)
            {
                StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes, AlreadyChosenInputItems);
            }
            else
            {
                Mode = Mode.SelectionComplete;
            }
        }
    }
}
