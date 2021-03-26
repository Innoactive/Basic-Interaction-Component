using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.BasicInteraction
{
    /// <summary>
    /// Collects render information from a <see cref="IHighlighter"/> object and provides basic utilities for highlighting. 
    /// </summary>
    public abstract class AbstractHighlighter : MonoBehaviour, IHighlighter
    {
        [SerializeField, HideInInspector]
        protected Renderer[] renderers = {};
        
        [SerializeField, HideInInspector]
        protected MeshRenderer highlightMeshRenderer = null;
        
        [SerializeField , HideInInspector]
        protected MeshFilter highlightMeshFilter = null;
        
        /// <inheritdoc/>
        public abstract bool IsHighlighting { get; }

        /// <inheritdoc/>
        public abstract void StartHighlighting(Material highlightMaterial);

        /// <inheritdoc/>
        public abstract void StopHighlighting();
        
        /// <inheritdoc/>
        public abstract Material GetHighlightMaterial();

        protected void ClearCacheRenderers()
        {
            renderers = default;
        }

        protected void RefreshCachedRenderers()
        {
            if (highlightMeshRenderer != null && renderers != null && renderers.Any())
            {
                return;
            }
            
            if (highlightMeshRenderer == null)
            {
                GenerateHighlightRenderer();
            }
            else
            {
                highlightMeshRenderer.enabled = false;
                highlightMeshRenderer.gameObject.SetActive(false);
            }

            renderers = GetComponentsInChildren<SkinnedMeshRenderer>()
                .Where(skinnedMeshRenderer => skinnedMeshRenderer.gameObject.activeInHierarchy && skinnedMeshRenderer.enabled)
                .Concat<Renderer>(GetComponentsInChildren<MeshRenderer>()
                .Where(meshRenderer => meshRenderer.gameObject.activeInHierarchy && meshRenderer.enabled)).ToArray();

            if (renderers == null || renderers.Any() == false)
            {
                throw new NullReferenceException($"{name} has no renderers to be highlighted.");
            }

            GeneratePreviewMesh();
        }

        private void GenerateHighlightRenderer()
        {
            Transform child = transform.Find("Highlight Renderer");

            if (child == null)
            {
                child = new GameObject("Highlight Renderer").transform;
            }
            
            child.SetPositionAndRotation(transform.position, transform.rotation);
            child.SetParent(transform);
            
            highlightMeshFilter = child.gameObject.GetOrAddComponent<MeshFilter>();
            highlightMeshRenderer = child.gameObject.GetOrAddComponent<MeshRenderer>();

            highlightMeshRenderer.enabled = false;
            highlightMeshRenderer.gameObject.SetActive(false);
        }

        private void GeneratePreviewMesh()
        {
            List<CombineInstance> meshes = new List<CombineInstance>();

            Vector3 cachedPosition = transform.position;
            Quaternion cachedRotation = transform.rotation;
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            try
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.isPartOfStaticBatch)
                    {
                        throw new InvalidOperationException($"{name} is marked as 'Batching Static', no preview mesh to be highlighted could be generated at runtime.\n" +
                            "In order to fix this issue, please either remove the static flag of this GameObject or simply " +
                            "select it in edit mode so a preview mesh could be generated and cached.");
                    }
                    
                    Type renderType = renderer.GetType();

                    if (renderType == typeof(MeshRenderer))
                    {
                        MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                        
                        if (meshFilter.sharedMesh == null)
                        {
                            continue;
                        }
                        
                        meshes.AddRange(CollectMeshInformationFromMeshFilter(meshFilter));
                    }
                    else if (renderType == typeof(SkinnedMeshRenderer))
                    {
                        SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;

                        if (skinnedMeshRenderer.sharedMesh == null)
                        {
                            continue;
                        }

                        meshes.AddRange(CollectMeshInformationFromSkinnedMeshRenderer(skinnedMeshRenderer));
                    }
                }
            }
            finally
            {
                transform.SetPositionAndRotation(cachedPosition, cachedRotation);
            }
            
            if (meshes.Any())
            {
                Mesh previewMesh = new Mesh();
                previewMesh.CombineMeshes(meshes.ToArray());
                
                highlightMeshFilter.mesh = previewMesh;
            }
            else
            {
                throw new NullReferenceException($"{name} has no valid meshes to be highlighted.");
            }
        }

        private IEnumerable<CombineInstance> CollectMeshInformationFromMeshFilter(MeshFilter meshFilter)
        {
            List<CombineInstance> combinedInstances = new List<CombineInstance>();

            for (int i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
            {
                CombineInstance combineInstance = new CombineInstance
                {
                    subMeshIndex = i,
                    mesh = meshFilter.sharedMesh,
                    transform = meshFilter.transform.localToWorldMatrix
                };

                combinedInstances.Add(combineInstance);
            }

            return combinedInstances;
        }
        
        private IEnumerable<CombineInstance> CollectMeshInformationFromSkinnedMeshRenderer(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            List<CombineInstance> combinedInstances = new List<CombineInstance>();

            for (int i = 0; i < skinnedMeshRenderer.sharedMesh.subMeshCount; i++)
            {
                CombineInstance combineInstance = new CombineInstance
                {
                    subMeshIndex = i,
                    mesh = skinnedMeshRenderer.sharedMesh,
                    transform = skinnedMeshRenderer.transform.localToWorldMatrix
                };

                combinedInstances.Add(combineInstance);
            }

            return combinedInstances;
        }
        
        protected Material CreateHighlightMaterial(Color highlightColor)
        {
            Shader shader = GetShader();
            Material material = new Material(shader);
            material.color = highlightColor;

            // In case the color has some level of transparency,
            // we set the Material's Rendering Mode to Transparent. 
            if (Mathf.Approximately(highlightColor.a, 1f) == false)
            {
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            
            return material;
        }
        
        protected Material CreateHighlightMaterial(Texture mainTexture)
        {
            Shader shader = GetShader();
            Material material = new Material(shader);
            material.mainTexture = mainTexture;
            return material;
        }

        protected virtual Shader GetShader()
        {
            string shaderName = GraphicsSettings.currentRenderPipeline ? "Universal Render Pipeline/Lit" : "Standard";
            Shader defaultShader = Shader.Find(shaderName);

            if (defaultShader == null)
            {
                throw new NullReferenceException($"{name} failed to create a default material," + 
                    $" shader \"{shaderName}\" was not found. Make sure the shader is included into the game build.");
            }

            return defaultShader;
        }

        protected virtual bool CanObjectBeHighlighted()
        {
            if (enabled == false)
            {
                Debug.LogError($"{GetType().Name} component is disabled for {name} and can not be highlighted.", gameObject);
                return false;
            }
            
            if (gameObject.activeInHierarchy == false)
            {
                Debug.LogError($"{name} is disabled and can not be highlighted.", gameObject);
                return false;
            }

            return true;
        }
    }
}
