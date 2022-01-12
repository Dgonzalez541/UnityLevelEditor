using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    interface IShapeEntity : IEntity
    {
        Mesh Mesh {get;}
        Color Color {get;}

        void SetColor(Color color);
        void SetMesh(Mesh mesh);
    }
}