using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Light))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SpotLightEntity : MonoBehaviour, ILightEntity
    {
        [SerializeField] Material highlightMat;
        [SerializeField] Material unhighlightMat;
        
        public float Intensity {get; private set;}

        public Light Light {get; private set;}

        public GameObject GameObject {get{return gameObject;}}

        [SerializeField] GameObject Visualizer;

        public void Highlight()
        {
            GetComponent<Renderer>().material = highlightMat;
        }

        public void Unhighlight()
        {
            GetComponent<Renderer>().material = unhighlightMat;
        }

        void Start()
        {
            Light = GetComponent<Light>();
        }
    }
}