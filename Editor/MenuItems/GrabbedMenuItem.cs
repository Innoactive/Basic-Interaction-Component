using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class GrabbedMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Grab Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new GrabbedCondition();
        }
    }
}
