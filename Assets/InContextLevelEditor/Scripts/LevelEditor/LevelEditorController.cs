using System;
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
        public Type EntityToPlace {get; private set;}

        public List<GameObject> SpawnedEntities {get; private set;}

        void Awake()
        {
            IEntity entity = testingEntity.GetComponent<IEntity>();
            EntityToPlace = entity.GetType();

            CurrentInteraction = InteractionState.Translate;
            SelectedEntity = entity;

            SpawnedEntities = new List<GameObject>();
        }

        public void SelectEntity(IEntity newEntity)
        {
            SelectedEntity.Unhighlight();
            DisableEntityInteractions(SelectedEntity);
            UnhighlightAllEntities();

            SelectedEntity = newEntity;
            SelectedEntity.Highlight();
            EnableEntityInteractions(SelectedEntity);
        }

        void UnhighlightAllEntities()
        {
            foreach(var go in SpawnedEntities)
            {
                go.GetComponent<IEntity>().Unhighlight();
            }
        }

        public IEntity DetermineHitEntity(Vector2 position, InputAction.CallbackContext obj)
        {
            IEntity entity = IsRaycastHittingEntity(position);
            if(entity != null)
            {
                if(entity != SelectedEntity)
                {
                    SelectEntity(entity);
                }
                else
                {
                    DetermineAction(entity, CurrentInteraction, obj.action);
                }
            }
            else//Not hitting enity, so spawn new entity
            {
                GameObject entityObject = SpawnGameObjectAtMousePosition(testingEntity, position);
                entity = entityObject.GetComponent<IEntity>();

                SpawnedEntities.Add(entityObject);
                SelectEntity(entity);   
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
                
                GameObject instance = Instantiate(go, hit.point, Quaternion.identity) as GameObject;   
                return instance;
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