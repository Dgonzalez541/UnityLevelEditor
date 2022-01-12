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
        Material coloredMat;

        bool selected = false;

        void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();

            selected = true;
        }

        public void SetColor(Color color)
        {
            Color = color;
            // if(rend == null)
            //     rend = gameObject.AddComponent<MeshRenderer>();
            coloredMat.color = color;
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

        public void OnSelected()
        {
            selected = true;
        }

        public void OnDeselected()
        {
            selected = false;
        }
    }
}