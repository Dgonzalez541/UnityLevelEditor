using System.Collections;
using InContextLevelEditor.LevelEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InContextLevelEditor.Strategy
{
    class Rotate : Strategy
    {
        InputAction inputAction;
        Vector3 prevPos = Vector3.zero;
        Vector3 posDelta = Vector3.zero;

        public Rotate(IEntity entity, InputAction inputAction) : base(entity)
        {
            this.entity = entity;
            this.inputAction = inputAction;
        }

        public override void Execute()
        {
            MonoBehaviour mono = (MonoBehaviour) entity.GameObject.GetComponent<IEntity>();
            mono.StartCoroutine(RotateEntity());
        }

        public IEnumerator RotateEntity()
        {
            while(inputAction.ReadValue<float>() != 0)
            {
                var worldPos = Mouse.current.position.ReadValue();
                posDelta = (Vector3) Mouse.current.position.ReadValue() - prevPos;
                Transform transform = entity.GameObject.transform;
                if(Vector3.Dot(transform.up, Vector3.up) > 0)
                {
                    transform.Rotate(transform.up, -Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);
                }
                else
                {
                    transform.Rotate(transform.up, Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);
                }
                
                transform.Rotate(Camera.main.transform.right, Vector3.Dot(posDelta, Camera.main.transform.up), Space.World);
                prevPos = worldPos;
                yield return null;
            }
        }
    }
}