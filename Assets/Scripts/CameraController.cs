using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSmoothingFactor = 1;
    public float lookUpMax = 60;
    public float lookUpMin = -60;
    public Transform _camera;

    private Quaternion camRotation;
    private Vector3 camOffset;
    private RaycastHit hit;

     void Start()
     {
        camRotation = transform.localRotation;
        camOffset = _camera.localPosition;
     }

     void FixedUpdate()
     {
        camRotation.x += Input.GetAxis("Mouse Y") * cameraSmoothingFactor * (-1);
        camRotation.y += Input.GetAxis("Mouse X") * cameraSmoothingFactor;

        camRotation.x = Mathf.Clamp(camRotation.x, lookUpMin, lookUpMax);

        transform.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, camRotation.z);

        if (Physics.Linecast(transform.position, transform.position + transform.localRotation * camOffset, out hit))
        {
            _camera.localPosition = new Vector3(0, 0, - Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, camOffset, Time.deltaTime);
        }
     }
}
