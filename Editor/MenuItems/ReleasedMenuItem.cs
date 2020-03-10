using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Training.Interaction.Conditions;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Interaction.Editors
{
    public class ReleasedMenuItem : Menu.Item<ICondition>
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
