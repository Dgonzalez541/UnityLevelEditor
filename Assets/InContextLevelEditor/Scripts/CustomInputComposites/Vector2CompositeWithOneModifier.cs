using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEditor;

namespace InContextLevelEditor.CustomInputComposites
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif
    [DisplayStringFormat("{firstPart}+{secondPart}")]
    public class Vector2CompositeWithOneModifier : Vector2Composite
    {
    
        [InputControl(layout = "Button")]
        public int modifier;
    
    #if UNITY_EDITOR
        static Vector2CompositeWithOneModifier()
        {
            Initialize();
        }
    #endif
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            InputSystem.RegisterBindingComposite<Vector2CompositeWithOneModifier>();
        }
    
        public override Vector2 ReadValue(ref InputBindingCompositeContext context)
        {
            bool modifierValue = context.ReadValue<float>(modifier) > float.Epsilon;
            return modifierValue ? base.ReadValue(ref context) : Vector2.zero;
        }
    
        public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
        {
            return ReadValue(ref context).magnitude;
        }
    }
}