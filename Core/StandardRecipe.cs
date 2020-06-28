using System;
using System.Collections.Generic;
using System.Linq;

namespace CramIt.Core
{
    public class StandardRecipe
    {
        public int MinimumTotalValue { get; }
        public int MaximumTotalValue { get; }

        public IReadOnlyList<Type> Types              { get; }
        public IReadOnlyList<Item> OtherPossibleItems { get; }

        public StandardRecipe(int minimumTotalValue, IEnumerable<Type> types, IEnumerable<string> otherPossibleItemNames = null)
        {
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

        public StandardRecipe(int minimumTotalValue, params Type[] types):
            this(minimumTotalValue, types as IEnumerable<Type>)
        {}

        public static StandardRecipe AllTypes(int minimumTotalValue)
            => new StandardRecipe(minimumTotalValue, Enum.GetValues(typeof(Type)).Cast<Type>());

        public static StandardRecipe AllTypesExcept(int minimumTotalValue, IEnumerable<Type> excludedTypes)
        {
            var includedTypes = Enum.GetValues(typeof(Type)).Cast<Type>().Except(excludedTypes);
            if ( ! includedTypes.Any())
            {
                throw new ArgumentException("Can't exclude every type", nameof(excludedTypes));
            }

            return new StandardRecipe(minimumTotalValue, includedTypes);
        }

        public static StandardRecipe AllTypesExcept(int minimumTotalValue, params Type[] types)
            => AllTypesExcept(minimumTotalValue, types as IEnumerable<Type>);

        private static readonly IReadOnlyDictionary<int, int> _lookupMinimumTotalValueToMaximumTotalValue
            = new Dictionary<int, int>
              {
                  { 1, 10}, {11, 15}, {16, 20}, {21, 25}, {26, 30},
                  {31, 35}, {36, 40}, {41, 45}, {46, 50}, {51, 55},
                  {56, 60}, {61, 65}, {66, 70}, {71, 75}, {76, 80},
              };
    }
}
