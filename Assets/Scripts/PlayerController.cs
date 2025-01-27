using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 1.0f;

    [SerializeField] private float gravForce = 10.0f;
    [SerializeField] private Vector3 gravDirection = Vector3.down;
    [SerializeField] private float rotateSpeed = 180.0f;

    [SerializeField] private Transform hoverModel;

    private Rigidbody rb;
    private Quaternion targetRotation;
    private Transform gravSurfaceNear;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        hoverModel.position = transform.position;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        targetRotation = transform.rotation;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gravSurfaceNear != null)
            {
                gravDirection = gravDirection == Vector3.down ? -gravSurfaceNear.up : Vector3.down;
            }
            else
            {
                gravDirection = Vector3.down;
            }

            targetRotation = Quaternion.LookRotation(transform.forward, -gravDirection);
        }

        //Rotate model slowly to correct direction
        hoverModel.rotation = Quaternion.Slerp(hoverModel.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //Rotate hitbox instantly to correct direction
        transform.rotation = targetRotation;
        transform.Rotate(-gravDirection, x * turnSpeed * Time.deltaTime, Space.World);

        Vector3 forwardVector = transform.forward * moveSpeed;
        transform.position = transform.position + (forwardVector * Time.deltaTime);
    }

    private void FixedUpdate()
    {

        Vector3 gravVector = gravDirection * gravForce;

        rb.AddForce(gravVector, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("GravitySurface"))
        {
            gravSurfaceNear = other.transform;
        }
    }

    private void OnTriggerLeave(Collider other)
    {
        if (other.tag.Equals("GravitySurface"))
        {
            gravSurfaceNear = null;
        }
    }
}
