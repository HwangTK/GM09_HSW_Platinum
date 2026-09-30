using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThird : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] Transform _target;
    [Header("카메라")]
    [SerializeField] private Transform _cameraTr;


    [SerializeField] private Vector3 _thirdOffset = new Vector3(0f, 3f, -2f);
    [SerializeField] private float _thirdLookHeight = 1.5f;
    [Min(0f)]
    [SerializeField] private float _thirdSharpness = 20f;

    [SerializeField] private float _orbitSensitivity = 3.0f;

    [SerializeField] private float _orbitMinPitch = -10.0f;
    [SerializeField] private float _orbitMaxPitch = 25.0f;

    [SerializeField] private bool _thirdUseOrbit = true;

    private float _orbitYaw;
    private float _orbitPitch;


    private void InitThird(bool snap)
    {
        _orbitYaw = _target.eulerAngles.y;
        _orbitPitch = 12.0f;

        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _thirdSharpness, snap);


    }

    private void TickThird()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _thirdSharpness, false);


    }



    private void ApplyPose(Vector3 desiredPos, Quaternion desiredRot, float sharpness, bool snap)
    {
        if (snap)
        {
            _cameraTr.position = desiredPos;
            _cameraTr.rotation = desiredRot;

            return;
        }

        float t = GetSmoothT(sharpness);

        _cameraTr.position = Vector3.Lerp(_cameraTr.position, desiredPos, t);
        _cameraTr.rotation = Quaternion.Slerp(_cameraTr.rotation, desiredRot, t);

    }

    private float GetSmoothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }


    private void BuildThirdPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        if (Input.GetMouseButton(1))
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            _orbitYaw += mx * _orbitSensitivity;
            _orbitPitch -= my * _orbitSensitivity;

            _orbitPitch = Mathf.Clamp(_orbitPitch, _orbitMinPitch, _orbitMaxPitch);

        }

        if (_thirdUseOrbit)
        {
            Quaternion orbitRot = Quaternion.Euler(_orbitPitch, _orbitYaw, 0f);

            desiredPos = _target.position + (orbitRot * _thirdOffset);
            Vector3 lookPos = _target.position + Vector3.up * _thirdLookHeight;

            desiredRot = Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);


        }

        else
        {
            desiredPos = _target.position + (_target.rotation * _thirdOffset);

            Vector3 lookPos = _target.position + Vector3.up * _thirdLookHeight;
            desiredRot = Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);
        }

    }




    void Start()
    {
        InitThird(true);
    }



    void Update()
    {
        TickThird();
    }
}
