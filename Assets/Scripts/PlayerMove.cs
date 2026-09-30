using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private Transform _cameraTr;

    [Header("플레이어")]
    [SerializeField] private Transform _player;

    [Header("이동속도")]
    [SerializeField] private float _walkSpeed = 3;
    [SerializeField] private float _runSpeed = 6;

    [Header("리지드바디")]
    [SerializeField] private Rigidbody _rb;

    [Header("점프력")]
    [SerializeField] private float _jumpPower;

    [Header("지면거리")]
    [SerializeField] private float _checkGroundDistance = 2.5f;

    [Header("애니메이터")]
    [SerializeField] private Animator _animator;


    private Vector3 _dir;


    void Start()
    {
        
    }



    void Update()
    {
        Move();
        Jump();
    }



    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        _dir = new Vector3(x, 0f, z);

        Vector3 forward = _cameraTr.transform.forward;
        Vector3 right = _cameraTr.transform.right;

        forward.y = 0f;
        right.y = 0f;


        _dir = (_dir.x * right) + (_dir.z * forward);
        _dir.Normalize();

        float currentspeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentspeed = _runSpeed;
        }

        else
        {
            currentspeed = _walkSpeed;
        }


            _player.transform.Translate(_dir * currentspeed * Time.deltaTime, Space.World);

        if (_dir != Vector3.zero)
        {
            _player.rotation = Quaternion.LookRotation(_dir);
        }



        if (_dir == Vector3.zero)
        {
            _animator.SetFloat("Speed", 0f);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetFloat("Speed", 1f);
        }
        else
        {
            _animator.SetFloat("Speed", 0.5f);
        }
    }
    

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            

            if (GroundCheck())
            {
                _animator.SetTrigger("Jump");
                _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);

            }

        }
    }
    
    private bool GroundCheck()
    {
        return Physics.Raycast(_player.position + Vector3.up * 0.3f, Vector3.down, _checkGroundDistance);
    }

}
