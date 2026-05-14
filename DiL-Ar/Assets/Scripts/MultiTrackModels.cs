using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultiTrackModels : MonoBehaviour
{
    //libreria de ArTrackImagen para leer la referencia de las imagenes
    [SerializeField] private ARTrackedImageManager aRTrackedImageManager;
    //Array de GameObjects 
    [SerializeField] private GameObject[] models;
    private Dictionary<string, GameObject> arModels = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> arState = new Dictionary<string, bool>();
    [SerializeField] private Vector3 rotationOffset = new Vector3(-90, 0, 0);

    void Start()
    {
        foreach(var model in models)
        {
            GameObject newModel = Instantiate(model, Vector3.zero, Quaternion.identity);
            newModel.name = model.name;
            arModels.Add(model.name, newModel);
            newModel.SetActive(false);
            arState.Add(model.name, newModel);
        }
    }

    void Onable()
    {
        aRTrackedImageManager.trackablesChanged.AddListener(ImageFound);
    }

    void OnDisable()
    {
        aRTrackedImageManager.trackablesChanged.RemoveListener(ImageFound);
    }
    
    private void ImageFound(ARTrackablesChangedEventArgs<ARTrackedImage> Data)
    {
        foreach (var trackedImage in Data.updated)
        {
            if(trackedImage.trackingState == TrackingState.Tracking)
            {
                ShowModel(trackedImage);
            }
            else if(trackedImage.trackingState == TrackingState.Limited)
            {
                HideModel(trackedImage);
            }
        }
    }

    private void HideModel(ARTrackedImage trackedImage)
    {
        bool isModelActivated = arState[trackedImage.referenceImage.name];
        if (isModelActivated)
        {
            GameObject aRModel = arModels[trackedImage.referenceImage.name];
            aRModel.SetActive(false);
            arState[trackedImage.referenceImage.name] = false;
        }
    }

    private void ShowModel(ARTrackedImage trackedImage)
    {
        //realiza una comparativa de nombre para que coincida con el nombre del modelo e imagen
        string imageName = trackedImage.referenceImage.name;
        GameObject aRModel = arModels[imageName];
        //Se coloca sobre posición de imagen ademas de dar seguimiento 
        aRModel.transform.position = trackedImage.transform.position;
        //Se coloca siguiendo la rotación de la imagen ademas de ir actualizando
        aRModel.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(rotationOffset);
        //validación para en cuanto el estado sea verdadero mostrar el modelo
        if (!arState[imageName])
        {
            aRModel.SetActive(true);
            arState[imageName] = true;
        }
    }
}
