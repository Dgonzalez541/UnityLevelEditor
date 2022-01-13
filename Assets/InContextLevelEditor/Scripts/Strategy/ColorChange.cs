using System.Collections;
using System.Collections.Generic;
using InContextLevelEditor.LevelEditor;
using UnityEngine;

namespace InContextLevelEditor.Strategy
{
    class ColorChange : Strategy
    {
        Color color;

        public ColorChange(IShapeEntity entity, Color color) : base(entity)
        {
            this.entity = entity;
            this.color = color;
        }

        public override void Execute()
        {
            var shapeEntity = entity as IShapeEntity;
            shapeEntity.SetColor(color);
        }
    }
}