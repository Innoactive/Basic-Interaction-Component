using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.CreatorEditor.UI.StepInspector.Menu;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    /// <inheritdoc />
    public class TeleportMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Teleport";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new TeleportCondition();
        }
    }
}

