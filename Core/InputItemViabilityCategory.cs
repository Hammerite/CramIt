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
