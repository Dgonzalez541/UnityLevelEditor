using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using InContextLevelEditor.Strategy;

namespace InContextLevelEditor.LevelEditor
{
    class EntitySpawner : MonoBehaviour
    {
        public IEntity EntityToPlace {get; private set;}

        public void SetEntityToPlace(IEntity entity)
        {
            EntityToPlace = entity;
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
    }
}