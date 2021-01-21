using System;
using System.Collections.Generic;
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

        if (Application.isPlaying == false)
        {
            UpdateRigList(rigSetup);
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
            EditorGUI.LabelField(labelRect, rigSetup.PossibleInteractionRigs[index].Name);
            
            Rect toggleRect = new Rect(rect.x + labelRect.width, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            rigSetup.PossibleInteractionRigs[index].Enabled = EditorGUI.Toggle(toggleRect, rigSetup.PossibleInteractionRigs[index].Enabled);
        };

        list.drawFooterCallback = rect => { };
    }

    private static void UpdateRigList(InteractionRigSetup rigSetup)
    {
        List<InteractionRigSetup.RigInfo> rigs = rigSetup.PossibleInteractionRigs.ToList();

        IEnumerable<Type> foundTypes = ReflectionUtils.GetConcreteImplementationsOf<InteractionRigProvider>();
        IEnumerable<InteractionRigProvider> foundProvider = foundTypes.Select(type => (InteractionRigProvider) ReflectionUtils.CreateInstanceOfType(type));

        foreach (InteractionRigProvider provider in foundProvider)
        {
            if (rigs.All(rigProvider => rigProvider.Name != provider.Name))
            {
                rigs.Add(new InteractionRigSetup.RigInfo()
                {
                    Name =  provider.Name,
                    Enabled = true,
                }); 
            }
        }

        rigSetup.PossibleInteractionRigs = rigs.ToArray();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Box("Enable/Disable available interaction Rigs, you are also able to prioritize them by changing the position in the array. Top most has the highest priority. The interaction Rig will be spawned at the [TRAINEE] GameObject.");
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
