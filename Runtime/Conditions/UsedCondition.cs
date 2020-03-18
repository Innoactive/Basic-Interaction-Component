using System.Runtime.Serialization;
using Innoactive.Creator.BasicInteraction.Properties;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Utils;

namespace Innoactive.Creator.BasicInteraction.Conditions
{
    /// <summary>
    /// Condition which becomes completed when UsableProperty is used.
    /// </summary>
    [DataContract(IsReference = true)]
    public class UsedCondition : Condition<UsedCondition.EntityData>
    {
        [DisplayName("Use Object")]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Usable object")]
            public ScenePropertyReference<IUsableProperty> UsableProperty { get; set; }

            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseStageProcessOverCompletable<EntityData>
        {
            protected override bool CheckIfCompleted(EntityData data)
            {
                return data.UsableProperty.Value.IsBeingUsed;
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.UsableProperty.Value.FastForwardUse();
                base.Complete(data);
            }
        }

        public UsedCondition() : this("")
        {
        }

        public UsedCondition(IUsableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public UsedCondition(string target, string name = "Use Object")
        {
            Data = new EntityData()
            {
                UsableProperty = new ScenePropertyReference<IUsableProperty>(target),
                Name = name
            };
        }

        private readonly IProcess<EntityData> process = new ActiveOnlyProcess<EntityData>(new ActiveProcess());
        private readonly IAutocompleter<EntityData> autocompleter = new EntityAutocompleter();

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        protected override IAutocompleter<EntityData> Autocompleter
        {
            get
            {
                return autocompleter;
            }
        }
    }
}
