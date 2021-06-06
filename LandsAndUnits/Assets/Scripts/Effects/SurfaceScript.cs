using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimStuff
{
    public class SurfaceScript : MonoBehaviour
    {
        Vector3 SurfacePos;
        public Transform PlayerPos;
        public float speed = 2.0f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float interpolation = speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            position.z = Mathf.Lerp(this.transform.position.z, PlayerPos.transform.position.z, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, PlayerPos.transform.position.x, interpolation);

            this.transform.position = position;
        }
    }
}
