using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{firstPart}+{secondPart}")]
public class Vector2WithOneModifier : InputBindingComposite<Vector2>
{
    [InputControl(layout = "Vector2")]
    public int vector2;
 
    [InputControl(layout = "Button")]
    public int modifier;
 
#if UNITY_EDITOR
    static Vector2WithOneModifier()
    {
        Initialize();
    }
#endif
 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        InputSystem.RegisterBindingComposite<Vector2WithOneModifier>();
    }
 
    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        bool modifierValue = context.ReadValue<float>(modifier) > float.Epsilon;
        return modifierValue ? context.ReadValue<Vector2, Vector2MagnitudeComparer>(vector2) : Vector2.zero;
    }
 
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context).magnitude;
    }
}
