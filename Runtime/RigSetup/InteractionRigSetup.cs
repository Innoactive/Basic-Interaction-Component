using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.Creator.BasicInteraction.RigSetup
{
    /// <summary>
    /// Will setup the interaction rig on awake. Priority is from top to bottom of the list, only rigs which full fill
    /// all RigUsabilityChecker will be initialized.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class InteractionRigSetup : MonoBehaviour
    {
        /// <summary>
        /// Info struct about one rig.
        /// </summary>
        [Serializable]
        public struct RigInfo
        {
            public string Name;
            public bool Enabled;
        }

        /// <summary>
        /// Information about possible interaction rigs, serializable.
        /// </summary>
        [SerializeField]
        public RigInfo[] PossibleInteractionRigs = new RigInfo[0];
        
        [Tooltip("Dummy Trainee object")]
        public GameObject DummyTrainee;

        /// <summary>
        /// Enforced provider will be use.
        /// </summary>
        protected static InteractionRigProvider enforcedProvider = null;
        
        private void Awake()
        {
            InteractionRigProvider rigProvider = null;
            
            if (enforcedProvider != null)
            {
                rigProvider = enforcedProvider;
            }
            else if (PossibleInteractionRigs != null)
            {
                rigProvider = FindAvailableInteractionRig();
            }

            if (rigProvider != null)
            {
                Vector3 position = Vector3.zero;
                Quaternion rotation = new Quaternion();

                if (DummyTrainee != null)
                {
                    position = DummyTrainee.transform.position;
                    rotation = DummyTrainee.transform.rotation;
                    DestroyImmediate(DummyTrainee);
                }

                GameObject prefab = rigProvider.GetPrefab();
                if (prefab != null)
                {
                    GameObject instance = Instantiate(rigProvider.GetPrefab());
                    instance.name = instance.name.Replace("(Clone)", "");
                    instance.transform.position = position;
                    instance.transform.rotation = rotation;
                }

                rigProvider.OnSetup();
            }
            else if (DummyTrainee != null)
            {
                DestroyImmediate(DummyTrainee);
            }
            Destroy(gameObject);
        }

        private InteractionRigProvider FindAvailableInteractionRig()
        {
            IEnumerable<InteractionRigProvider> availableRigs = ReflectionUtils.GetFinalImplementationsOf<InteractionRigProvider>()
                .Select(type => (InteractionRigProvider) ReflectionUtils.CreateInstanceOfType(type))
                .Where(provider => provider.CanBeUsed());

            foreach (RigInfo rigInfo in PossibleInteractionRigs)
            {
                if (rigInfo.Enabled)
                {
                    InteractionRigProvider provider = availableRigs.FirstOrDefault(p => p.Name == rigInfo.Name);
                    if (provider != null)
                    {
                        return provider;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Enforces the giving Rig to be used, if possible.
        /// </summary>
        /// <param name="prefab">Prefab of the rig to be used.</param>
        public static void SetEnforcedInteractionRig(InteractionRigProvider provider)
        {
            enforcedProvider = provider;
        }
    }
}