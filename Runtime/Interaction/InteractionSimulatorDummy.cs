using UnityEngine;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Interaction simulator dummy, does nothing beside of warning to there is no concrete implementation of a simulator.
    /// </summary>
    public class BaseInteractionSimulatorDummy : BaseInteractionSimulator
    {
        private const string ErrorMessage = "You are using the interaction simulator without providing a concrete implementation for your VR interaction framework.";
        
        /// <inheritdoc />
        public override void Touch(IInteractableObject interactable)
        {
            Debug.LogWarning(ErrorMessage);
        }

        /// <inheritdoc />
        public override void StopTouch()
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