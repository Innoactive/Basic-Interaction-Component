namespace Innoactive.CreatorEditor.BasicInteraction
{
    /// <summary>
    /// This base class is supposed to be implemented by classes which will be called to setup the scene,
    /// specifically interaction frameworks.
    /// </summary>
    public abstract class OnInteractionFrameworkSetup : OnSceneSetup
    {
        /// <inheritdoc />
        public override string Key { get; } = "InteractionFrameworkSetup";
    }
}