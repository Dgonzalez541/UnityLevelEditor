using System;
using System.Collections;
using System.Collections.Generic;
using InContextLevelEditor.LevelEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InContextLevelEditor.Strategy
{
    class Intensity : Strategy
    {
        float intensity;

        public Intensity(ILightEntity entity, float intensity) : base(entity)
        {
            this.entity = entity;
            this.intensity = intensity;
        }

        public override void Execute()
        {
            var lightEntity = entity as ILightEntity;
            lightEntity.Light.intensity = intensity;
        }
    }
}