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

namespace CramIt.Core
{
    public class StandardRecipeGroupItemFilterer
    {
        public StandardRecipeGroupItemFilterer(IEnumerable<StandardRecipe> targetRecipes, InputItemOptions inputItemOptions):
            this(targetRecipes, inputItemOptions, new Item[0])
        {}

        public StandardRecipeGroupItemFilterer(IEnumerable<StandardRecipe> targetRecipes, InputItemOptions inputItemOptions, IEnumerable<Item> alreadyChosenInputs)
        {
            if (targetRecipes is null)
            {
                throw new ArgumentNullException(nameof(targetRecipes));
            }

            _itemFilterers = targetRecipes.Select(r => new StandardRecipeItemFilterer(r, inputItemOptions, alreadyChosenInputs)).ToList();
        }

        private IReadOnlyList<StandardRecipeItemFilterer> _itemFilterers;

        public bool TypedInputRequiredForAnyRecipe
            => _itemFilterers.Any(f => f.TypedInputRequired);

        public bool ItemIsViableForAnyRecipe(Item item)
            => _itemFilterers.Any(f => f.ItemIsViable(item));

        public bool ItemIsOfPlacatoryTypeForAnyRecipe(Item item)
            => _itemFilterers.Any(f => f.ItemIsOfPlacatoryType(item));

        public bool ItemIsOfPlacatoryTypeForAllRecipes(Item item)
            => _itemFilterers.All(f => f.ItemIsOfPlacatoryType(item));

        public InputItemViabilityCategory BestViabilityCategory(Item item)
        {
            bool anyViable = false;

            foreach (var filterer in _itemFilterers)
            {
                var viabilityCategory = filterer.ViabilityCategory(item);

                if (viabilityCategory == InputItemViabilityCategory.Viable_OfPlacatoryType)
                {
                    return InputItemViabilityCategory.Viable_OfPlacatoryType;
                }
                anyViable |= viabilityCategory != InputItemViabilityCategory.Nonviable;
            }

            return anyViable ? InputItemViabilityCategory.Viable_NotOfPlacatoryType : InputItemViabilityCategory.Nonviable;
        }
    }
}
