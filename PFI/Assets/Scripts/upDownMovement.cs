using UnityEngine;

public class upDownMovement : MonoBehaviour
{
   //adjust this to change speed
   public float speed = 5f;
   //adjust this to change how high it goes
   public float height = 0.5f;
    
   void Update() {
       Vector3 pos = transform.localPosition;
       float newY = Mathf.Sin(Time.time * speed);
       transform.localPosition = new Vector3(pos.x, newY, pos.z) * height;
   }
}