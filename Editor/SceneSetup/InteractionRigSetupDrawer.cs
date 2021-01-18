using System.Linq;
using Innoactive.Creator.Components.Runtime.SceneSetup;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(InteractionRigSetup))]
internal class InteractionRigSetupDrawer : Editor 
{
    private ReorderableList list;
	
    private void OnEnable() 
    {
        InteractionRigSetup rigSetup = (InteractionRigSetup)target;

        rigSetup.PossibleInteractionRigs = rigSetup.PossibleInteractionRigs.FindAll(provider => provider != null);

        foreach (InteractionRigProvider provider in ReflectionUtils.GetFinalImplementationsOf<InteractionRigProvider>().Select(type => (InteractionRigProvider)CreateInstance(type)))
        {
            if (rigSetup.PossibleInteractionRigs.All(rigProvider => rigProvider.Name != provider.Name))
            {
                rigSetup.PossibleInteractionRigs.Add(provider);
            }  
        }
        
        list = new ReorderableList(serializedObject, 
            serializedObject.FindProperty("PossibleInteractionRigs"), 
            true, true, false, false);
        
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Available Interaction Rigs");
        };
        
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            string text = "null";
            if (rigSetup.PossibleInteractionRigs[index] != null)
            {
                text = rigSetup.PossibleInteractionRigs[index].Name;
            }
            EditorGUI.LabelField(labelRect, text);
            
            Rect toggleRect = new Rect(rect.x + labelRect.width, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            rigSetup.PossibleInteractionRigs[index].Enabled =
                EditorGUI.Toggle(toggleRect, rigSetup.PossibleInteractionRigs[index].Enabled);
        };

        list.drawFooterCallback = rect => { };

        list.onAddCallback = reorderableList =>
        {
            rigSetup.PossibleInteractionRigs.Add(null);
        };

        list.onRemoveCallback = reorderableList =>
        {
            rigSetup.PossibleInteractionRigs.RemoveAt(reorderableList.index);
        };
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Box("Enable/Disable available interaction Rigs, you are also able to prioritize them by changing the position in the array. Top most has the highest priority. The interaction Rig will be spawned at the [TRAINEE] GameObject.");
        
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
