using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject following;
    void LateUpdate()
    {
        transform.position = following.transform.position + new Vector3(0, 0, -10);
    }
}
