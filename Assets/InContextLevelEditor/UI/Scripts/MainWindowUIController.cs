using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace InContextLevelEditor.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainWindowUIController : MonoBehaviour
    {
        public Button CubeButton {get; private set;}
        public Button CylinderButton {get; private set;}
        public Button SphereButton {get; private set;}

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            CubeButton = root.Q<Button>("CubeButton");
            CylinderButton = root.Q<Button>("CylinderButton");
            SphereButton = root.Q<Button>("SphereButton");

        }

        void OnDestroy()
        {

        }

        void OnShapeButtonPressed()
        {
            
        }
    }
}