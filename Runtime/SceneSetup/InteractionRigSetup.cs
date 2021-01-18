using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Components.Runtime.SceneSetup;
using Innoactive.Creator.Core.Properties;
using UnityEngine;

/// <summary>
/// Will setup the interaction rig on awake. Priority is from top to bottom of the list, only rigs which full fill
/// all RigUseabilityChecker will be initialized.
/// </summary>
public class InteractionRigSetup : MonoBehaviour
{
    public List<InteractionRigProvider> PossibleInteractionRigs;

    protected static InteractionRigProvider enforcedProvider = null;
    
    void Awake()
    {
        InteractionRigProvider rigProvider = PossibleInteractionRigs.FirstOrDefault(provider => provider.Enabled && provider.CanBeUsed());

        if (enforcedProvider != null)
        {
            rigProvider = enforcedProvider;
        }

        if (rigProvider != null)
        {
            GameObject trainee = FindObjectOfType<TraineeSceneObject>().gameObject;
            Vector3 position = trainee.transform.position;
            Quaternion rotation = trainee.transform.rotation;
            
            DestroyImmediate(trainee);
            
            GameObject instance = Instantiate(rigProvider.GetPrefab());
            instance.name = instance.name.Replace("(Clone)", "");
            instance.transform.position = position;
            instance.transform.rotation = rotation;
        }
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
