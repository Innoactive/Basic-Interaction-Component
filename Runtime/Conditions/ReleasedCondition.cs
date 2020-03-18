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
    /// Condition which is completed when `GrabbableProperty` becomes ungrabbed.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ReleasedCondition : Condition<ReleasedCondition.EntityData>
    {
        [DisplayName("Release Object")]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Grabbable object")]
            public ScenePropertyReference<IGrabbableProperty> GrabbableProperty { get; set; }

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
                return data.GrabbableProperty.Value.IsGrabbed == false;
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.GrabbableProperty.Value.FastForwardUngrab();
                base.Complete(data);
            }
        }

        public ReleasedCondition() : this("")
        {
        }

        public ReleasedCondition(IGrabbableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public ReleasedCondition(string target, string name = "Release Object")
        {
            Data = new EntityData()
            {
                GrabbableProperty = new ScenePropertyReference<IGrabbableProperty>(target),
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
