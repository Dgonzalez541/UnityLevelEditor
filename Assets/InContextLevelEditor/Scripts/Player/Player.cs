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
    public class Player : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;

        private AA_Player m_Controls;
        private Vector2 m_Rotation;

        [SerializeField] PlayerInput playerInput; 
        private PointerEventData m_PointerData;
        private List<RaycastResult> m_RaycastResults = new List<RaycastResult>();

        [SerializeField] LevelEditorController editorController;

        public void Awake()
        {
            m_Controls = new AA_Player();
            m_Controls.Player.Fire.performed += Fire;
        }

        public void OnEnable()
        {
            m_Controls.Enable();
        }

        public void OnDisable()
        {
            m_Controls.Disable();
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

            editorController.DetermineHitEntity(position, obj);
        }

        private bool IsRaycastHittingUIObject(Vector2 position)
        {
            if (m_PointerData == null)
                m_PointerData = new PointerEventData(EventSystem.current);
            m_PointerData.position = position;
            EventSystem.current.RaycastAll(m_PointerData, m_RaycastResults);
            return m_RaycastResults.Count > 0;
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