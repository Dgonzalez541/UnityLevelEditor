using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    interface IShape
    {
        Mesh Mesh {get;}
        Color Color {get;}

        void SetColor(Color color);
        void SetMesh(Mesh mesh);
    }
}