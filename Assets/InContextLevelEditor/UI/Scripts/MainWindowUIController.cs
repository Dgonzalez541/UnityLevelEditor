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

    public class IntensitySliderChangeEventArgs : EventArgs
    {
        public float Intensity {get; set;}
    }

    public class RGBChangeEventArgs : EventArgs
    {
        public Color Color {get; set;}
    }

    [RequireComponent(typeof(UIDocument))]
    public class MainWindowUIController : MonoBehaviour
    {
        [SerializeField] string CubeAddress;
        [SerializeField] string CylinderAddress;
        [SerializeField] string SphereAddress;
        [SerializeField] string SpotLightAddress;

        Toggle CubeToggle;
        Toggle CylinderToggle;
        Toggle SphereToggle;
        Toggle SpotlightToggle;
        HashSet<Toggle> EntityToggleGroup = new HashSet<Toggle>();

        Toggle TranslateToggle;
        Toggle RotateToggle;
        Toggle PaintToggle;
        Toggle IntensityToggle;
        HashSet<Toggle> ActionToggleGroup = new HashSet<Toggle>();

        Slider IntensitySlider;

        VisualElement RGBSliderPane;
        Slider RedSlider;
        Slider GreenSlider;
        Slider BlueSlider;

        public event EventHandler<EntitySelectionEventArgs> OnButtonPressHandler;
        public event EventHandler<InteractionSelectionEventArgs> OnInteractionButtonPressHandler;
        public event EventHandler<IntensitySliderChangeEventArgs> OnIntensitySliderChangeHandler;
        public event EventHandler<RGBChangeEventArgs> OnRGBColorChangeHandler;

        [SerializeField] LevelEditorController mainController;
        
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            CubeToggle = root.Q<Toggle>("CubeToggle");
            CylinderToggle = root.Q<Toggle>("CylinderToggle");
            SphereToggle = root.Q<Toggle>("SphereToggle");
            SpotlightToggle = root.Q<Toggle>("SpotLightToggle");

            EntityToggleGroup.Add(CubeToggle);
            EntityToggleGroup.Add(CylinderToggle);
            EntityToggleGroup.Add(SphereToggle);
            EntityToggleGroup.Add(SpotlightToggle);


            CubeToggle.RegisterValueChangedCallback(x => OnEntitySelectButtonPressed(CubeAddress, CubeToggle));
            CylinderToggle.RegisterValueChangedCallback(x => OnEntitySelectButtonPressed(CylinderAddress, CylinderToggle));
            SphereToggle.RegisterValueChangedCallback(x => OnEntitySelectButtonPressed(SphereAddress, SphereToggle));
            SpotlightToggle.RegisterValueChangedCallback(x => OnEntitySelectButtonPressed(SpotLightAddress, SpotlightToggle));

            TranslateToggle = root.Q<Toggle>("TranslateInteractionToggle");
            RotateToggle = root.Q<Toggle>("RotateInteractionToggle");
            PaintToggle = root.Q<Toggle>("PaintInteractionToggle");
            IntensityToggle = root.Q<Toggle>("IntensityInteractionToggle");

            ActionToggleGroup.Add(TranslateToggle);
            ActionToggleGroup.Add(RotateToggle);
            ActionToggleGroup.Add(PaintToggle);
            ActionToggleGroup.Add(IntensityToggle);

            TranslateToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Translate, TranslateToggle));
            RotateToggle.RegisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Rotate, RotateToggle));
            
            PaintToggle.RegisterValueChangedCallback(x => {
                OnInteractionSelectButtonPress(InteractionState.Paint, PaintToggle);
                RGBSliderPane.SetEnabled(PaintToggle.value);
                });
            IntensityToggle.RegisterValueChangedCallback(x => {
                OnInteractionSelectButtonPress(InteractionState.Intensity, IntensityToggle);
                IntensitySlider.SetEnabled(IntensityToggle.value);
                });
        
            IntensitySlider = root.Q<Slider>("IntensitySlider");
            IntensitySlider.RegisterValueChangedCallback(x => OnIntensitySliderChange(IntensitySlider));
        
            RGBSliderPane = root.Q<VisualElement>("RGBPane");
            RedSlider = root.Q<Slider>("RedSlider");
            GreenSlider = root.Q<Slider>("GreenSlider");
            BlueSlider = root.Q<Slider>("BlueSlider");

            RedSlider.RegisterValueChangedCallback(x => OnRGBColorChange());
            GreenSlider.RegisterValueChangedCallback(x => OnRGBColorChange());
            BlueSlider.RegisterValueChangedCallback(x => OnRGBColorChange());

            CubeToggle.value = true;
            TranslateToggle.value = true;
            IntensityToggle.SetEnabled(false);

            IntensitySlider.visible = false;
            IntensitySlider.SetEnabled(false);

            RGBSliderPane.visible = false;
            RGBSliderPane.SetEnabled(false);

            mainController.OnInteractionStateChagneHandler += OnInteractionStateChange;
        }

        void OnDestroy()
        {
            CubeToggle.UnregisterValueChangedCallback(x => OnEntitySelectButtonPressed(CubeAddress, CubeToggle));
            CylinderToggle.UnregisterValueChangedCallback(x => OnEntitySelectButtonPressed(CylinderAddress, CylinderToggle));
            SphereToggle.UnregisterValueChangedCallback(x => OnEntitySelectButtonPressed(SphereAddress, SphereToggle));
            SpotlightToggle.UnregisterValueChangedCallback(x => OnEntitySelectButtonPressed(SpotLightAddress, SpotlightToggle));

            TranslateToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Translate, TranslateToggle));
            RotateToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Rotate, RotateToggle));
            PaintToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Paint, PaintToggle));
            IntensityToggle.UnregisterValueChangedCallback(x => OnInteractionSelectButtonPress(InteractionState.Intensity, IntensityToggle));
        
            RedSlider.UnregisterValueChangedCallback(x => OnRGBColorChange());
            GreenSlider.UnregisterValueChangedCallback(x => OnRGBColorChange());
            BlueSlider.UnregisterValueChangedCallback(x => OnRGBColorChange());
        
            mainController.OnInteractionStateChagneHandler -= OnInteractionStateChange;
        }

        void OnInteractionStateChange(object sender, InteractionStateEventArgs e)
        {
            SetEnableInteraction(e.Interaction, e.Enabled);
        }

        void SetEnableInteraction(InteractionState interactionState, bool enabled)
        {
            RGBSliderPane.SetEnabled(false);
            IntensitySlider.SetEnabled(false);

            if(interactionState == InteractionState.Paint)
            {
                PaintToggle.SetEnabled(enabled);
                RGBSliderPane.visible = enabled;
                RGBSliderPane.SetEnabled(PaintToggle.value);
            }

            if(interactionState == InteractionState.Intensity)
            {
                IntensityToggle.SetEnabled(enabled);
                IntensitySlider.visible = enabled;
                IntensitySlider.SetEnabled(IntensityToggle.value);
            }
        }

        void OnEntitySelectButtonPressed(string assetAddress, Toggle activeToggle)
        {
            if(activeToggle.value)
            {
                SetTogglesFlase(activeToggle, EntityToggleGroup);
                EntitySelectionEventArgs args = new EntitySelectionEventArgs();
                args.AssetAddress = assetAddress;
                var handler = OnButtonPressHandler;
                if(handler != null)
                    handler(this, args);
            }
        }

        void OnInteractionSelectButtonPress(InteractionState interactionState, Toggle activeToggle)
        {
            if(activeToggle.value)
            {
                SetTogglesFlase(activeToggle, ActionToggleGroup);
                InteractionSelectionEventArgs args = new InteractionSelectionEventArgs();
                args.Interaction = interactionState;
                var handler = this.OnInteractionButtonPressHandler;
                if(handler != null)
                    handler(this, args);
            }
        }

        void SetTogglesFlase(Toggle activeToggle, HashSet<Toggle> toggleGroup)
        {
            foreach(Toggle toggle in toggleGroup)
            {
                if(toggle != activeToggle)
                    toggle.value = false;
            }
        }

        void OnIntensitySliderChange(Slider slider)
        {
            IntensitySliderChangeEventArgs args = new IntensitySliderChangeEventArgs();
            args.Intensity = slider.value;
            var handler = OnIntensitySliderChangeHandler;
            if(handler != null)
                handler(this, args);
        }

        void OnRGBColorChange()
        {
            RGBChangeEventArgs args = new RGBChangeEventArgs();

            float r = Mathf.InverseLerp(0,255, RedSlider.value);
            float g = Mathf.InverseLerp(0,255, GreenSlider.value);
            float b = Mathf.InverseLerp(0,255, BlueSlider.value);
            args.Color = new Color(r,g,b,255);

            var handler = OnRGBColorChangeHandler;
            if(handler != null)
                handler(this, args);
        }
    }
}