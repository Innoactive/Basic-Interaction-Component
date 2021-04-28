using UnityEngine;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Can be used a script on GameObjects to exclude the mesh from the highlight.
    /// If you want to add this to your MonoBehaviour, use <see cref="IExcludeFromHighlightMesh"/>
    /// </summary>
    public sealed class ExcludeFromHighlightMesh : MonoBehaviour, IExcludeFromHighlightMesh
    {
        
    }
}