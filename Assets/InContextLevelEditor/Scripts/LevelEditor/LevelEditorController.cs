using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InContextLevelEditor.Strategy;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using InContextLevelEditor.UI;

namespace InContextLevelEditor.LevelEditor
{
    enum InteractionState
    {
        Translate,
        Rotate
    }

    public class LevelEditorController : MonoBehaviour
    {
        public IEntity SelectedEntity {get; private set;}
        InteractionState CurrentInteraction;

        public string EntityToPlaceAddress {get; private set;}
        public List<GameObject> SpawnedEntities {get; private set;}
        
        [SerializeField] string defaultEntityAddress;
        [SerializeField] List<string> entityAddresses;

        [SerializeField] MainWindowUIController mainWindowUIController;

        void Awake()
        {
            CurrentInteraction = InteractionState.Rotate;
            SpawnedEntities = new List<GameObject>();

            LoadAssets();
            SetEntityToPlace(defaultEntityAddress);

            mainWindowUIController.OnButtonPressHandler += OnEntitySelect;
        }

        private void OnEntitySelect(object sender, EntitySelectionEventArgs e)
        {
            EntityToPlaceAddress = e.AssetAddress;
        }

        void LoadAssets()
        {
            foreach(string address in entityAddresses)
            {
                Addressables.LoadAssetAsync<GameObject>(address);
            }
        }
        
        void SetEntityToPlace(string newEntityAddress)
        {
            EntityToPlaceAddress = newEntityAddress;
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
                StartCoroutine(SpawnGameObjectAtMousePosition(EntityToPlaceAddress, position));
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

        IEnumerator SpawnGameObjectAtMousePosition(string assetAddress, Vector2 mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Addressables.InstantiateAsync(assetAddress, hit.point, Quaternion.identity).Completed += OnInstantiateEntity;   
            }
            yield return null;
        }

        private void OnInstantiateEntity(AsyncOperationHandle<GameObject> obj)
        {
            GameObject go = obj.Result;
            SpawnedEntities.Add(go);
            SelectEntity(go.GetComponent<IEntity>());   
        }

        public void SelectEntity(IEntity newEntity)
        {
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

        void DetermineAction(IEntity entity, InteractionState currentInteraction, InputAction action)
        {

            if(currentInteraction == InteractionState.Translate)
            {
                Debug.Log("Translate");
                Translate translate = new Translate(entity, action);
                translate.Execute();
            }

            if(currentInteraction == InteractionState.Rotate)
            {
                Rotate rotate = new Rotate(entity, action);
                rotate.Execute();
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