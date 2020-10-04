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
using System.Diagnostics;

namespace CramIt.Core
{
    public class Item: IEquatable<Item>
    {
        public string Name                  { get; }
        public string TRMoveName            { get; }
        public int    SpriteRowIndex        { get; }
        public int    SpriteColumnIndex     { get; }
        public bool   CanBeInput            { get; }
        public Type   Type                  { get; }
        public int    Value                 { get; }
        public bool   IsIrreplaceable       { get; }
        public bool   IsGroupRepresentative { get; }

        public bool IsTR
            => ! (TRMoveName is null);

        public override string ToString()
            => ToString(false);

        public string ToString(bool combineGroupsOfSimilarItems)
            => (combineGroupsOfSimilarItems && Name == "Serious Mint"  ) ? "Mint (any)"         :
               (combineGroupsOfSimilarItems && Name == "Health Feather") ? "Stat feather (any)" :
               (Name + (IsTR ? $" {TRMoveName}" : ""));

        public string NameForHtmlId
            => Name.ToLowerInvariant().Replace(' ', '-').Replace('\u00e9', 'e');

        public string HtmlSpriteStyle
        {
            get
            {
                const int spriteWidth  = 24;
                const int spriteHeight = 24;
                int offset_X = -spriteWidth  * SpriteColumnIndex;
                int offset_Y = -spriteHeight * SpriteRowIndex;
                return $"background-position: {offset_X}px {offset_Y}px;";
            }
        }

        public static string PlaceholderHtmlSpriteStyle
            => "background-position: 0px 0px;";

        public static Item Input(string name, int spriteRowIndex, int spriteColumnIndex, Type type, int value, bool isIrreplaceable, bool isGroupRepresentative)
        {
            const int maxItemValue = 20;

            if (value < 0)
            {
                throw new ArgumentException("Must be nonnegative", nameof(value));
            }
            else if (value > maxItemValue)
            {
                throw new ArgumentException($"Must not be greater than {maxItemValue}", nameof(value));
            }

            if ( ! Enum.IsDefined(typeof(Type), type))
            {
                throw new ArgumentException("Must be a defined member of the enum", nameof(type));
            }

            return new Item(name, null, spriteRowIndex, spriteColumnIndex, true, type, value, isIrreplaceable, isGroupRepresentative);
        }

        public static Item TR(int number, string trMoveName, int spriteRowIndex, int spriteColumnIndex, Type type)
        {
            if (trMoveName is null)
            {
                throw new ArgumentNullException(nameof(trMoveName));
            }
            else if (string.IsNullOrWhiteSpace(trMoveName))
            {
                throw new ArgumentException("Must not be empty nor consist entirely of whitespace", nameof(trMoveName));
            }

            if ( ! Enum.IsDefined(typeof(Type), type))
            {
                throw new ArgumentException("Must be a defined member of the enum", nameof(type));
            }

            return new Item($"TR{number:D2}", trMoveName, spriteRowIndex, spriteColumnIndex, false, type, InvalidValue, false, true);
        }

        public static Item Ball(string name, int spriteRowIndex, int spriteColumnIndex)
            => new Item(name, null, spriteRowIndex, spriteColumnIndex, false, (Type)(int.MinValue), InvalidValue, false, true);

        const int InvalidValue = -1 * (1 << 10);

        private Item(
            string name,
            string trMoveName,
            int    spriteRowIndex,
            int    spriteColumnIndex,
            bool   canBeInput,
            Type   type,
            int    value,
            bool   isIrreplaceable,
            bool   isGroupRepresentative)
        {
            const int numberOfSpritesPerRow = 20;
            const int numberOfRowsOfSprites = 16;

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must not be empty nor consist entirely of whitespace", nameof(name));
            }

            if (spriteRowIndex < 0)
            {
                throw new ArgumentException("Must be nonnegative", nameof(spriteRowIndex));
            }
            else if (spriteRowIndex >= numberOfRowsOfSprites)
            {
                throw new ArgumentException($"Must be less than the number of rows of sprites, which is {numberOfRowsOfSprites}", nameof(spriteRowIndex));
            }

            if (spriteColumnIndex < 0)
            {
                throw new ArgumentException("Must be nonnegative", nameof(spriteColumnIndex));
            }
            else if (spriteColumnIndex >= numberOfSpritesPerRow)
            {
                throw new ArgumentException($"Must be less than the number of sprites per row, which is {numberOfSpritesPerRow}", nameof(spriteColumnIndex));
            }

            Name                  = name;
            TRMoveName            = trMoveName;
            SpriteRowIndex        = spriteRowIndex;
            SpriteColumnIndex     = spriteColumnIndex;
            CanBeInput            = canBeInput;
            Type                  = type;
            Value                 = value;
            IsIrreplaceable       = isIrreplaceable;
            IsGroupRepresentative = isGroupRepresentative;
        }

        public override bool Equals(object that)
            => Equals(that as Item);

        public bool Equals(Item that)
        {
            if (that is null)
            {
                return false;
            }

            bool equal = Name == that.Name;

            Debug.Assert( ! (equal && TRMoveName        != that.TRMoveName       ));
            Debug.Assert( ! (equal && SpriteRowIndex    != that.SpriteRowIndex   ));
            Debug.Assert( ! (equal && SpriteColumnIndex != that.SpriteColumnIndex));
            Debug.Assert( ! (equal && CanBeInput        != that.CanBeInput       ));
            Debug.Assert( ! (equal && Type              != that.Type             ));
            Debug.Assert( ! (equal && Value             != that.Value            ));

            Debug.Assert( ! (equal && GetHashCode() != that.GetHashCode()));
            return equal;
        }

        public static bool operator ==(Item a, Item b)
            => a is null ? b is null : a.Equals(b);

        public static bool operator !=(Item a, Item b)
            => ! (a == b);

        public override int GetHashCode()
            => Name.GetHashCode();
    }
}
