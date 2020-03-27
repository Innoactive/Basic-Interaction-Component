using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class ReleasedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Release Object";

        public override ICondition GetNewItem()
        {
            return new ReleasedCondition();
        }
    }
}
