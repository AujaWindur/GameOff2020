using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public ControllerState State => state;

  public float BallSpawnOffset;
  public float HitForce;

#pragma warning disable CS0649
  [SerializeField] private GameObject ballPrefab;
  [SerializeField] private GameObject clubPrefab;
  [SerializeField] private CameraController cameraController;
  [SerializeField] private ControllerState state;
  [SerializeField] private GolfShotDebugGUI golfShotDebugGUI;
#pragma warning restore CS0649
  private GolfBall ball;
  private Vector3 cameraOffset;
  private new Transform camera;

  public enum ControllerState
  {
    HittingBall,
    SpectatingWhileBallIsResting,
    WaitingForBallToLand
  }

  private void Awake()
  {
    //transform.position = StartPosition + new Vector3 (0f, ballPrefab.GetComponent<SphereCollider> ().radius);

    ball = Instantiate (ballPrefab).GetComponent<GolfBall> ();
    ball.Reset (transform, transform.position, BallSpawnOffset);
    camera = cameraController.transform;
    cameraOffset = camera.localPosition;
  }

  private void Update()
  {
    if (State == ControllerState.HittingBall)
    {
      if (Input.GetButtonDown ("Spectate"))
      {
        state = ControllerState.SpectatingWhileBallIsResting;
        cameraController.Fly ();
      }

      else if (Input.GetMouseButtonDown (0))
      {
        //Should only take rotation on Z axis into account, and X axis should be based on HitForce, or static
        ball.Hit (camera.forward * HitForce);
        golfShotDebugGUI.LastShot = HitForce.ToString ();
        cameraController.Fly ();
        state = ControllerState.WaitingForBallToLand;
      }
    }
    else if (State == ControllerState.SpectatingWhileBallIsResting)
    {
      if (Input.GetButtonDown ("Spectate"))
      {
        state = ControllerState.HittingBall;
        cameraController.EndFly (transform.position + cameraOffset);
      }
      else if (Input.GetButtonDown ("ResetBall"))
      {
        state = ControllerState.HittingBall;
        ball.Reset (transform, transform.position, BallSpawnOffset);
        cameraController.EndFly (transform.position + cameraOffset);
      }

    }
    else if (State == ControllerState.WaitingForBallToLand)
    {
      if (Input.GetMouseButtonDown (1))
      {
        transform.position = ball.transform.position;
        cameraController.EndFly (transform.position + cameraOffset);
        ball.Reset (transform, ball.transform.position, BallSpawnOffset);
        state = ControllerState.HittingBall;
      }
      else if (Input.GetButtonDown ("ResetBall"))
      {
        ball.Reset (transform, transform.position, BallSpawnOffset);
        cameraController.EndFly (transform.position + cameraOffset);
        state = ControllerState.HittingBall;
      }
    }
  }
}
