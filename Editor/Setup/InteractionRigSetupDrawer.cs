using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.BasicInteraction.Setup;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.Setup
{
    [CustomEditor(typeof(InteractionRigSetup))]
    internal class InteractionRigSetupDrawer : Editor
    {
        private ReorderableList list;

        private List<InteractionRigProvider> foundProvider;
        
        private GUIContent warningIcon;
        
        private void OnEnable()
        {
            InteractionRigSetup rigSetup = (InteractionRigSetup) target;
            
            if (Application.isPlaying == false)
            {
                UpdateRigList(rigSetup);
            }

            list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("PossibleInteractionRigs"),
                true, true, false, false);

            list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Available Interaction Rigs"); };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                bool canBeUsed = foundProvider[index].CanBeUsed();
                
                Rect labelRect = new Rect(rect.x, rect.y, rect.width - 2 * EditorGUIUtility.singleLineHeight - 4,
                    EditorGUIUtility.singleLineHeight);
                GUI.enabled = canBeUsed;
                EditorGUI.LabelField(labelRect, rigSetup.PossibleInteractionRigs[index].Name);
                GUI.enabled = true;
                
                if (!canBeUsed)
                {
                    Rect warningRect = new Rect(rect.x + labelRect.width - EditorGUIUtility.singleLineHeight - 4, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                    GUIContent labelContent = new GUIContent("", warningIcon.image, foundProvider[index].GetSetupTooltip());
                    EditorGUI.LabelField(warningRect, labelContent);
                }
                
                Rect toggleRect = new Rect(rect.x + labelRect.width, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                rigSetup.PossibleInteractionRigs[index].Enabled =
                    EditorGUI.Toggle(toggleRect, rigSetup.PossibleInteractionRigs[index].Enabled);
            };

            list.onReorderCallback = reorderableList =>
            {
                OrderFoundProvider(rigSetup);
            }; 

            list.drawFooterCallback = rect => { };
        }

        private void UpdateRigList(InteractionRigSetup rigSetup)
        {
            List<InteractionRigSetup.RigInfo> rigs = rigSetup.PossibleInteractionRigs.ToList();

            IEnumerable<Type> foundTypes = ReflectionUtils.GetConcreteImplementationsOf<InteractionRigProvider>();
            foundProvider = foundTypes.Select(type =>
                (InteractionRigProvider) ReflectionUtils.CreateInstanceOfType(type)).ToList();

            foreach (InteractionRigProvider provider in foundProvider)
            {
                if (rigs.All(rigProvider => rigProvider.Name != provider.Name))
                {
                    rigs.Add(new InteractionRigSetup.RigInfo()
                    {
                        Name = provider.Name,
                        Enabled = true,
                    });
                }
            }
            
            OrderFoundProvider(rigSetup);
            rigSetup.PossibleInteractionRigs = rigs.ToArray();
        }

        private void OrderFoundProvider(InteractionRigSetup rigSetup)
        {
            foundProvider = rigSetup.PossibleInteractionRigs
                .Select(info => foundProvider.Find(provider => provider.Name == info.Name)).ToList();
        }
        
        public override void OnInspectorGUI()
        {
            if (warningIcon == null)
            {
                warningIcon = EditorGUIUtility.IconContent("Warning@2x");
            } 
            
            GUILayout.Box(
                "Enable/Disable available interaction Rigs, you are also able to prioritize them by changing the position in the array. Top most has the highest priority. The interaction Rig will be spawned at the [TRAINEE] GameObject.");
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}