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
    /// Condition which is completed when TouchableProperty is touched.
    /// </summary>
    [DataContract(IsReference = true)]
    public class TouchedCondition : Condition<TouchedCondition.EntityData>
    {
        [DisplayName("Touch Object")]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Touchable object")]
            public ScenePropertyReference<ITouchableProperty> TouchableProperty { get; set; }

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
                return data.TouchableProperty.Value.IsBeingTouched;
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.TouchableProperty.Value.FastForwardTouch();
                base.Complete(data);
            }
        }

        public TouchedCondition() : this("")
        {
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        public TouchedCondition(ITouchableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public TouchedCondition(string target, string name = "Touch Object")
        {
            Data = new EntityData()
            {
                TouchableProperty = new ScenePropertyReference<ITouchableProperty>(target),
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
