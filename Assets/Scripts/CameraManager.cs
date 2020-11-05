using UnityEngine;

public class CameraManager : MonoBehaviour
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
    orbitCamera.Activated = false;
    flyCamera.enabled = true;
    flyCamera.Fly ();
    IsFlying = true;
  }

  public void EndFly()
  {
    flyCamera.EndFly ();
    flyCamera.enabled = false;
    orbitCamera.Activated = true;
    IsFlying = false;
  }

}