using System;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Base teleportation simulator, only have one concrete simulator implementation in your project.
    /// If no concrete implementation is found a <see cref="TeleportationSimulatorDummy"/> will be used.
    /// </summary>
    public abstract class BaseTeleportationSimulator
    {
        private static BaseTeleportationSimulator instance;
        
        /// <summary>
        /// Current instance of the interaction simulator.
        /// </summary>
        public static BaseTeleportationSimulator Instance
        {
            get
            {
                if (instance == null)
                {
                    Type type = ReflectionUtils
                        .GetConcreteImplementationsOf(typeof(BaseTeleportationSimulator))
                        .FirstOrDefault(t => t != typeof(TeleportationSimulatorDummy));

                    if (type == null)
                    {
                        type = typeof(TeleportationSimulatorDummy);
                    }

                    instance = (BaseTeleportationSimulator)ReflectionUtils.CreateInstanceOfType(type);
                    SceneManager.sceneUnloaded += OnSceneLoad;
                }

                return instance;
            }
        }

        private static void OnSceneLoad(Scene scene)
        {
            instance = null;
            SceneManager.sceneUnloaded -= OnSceneLoad;
        }
        
        /// <summary>
        /// Returns the base class used for teleportation in your VR framework.
        /// </summary>
        public abstract Type GetTeleportationBaseType();

        /// <summary>
        /// Executes a teleport action.
        /// </summary>
        /// <param name="rig">The rig object.</param>
        /// <param name="teleportationObject">The object with the teleportation logic or used to teleport into.</param>
        /// <param name="targetPosition">Desired position.</param>
        /// <param name="targetRotation">Desired rotation</param>
        public abstract void Teleport(GameObject rig, GameObject teleportationObject, Vector3 targetPosition, Quaternion targetRotation);

        /// <summary>
        /// True if the provided <paramref name="colliderToValidate"/> is an active collider of the <paramref name="teleportationObject"/>
        /// </summary>
        /// <param name="teleportationObject">The object with the teleportation logic or used to teleport into.</param>
        /// <param name="colliderToValidate">Collider to validate.</param>
        /// <returns></returns>
        public abstract bool IsColliderValid(GameObject teleportationObject, Collider colliderToValidate);
    }
}