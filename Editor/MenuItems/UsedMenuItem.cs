using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class UsedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Use Object";

        public override ICondition GetNewItem()
        {
            return new UsedCondition();
        }
    }
}