using System;
using System.Collections.Generic;
using CramIt.Core;
using Microsoft.AspNetCore.Components;

namespace CramIt.Site.Pages
{
    public class IndexBase: ComponentBase
    {
        protected Mode Mode { get; set; } = Mode.SelectingDesiredOutput;

        protected Item DesiredOutputItem { get; set; }

        protected IReadOnlyList<Item> ChosenInputItems { get; set; }

        protected StandardRecipeGroupItemFilterer StandardRecipeGroupItemFilterer { get; set; }

        protected string UserMessage { get; set; } = "Click something please";

        protected void ItemClicked(Item item)
        {
            UserMessage = $"You clicked {item}, great job";
        }
    }
}
