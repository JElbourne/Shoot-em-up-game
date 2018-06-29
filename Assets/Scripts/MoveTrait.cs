using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrait : MonoBehaviour {

    public void Move(float hor, float ver, float speed, bool rotatePosition = false)
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(hor * speed * Time.deltaTime, ver * speed * Time.deltaTime, 0f);

        if (rotatePosition)
        {
            pos += transform.rotation * velocity;
        } else
        {
            pos += velocity;
        }
        
        transform.position = pos;
    }
}
