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
