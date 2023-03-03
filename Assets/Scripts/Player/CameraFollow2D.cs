using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour {

    public float Speed = 25f;
    public float interpVelocity;

    public GameObject target;
    public Vector3 offset;

    [SerializeField] private Vector2 minBounary;
    [SerializeField] private Vector2 maxBounary;

    private Vector3 _targetPos;

    private void Start()
    {
        // start the camera at the players position
        _targetPos = transform.position;
    }


    void FixedUpdate()
    {
        if (target)
        {
            // remove the z component of transform
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            // get difference between target's position and camera's current position
            Vector3 targetDirection = (target.transform.position - posNoZ);

            // velocity with which to move the camera - Speed is defined as a public variable
            interpVelocity = targetDirection.magnitude * Speed;

            // calculate target position as the camera's current position * velocity * time
            _targetPos = transform.position + (targetDirection.normalized * (interpVelocity * Time.deltaTime));

            // smoothly update the position of the camera
            transform.position = Vector3.Lerp(transform.position, _targetPos + offset, 0.25f);

            // clamp the position of the camera to be within the min and max bounds set as variables
            transform.position = ClampCamera(transform.position);
        }
    }
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        var originalPosition = transform.position;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = ClampCamera(_targetPos);
    }

    private Vector3 ClampCamera(Vector3 desiredPosition)
    {
        return new Vector3(
            Mathf.Clamp(desiredPosition.x, minBounary.x, maxBounary.x),
            Mathf.Clamp(desiredPosition.y, minBounary.y, maxBounary.y),
            desiredPosition.z
        );
    }
}
