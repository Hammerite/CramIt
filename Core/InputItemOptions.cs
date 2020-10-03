using System;
using System.Diagnostics;

namespace CramIt.Core
{
    public class InputItemOptions: IEquatable<InputItemOptions>
    {
        public bool IncludeIrreplaceableItems   { get; set; } = false;
        public bool CombineGroupsOfSimilarItems { get; set; } = true;

        public InputItemOptions()
        {}

        private InputItemOptions(InputItemOptions existing)
        {
            IncludeIrreplaceableItems   = existing.IncludeIrreplaceableItems;
            CombineGroupsOfSimilarItems = existing.CombineGroupsOfSimilarItems;
        }

        public InputItemOptions Clone()
            => new InputItemOptions(this);

        public override bool Equals(object that)
            => Equals(that as InputItemOptions);

        public bool Equals(InputItemOptions that)
        {
            if (that is null)
            {
                return false;
            }

            bool equal = true;

            equal &= IncludeIrreplaceableItems   == that.IncludeIrreplaceableItems;
            equal &= CombineGroupsOfSimilarItems == that.CombineGroupsOfSimilarItems;

            Debug.Assert( ! (equal && GetHashCode() != that.GetHashCode()));
            return equal;
        }

        public static bool operator ==(InputItemOptions a, InputItemOptions b)
            => a is null ? b is null : a.Equals(b);

        public static bool operator !=(InputItemOptions a, InputItemOptions b)
            => ! (a == b);

        public override int GetHashCode()
        {
            const int prime = 5_640_689;

            int hashCode = 0;
            unchecked
            {
                hashCode = prime * hashCode + IncludeIrreplaceableItems  .GetHashCode();
                hashCode = prime * hashCode + CombineGroupsOfSimilarItems.GetHashCode();
            }

            return hashCode;
        }
    }
}
