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
    /// Condition which is completed when `GrabbableProperty` is grabbed.
    /// </summary>
    [DataContract(IsReference = true)]
    public class GrabbedCondition : Condition<GrabbedCondition.EntityData>
    {
        [DisplayName("Grab Object")]
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

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            public override void Complete()
            {
                Data.GrabbableProperty.Value.FastForwardGrab();
            }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            protected override bool CheckIfCompleted()
            {
                return Data.GrabbableProperty.Value.IsGrabbed;
            }

            public ActiveProcess(EntityData data) : base(data)
            {
            }
        }

        public GrabbedCondition() : this("")
        {
        }

        public GrabbedCondition(IGrabbableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public GrabbedCondition(string target, string name = "Grab Object")
        {
            Data.GrabbableProperty = new ScenePropertyReference<IGrabbableProperty>(target);
            Data.Name = name;
        }

        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}