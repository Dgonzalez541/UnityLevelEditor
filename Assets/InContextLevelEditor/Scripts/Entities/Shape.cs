using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Mesh))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class Shape : MonoBehaviour, IShapeEntity
    {
        public Mesh Mesh {get; private set;}
        public Color Color {get; private set;}

        public GameObject GameObject {get {return gameObject;} set{}}

        MeshRenderer rend;
        MeshFilter meshFilter;

        [SerializeField] Material highlightMat;
        [SerializeField] Material unhighlightMat;
        [SerializeField] Material coloredMat;

        void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetColor(Color color)
        {
            Color = color;
            // if(rend == null)
            //     rend = gameObject.AddComponent<MeshRenderer>();
            coloredMat.color = color;

            Debug.Log($"Renderer: {GetComponent<Renderer>()}");
            Debug.Log($"material: {GetComponent<Renderer>().material}");

            GetComponent<Renderer>().material = coloredMat;
            
        }

        public void SetMesh(Mesh mesh)
        {
            Mesh = mesh;
            meshFilter.mesh = mesh;
        }

        public void Highlight()
        {
            GetComponent<Renderer>().material = highlightMat;
        }

        public void Unhighlight()
        {
            GetComponent<Renderer>().material = unhighlightMat;
        }
    }
}