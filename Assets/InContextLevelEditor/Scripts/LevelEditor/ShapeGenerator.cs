using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InContextLevelEditor.LevelEditor
{
    public class ShapeGenerator : MonoBehaviour
    {
        public GameObject SelectedObject {get; private set;}
        [SerializeField] Camera mainCamera;

       public GameObject testingCube;

        void Start()
        {
            mainCamera = Camera.main;

            SetSelectedObject(testingCube);
        }

        public void SetSelectedObject(GameObject obj)
        {
            SelectedObject = obj;
        }

        public void SpawnGameObjectAtMousePosition()
        {
 
            Debug.Log($"Mouse: {Mouse.current}");

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            
 
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Hit: {hit.collider.gameObject}");
                Instantiate(SelectedObject, hit.point, Quaternion.identity);
            }
        }
    }
}