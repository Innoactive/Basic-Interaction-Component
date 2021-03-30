using System;
using UnityEngine;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Interaction simulator dummy, does nothing beside of warning to there is no concrete implementation of a simulator.
    /// </summary>
    public class TeleportationSimulatorDummy : BaseTeleportationSimulator
    {
        private const string ErrorMessage = "You are using the teleportation simulator without providing a concrete implementation for your VR interaction framework.";
        
        /// <inheritdoc />
        public override Type GetTeleportationBaseType()
        {
            Debug.LogWarning(ErrorMessage);
            return null;
        }

        /// <inheritdoc />
        public override void Teleport(GameObject rig, GameObject teleportationObject, Vector3 targetPosition, Quaternion targetRotation)
        {
            Debug.LogWarning(ErrorMessage);
        }
    }
}
