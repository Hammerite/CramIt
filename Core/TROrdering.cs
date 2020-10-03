﻿using System;
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

        public static IEnumerable<Item> OrderBy(this IEnumerable<Item> trs, TROrdering ordering)
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

        public static IEnumerable<StandardRecipe> OrderBy(this IEnumerable<StandardRecipe> recipes, TROrdering ordering)
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