using UnityEngine;
namespace InContextLevelEditor.LevelEditor
{
    public interface IEntity
    {
        GameObject GameObject {get;}
        
        void OnSelected();
        void OnDeselected();

        void Highlight();
        void Unhighlight();
    }
}