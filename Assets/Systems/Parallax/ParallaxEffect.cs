using System;
using UnityEngine;

namespace se.nondescriptgames.utility
{ 

    [SelectionBase]
    public class ParallaxEffect : MonoBehaviour
    {

        [Header("Near")]
        [SerializeField] Transform layerNear;
        [SerializeField] [Range(0f,1f)] float parallaxNearHorizontal = 0.7f;
        Vector3 startPosNear = Vector3.zero;
        [Header("Mid")]
        [SerializeField] Transform layerMid;
        [SerializeField][Range(0f, 1f)] float parallaxMidHorizontal = 0.5f;
        Vector3 startPosMid = Vector3.zero;
        [Header("Far")]
        [SerializeField] Transform layerFar;
        [SerializeField][Range(0f, 1f)] float parallaxFarHorizontal = 0.2f;
        Vector3 startPosFar = Vector3.zero;

        Camera cam;
        void Start()
        {
            if(layerNear == null || layerMid == null || layerFar == null) 
            {
                Debug.LogError("Missing parallax layer", this);
            }

            startPosNear = layerNear.position;
            startPosMid = layerMid.position;
            startPosFar = layerFar.position;

            cam = Camera.main;
        }

        void Update()
        {
            UpdateHorizontalPosition(layerNear, parallaxNearHorizontal, startPosNear);
            UpdateHorizontalPosition(layerMid, parallaxMidHorizontal, startPosMid);
            UpdateHorizontalPosition(layerFar, parallaxFarHorizontal, startPosFar);
        }

        private void UpdateHorizontalPosition(Transform layer, float factor, Vector3 startPos)
        {
            Vector3 Position = cam.transform.position;
            float Temp = Position.x * (1 - factor);
            float Distance = Position.x * factor;

            Vector3 NewPosition = new Vector3(startPos.x + Distance, layer.position.y, layer.position.z);

            layer.position = NewPosition;
        }
    }
}