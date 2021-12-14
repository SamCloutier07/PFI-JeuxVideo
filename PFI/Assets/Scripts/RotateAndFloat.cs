using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndFloat : MonoBehaviour
{
  
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float height = 0.5f;
    
    [SerializeField]
    float degreesPerSecond = 120f;
    
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
        transform.Rotate(new Vector3(0,degreesPerSecond * Time.deltaTime,0));
    }
}
