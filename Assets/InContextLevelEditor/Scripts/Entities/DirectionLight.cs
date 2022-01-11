using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Light))]
    public class SpotLight : Entity, ILight
    {
        public float Intensity {get; private set;}

        public Light Light {get; private set;}

        void Start()
        {
            Light = GetComponent<Light>();

            //Create Directional Light
            Light.type = LightType.Spot;
            Light.intensity = 2;
        }
    }
}