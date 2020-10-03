﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Components;
using CramIt.Core;

namespace CramIt.Site.Pages
{
    public abstract class IndexBase: ComponentBase
    {
        public const int NumberOfItemsPerBatch = 4;

        protected Mode Mode { get; set; }

        protected Item TargetItem { get; set; }

        protected IReadOnlyList<StandardRecipe> TargetRecipes { get; set; }

        private Item[] _inputItemSlots;
        protected IReadOnlyList<Item> InputItemSlots
            => _inputItemSlots;

        private IEnumerable<Item> AlreadyChosenInputItems
            => InputItemSlots.Where(slot => ! (slot is null));
        private int NumberOfFreeItemSlots
            => InputItemSlots.Count(slot => slot is null);
        private bool AnyFreeInputItemSlots
            => InputItemSlots.Any(slot => slot is null);
        private int FirstFreeInputItemSlotIndex
            => AnyFreeInputItemSlots ? Enumerable.Range(0, NumberOfItemsPerBatch).First(i => InputItemSlots[i] is null) : -1;

        protected StandardRecipeGroupItemFilterer StandardRecipeGroupItemFilterer { get; set; }

        protected IndexBase()
        {
            Restart();
        }

        protected void Restart()
        {
            Mode = Mode.SelectingDesiredOutput;
            ClearChosenItems();
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

            ClearChosenItems();
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

            _inputItemSlots[FirstFreeInputItemSlotIndex] = inputItem;

            EnforceOrderOfChosenItems();
            SetModeAccordingToFreeItemSlots();
        }

        protected void InputItemUnchosen(int slotIndex)
        {
            Debug.Assert(slotIndex >= 0);
            Debug.Assert(slotIndex < NumberOfItemsPerBatch);

            Debug.Assert( ! (InputItemSlots[slotIndex] is null));
            _inputItemSlots[slotIndex] = null;

            EnforceOrderOfChosenItems();
            SetModeAccordingToFreeItemSlots();
        }

        private void ClearChosenItems()
        {
            _inputItemSlots = new Item[NumberOfItemsPerBatch];
        }

        private void EnforceOrderOfChosenItems()
        {
            var reorderedNonNullSlots = _inputItemSlots
                .Where(slot => ! (slot is null))
                .OrderBy(slot => StandardRecipeGroupItemFilterer.ItemIsOfPlacatoryTypeForAllRecipes(slot) ? 0 : 1)
                .ThenBy (slot => StandardRecipeGroupItemFilterer.ItemIsOfPlacatoryTypeForAnyRecipe (slot) ? 0 : 1)
                .ToArray();

            _inputItemSlots = reorderedNonNullSlots.Concat(Enumerable.Repeat<Item>(null, NumberOfFreeItemSlots)).ToArray();
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
    }
}
