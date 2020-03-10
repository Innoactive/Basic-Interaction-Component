using System;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Training.SceneObjects.Interaction.Properties
{
    public interface ITouchableProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> Touched;
        event EventHandler<EventArgs> Untouched;
        
        /// <summary>
        /// Is object currently touched.
        /// </summary>
        bool IsBeingTouched { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was touched.
        /// </summary>
        void FastForwardTouch();
    }
}