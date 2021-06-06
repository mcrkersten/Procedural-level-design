using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDetailPlacementFixer : MonoBehaviour
{
    [SerializeField] LayerMask _mask;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit groundDetection;
        if (Physics.Raycast(this.transform.position, Vector3.down, out groundDetection, Mathf.Infinity, _mask))
        {
            this.transform.position = groundDetection.point;
        }
    }
}
