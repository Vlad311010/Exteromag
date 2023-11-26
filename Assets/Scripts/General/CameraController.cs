using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera cinemachine;

    [SerializeField] Transform target;
    [SerializeField] Transform aim;
    [SerializeField] float maxOffset;
    [SerializeField] float aimWeight;

    private float offset;
    private Vector2 offsetDirection;
    
    void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();   
    }

    void Update()
    {
        offsetDirection = Vector2.up;
        float distance = Vector3.Distance(aim.position, target.position);
        offset = Mathf.Clamp(distance * aimWeight, 0, maxOffset);
        Vector3 cameraOffset = offsetDirection * offset;
        CinemachineFramingTransposer framingTransposer = cinemachine.GetComponentInChildren<CinemachineFramingTransposer>();
        framingTransposer.m_TrackedObjectOffset = cameraOffset;
    }
}
