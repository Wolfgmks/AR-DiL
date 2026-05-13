using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceViewer : MonoBehaviour
{
    
    [SerializeField] private ARFaceManager arFace;
    [SerializeField]private GameObject point;
    private List<GameObject> marcador= new List<GameObject>();
    //Comportamiento para detectar cara se activa
    private void Onable()
    {
        arFace.trackablesChanged.AddListener(FaceMarkers);
    }
    //Comportamiento para detectar cara se desactiva
    private void OnDisable()
    {
        arFace.trackablesChanged.RemoveListener(FaceMarkers);
    }
    //Inicializa la deteccion de la cara y los puntos a colocar
    private void FaceMarkers(ARTrackablesChangedEventArgs<ARFace> Data)
    {
        //me crea la lectura de los puntos
        foreach (var face in Data.added)
        {
            var render = face.GetComponent<MeshRenderer>();
            if(render) render.enabled = false;
            var filter = face.GetComponent<MeshFilter>();
            if(filter) filter.mesh = null;

            MarcadorFace(face);
        }
        //Me actualiza la lectura de los puntos
        foreach (var face in Data.updated)
        {
            MarcadorFace(face);
        }
    }

    private void MarcadorFace(ARFace face)
    {
        var mesh = face.GetComponent<ARFaceMeshVisualizer>().mesh;

        if(mesh == null) return;
        var vertices = mesh.vertices;

        while (marcador.Count < vertices.Length)
        {
            var meshFace = Instantiate(point, face.transform);
            marcador.Add(meshFace);
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            marcador[i].transform.localPosition = vertices[i];
        }
    }
}
