using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    [RequireComponent(typeof(Mesh))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class Shape : Entity, IShape
    {
        public Mesh Mesh {get; private set;}
        public Color Color {get; private set;}

        Renderer rend;
        MeshFilter meshFilter;

        void Start()
        {
            rend = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetColor(Color color)
        {
            Color = color;
            rend.material.color = color;
        }

        public void SetMesh(Mesh mesh)
        {
            Mesh = mesh;
            meshFilter.mesh = mesh;
        } 
    }
}