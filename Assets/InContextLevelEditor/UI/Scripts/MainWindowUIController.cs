using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace InContextLevelEditor.UI
{
    public class EntitySelectionEventArgs : EventArgs
    {
        public string AssetAddress {get; set;}
    }

    [RequireComponent(typeof(UIDocument))]
    public class MainWindowUIController : MonoBehaviour
    {
        public Button CubeButton {get; private set;}
        public Button CylinderButton {get; private set;}
        public Button SphereButton {get; private set;}

        [SerializeField] string CubeAddress;
        [SerializeField] string CylinderAddress;
        [SerializeField] string SphereAddress;

        public event EventHandler<EntitySelectionEventArgs> OnButtonPressHandler;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            CubeButton = root.Q<Button>("CubeButton");
            CylinderButton = root.Q<Button>("CylinderButton");
            SphereButton = root.Q<Button>("SphereButton");

            CubeButton.clicked += (() => OnButtonPressed(CubeAddress));
            CylinderButton.clicked += (() => OnButtonPressed(CylinderAddress));
            SphereButton.clicked += (() => OnButtonPressed(SphereAddress));
        }

        void OnDestroy()
        {
            CubeButton.clicked -= (() => OnButtonPressed(CubeAddress));
            CylinderButton.clicked -= (() => OnButtonPressed(CylinderAddress));
            SphereButton.clicked -= (() => OnButtonPressed(SphereAddress));
        }

        void OnButtonPressed(string assetAddress)
        {
            EntitySelectionEventArgs args = new EntitySelectionEventArgs();
            args.AssetAddress = assetAddress;
            OnButtonPressHandler(this, args);
        }
    }
}