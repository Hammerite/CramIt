using System;
using System.Collections.Generic;
using System.Linq;

namespace CramIt.Core
{
    public enum InputItemOrdering
    {
        ByName,
        ByValue,
        ByViabilityCategoryThenName,
        ByViabilityCategoryThenValue,
        ByViabilityCategoryThenValueExceptThatNonviableItemsAreOrderedByName,
    }

    public static class InputItemOrderingExtensions
    {
        public static string HumanReadableName(this InputItemOrdering ordering)
        {
            switch (ordering)
            {
                case InputItemOrdering.ByName:                       return "By name";
                case InputItemOrdering.ByValue:                      return "By value";
                case InputItemOrdering.ByViabilityCategoryThenName:  return "By viability category, then name";
                case InputItemOrdering.ByViabilityCategoryThenValue: return "By viability category, then value";
                case InputItemOrdering.ByViabilityCategoryThenValueExceptThatNonviableItemsAreOrderedByName:
                    return "By viability category; then by value, except that nonviable items are ordered by name";
                default:
                    throw new Exception($@"Unhandled {nameof(InputItemOrdering)} ""{ordering}"" in switch");
            }
        }

        public static IEnumerable<InputItemWithViabilityCategoryInformation> ApplyOrdering(
            this IEnumerable<InputItemWithViabilityCategoryInformation> inputItems, InputItemOrdering ordering, InputItemOptions options)
        {
            switch (ordering)
            {
                case InputItemOrdering.ByName:
                    return inputItems.OrderBy(item => item.Item.ToString(options.CombineGroupsOfSimilarItems));
                case InputItemOrdering.ByValue:
                    return inputItems.OrderBy(item => item.Item.Value);
                case InputItemOrdering.ByViabilityCategoryThenName:
                    return inputItems.OrderByDescending(item => item.BestViabilityCategory).ThenBy(item => item.Item.ToString(options.CombineGroupsOfSimilarItems));
                case InputItemOrdering.ByViabilityCategoryThenValue:
                    return inputItems.OrderByDescending(item => item.BestViabilityCategory).ThenBy(item => item.Item.Value);
                case InputItemOrdering.ByViabilityCategoryThenValueExceptThatNonviableItemsAreOrderedByName:
                    return inputItems
                        .OrderByDescending(item => item.BestViabilityCategory)
                        .ThenBy(item => item.BestViabilityCategory == InputItemViabilityCategory.Nonviable ? item.Item.ToString(options.CombineGroupsOfSimilarItems) : "")
                        .ThenBy(item => item.BestViabilityCategory == InputItemViabilityCategory.Nonviable ? 0 : item.Item.Value);
                default:
                    throw new Exception($@"Unhandled {nameof(TROrdering)} ""{ordering}"" in switch");
            }
        }
    }
}
