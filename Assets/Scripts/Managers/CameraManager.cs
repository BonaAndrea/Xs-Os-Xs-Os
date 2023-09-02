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

    [SerializeField] private float duration = 1f;
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveToPosition(int identifier)
    {
        float startSize = _mainCamera.orthographicSize;
        Vector3 startPosition = _mainCamera.transform.position;
        float startTime = Time.time;

        float newSize = (identifier==0)?_cameraSize0:_cameraSize1;
        Vector3 newPosition = _cameraPositions[identifier].position;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            _mainCamera.orthographicSize = Mathf.Lerp(startSize, newSize, t);
            _mainCamera.transform.localPosition = Vector3.Lerp(startPosition, newPosition, t);
            yield return null;
        }

        _mainCamera.orthographicSize = newSize;
        _mainCamera.transform.localPosition = newPosition;
    }
}
