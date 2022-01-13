using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using InContextLevelEditor.LevelEditor;

namespace InContextLevelEditor.UI
{
    public class EntitySelectionEventArgs : EventArgs
    {
        public string AssetAddress {get; set;}
    }

    public class InteractionSelectionEventArgs : EventArgs
    {
        public InteractionState Interaction {get; set;}
    }

    [RequireComponent(typeof(UIDocument))]
    public class MainWindowUIController : MonoBehaviour
    {
        [SerializeField] string CubeAddress;
        [SerializeField] string CylinderAddress;
        [SerializeField] string SphereAddress;
        [SerializeField] string SpotLightAddress;

        Button CubeButton;
        Button CylinderButton;
        Button SphereButton;
        Button SpotlightButton;

        Toggle TranslateToggle;
        Toggle RotateToggle;
        Toggle PaintToggle;
        Toggle IntensityToggle;
        HashSet<Toggle> ToggleGroup = new HashSet<Toggle>();

        public event EventHandler<EntitySelectionEventArgs> OnButtonPressHandler;
        public event EventHandler<InteractionSelectionEventArgs> OnInteractionButtonPressHandler;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            CubeButton = root.Q<Button>("CubeButton");
            CylinderButton = root.Q<Button>("CylinderButton");
            SphereButton = root.Q<Button>("SphereButton");
            SpotlightButton = root.Q<Button>("SpotLightButton");

            CubeButton.clicked += (() => OnEntitySelectButtonPressed(CubeAddress));
            CylinderButton.clicked += (() => OnEntitySelectButtonPressed(CylinderAddress));
            SphereButton.clicked += (() => OnEntitySelectButtonPressed(SphereAddress));
            SpotlightButton.clicked += (() => OnEntitySelectButtonPressed(SpotLightAddress));

            TranslateToggle = root.Q<Toggle>("TranslateInteractionToggle");
            RotateToggle = root.Q<Toggle>("RotateInteractionToggle");
            PaintToggle = root.Q<Toggle>("PaintInteractionToggle");
            IntensityToggle = root.Q<Toggle>("IntensityInteractionToggle");

            TranslateToggle.value = true;
            PaintToggle.SetEnabled(false);
            IntensityToggle.SetEnabled(false);

            ToggleGroup.Add(TranslateToggle);
            ToggleGroup.Add(RotateToggle);
            ToggleGroup.Add(PaintToggle);
            ToggleGroup.Add(IntensityToggle);

            TranslateToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Translate, TranslateToggle));
            RotateToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Rotate, RotateToggle));
            PaintToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Paint, PaintToggle));
            IntensityToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Intensity, IntensityToggle));
        }

        void OnDestroy()
        {
            CubeButton.clicked -= (() => OnEntitySelectButtonPressed(CubeAddress));
            CylinderButton.clicked -= (() => OnEntitySelectButtonPressed(CylinderAddress));
            SphereButton.clicked -= (() => OnEntitySelectButtonPressed(SphereAddress));
            SpotlightButton.clicked -= (() => OnEntitySelectButtonPressed(SpotLightAddress));

            TranslateToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Translate, TranslateToggle));
            RotateToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Rotate, RotateToggle));
            PaintToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Paint, PaintToggle));
            IntensityToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Intensity, IntensityToggle));
        }

        public void SetEnableToggle(InteractionState interactionState, bool enabled)
        {
            if(interactionState == InteractionState.Paint)
                PaintToggle.SetEnabled(enabled);
            if(interactionState == InteractionState.Intensity)
                IntensityToggle.SetEnabled(enabled);
        }

        void OnEntitySelectButtonPressed(string assetAddress)
        {
            EntitySelectionEventArgs args = new EntitySelectionEventArgs();
            args.AssetAddress = assetAddress;
            var handler = this.OnButtonPressHandler;
            if(handler != null)
                OnButtonPressHandler(this, args);
        }

        void OnInteractionSelectButtonPress(InteractionState interactionState, Toggle activeToggle)
        {
            if(activeToggle.value)
            {
                SetTogglesFlase(activeToggle);
                InteractionSelectionEventArgs args = new InteractionSelectionEventArgs();
                args.Interaction = interactionState;
                var handler = this.OnInteractionButtonPressHandler;
                if(handler != null)
                    OnInteractionButtonPressHandler(this, args);
            }
        }

        void SetTogglesFlase(Toggle activeToggle)
        {
            foreach(Toggle toggle in ToggleGroup)
            {
                if(toggle != activeToggle)
                    toggle.value = false;
            }
        }
    }
}