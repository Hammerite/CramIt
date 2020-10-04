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
