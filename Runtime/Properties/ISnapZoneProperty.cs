﻿using System;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using UnityEngine;

namespace Innoactive.Creator.BasicInteraction.Properties
{
    public interface ISnapZoneProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> ObjectSnapped;
        event EventHandler<EventArgs> ObjectUnsnapped;
        
        /// <summary>
        /// Currently has an object snapped into this snap zone.
        /// </summary>
        bool IsObjectSnapped { get; }
        
        /// <summary>
        /// Object which is snapped into this snap zone.
        /// </summary>
        ISnappableProperty SnappedObject { get; set; }
        
        /// <summary>
        /// Snap zone object.
        /// </summary>
        GameObject SnapZoneObject { get; }
        
        // TODO: Probably make a IConfigurable interface for modes
        void Configure(IMode mode);
    }
}