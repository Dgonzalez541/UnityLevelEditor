using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InContextLevelEditor.LevelEditor
{
    public class Entity : MonoBehaviour
    {

        public void Translate()
        {
            throw new NotImplementedException();
        }

        public void Rotate()
        {
            throw new NotImplementedException();
        }

        public void Scale()
        {
            throw new NotImplementedException();
        }
    }

    interface IShape
    {
        Mesh Mesh {get;}
        Color Color {get;}

        void SetColor(Color color);
        void SetMesh(Mesh mesh);
    }

    [RequireComponent(typeof(Mesh))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class Shape : Entity, IShape
    {
        public Mesh Mesh {get; private set;}
        public Color Color {get; private set;}

        Renderer rend;
        MeshFilter meshFilter;

        void Start()
        {
            rend = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetColor(Color color)
        {
            Color = color;
            rend.material.color = color;
        }

        public void SetMesh(Mesh mesh)
        {
            Mesh = mesh;
            meshFilter.mesh = mesh;
        } 
    }
}