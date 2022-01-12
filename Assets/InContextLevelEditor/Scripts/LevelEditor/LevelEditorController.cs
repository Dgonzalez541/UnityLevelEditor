using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InContextLevelEditor.Strategy;
using UnityEngine.InputSystem;

namespace InContextLevelEditor.LevelEditor
{
    enum InteractionState
    {
        Translate,
        Rotate
    }

    public class LevelEditorController : MonoBehaviour
    {
        [SerializeField] GameObject testingEntity;

        public IEntity SelectedEntity {get; private set;}
        InteractionState CurrentInteraction;

        public IEntity EntityToPlace {get; private set;}

        void Awake()
        {
            IEntity entity = testingEntity.GetComponent<IEntity>();
            EntityToPlace = entity;

            CurrentInteraction = InteractionState.Translate;
            SelectedEntity = entity;
        }

        public void SetEntity(IEntity newEntity)
        {
            SelectedEntity.Unhighlight();
            DisableEntityInteractions(SelectedEntity);

            SelectedEntity = newEntity;
            SelectedEntity.Highlight();
        }

        public IEntity DetermineHitEntity(Vector2 position, InputAction.CallbackContext obj)
        {
            IEntity entity = IsRaycastHittingEntity(position);
            if(entity != null)
            {
                Debug.Log($"Hit entity {entity.GameObject}");

                if(entity != SelectedEntity)
                {
                    Debug.Log("Entity is not selected entity");
                    SetEntity(entity);
                    EnableEntityInteractions(entity);
                }
                else //
                {
                    Debug.Log($"Hit selected entity");
                    DetermineAction(entity, CurrentInteraction, obj.action);
                }
            }
            else//Not hitting enity, so spawn new entity
            {
                Debug.Log("Did not hit an entity, spawingin new one");
                GameObject entityObject = SpawnGameObjectAtMousePosition(testingEntity, position);
                entity = entityObject.GetComponent<IEntity>();
                SetEntity(entity);
                EnableEntityInteractions(entity);
            }
            return entity; 
        }

        private IEntity IsRaycastHittingEntity(Vector2 mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                IEntity entity = hit.collider.gameObject.GetComponentInChildren<IEntity>();
                return entity;       
            }
            return null;
        }

        public GameObject SpawnGameObjectAtMousePosition(GameObject go, Vector2 mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                
                Instantiate(go, hit.point, Quaternion.identity);   
                return go;
            }
            return null;
        }

        void DetermineAction(IEntity entity, InteractionState currentInteraction, InputAction action)
        {
            if(currentInteraction == InteractionState.Translate)
            {
                Translate translate = new Translate(entity, action);
                translate.Execute();
            }

            if(currentInteraction == InteractionState.Rotate)
            {
                //Rotate rotate = new Rotate()
            }
        }

        void EnableEntityInteractions(IEntity entity)
        {
            //Enable Translate
            //Enable Rotate
            if(entity is IShapeEntity)
            {
                //Enable Color change
            }

            if(entity is IEntity)
            {
                //Enable Intensity change
            }
        }

        void DisableEntityInteractions(IEntity entity)
        {
            //Disable Translate
            //Disable Rotate

            if(entity is IShapeEntity)
            {
                //Enable Color change
            }

            if(entity is IEntity)
            {
                //Enable Intensity change
            }
        }
    }

    
}