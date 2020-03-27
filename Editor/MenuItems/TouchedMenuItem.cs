using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class TouchedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Touch Object";

        public override ICondition GetNewItem()
        {
            return new TouchedCondition();
        }
    }
}
