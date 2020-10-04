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

namespace CramIt.Core
{
    public enum InputItemViabilityCategory
    {
        Nonviable, // Worst
        Viable_NotOfPlacatoryType,
        Viable_OfPlacatoryType, // Best
    }

    public static class InputItemViabilityCategoryExtensions
    {
        public static string CssClassName(this InputItemViabilityCategory viabilityCategory, string prefix)
        {
            switch (viabilityCategory)
            {
                case InputItemViabilityCategory.Nonviable:                 return $"{prefix}-nonviable";
                case InputItemViabilityCategory.Viable_NotOfPlacatoryType: return $"{prefix}-viable-not-of-placatory-type";
                case InputItemViabilityCategory.Viable_OfPlacatoryType:    return $"{prefix}-viable-of-placatory-type";
                default:
                    throw new Exception($@"Unhandled {nameof(InputItemViabilityCategory)} ""{viabilityCategory}"" in switch");
            }
        }
    }
}
