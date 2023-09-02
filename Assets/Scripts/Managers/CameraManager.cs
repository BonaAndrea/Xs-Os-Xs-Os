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

        float newSize = _cameraSize0;
        Vector3 newPosition = _cameraPositions[0].position;

        // Prima animazione verso camerasize e position 0
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            _mainCamera.orthographicSize = Mathf.Lerp(startSize, newSize, t);
            _mainCamera.transform.localPosition = Vector3.Lerp(startPosition, newPosition, t);
            yield return null;
        }

        // Imposta le dimensioni e la posizione finali
        startSize = _mainCamera.orthographicSize;
        startPosition = _mainCamera.transform.position;
        newSize = (identifier == 0) ? _cameraSize0 : _cameraSize1;
        newPosition = _cameraPositions[identifier].position;

        // Animazione verso la posizione finale
        startTime = Time.time;
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
