using System;
using System.Collections.Generic;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipeGroupItemFilterer
    {
        public StandardRecipeGroupItemFilterer(IEnumerable<StandardRecipe> targetRecipes, IReadOnlyList<Item> alreadyChosenInputs)
        {
            if (targetRecipes is null)
            {
                throw new ArgumentNullException(nameof(targetRecipes));
            }

            _itemFilterers = targetRecipes.Select(r => new StandardRecipeItemFilterer(r, alreadyChosenInputs)).ToList();
        }

        private IReadOnlyList<StandardRecipeItemFilterer> _itemFilterers;

        public bool CanCompleteAnyRecipeUsingItem(Item item)
            => _itemFilterers.Any(f => f.CanCompleteRecipeUsingItem(item));
    }
}
