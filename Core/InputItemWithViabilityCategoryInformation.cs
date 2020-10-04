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
