using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    [SerializeField] float _targetFov = 90;
    [SerializeField] Move _moveScript;
    [SerializeField] float _fovEaseInTimer = 0.3f;
    [SerializeField] float _fovEaseOutTimer = 0.6f;
    private bool transitionStarted = false;
    private float _initialFov;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _initialFov = _cinemachineCamera.Lens.FieldOfView;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_moveScript._isDashing && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(FovTransitionIn());
        }


    }

    IEnumerator FovTransitionIn()
    {
        Debug.Log("Transition");
        float time = 0;
        while (time < _fovEaseInTimer)
        {   
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_initialFov, _targetFov, time / _fovEaseInTimer);
            yield return null;
            time += Time.deltaTime;
        }
        // _cinemachineCamera.Lens.FieldOfView = _initialFov;
        transitionStarted = false;
        StartCoroutine(FovTransitionOut());
    }
    IEnumerator FovTransitionOut()
    {
        float time = 0;
        while (time < _fovEaseOutTimer)
        {
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_targetFov, _initialFov, time / _fovEaseOutTimer);
            yield return null;
            time += Time.deltaTime;
        }
        // _cinemachineCamera.Lens.FieldOfView = _initialFov;
    }


}
