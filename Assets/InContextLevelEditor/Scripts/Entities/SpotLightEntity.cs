using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Light))]
    public class SpotLightEntity : MonoBehaviour, ILightEntity
    {
        public float Intensity {get; private set;}

        public Light Light {get; private set;}

        public GameObject GameObject {get{return gameObject;}}

        public void Highlight()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeselected()
        {
            throw new System.NotImplementedException();
        }

        public void OnSelected()
        {
            throw new System.NotImplementedException();
        }

        public void Unhighlight()
        {
            throw new System.NotImplementedException();
        }

        void Start()
        {
            Light = GetComponent<Light>();

            //Create Directional Light
            Light.type = LightType.Spot;
            Light.intensity = 2;
        }
    }
}