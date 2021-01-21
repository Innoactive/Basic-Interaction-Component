using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Components.Runtime.SceneSetup
{
    /// <summary>
    /// Provides all information and methods to setup a scene with a fitting and working rig.
    /// </summary>
    public abstract class InteractionRigProvider
    {
        /// <summary>
        /// The name for this rig, has to be unique.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// Name of the prefab which should be loaded.
        /// </summary>
        public abstract string PrefabName { get; }

        /// <summary>
        /// Decides if this rig is useable at this moment. Can be overwritten to be more sophisticated.
        /// </summary>
        public virtual bool CanBeUsed()
        {
            return true;
        }

        /// <summary>
        /// Returns the found Prefab object.
        /// </summary>
        public virtual GameObject GetPrefab()
        {
            return FindPrefab(PrefabName);
        }

        /// <summary>
        /// Will be called when the scene is done setting up this rig to allow additional changes.
        /// </summary>
        public virtual void OnSetup()
        {
            // Allow additional actions.
        }
        
        /// <summary>
        /// Searches the given prefab name and returns it.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        protected GameObject FindPrefab(string prefab)
        {
            string filter = $"{prefab} t:Prefab";
            string[] prefabsGUIDs = AssetDatabase.FindAssets(filter, null);

            if (prefabsGUIDs.Any() == false)
            {
                throw new FileNotFoundException($"No prefabs found that match \"{prefab}\".");
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(prefabsGUIDs.First());
            string[] brokenPaths = Regex.Split(assetPath, "Resources/");
            string relativePath = brokenPaths.Last().Replace(".prefab", string.Empty);

            return Resources.Load(relativePath, typeof(GameObject)) as GameObject;
        }
    }
}