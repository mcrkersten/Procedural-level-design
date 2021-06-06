using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWheatSizer : MonoBehaviour
{
    [SerializeField] LayerMask _mask;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = this.transform.position;
        if (position.y > 0f)
        {
            this.transform.localScale = new Vector3(1, position.y, 1);
            RaycastHit groundDetection;
            if (Physics.Raycast(this.transform.position, Vector3.down, out groundDetection, Mathf.Infinity, _mask))
            {
                this.transform.position = groundDetection.point;
            }
        }
    }
}
