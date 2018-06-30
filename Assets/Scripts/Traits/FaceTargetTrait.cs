using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTargetTrait : MonoBehaviour {

    public float rotSpeed = 150f;
    public List<GameObject> targetGameObjects = new List<GameObject>();

    private void Start()
    {
        targetGameObjects.Add(gameObject);
    }

    public void SetDirection (Vector3 targetPosition)
    {
        foreach(var go in targetGameObjects)
        {
            Vector3 direction = targetPosition - go.transform.position;
            direction.Normalize();

            float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            Quaternion desiredRotation = Quaternion.Euler(0f, 0f, zAngle);

            go.transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                desiredRotation,
                rotSpeed * Time.deltaTime);
        }

    }
}
