using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject dash, dot;

    public void Init(Vector3 direction, Vector3 preDirection) {
        if (Mathf.Abs(preDirection.y) == 1) {
            float y = preDirection.y * 0.4f;
            Instantiate(preDirection.y > 0 ? dash : dot, new Vector3(0, y, -1) + transform.position, Quaternion.Euler(0, 0, 90), transform);
            float x = direction.x * -0.4f;
            Instantiate(direction.x > 0 ? dot : dash, new Vector3(x, 0, -1) + transform.position, Quaternion.identity, transform);
        } else if (Mathf.Abs(preDirection.x) == 1) {
            float x = preDirection.x * 0.4f;
            Instantiate(preDirection.x > 0 ? dash : dot, new Vector3(x, 0, -1) + transform.position, Quaternion.identity, transform);
            float y = direction.y * -0.4f;
            Instantiate(direction.y > 0 ? dot : dash, new Vector3(0, y, -1) + transform.position, Quaternion.Euler(0, 0, 90), transform);
        }
    }
}
