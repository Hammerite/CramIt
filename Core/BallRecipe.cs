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
using System.Collections.Generic;
using System.Linq;

namespace CramIt.Core
{
    public class BallRecipe
    {
        public IReadOnlyList<Item>                                 ContributingApricorns                      { get; }
        public Probability                                         ProbabilityOfSuccess                       { get; }
        public Probability                                         ProbabilityOfUnspecifiedAlternativeOutcome { get; }
        public IReadOnlyList<BallRecipeSpecificAlternativeOutcome> SpecificAlternativeOutcomes                { get; }

        public BallRecipe(
            string                                        contributingApricornColourName,
            Probability                                   probabilityOfSuccess,
            Probability                                   probabilityOfUnspecifiedAlternativeOutcome,
            params BallRecipeSpecificAlternativeOutcome[] specificAlternativeOutcomes):
            this(new [] {contributingApricornColourName}, probabilityOfSuccess, probabilityOfUnspecifiedAlternativeOutcome, specificAlternativeOutcomes)
        {}

        public BallRecipe(
            string                                            contributingApricornColourName,
            Probability                                       probabilityOfSuccess,
            Probability                                       probabilityOfUnspecifiedAlternativeOutcome,
            IEnumerable<BallRecipeSpecificAlternativeOutcome> specificAlternativeOutcomes):
            this(new [] {contributingApricornColourName}, probabilityOfSuccess, probabilityOfUnspecifiedAlternativeOutcome, specificAlternativeOutcomes)
        {}

        public BallRecipe(
            IEnumerable<string>                               contributingApricornColourNames,
            Probability                                       probabilityOfSuccess,
            Probability                                       probabilityOfUnspecifiedAlternativeOutcome,
            IEnumerable<BallRecipeSpecificAlternativeOutcome> specificAlternativeOutcomes)
        {
            if (probabilityOfSuccess.IsImpossible)
            {
                throw new ArgumentException("Must not be impossible", nameof(probabilityOfSuccess));
            }
            else if (probabilityOfSuccess.IsCertain)
            {
                throw new ArgumentException("Must not be certain", nameof(probabilityOfSuccess));
            }
            ProbabilityOfSuccess = probabilityOfSuccess;

            if (probabilityOfUnspecifiedAlternativeOutcome.IsCertain)
            {
                throw new ArgumentException("Must not be certain", nameof(probabilityOfUnspecifiedAlternativeOutcome));
            }
            ProbabilityOfUnspecifiedAlternativeOutcome = probabilityOfUnspecifiedAlternativeOutcome;

            if (contributingApricornColourNames is null)
            {
                throw new ArgumentNullException(nameof(contributingApricornColourNames));
            }
            var contributingApricornColourNames_Array = contributingApricornColourNames.ToArray();
            if (contributingApricornColourNames_Array.Any(colourName => colourName is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(contributingApricornColourNames));
            }
            ContributingApricorns = contributingApricornColourNames_Array.Select(colourName => Items.ItemsByName[$"{colourName} Apricorn"]).ToArray();
            var contributingApricornNames = ContributingApricorns.Select(ca => ca.Name).ToArray();
            if (contributingApricornNames.Distinct().Count() != contributingApricornNames.Length)
            {
                throw new ArgumentException("Elements must be distinct", nameof(contributingApricornColourNames));
            }

            if (specificAlternativeOutcomes is null)
            {
                throw new ArgumentNullException(nameof(specificAlternativeOutcomes));
            }
            SpecificAlternativeOutcomes = specificAlternativeOutcomes.ToArray();
            if (SpecificAlternativeOutcomes.Any(t => t is null))
            {
                throw new ArgumentException("Elements must not be null", nameof(specificAlternativeOutcomes));
            }
            var specificAlternativeOutcomeItemNames = SpecificAlternativeOutcomes.Select(outcome => outcome.Item.Name).ToArray();
            if (specificAlternativeOutcomeItemNames.Distinct().Count() != specificAlternativeOutcomeItemNames.Length)
            {
                throw new ArgumentException("Elements must be distinct items", nameof(specificAlternativeOutcomes));
            }
            var allProbabilities = new [] {ProbabilityOfSuccess, ProbabilityOfUnspecifiedAlternativeOutcome}
                .Concat(SpecificAlternativeOutcomes.Select(outcome => outcome.Probability));
            if ( ! Probability.ProbabilitiesSumToUnity(allProbabilities))
            {
                throw new ArgumentException("Probabilities must sum to unity", nameof(specificAlternativeOutcomes));
            }
        }
    }
}
