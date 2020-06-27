using System;

namespace CramIt.Core
{
    public class Item
    {
        public string Name              { get; }
        public int    SpriteRowIndex    { get; }
        public int    SpriteColumnIndex { get; }
        public bool   CanBeInput        { get; }
        public Type   Type              { get; }
        public int    Value             { get; }

        public static Item Input(string name, int spriteRowIndex, int spriteColumnIndex, Type type, int value)
        {
            const int maxItemValue = 20;

            if ( ! Enum.IsDefined(typeof(Type), type))
            {
                throw new ArgumentException("Must be a defined member of the enum", nameof(type));
            }

            if (value < 0)
            {
                throw new ArgumentException("Must be nonnegative", nameof(value));
            }
            else if (value > maxItemValue)
            {
                throw new ArgumentException($"Must not be greater than {maxItemValue}", nameof(value));
            }

            return new Item(name, spriteRowIndex, spriteColumnIndex, true, type, value);
        }

        public static Item OutputOnly(string name, int spriteRowIndex, int spriteColumnIndex)
            => new Item(name, spriteRowIndex, spriteColumnIndex, false, (Type)(int.MinValue), -1 * (1 << 10));

        private Item(string name, int spriteRowIndex, int spriteColumnIndex, bool canBeInput, Type type, int value)
        {
            const int numberOfSpritesPerRow = 20;
            const int numberOfRowsOfSprites = 16;

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
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

            Name              = name;
            SpriteRowIndex    = spriteRowIndex;
            SpriteColumnIndex = spriteColumnIndex;
            CanBeInput        = canBeInput;
            Type              = type;
            Value             = value;
        }
    }
}
