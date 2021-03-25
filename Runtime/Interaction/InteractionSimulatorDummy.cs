using Innoactive.Creator.BasicInteraction;
using UnityEngine;

namespace Innoactive.Creator.BasicInteraction
{
    public class BaseInteractionSimulatorDummy : BaseInteractionSimulator
    {
        private const string ErrorMessage = "You are using the interaction simulator without providing a concrete implementation of it your VR interaction framework.";
        
        /// <inheritdoc />
        public override void Touch(IInteractableObject interactable)
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void UnTouch()
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void Grab(IInteractableObject interactable)
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void Release()
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void Use(IInteractableObject interactable)
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void StopUse(IInteractableObject interactable)
        {
            Debug.LogWarning(ErrorMessage);
        }
    }
}