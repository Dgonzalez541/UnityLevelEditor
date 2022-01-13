using System.Collections;
using InContextLevelEditor.LevelEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InContextLevelEditor.Strategy
{
    class Translate : Strategy
    {
        InputAction inputAction;
        float mouseDragSpeed = .1f;
        Vector3 velocity = Vector3.zero;

        public Translate(IEntity entity, InputAction inputAction) : base(entity)
        {
            this.entity = entity;
            this.inputAction = inputAction;
        }

        public override void Execute()
        {
            MonoBehaviour mono = (MonoBehaviour) entity.GameObject.GetComponent<IEntity>();
            mono.StartCoroutine(DragTranslate());
        }

        IEnumerator DragTranslate()
        {
            float initialDistance = Vector3.Distance(entity.GameObject.transform.position, Camera.main.transform.position);
            while(inputAction.ReadValue<float>() != 0)
            {
                Debug.Log($"DRAGTRANSLATE: {Mouse.current.position.ReadValue()}");
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                entity.GameObject.transform.position = Vector3.SmoothDamp(entity.GameObject.transform.position, 
                                                                            ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                yield return null;
            }

        }
    }
}