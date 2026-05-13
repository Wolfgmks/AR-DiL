using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArPlace : MonoBehaviour
{
    //ARRaycasManager es un elemento directo de la libreria de AR Foundation que nos permite saber donde estamos tocando en una pantalla con camara
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject ArPrefab;
    [SerializeField] private float CoolDown = 0.1f;
    private readonly List<ARRaycastHit> raycastHits = new();
    private bool isPlacing;

    private void Update()
    {
        //verificaciones
        if (raycastManager == null || ArPrefab == null)
            return;
        if (isPlacing)
            return;
        if(!GetInputPos(out Vector2 inputPosition))
            return;

        GetPlace(inputPosition);
    }

    private bool GetInputPos(out Vector2 inputPosition)
    {
        
        inputPosition = default;
        //si el dedo toca la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                inputPosition = touch.position;
                return true;                
            }
        }
        //opcion de pc para testing
        if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            return true;   
        }

        //si el dedo se separa de la pantalla
        return false;
    }
    private void GetPlace(Vector2 inputPosition)
    {
        bool hasHit = raycastManager.Raycast( inputPosition, raycastHits, TrackableType.PlaneWithinPolygon);
        
        if (!hasHit || raycastHits.Count == 0)
            return;

        Pose hitPose = raycastHits[0].pose;

        Instantiate(ArPrefab, hitPose.position, hitPose.rotation);
        StartCoroutine(DelayCd());
    }

    private IEnumerator DelayCd()
    {
        isPlacing = true;
        yield return new WaitForSeconds(CoolDown);
        isPlacing = false;
    }
}
