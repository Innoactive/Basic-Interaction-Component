using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Training.Interaction.Conditions;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Interaction.Editors
{
    public class GrabbedMenuItem : Menu.Item<ICondition>
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
