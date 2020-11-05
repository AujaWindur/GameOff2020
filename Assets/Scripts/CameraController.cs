using UnityEngine;

public class CameraController : MonoBehaviour
{
  public bool IsFlying { get; private set; }
  private FlyCamera flyCamera;
  private OrbitCamera orbitCamera;

  private void Awake()
  {
    flyCamera = Camera.main.GetComponent<FlyCamera> ();
    flyCamera.enabled = false;
    orbitCamera = Camera.main.GetComponent<OrbitCamera> ();
    orbitCamera.enabled = true;
  }

  public void Fly()
  {
    orbitCamera.enabled = false;
    flyCamera.enabled = true;
    flyCamera.Fly ();
    IsFlying = true;
  }

  public void EndFly(Vector3 position)
  {
    flyCamera.EndFly (position);
    flyCamera.enabled = false;
    orbitCamera.enabled = true;
    IsFlying = false;
  }

}