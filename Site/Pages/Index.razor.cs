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

        protected void InputItemChosen(Item inputItem)
        {
            Debug.Assert(Mode == Mode.SelectingInputs);

            if (inputItem is null)
            {
                throw new ArgumentNullException(nameof(inputItem));
            }

            Debug.Assert(inputItem.CanBeInput);
            Debug.Assert(AnyFreeInputItemSlots);

            if ( ! StandardRecipeGroupItemFilterer.ItemIsViableForAnyRecipe(inputItem))
            {
                return;
            }

            InputItemSlots[FirstFreeInputItemSlotIndex] = inputItem;

            SetModeAccordingToFreeItemSlots();
            ReorderChosenItems();
        }

        protected void InputItemUnchosen(int slotIndex)
        {
            Debug.Assert(slotIndex >= 0);
            Debug.Assert(slotIndex < NumberOfItemsPerBatch);

            Debug.Assert( ! (InputItemSlots[slotIndex] is null));
            InputItemSlots[slotIndex] = null;

            SetModeAccordingToFreeItemSlots();
            ReorderChosenItems();
        }

        private void SetModeAccordingToFreeItemSlots()
        {
            if (AnyFreeInputItemSlots)
            {
                StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes, AlreadyChosenInputItems);
                Mode = Mode.SelectingInputs;
            }
            else
            {
                Mode = Mode.SelectionComplete;
            }
        }

        private void ReorderChosenItems()
        {
            MoveItemsEarlierWhen(slot => ! (slot is null), NumberOfItemsPerBatch);

            MoveNonNullItemsEarlierWhen(slot => StandardRecipeGroupItemFilterer.ItemIsOfPlacatoryTypeForAllRecipes(slot));
            MoveNonNullItemsEarlierWhen(slot => StandardRecipeGroupItemFilterer.ItemIsOfPlacatoryTypeForAnyRecipe (slot));
        }

        private void MoveNonNullItemsEarlierWhen(Predicate<Item> p)
            => MoveItemsEarlierWhen(p, AlreadyChosenInputItems.Count());

        private void MoveItemsEarlierWhen(Predicate<Item> p, int numberOfSlots)
        {
            for (int i = 0; i < numberOfSlots; ++i)
            {
                if ( ! p(InputItemSlots[i]))
                {
                    for (int j = i + 1; j < numberOfSlots; ++j)
                    {
                        if (p(InputItemSlots[j]))
                        {
                            for (int k = j; k > i; --k)
                            {
                                Swap(ref InputItemSlots[k], ref InputItemSlots[k - 1]);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void Swap<T>(ref T t0, ref T t1)
        {
            T temp = t0;
            t0 = t1;
            t1 = temp;
        }
    }
}
