using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Validation;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.BasicInteraction.Properties;

namespace Innoactive.Creator.BasicInteraction.Conditions
{
    /// <summary>
    /// Condition which is completed when a teleportation action was executed into the referenced <see cref="ITeleportProperty"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    public class TeleportCondition : Condition<TeleportCondition.EntityData>
    {
        [DisplayName("Teleport")]
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Teleport Point")]
#if CREATOR_PRO
            [CheckForCollider]
#endif
            public ScenePropertyReference<ITeleportProperty> TeleportPoint { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        public TeleportCondition() : this( "")
        {
        }

        public TeleportCondition(ITeleportProperty teleportPoint, string name = null) : this(TrainingReferenceUtils.GetNameFrom(teleportPoint), name)
        {
        }

        public TeleportCondition(string teleportPoint, string name = "Teleport")
        {
            Data.TeleportPoint = new ScenePropertyReference<ITeleportProperty>(teleportPoint);
            Data.Name = name;
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }
            
            /// <inheritdoc />
            public override void Start()
            {
                base.Start();
                Data.TeleportPoint.Value.Initialize();
            }

            /// <inheritdoc />
            protected override bool CheckIfCompleted()
            {
                return Data.TeleportPoint.Value.WasUsedToTeleport;
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Complete()
            {
                Data.TeleportPoint.Value.FastForwardTeleport();
            }
        }

        /// <inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        /// <inheritdoc />
        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}
