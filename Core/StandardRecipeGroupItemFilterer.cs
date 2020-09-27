using System;
using System.Collections.Generic;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipeGroupItemFilterer
    {
        public StandardRecipeGroupItemFilterer(IEnumerable<StandardRecipe> targetRecipes): this(targetRecipes, new Item[0])
        {}

        public StandardRecipeGroupItemFilterer(IEnumerable<StandardRecipe> targetRecipes, IEnumerable<Item> alreadyChosenInputs)
        {
            if (targetRecipes is null)
            {
                throw new ArgumentNullException(nameof(targetRecipes));
            }

            _itemFilterers = targetRecipes.Select(r => new StandardRecipeItemFilterer(r, alreadyChosenInputs)).ToList();
        }

        private IReadOnlyList<StandardRecipeItemFilterer> _itemFilterers;

        public bool TypedInputRequiredForAnyRecipe
            => _itemFilterers.Any(f => f.TypedInputRequired);

        public bool ItemIsViableForAnyRecipe(Item item)
            => _itemFilterers.Any(f => f.ItemIsViable(item));

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
