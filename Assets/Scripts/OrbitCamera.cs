// https://bitbucket.org/catlikecodingunitytutorials/movement-04-orbit-camera/src/master/Assets/Scripts/OrbitCamera.cs

using UnityEngine;

[RequireComponent (typeof (Camera))]
public class OrbitCamera : MonoBehaviour
{

  [SerializeField]
  Transform focus = default;

  [SerializeField, Range (1f, 20f)]
  float distance = 5f;

  [SerializeField, Range (1f, 1000f)]
  float rotationSpeed = 90f;

  [SerializeField, Range (-89f, 89f)]
  float minVerticalAngle = -45f, maxVerticalAngle = 45f;

  [SerializeField, Range (0f, 90f)]
  float alignSmoothRange = 45f;

  [SerializeField]
  LayerMask obstructionMask = -1;

  [SerializeField] private bool invertVertical;
  [SerializeField] private Vector3 focusOffset;
  public bool Activated = true;

  Camera regularCamera;

  Vector3 focusPoint, previousFocusPoint;

  Vector2 orbitAngles = new Vector2 (45f, 0f);

  float lastManualRotationTime;

  Vector3 CameraHalfExtends
  {
    get
    {
      Vector3 halfExtends;
      halfExtends.y =
        regularCamera.nearClipPlane *
        Mathf.Tan (0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
      halfExtends.x = halfExtends.y * regularCamera.aspect;
      halfExtends.z = 0f;
      return halfExtends;
    }
  }

  void OnValidate()
  {
    if (maxVerticalAngle < minVerticalAngle)
    {
      maxVerticalAngle = minVerticalAngle;
    }
  }

  void Awake()
  {
    regularCamera = GetComponent<Camera> ();
    focusPoint = focus.position + focusOffset;
    transform.localRotation = Quaternion.Euler (orbitAngles);
  }

  void Update()
  {
    focusPoint = focus.position + focusOffset;
    Quaternion lookRotation;
    if (ManualRotation ())
    {
      ConstrainAngles ();
      lookRotation = Quaternion.Euler (orbitAngles);
    }
    else
    {
      lookRotation = transform.localRotation;
    }

    if (!Activated)
    {
      return;
    }

    Vector3 lookDirection = lookRotation * Vector3.forward;
    Vector3 lookPosition = focusPoint - lookDirection * distance;

    Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
    Vector3 rectPosition = lookPosition + rectOffset;
    Vector3 castFrom = focus.position + focusOffset;
    Vector3 castLine = rectPosition - castFrom;
    float castDistance = castLine.magnitude;
    Vector3 castDirection = castLine / castDistance;

    if (Physics.BoxCast (
      castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
      lookRotation, castDistance, obstructionMask
    ))
    {
      rectPosition = castFrom + castDirection * hit.distance;
      lookPosition = rectPosition - rectOffset;
    }

    transform.SetPositionAndRotation (lookPosition, lookRotation);
  }

  bool ManualRotation()
  {
    Vector2 input = new Vector2 (
      -Input.GetAxis ("Mouse Y"),
      Input.GetAxis ("Mouse X")
    );
    if (invertVertical)
    {
      input.y = -input.y;
    }
    const float e = 0.001f;
    if (input.x < -e || input.x > e || input.y < -e || input.y > e)
    {
      orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
      lastManualRotationTime = Time.unscaledTime;
      return true;
    }
    return false;
  }

  void ConstrainAngles()
  {
    orbitAngles.x =
      Mathf.Clamp (orbitAngles.x, minVerticalAngle, maxVerticalAngle);

    if (orbitAngles.y < 0f)
    {
      orbitAngles.y += 360f;
    }
    else if (orbitAngles.y >= 360f)
    {
      orbitAngles.y -= 360f;
    }
  }
}