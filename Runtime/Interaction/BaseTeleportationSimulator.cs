using System;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Type GetTeleportationBaseType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rig"></param>
        /// <param name="teleportationObject"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetRotation"></param>
        public abstract void Teleport(GameObject rig, GameObject teleportationObject, Vector3 targetPosition, Quaternion targetRotation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teleportationObject"></param>
        /// <param name="colliderToValidate"></param>
        /// <returns></returns>
        public abstract bool IsColliderValid(GameObject teleportationObject, Collider colliderToValidate);
    }
}