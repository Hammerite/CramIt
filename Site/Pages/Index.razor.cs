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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CramIt.Core;

namespace CramIt.Site.Pages
{
    public abstract class IndexBase: ComponentBase
    {
        public const int NumberOfItemsPerBatch = 4;

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        protected bool SettingOptions { get; set; }
        protected string OptionsAndInfoButtonLabel
            => SettingOptions ? "Done" : "Options/Info";

        protected Mode Mode { get; set; }

        protected InputItemOptions InputItemOptions         { get; set; } = new InputItemOptions();
        protected InputItemOptions PreviousInputItemOptions { get; set; }

        protected TargetItemCategory TargetItemCategory { get; set; } = TargetItemCategory.NotTR;

        protected Item TargetItem { get; set; }

        protected IReadOnlyList<StandardRecipe> TargetRecipes { get; set; }

        private Item[] _inputItemSlots;
        protected IReadOnlyList<Item> InputItemSlots
            => _inputItemSlots;

        private IEnumerable<Item> AlreadyChosenInputItems
            => InputItemSlots.Where(slot => ! (slot is null));
        private int NumberOfFreeItemSlots
            => InputItemSlots.Count(slot => slot is null);
        protected bool AnyInputItemsChosen
            => ! InputItemSlots.All(slot => slot is null);
        private bool AnyFreeInputItemSlots
            => InputItemSlots.Any(slot => slot is null);
        private int FirstFreeInputItemSlotIndex
            => AnyFreeInputItemSlots ? Enumerable.Range(0, NumberOfItemsPerBatch).First(i => InputItemSlots[i] is null) : -1;

        protected int AlreadyChosenInputItemsTotalValue
            => AlreadyChosenInputItems.Sum(slot => slot.Value);

        protected StandardRecipeGroupItemFilterer StandardRecipeGroupItemFilterer { get; set; }

        protected IndexBase()
        {
            Restart();
        }

        private void Restart()
        {
            Mode = Mode.SelectingDesiredOutput;
            ClearChosenItems();
        }

        protected void ToggleSettingOptions()
        {
            SettingOptions = ! SettingOptions;

            if (SettingOptions)
            {
                PreviousInputItemOptions = InputItemOptions.Clone();
            }
            else if (InputItemOptions != PreviousInputItemOptions)
            {
                Restart();
            }
        }

        protected bool Option_IncludeIrreplaceableInputItems
        {
            get => InputItemOptions.IncludeIrreplaceableItems;
            set
            {
                InputItemOptions.IncludeIrreplaceableItems = value;
            }
        }

        protected bool Option_CombineGroupsOfSimilarInputItems
        {
            get => InputItemOptions.CombineGroupsOfSimilarItems;
            set
            {
                InputItemOptions.CombineGroupsOfSimilarItems = value;
            }
        }

        protected TROrdering        Option_TROrdering        { get; set; } = TROrdering.ByMoveName;
        protected InputItemOrdering Option_InputItemOrdering { get; set; } = InputItemOrdering.ByName;

        protected void RecipeClicked(StandardRecipe recipe)
            => RecipeClicked(new [] {recipe});

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
            StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes, InputItemOptions);
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

        protected string InputItemValueContributionDivStyle_Inner(Item inputItem)
        {
            Debug.Assert(TargetRecipes.Count == 1);
            Debug.Assert(AlreadyChosenInputItemsTotalValue <= TargetRecipes[0].MaximumTotalValue);

            if (AlreadyChosenInputItemsTotalValue == TargetRecipes[0].MaximumTotalValue)
            {
                return "height: 0%;";
            }

            double differenceBetweenCurrentTotalAndMaximum = TargetRecipes[0].MaximumTotalValue - AlreadyChosenInputItemsTotalValue;

            double proportionateHeight = inputItem.Value / differenceBetweenCurrentTotalAndMaximum;
            string heightAsString = (100 * proportionateHeight).ToString("F1", CultureInfo.InvariantCulture);

            return $"height: {heightAsString}%;";
        }

        protected string InputItemValueContributionDivStyle_MinimumMarker()
        {
            Debug.Assert(TargetRecipes.Count == 1);
            Debug.Assert(AlreadyChosenInputItemsTotalValue < TargetRecipes[0].MinimumTotalValue);

            double differenceBetweenCurrentTotalAndMaximum = TargetRecipes[0].MaximumTotalValue - AlreadyChosenInputItemsTotalValue;
            double differenceBetweenMinimumAndMaximum      = TargetRecipes[0].MaximumTotalValue - TargetRecipes[0].MinimumTotalValue;

            double proportionateHeight = 1.0 - (differenceBetweenMinimumAndMaximum / differenceBetweenCurrentTotalAndMaximum);
            string heightAsString = (100 * proportionateHeight).ToString("F1", CultureInfo.InvariantCulture);

            return $"height: {heightAsString}%;";
        }

        protected async Task CopyToClipboard()
            => await JSRuntime.InvokeAsync<string>("navigator.clipboard.writeText", new [] {BuildTextForClipboard()});

        private string BuildTextForClipboard()
        {
            string inputItemList = string.Join(" + ", InputItemSlots.Select(slot => slot.ToString(InputItemOptions.CombineGroupsOfSimilarItems)));
            return $"{TargetItem} = {inputItemList}";
        }

        protected void OnClear()
        {
            ClearChosenItems();
            StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes, InputItemOptions);
            SetModeAccordingToFreeItemSlots();
        }

        protected void OnRestart()
            => Restart();

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

            MoveIdenticalItemsAdjacent();
        }

        private void MoveIdenticalItemsAdjacent()
        {
            for (int i = 0; i < NumberOfItemsPerBatch - 1; ++i)
            {
                if (_inputItemSlots[i + 1] == _inputItemSlots[i])
                {
                    continue;
                }

                for (int j = i + 2; j < NumberOfItemsPerBatch; ++j)
                {
                    if (_inputItemSlots[j] == _inputItemSlots[i])
                    {
                        if (i == 0 && j == 3)
                        {
                            Swap(ref _inputItemSlots[3], ref _inputItemSlots[2]);
                            Swap(ref _inputItemSlots[2], ref _inputItemSlots[1]);
                        }
                        else
                        {
                            Swap(ref _inputItemSlots[i + 1], ref _inputItemSlots[j]);
                        }
                        break;
                    }
                }
            }
        }

        private void Swap<T>(ref T t0, ref T t1)
        {
            var temporary = t0;
            t0 = t1;
            t1 = temporary;
        }

        private void SetModeAccordingToFreeItemSlots()
        {
            if (AnyFreeInputItemSlots)
            {
                StandardRecipeGroupItemFilterer = new StandardRecipeGroupItemFilterer(TargetRecipes, InputItemOptions, AlreadyChosenInputItems);
                Mode = Mode.SelectingInputs;
            }
            else
            {
                Mode = Mode.SelectionComplete;
            }
        }
    }
}
