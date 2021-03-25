using System;
using System.Linq;
using Innoactive.Creator.BasicInteraction;
using Innoactive.Creator.Core.Utils;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.BasicInteraction
{
    public abstract class BaseInteractionSimulator
    {
        private static BaseInteractionSimulator instance;
        public static BaseInteractionSimulator Instance
        {
            get
            {
                if (instance == null)
                {
                    Type type = ReflectionUtils
                        .GetConcreteImplementationsOf(typeof(BaseInteractionSimulator))
                        .FirstOrDefault(t => t != typeof(BaseInteractionSimulatorDummy));

                    if (type == null)
                    {
                        type = typeof(BaseInteractionSimulatorDummy);
                    }

                    instance = (BaseInteractionSimulator)ReflectionUtils.CreateInstanceOfType(type);
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
        /// Simulates touching the given object. To un touch it again, run the specific method.
        /// </summary>
        public abstract void Touch(IInteractableObject interactable);

        /// <summary>
        /// Simulates un touching the given object.
        /// </summary>
        public abstract void UnTouch();

        /// <summary>
        /// Simulates grabbing the given object.
        /// </summary>
        public abstract void Grab(IInteractableObject interactable);

        /// <summary>
        /// Simulates releasing the given object.
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// Uses and keeps using the given object.
        /// </summary>
        public abstract void Use(IInteractableObject interactable);

        /// <summary>
        /// Stops using the given object.
        /// </summary>
        public abstract void StopUse(IInteractableObject interactable);
    }
}