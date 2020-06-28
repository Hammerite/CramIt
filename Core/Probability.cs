using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace CramIt.Core
{
    public struct Probability: IEquatable<Probability>
    {
        public int ValueInThousandths { get; }

        public Probability(int valueInThousandths)
        {
            if (valueInThousandths < 0)
            {
                throw new ArgumentException("Must be nonnegative", nameof(valueInThousandths));
            }
            else if (valueInThousandths > 1000)
            {
                throw new ArgumentException("Must not be greater than 1000", nameof(valueInThousandths));
            }
            ValueInThousandths = valueInThousandths;
        }

        public static implicit operator Probability(int valueInThousandths)
            => new Probability(valueInThousandths);

        public bool IsImpossible
            => ValueInThousandths == 0;

        public bool IsCertain
            => ValueInThousandths == 1000;

        public string ProbabilityAsPercentageForDisplay
            => (ValueInThousandths / 1000.0).ToString("P1", CultureInfo.CurrentCulture);

        public override bool Equals(object that)
            => that is Probability && Equals((Probability)that);

        public bool Equals(Probability that)
        {
            bool returnValue = ValueInThousandths == that.ValueInThousandths;
            Debug.Assert( ! (returnValue && GetHashCode() != that.GetHashCode()));
            return returnValue;
        }

        public static bool operator ==(Probability p, Probability q)
            => p.Equals(q);

        public static bool operator !=(Probability p, Probability q)
            => ! (p == q);

        public override int GetHashCode()
            => ValueInThousandths.GetHashCode();

        public static bool ProbabilitiesSumToUnity(IEnumerable<Probability> probabilities)
            => probabilities.Aggregate(0, (v, p) => v + p.ValueInThousandths) == 1000;
    }
}
