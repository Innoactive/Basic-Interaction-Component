using System;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;

namespace Innoactive.Creator.BasicInteraction.Properties
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