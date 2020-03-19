using System;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.BasicInteraction.Properties
{
    public interface IUsableProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> UsageStarted;
        event EventHandler<EventArgs> UsageStopped;
        
        /// <summary>
        /// Is object currently used.
        /// </summary>
        bool IsBeingUsed { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was used.
        /// </summary>
        void FastForwardUse();
        
    }
}
