using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 1.0f;

    [SerializeField] private float gravForce = 10.0f;
    [SerializeField] private Vector3 gravDirection = Vector3.down;
    [SerializeField] private float rotateSpeed = 180.0f;
    [SerializeField] private float groundAlignSpeed = 45.0f;

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
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

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

            SwitchGrav();

            //Rotate hitbox instantly to correct direction
            transform.rotation = targetRotation;
        }

        //Rotate model slowly to correct direction
        hoverModel.rotation = Quaternion.RotateTowards(hoverModel.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.Rotate(-gravDirection, x * turnSpeed * Time.deltaTime, Space.World);

        Vector3 forwardVector = transform.forward * moveSpeed;
        transform.position = transform.position + (forwardVector * Time.deltaTime);


        //Angle to slope
        RaycastHit hit;
        var grounded = Physics.Raycast(transform.position, -transform.up, out hit, 2f, LayerMask.GetMask("Ground"));

        var slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
        Quaternion forward = Quaternion.LookRotation(transform.forward, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotation * forward, groundAlignSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 gravVector = gravDirection * gravForce;
        rb.AddForce(gravVector, ForceMode.Acceleration);
    }

    private void SwitchGrav()
    {
        targetRotation = Quaternion.LookRotation(transform.forward, -gravDirection);

        //targetRotation = Quaternion.FromToRotation(transform.up, -gravDirection);

        //rb.AddForce(-gravSurfaceNear.up * 10.0f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Quaternion rot = Quaternion.FromToRotation(transform.up, collision.transform.up);

        //transform.rotation = rot;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag.Equals("GravitySurface"))
        {
            gravDirection = Vector3.down;
            SwitchGrav();

            //Rotate hitbox instantly to correct direction
            transform.rotation = targetRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("GravitySurface"))
        {
            gravSurfaceNear = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("GravitySurface"))
        {
            gravSurfaceNear = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("GravitySurface"))
        {
            gravSurfaceNear = null;
        }
    }
}
