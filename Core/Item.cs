using System;

namespace CramIt.Core
{
    public class Item
    {
        public string Name              { get; }
        public string TRMoveName        { get; }
        public int    SpriteRowIndex    { get; }
        public int    SpriteColumnIndex { get; }
        public bool   CanBeInput        { get; }
        public Type   Type              { get; }
        public int    Value             { get; }

        public static Item Input(string name, int spriteRowIndex, int spriteColumnIndex, Type type, int value)
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

            return new Item(name, null, spriteRowIndex, spriteColumnIndex, true, type, value);
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

            return new Item($"TR{number:D2}", trMoveName, spriteRowIndex, spriteColumnIndex, false, type, InvalidValue);
        }

        public static Item Ball(string name, int spriteRowIndex, int spriteColumnIndex)
            => new Item(name, null, spriteRowIndex, spriteColumnIndex, false, (Type)(int.MinValue), InvalidValue);

        const int InvalidValue = -1 * (1 << 10);

        private Item(string name, string trMoveName, int spriteRowIndex, int spriteColumnIndex, bool canBeInput, Type type, int value)
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

            Name              = name;
            TRMoveName        = trMoveName;
            SpriteRowIndex    = spriteRowIndex;
            SpriteColumnIndex = spriteColumnIndex;
            CanBeInput        = canBeInput;
            Type              = type;
            Value             = value;
        }
    }
}
