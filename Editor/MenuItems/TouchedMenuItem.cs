using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class TouchedMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Touch Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new TouchedCondition();
        }
    }
}
