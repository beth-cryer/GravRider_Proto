using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //var pos = player.transform.position;

        //transform.position = Vector3.SmoothDamp(transform.position, pos + player.spinAxis.normalized * 15f, ref velocity, smoothTime);
        //transform.LookAt(pos);
    }
}
