using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform TargetTransform;
    public Transform CameraTransform;
    public Transform PivotTransform;
    private Vector3 cameraPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler Instance { get; private set; }

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float MaximumPivot = 35f;
    public float MinimumPivot = -35f;

    private LayerMask ignoreLayers;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;


    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else
            Destroy(gameObject);
        defaultPosition = CameraTransform.localPosition.x;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, TargetTransform.position,
            ref cameraFollowVelocity, delta / followSpeed);  //Lerp(transform.position, TargetTransform.position, delta / followSpeed);
        transform.position = targetPosition;
        HandleCameraCollisions(delta);
    }
    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        lookAngle += (mouseX * lookSpeed) / delta;
        pivotAngle -= (mouseY * lookSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, MinimumPivot, MaximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.z = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        PivotTransform.localRotation = targetRotation;
    }
    public void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = CameraTransform.position - PivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (PivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(PivotTransform.position, hit.point);
            targetPosition = (dis - cameraCollisionOffset);
        }
        /*if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            //targetPosition = -minimumCollisionOffset;
        }*/
        cameraPosition.x = Mathf.Lerp(CameraTransform.localPosition.x, targetPosition, delta / 0.2f); //targetPosition; //CameraTransform.localPosition; 
        CameraTransform.localPosition = cameraPosition;
    }
}
