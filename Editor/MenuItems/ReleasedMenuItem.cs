using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.UI;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class ReleasedMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Release Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new ReleasedCondition();
        }
    }
}
