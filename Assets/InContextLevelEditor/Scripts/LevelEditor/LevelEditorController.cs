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
    public enum InteractionState
    {
        Translate,
        Rotate,
        Paint,
        Intensity
    }

    public class InteractionStateEventArgs : EventArgs
    {
        public InteractionState Interaction {get; set;}
        public bool Enabled {get; set;}
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

        public event EventHandler<InteractionStateEventArgs> OnInteractionStateChagneHandler;

        void Awake()
        {
            
            SpawnedEntities = new List<GameObject>();

            LoadAssets();
            SetEntityToPlace(defaultEntityAddress);

            mainWindowUIController.OnButtonPressHandler += OnEntitySelect;
            mainWindowUIController.OnInteractionButtonPressHandler += OnInteractionSelect;
            mainWindowUIController.OnIntensitySliderChangeHandler += OnIntensityChange;
            mainWindowUIController.OnRGBColorChangeHandler += OnRGBColorChange;

            CurrentInteraction = InteractionState.Translate;            
        }

        

        void OnDestroy()
        {

        }

        private void OnInteractionSelect(object sender, InteractionSelectionEventArgs e)
        {
            CurrentInteraction = e.Interaction;
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
            SetEntityInteractions(SelectedEntity, false);
            UnhighlightAllEntities();

            SelectedEntity = newEntity;
            SelectedEntity.Highlight();
            SetEntityInteractions(SelectedEntity, true);
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
                Translate translate = new Translate(entity, action);
                translate.Execute();
            }

            if(currentInteraction == InteractionState.Rotate)
            {
                Rotate rotate = new Rotate(entity, action);
                rotate.Execute();
            }
        }

        void SetEntityInteractions(IEntity entity, bool enabled)
        {

            if(entity is IShapeEntity)
            {
                InteractionStateEventArgs args = new InteractionStateEventArgs();
                args.Interaction = InteractionState.Paint;
                args.Enabled = enabled;
                var handler = OnInteractionStateChagneHandler;
                if(handler != null)
                    OnInteractionStateChagneHandler(this, args);
            }

            if(entity is ILightEntity)
            {
                InteractionStateEventArgs args = new InteractionStateEventArgs();
                args.Interaction = InteractionState.Intensity;
                args.Enabled = enabled;
                var handler = OnInteractionStateChagneHandler;
                if(handler != null)
                    OnInteractionStateChagneHandler(this, args);
            }
        }

        private void OnIntensityChange(object sender, IntensitySliderChangeEventArgs e)
        {
            if(SelectedEntity is ILightEntity)
            {
                var lightEntity = SelectedEntity as ILightEntity;
                Intensity intensity = new Intensity(lightEntity, e.Intensity);
                intensity.Execute();
            }
        }

        private void OnRGBColorChange(object sender, RGBChangeEventArgs e)
        {
            if(SelectedEntity is IShapeEntity)
            {
                var shapeEntity = SelectedEntity as IShapeEntity;
                ColorChange colorChange = new ColorChange(shapeEntity, e.Color);
                colorChange.Execute();
            }
        }
    }

    
}