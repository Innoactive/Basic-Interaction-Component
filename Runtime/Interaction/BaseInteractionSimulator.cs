using System;
using System.Linq;
using Innoactive.Creator.BasicInteraction;
using Innoactive.Creator.Core.Utils;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Base interaction simulator, only have one concrete simulator implementation in your project.
    /// If no concrete implementation is found a <see cref="BaseInteractionSimulatorDummy"/> will be used.
    /// </summary>
    public abstract class BaseInteractionSimulator
    {
        private static BaseInteractionSimulator instance;
        
        /// <summary>
        /// Current instance of the interaction simulator.
        /// </summary>
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
        /// Simulates touching the given object. Expected behavior is that the object stays touched until StopTouch is called.
        /// </summary>
        public abstract void Touch(IInteractableObject interactable);

        /// <summary>
        /// Simulates stop touching the given object.
        /// </summary>
        public abstract void StopTouch();

        /// <summary>
        /// Simulates grabbing the given object.
        /// </summary>
        public abstract void Grab(IInteractableObject interactable);

        /// <summary>
        /// Simulates releasing the given object.
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// Simulates usage of the object and keeps using the given object until StopUse is called.
        /// </summary>
        public abstract void Use(IInteractableObject interactable);

        /// <summary>
        /// Simulates stop using the given object.
        /// </summary>
        public abstract void StopUse(IInteractableObject interactable);
    }
}
