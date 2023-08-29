using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField]
    private List<Transform> _cameraPositions = new List<Transform>();
    [SerializeField]
    private float _cameraSize0 ;
    [SerializeField]
    private float _cameraSize1;
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
