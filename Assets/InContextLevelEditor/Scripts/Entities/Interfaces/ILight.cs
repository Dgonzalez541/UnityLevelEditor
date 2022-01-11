using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    interface ILight
    {
        float Intensity {get;}
        Light Light {get;}
    }
}