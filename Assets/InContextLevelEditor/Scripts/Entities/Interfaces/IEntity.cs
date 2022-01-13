using UnityEngine;
namespace InContextLevelEditor.LevelEditor
{
    public interface IEntity
    {
        GameObject GameObject {get;}

        void Highlight();
        void Unhighlight();
    }
}