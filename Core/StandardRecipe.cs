using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipe: IEquatable<StandardRecipe>
    {
        public Item Item { get; }

        public int MinimumTotalValue { get; }
        public int MaximumTotalValue { get; }

        public IReadOnlyList<Type> Types              { get; }
        public IReadOnlyList<Item> OtherPossibleItems { get; }

        public override string ToString()
        {
            string typePart;
            if (Types.Count == 18)
            {
                typePart = "any type";
            }
            else if (Types.Count > 12)
            {
                typePart = "any type except " + string.Join("/", Enum.GetValues(typeof(Type)).Cast<Type>().Except(Types).Select(t => t.ToString()));
            }
            else
            {
                typePart = string.Join("/", Types.Select(t => t.ToString()));
            }

            return $"Recipe for {Item} ({MinimumTotalValue}-{MaximumTotalValue}, {typePart})";
        }

        public StandardRecipe(string itemName, int minimumTotalValue, IEnumerable<Type> types, IEnumerable<string> otherPossibleItemNames = null)
        {
            if (itemName is null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }
            Item = Items.ItemsByName[itemName];

            if ( ! _lookupMinimumTotalValueToMaximumTotalValue.ContainsKey(minimumTotalValue))
            {
                string permittedMinimumTotalValues =
                    string.Join(", ", _lookupMinimumTotalValueToMaximumTotalValue.Keys.OrderBy(n => n).Select(n => n.ToString()));
                throw new ArgumentException($"Must be one of: {permittedMinimumTotalValues}", nameof(minimumTotalValue));
            }
            MinimumTotalValue  = minimumTotalValue;
            MaximumTotalValue  = _lookupMinimumTotalValueToMaximumTotalValue[minimumTotalValue];

            if (types is null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            Types = types.ToArray();
            if ( ! Types.Any())
            {
                throw new ArgumentException("Must not be empty", nameof(types));
            }

            var otherPossibleItemNames_List = otherPossibleItemNames?.ToArray() ?? new string[0];
            if (otherPossibleItemNames_List.Any(s => s is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(otherPossibleItemNames));
            }
            OtherPossibleItems = otherPossibleItemNames_List.Select(s => Items.ItemsByName[s]).ToArray();
        }

        public StandardRecipe(string itemName, int minimumTotalValue, params Type[] types):
            this(itemName, minimumTotalValue, types as IEnumerable<Type>)
        {}

        public static StandardRecipe AllTypes(string itemName, int minimumTotalValue)
            => new StandardRecipe(itemName, minimumTotalValue, Enum.GetValues(typeof(Type)).Cast<Type>());

        public static StandardRecipe AllTypesExcept(string itemName, int minimumTotalValue, IEnumerable<Type> excludedTypes)
        {
            var includedTypes = Enum.GetValues(typeof(Type)).Cast<Type>().Except(excludedTypes);
            if ( ! includedTypes.Any())
            {
                throw new ArgumentException("Can't exclude every type", nameof(excludedTypes));
            }

            return new StandardRecipe(itemName, minimumTotalValue, includedTypes);
        }

        public static StandardRecipe AllTypesExcept(string itemName, int minimumTotalValue, params Type[] types)
            => AllTypesExcept(itemName, minimumTotalValue, types as IEnumerable<Type>);

        private static readonly IReadOnlyDictionary<int, int> _lookupMinimumTotalValueToMaximumTotalValue
            = new Dictionary<int, int>
              {
                  { 1, 10}, {11, 15}, {16, 20}, {21, 25}, {26, 30},
                  {31, 35}, {36, 40}, {41, 45}, {46, 50}, {51, 55},
                  {56, 60}, {61, 65}, {66, 70}, {71, 75}, {76, 80},
              };

        public override bool Equals(object that)
            => Equals(that as StandardRecipe);

        public bool Equals(StandardRecipe that)
        {
            if (that is null)
            {
                return false;
            }

            bool equal = true;

            equal &= MinimumTotalValue == that.MinimumTotalValue;

            equal &= Enumerable.SequenceEqual(Types,              that.Types             );
            equal &= Enumerable.SequenceEqual(OtherPossibleItems, that.OtherPossibleItems);

            Debug.Assert( ! (equal && MaximumTotalValue != that.MaximumTotalValue));

            Debug.Assert( ! (equal && GetHashCode() != that.GetHashCode()));
            return equal;
        }

        public static bool operator ==(StandardRecipe a, StandardRecipe b)
            => a is null ? b is null : a.Equals(b);

        public static bool operator !=(StandardRecipe a, StandardRecipe b)
            => ! (a == b);

        public override int GetHashCode()
        {
            const int prime = 523_541;

            int hashCode = 0;
            unchecked
            {
                hashCode = prime * hashCode + MinimumTotalValue.GetHashCode();

                foreach (var t in Types)
                {
                    hashCode = prime * hashCode + t.GetHashCode();
                }

                foreach (var item in OtherPossibleItems)
                {
                    hashCode = prime * hashCode + item.GetHashCode();
                }
            }

            return hashCode;
        }
    }
}
