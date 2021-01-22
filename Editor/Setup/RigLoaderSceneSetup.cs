using Innoactive.Creator.BasicInteraction.Setup;
using Innoactive.Creator.Core.Properties;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.Setup
{
    /// <summary>
    /// Setups the rig loader, cleans up the scene and creates a dummy trainee. 
    /// </summary>
    public class RigLoaderSceneSetup : SceneSetup
    {
        /// <inheritdoc />
        public override int Priority { get; } = 10;
        
        /// <inheritdoc />
        public override string Key { get; } = "InteractionFrameworkSetup";
        
        /// <inheritdoc/>
        public override void Setup()
        {
            RemoveMainCamera();
            
            InteractionRigSetup setup = Object.FindObjectOfType<InteractionRigSetup>();
            if (setup == null)
            {
                SetupPrefab("[INTERACTION_RIG_LOADER]");
            }
            
            TraineeSceneObject trainee = Object.FindObjectOfType<TraineeSceneObject>();
            if (trainee == null)
            {
                SetupPrefab("[TRAINEE]");
            }
        }

        /// <summary>
        /// Removes current MainCamera.
        /// </summary>
        private void RemoveMainCamera()
        {
            if (Camera.main != null && Camera.main.transform.parent == null && Camera.main.gameObject.name != "[TRAINEE]")
            {
                Object.DestroyImmediate(Camera.main.gameObject);
            }
        }
    }
}