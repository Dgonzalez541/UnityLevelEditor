using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    interface ILightEntity : IEntity
    {
        float Intensity {get;}
        Light Light {get;}
    }
}