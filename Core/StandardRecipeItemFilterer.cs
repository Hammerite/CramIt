using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipeItemFilterer
    {
        public const int NumberOfItemsPerBatch = 4;

        public StandardRecipeItemFilterer(StandardRecipe targetRecipe): this(targetRecipe, new Item[0])
        {}

        public StandardRecipeItemFilterer(StandardRecipe targetRecipe, IEnumerable<Item> alreadyChosenInputs)
        {
            if (targetRecipe is null)
            {
                throw new ArgumentNullException(nameof(targetRecipe));
            }

            if (alreadyChosenInputs is null)
            {
                throw new ArgumentNullException(nameof(alreadyChosenInputs));
            }
            var alreadyChosenInputs_List = alreadyChosenInputs.ToList();
            if (alreadyChosenInputs_List.Any(input => input is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(alreadyChosenInputs));
            }
            if (alreadyChosenInputs_List.Any(input => ! input.CanBeInput))
            {
                throw new ArgumentException("Elements must be usable as input", nameof(alreadyChosenInputs));
            }
            if (alreadyChosenInputs_List.Count >= NumberOfItemsPerBatch)
            {
                throw new ArgumentException($"Must contain fewer than {NumberOfItemsPerBatch} elements", nameof(alreadyChosenInputs));
            }

            _placatoryTypes = targetRecipe.Types;
            TypedInputRequired = ! alreadyChosenInputs_List.Any(input => _placatoryTypes.Contains(input.Type));

            if (TypedInputRequired)
            {
                _minimumValueOfItemOfPlacatoryType = _placatoryTypes.Min(type => _minimumItemValuePerType[type]);
                _maximumValueOfItemOfPlacatoryType = _placatoryTypes.Max(type => _maximumItemValuePerType[type]);
            }

            _numberOfAdditionalInputsRequired = NumberOfItemsPerBatch - alreadyChosenInputs_List.Count;

            int totalValueContributionOfAlreadyChosenInputs = alreadyChosenInputs_List.Sum(input => input.Value);
            _minimumRequiredValueContributionOfAdditionalInputs    = targetRecipe.MinimumTotalValue - totalValueContributionOfAlreadyChosenInputs;
            _maximumPermissibleValueContributionOfAdditionalInputs = targetRecipe.MaximumTotalValue - totalValueContributionOfAlreadyChosenInputs;

            if (alreadyChosenInputs_List.Count == 3
            &&  alreadyChosenInputs.Distinct().Count() == 1
            &&  Recipes.RepeatedItemRecipes.Any(recipe => recipe.Key != targetRecipe.Item.Name && recipe.Value == alreadyChosenInputs_List[0]))
            {
                _disallowed4thItem = alreadyChosenInputs_List[0];
            }
        }

        private readonly IReadOnlyList<Type> _placatoryTypes;

        private readonly int _minimumValueOfItemOfPlacatoryType;
        private readonly int _maximumValueOfItemOfPlacatoryType;

        private readonly int _numberOfAdditionalInputsRequired;

        private readonly int _minimumRequiredValueContributionOfAdditionalInputs;
        private readonly int _maximumPermissibleValueContributionOfAdditionalInputs;

        private readonly Item _disallowed4thItem;

        public bool TypedInputRequired { get; }

        public bool ItemIsViable(Item item)
            => CanCompleteRecipeUsingItem(item, out _);

        public bool ItemIsOfPlacatoryType(Item item)
            => _placatoryTypes.Contains(item.Type);

        public InputItemViabilityCategory ViabilityCategory(Item item)
        {
            bool viable = CanCompleteRecipeUsingItem(item, out bool isOfPlacatoryType);

            if (viable)
            {
                return isOfPlacatoryType ? InputItemViabilityCategory.Viable_OfPlacatoryType : InputItemViabilityCategory.Viable_NotOfPlacatoryType;
            }
            else
            {
                return InputItemViabilityCategory.Nonviable;
            }
        }

        private bool CanCompleteRecipeUsingItem(Item item, out bool isOfPlacatoryType)
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

            isOfPlacatoryType = ItemIsOfPlacatoryType(item);
            bool typedInputRequired = TypedInputRequired && ! isOfPlacatoryType;

            if (_numberOfAdditionalInputsRequired == 1 && (typedInputRequired || item == _disallowed4thItem))
            {
                return false;
            }

            int numberOfAdditionalInputsRequired = _numberOfAdditionalInputsRequired - 1;

            int minimumRequiredValueContributionFromAdditionalInputs    = _minimumRequiredValueContributionOfAdditionalInputs    - item.Value;
            int maximumPermissibleValueContributionFromAdditionalInputs = _maximumPermissibleValueContributionOfAdditionalInputs - item.Value;

            if (numberOfAdditionalInputsRequired == 1 && typedInputRequired)
            {
                return Items.InputItems().Any(x => _placatoryTypes.Contains(x.Type)
                                                && x.Value >= minimumRequiredValueContributionFromAdditionalInputs
                                                && x.Value <= maximumPermissibleValueContributionFromAdditionalInputs);
            }

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

        private static readonly int _minimumItemValue = Items.InputItems().Min(item => item.Value);
        private static readonly int _maximumItemValue = Items.InputItems().Max(item => item.Value);

        private static readonly Dictionary<Type, int> _minimumItemValuePerType;
        private static readonly Dictionary<Type, int> _maximumItemValuePerType;

        static StandardRecipeItemFilterer()
        {
            _minimumItemValuePerType = Items.InputItems().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.Min(item => item.Value));
            _maximumItemValuePerType = Items.InputItems().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.Max(item => item.Value));

            Debug.Assert(Enum.GetValues(typeof(Type)).Cast<Type>().All(_minimumItemValuePerType.ContainsKey));
            Debug.Assert(Enum.GetValues(typeof(Type)).Cast<Type>().All(_maximumItemValuePerType.ContainsKey));
        }
    }
}
