using UnityEngine;

public class GolfShotDebugGUI : MonoBehaviour
{
  [HideInInspector] public string LastShot = "";

  private void OnGUI()
  {
    float width = 200f;
    if (LastShot != "")
    {
      GUI.Label (new Rect (Screen.width - width, Screen.height * 0.2f, width, 40f), "Last shot force: " + LastShot);
    }
  }
}
