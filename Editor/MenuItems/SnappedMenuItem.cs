using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class SnappedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Snap Object";

        public override ICondition GetNewItem()
        {
            return new SnappedCondition();
        }
    }
}