using System;
using System.Collections.Generic;
using UnityEngine;

public class BallDirectionVisualizer : MonoBehaviour
{
  public bool Showing { get; private set; }
  private GameObject meshObject;

  private void Awake()
  {
    meshObject = transform.GetChild (0).gameObject;
    meshObject.SetActive (false);
  }

  public void UpdateDirection(Vector3 position, float radius, Vector3 direction)
  {
    transform.position = position + new Vector3 (0f, -radius);
    Debug.Log (direction);
    transform.rotation = Quaternion.LookRotation (direction);
  }

  public void Show()
  {
    Showing = true;
    meshObject.SetActive (true);
  }

  public void Hide()
  {
    Showing = false;
    meshObject.SetActive (false);
  }
}