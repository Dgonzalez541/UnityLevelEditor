using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Light))]
    public class SpotLightEntity : MonoBehaviour, ILightEntity
    {
        public float Intensity {get; private set;}

        public Light Light {get; private set;}

        public GameObject GameObject {get{return gameObject;}}

        [SerializeField] GameObject Visualizer;

        public void Highlight()
        {
            Visualizer.SetActive(true);
        }

        public void Unhighlight()
        {
            Visualizer.SetActive(false);
        }

        void Start()
        {
            Light = GetComponent<Light>();
        }
    }
}