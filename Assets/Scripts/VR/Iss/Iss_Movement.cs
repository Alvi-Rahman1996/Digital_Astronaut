using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iss_Movement : MonoBehaviour
{
    public float speed = 2f;

void Update()
{
    transform.position+= Vector3.forward * speed * Time.deltaTime;
    transform.Rotate(Vector3.forward, speed * Time.deltaTime);
}

}
