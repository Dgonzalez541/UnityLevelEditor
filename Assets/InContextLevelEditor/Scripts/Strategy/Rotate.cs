using InContextLevelEditor.LevelEditor;
using UnityEngine;

namespace InContextLevelEditor.Strategy
{
    class Rotate : Strategy
    {
        Vector2 mousePosition;

        Rotate(IEntity entity, Vector2 mousePosition) : base(entity)
        {
            this.entity = entity;
            this.mousePosition = mousePosition;
        }

        public override void Execute()
        {
            RotateEntity(mousePosition);
        }

        public void RotateEntity(Vector2 mousePosition)
        {
            Vector2 objectPosition = (Vector2) Camera.main.WorldToScreenPoint(entity.GameObject.transform.position);
            Vector2 direction = (mousePosition - objectPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            entity.GameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}