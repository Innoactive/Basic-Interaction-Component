using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Components.Runtime.SceneSetup
{
    public abstract class InteractionRigProvider : ScriptableObject
    {
        public bool Enabled = true;
        
        public abstract string Name { get; }
        public abstract string PrefabName { get; }

        public virtual bool CanBeUsed()
        {
            return true;
        }

        public GameObject GetPrefab()
        {
            return FindPrefab(PrefabName);
        }
        
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