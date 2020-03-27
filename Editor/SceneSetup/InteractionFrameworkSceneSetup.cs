﻿namespace Innoactive.CreatorEditor.BasicInteraction
{
    /// <summary>
    /// This base class is supposed to be implemented by classes which will be called to setup the scene,
    /// specifically interaction frameworks.
    /// </summary>
    public abstract class InteractionFrameworkSceneSetup : SceneSetup
    {
        /// <inheritdoc />
        public override int Priority { get; } = 10;
        
        /// <inheritdoc />
        public override string Key { get; } = "InteractionFrameworkSetup";
    }
}