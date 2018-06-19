using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceInput : MonoBehaviour {

    public float rotSpeed = 20f;

    public void SetDirection (Vector3 inputPosition)
    {
        Vector3 inputWorldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

        Vector3 direction = inputWorldPosition - transform.position;
        direction.Normalize();

        float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotSpeed);
    }
}
