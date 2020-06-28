using System;

namespace CramIt.Core
{
    public class BallRecipeSpecificAlternativeOutcome
    {
        public Item        Item        { get; }
        public Probability Probability { get; }

        public BallRecipeSpecificAlternativeOutcome(string itemName, Probability probability)
        {
            if (itemName is null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }
            Item = Items.ItemsByName[itemName];

            if (probability.IsImpossible)
            {
                throw new ArgumentException("Must not be impossible", nameof(probability));
            }
            else if (probability.IsCertain)
            {
                throw new ArgumentException("Must not be certain", nameof(probability));
            }
            Probability = probability;
        }
    }
}
