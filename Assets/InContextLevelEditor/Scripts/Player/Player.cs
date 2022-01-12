using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using InContextLevelEditor.Input;
using InContextLevelEditor.LevelEditor;
using UnityEngine.EventSystems;
using InContextLevelEditor.Strategy;

namespace InContextLevelEditor.Player
{
    enum InteractionState
    {
        Translate,
        Rotate
    }

    [RequireComponent(typeof(EntitySpawner))]
    public class Player : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;

        private AA_FlyingCamera m_Controls;
        private Vector2 m_Rotation;

        [SerializeField] EntitySpawner entitySpawner;

        Camera mainCamera;

        [SerializeField] PlayerInput playerInput; 
        private PointerEventData m_PointerData;
        private List<RaycastResult> m_RaycastResults = new List<RaycastResult>();

        public IEntity SelectedEntity {get; private set;}

        InteractionState CurrentInteraction;

        [SerializeField] GameObject testingEntity;

        public void Awake()
        {
            m_Controls = new AA_FlyingCamera();
            m_Controls.Player.Fire.performed += Fire;

            mainCamera = GetComponent<Camera>();

            IEntity entity = testingEntity.GetComponent<IEntity>();
            entitySpawner = GetComponent<EntitySpawner>();
            entitySpawner.SetEntityToPlace(entity);

            CurrentInteraction = InteractionState.Translate;
            SelectedEntity = entity;
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            if (!obj.performed)
            return;

            var device = playerInput.GetDevice<Pointer>();

            if (device == null)
            return;
        
            Vector2 position = device.position.ReadValue();
            if (IsRaycastHittingUIObject(position))     
            return;

            IEntity entity = IsRaycastHittingEntity(position);
            if(entity != null)
            {
                Debug.Log($"Hit entity {entity.GameObject}");

                if(entity != SelectedEntity)
                {
                    Debug.Log("Entity is not selected entity");
                    SetEntity(entity.GameObject);
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
                GameObject entityObject = entitySpawner.SpawnGameObjectAtMousePosition(testingEntity, position);
                entity = entityObject.GetComponent<IEntity>();
                SetEntity(entityObject);
                EnableEntityInteractions(entity);
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

        public void SetEntity(GameObject newEntityGameObject)
        {
            SelectedEntity.Unhighlight();
            DisableEntityInteractions(SelectedEntity);

            SelectedEntity = newEntityGameObject.GetComponent<IEntity>();
            SelectedEntity.Highlight();
        }

        public void DisableActions()
        {
            throw new NotImplementedException();
            //Diable Color change
            //Disable Intensity change;
        }

        private bool IsRaycastHittingUIObject(Vector2 position)
        {
            if (m_PointerData == null)
                m_PointerData = new PointerEventData(EventSystem.current);
            m_PointerData.position = position;
            EventSystem.current.RaycastAll(m_PointerData, m_RaycastResults);
            return m_RaycastResults.Count > 0;
        }

        private IEntity IsRaycastHittingEntity(Vector2 mousePosition)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                IEntity entity = hit.collider.gameObject.GetComponentInChildren<IEntity>();
                return entity;       
            }
            return null;
        }

        public void OnEnable()
        {
            m_Controls.Enable();
        }

        public void OnDisable()
        {
            m_Controls.Disable();
        }

        public void Update()
        {
            var look = m_Controls.Player.Look.ReadValue<Vector2>();
            var move = m_Controls.Player.Move.ReadValue<Vector2>();
            var elevate = m_Controls.Player.Elevate.ReadValue<float>();
            
            Look(look);
            Move(move);
            Elevate(elevate);
        }

        private void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.01)
                return;
            var scaledMoveSpeed = moveSpeed * Time.deltaTime;
            var move = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(direction.x, 0, direction.y);
            transform.position += move * scaledMoveSpeed;
        }

        private void Elevate(float direction)
        {
            var scaledMoveSpeed = moveSpeed * Time.deltaTime;
            var move = new Vector3(0,direction,0);
            transform.position += move * scaledMoveSpeed;
        }

        private void Look(Vector2 rotate)
        {
            if (rotate.sqrMagnitude < 0.01)
                return;
            var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
            m_Rotation.y += rotate.x * scaledRotateSpeed;
            m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
            transform.localEulerAngles = m_Rotation;
        }
    }
}