using System;

namespace CramIt.Core
{
    public class InputItemWithViabilityCategoryInformation
    {
        public Item                       Item                  { get; }
        public InputItemViabilityCategory BestViabilityCategory { get; }

        public InputItemWithViabilityCategoryInformation(Item item, InputItemViabilityCategory bestViabilityCategory)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            else if ( ! item.CanBeInput)
            {
                throw new ArgumentException("Must be eligible to be an input item", nameof(item));
            }

            Item                  = item;
            BestViabilityCategory = bestViabilityCategory;
        }
    }
}
