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
    public enum TROrdering
    {
        ByMoveName,
        ByNumber,
        ByMoveTypeThenMoveName,
        ByMoveTypeThenNumber,
    }

    public static class TROrderingExtensions
    {
        public static string HumanReadableName(this TROrdering ordering)
        {
            switch (ordering)
            {
                case TROrdering.ByMoveName:             return "By move name";
                case TROrdering.ByNumber:               return "By number";
                case TROrdering.ByMoveTypeThenMoveName: return "By move type, then move name";
                case TROrdering.ByMoveTypeThenNumber:   return "By move type, then move number";
                default:
                    throw new Exception($@"Unhandled {nameof(TROrdering)} ""{ordering}"" in switch");
            }
        }

        public static IEnumerable<Item> ApplyOrdering(this IEnumerable<Item> trs, TROrdering ordering)
        {
            switch (ordering)
            {
                case TROrdering.ByMoveName:             return trs.OrderBy(tr => tr.TRMoveName);
                case TROrdering.ByNumber:               return trs.OrderBy(tr => tr.Name      );
                case TROrdering.ByMoveTypeThenMoveName: return trs.OrderBy(tr => tr.Type      ).ThenBy(tr => tr.TRMoveName);
                case TROrdering.ByMoveTypeThenNumber:   return trs.OrderBy(tr => tr.Type      ).ThenBy(tr => tr.Name      );
                default:
                    throw new Exception($@"Unhandled {nameof(TROrdering)} ""{ordering}"" in switch");
            }
        }

        public static IEnumerable<StandardRecipe> ApplyOrdering(this IEnumerable<StandardRecipe> recipes, TROrdering ordering)
        {
            switch (ordering)
            {
                case TROrdering.ByMoveName:             return recipes.OrderBy(r => r.Item.TRMoveName);
                case TROrdering.ByNumber:               return recipes.OrderBy(r => r.Item.Name      );
                case TROrdering.ByMoveTypeThenMoveName: return recipes.OrderBy(r => r.Item.Type      ).ThenBy(r => r.Item.TRMoveName);
                case TROrdering.ByMoveTypeThenNumber:   return recipes.OrderBy(r => r.Item.Type      ).ThenBy(r => r.Item.Name      );
                default:
                    throw new Exception($@"Unhandled {nameof(TROrdering)} ""{ordering}"" in switch");
            }
        }
    }
}
