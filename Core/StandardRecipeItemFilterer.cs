using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipeItemFilterer
    {
        public const int NumberOfItemsPerBatch = 4;

        public StandardRecipeItemFilterer(StandardRecipe targetRecipe, IReadOnlyList<Item> alreadyChosenInputs)
        {
            if (targetRecipe is null)
            {
                throw new ArgumentNullException(nameof(targetRecipe));
            }

            if (alreadyChosenInputs is null)
            {
                throw new ArgumentNullException(nameof(alreadyChosenInputs));
            }
            if (alreadyChosenInputs.Any(input => input is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(alreadyChosenInputs));
            }
            if (alreadyChosenInputs.Any(input => ! input.CanBeInput))
            {
                throw new ArgumentException("Elements must be usable as input", nameof(alreadyChosenInputs));
            }
            if (alreadyChosenInputs.Count >= NumberOfItemsPerBatch)
            {
                throw new ArgumentException($"Must contain fewer than {NumberOfItemsPerBatch} elements", nameof(alreadyChosenInputs));
            }

            _typedInputRequired = ! alreadyChosenInputs.Any(input => targetRecipe.Types.Contains(input.Type));
            if (_typedInputRequired)
            {
                _placatoryTypes = targetRecipe.Types;
                _minimumValueOfItemOfPlacatoryType = _placatoryTypes.Min(type => _minimumItemValuePerType[type]);
                _maximumValueOfItemOfPlacatoryType = _placatoryTypes.Max(type => _maximumItemValuePerType[type]);
            }

            _numberOfAdditionalInputsRequired = NumberOfItemsPerBatch - alreadyChosenInputs.Count;

            int totalValueContributionOfAlreadyChosenInputs = alreadyChosenInputs.Sum(input => input.Value);
            _minimumRequiredValueContributionOfAdditionalInputs    = targetRecipe.MinimumTotalValue - totalValueContributionOfAlreadyChosenInputs;
            _maximumPermissibleValueContributionOfAdditionalInputs = targetRecipe.MaximumTotalValue - totalValueContributionOfAlreadyChosenInputs;
        }

        private bool _typedInputRequired;
        private IReadOnlyList<Type> _placatoryTypes;
        private int _minimumValueOfItemOfPlacatoryType;
        private int _maximumValueOfItemOfPlacatoryType;

        private int _numberOfAdditionalInputsRequired;

        private int _minimumRequiredValueContributionOfAdditionalInputs;
        private int _maximumPermissibleValueContributionOfAdditionalInputs;

        public bool CanCompleteRecipeUsingItem(Item item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            else if ( ! item.CanBeInput)
            {
                throw new ArgumentException("Must be usable as input", nameof(item));
            }

            Debug.Assert(_numberOfAdditionalInputsRequired >= 1);

            bool typedInputRequired = _typedInputRequired && ! _placatoryTypes.Contains(item.Type);

            if (typedInputRequired && _numberOfAdditionalInputsRequired == 1)
            {
                return false;
            }

            int numberOfAdditionalInputsRequired = _numberOfAdditionalInputsRequired - 1;

            int minimumRequiredValueContributionFromAdditionalInputs    = _minimumRequiredValueContributionOfAdditionalInputs    - item.Value;
            int maximumPermissibleValueContributionFromAdditionalInputs = _maximumPermissibleValueContributionOfAdditionalInputs - item.Value;

            int minimumValueContributionFromAdditionalItems = 0;
            int maximumValueContributionFromAdditionalItems = 0;

            if (typedInputRequired)
            {
                minimumValueContributionFromAdditionalItems = _minimumValueOfItemOfPlacatoryType;
                maximumValueContributionFromAdditionalItems = _maximumValueOfItemOfPlacatoryType;

                --numberOfAdditionalInputsRequired;
                Debug.Assert(numberOfAdditionalInputsRequired >= 0);
            }

            minimumValueContributionFromAdditionalItems += numberOfAdditionalInputsRequired * _minimumItemValue;
            maximumValueContributionFromAdditionalItems += numberOfAdditionalInputsRequired * _maximumItemValue;

            bool valueContributionSoFarIsTooSmall = minimumRequiredValueContributionFromAdditionalInputs    > maximumValueContributionFromAdditionalItems;
            bool valueContributionSoFarIsTooBig   = maximumPermissibleValueContributionFromAdditionalInputs < minimumValueContributionFromAdditionalItems;

            return ! (valueContributionSoFarIsTooSmall || valueContributionSoFarIsTooBig);
        }

        private static readonly int _minimumItemValue = Items.InputItems.Min(item => item.Value);
        private static readonly int _maximumItemValue = Items.InputItems.Max(item => item.Value);

        private static readonly Dictionary<Type, int> _minimumItemValuePerType;
        private static readonly Dictionary<Type, int> _maximumItemValuePerType;

        static StandardRecipeItemFilterer()
        {
            _minimumItemValuePerType = Items.InputItems.GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.Min(item => item.Value));
            _maximumItemValuePerType = Items.InputItems.GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.Max(item => item.Value));

            Debug.Assert(Enum.GetValues(typeof(Type)).Cast<Type>().All(_minimumItemValuePerType.ContainsKey));
            Debug.Assert(Enum.GetValues(typeof(Type)).Cast<Type>().All(_maximumItemValuePerType.ContainsKey));
        }
    }
}
