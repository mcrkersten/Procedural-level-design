using UnityEngine;


public class Billboard : MonoBehaviour
{
    Transform cam;
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(cam.position, Vector3.up);
    }
}