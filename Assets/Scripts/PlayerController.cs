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
  [SerializeField] private ControllerState state;
  [SerializeField] private GolfShotDebugGUI golfShotDebugGUI;
#pragma warning restore CS0649
  private GolfBall ball;
  private Vector3 cameraOffset;
  private Transform cameraTransform;
  private CameraController cameraController;

  public enum ControllerState
  {
    Idle,
    HittingBall,
    SpectatingWhileBallIsResting,
    WaitingForBallToLand
  }

  private void Awake()
  {
    ball = Instantiate (ballPrefab).GetComponent<GolfBall> ();
    ball.Reset (transform.position, BallSpawnOffset);
    cameraTransform = Camera.main.transform;
    cameraOffset = cameraTransform.localPosition;
    cameraController = GetComponent<CameraController> ();
    SetState (ControllerState.HittingBall);
  }

  private void Update()
  {
    if (State == ControllerState.HittingBall)
    {
      if (Input.GetButtonDown ("Spectate"))
      {
        SetState (ControllerState.SpectatingWhileBallIsResting);
      }

      else if (Input.GetMouseButtonDown (0))
      {
        //Should only take rotation on Z axis into account, and X axis should be based on HitForce, or static
        ball.Hit (cameraTransform.forward * HitForce);
        golfShotDebugGUI.LastShot = HitForce.ToString ();
        SetState (ControllerState.WaitingForBallToLand);
      }
    }
    else if (State == ControllerState.SpectatingWhileBallIsResting)
    {
      if (Input.GetButtonDown ("Spectate"))
      {
        SetState (ControllerState.HittingBall);
      }
      else if (Input.GetButtonDown ("ResetBall"))
      {
        SetState (ControllerState.HittingBall);
        ball.Reset (transform.position, BallSpawnOffset);
      }

    }
    else if (State == ControllerState.WaitingForBallToLand)
    {
      if (Input.GetMouseButtonDown (1))
      {
        transform.position = ball.transform.position;
        ball.Reset (ball.transform.position, BallSpawnOffset);
        SetState (ControllerState.HittingBall);
      }
      else if (Input.GetButtonDown ("ResetBall"))
      {
        ball.Reset (transform.position, BallSpawnOffset);
        SetState (ControllerState.HittingBall);
      }
    }
  }

  private void SetState(ControllerState newState)
  {
    if (state == newState)
    {
      return;
    }

    if (state == ControllerState.Idle)
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    else if (newState == ControllerState.Idle)
    {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }

    if ((newState == ControllerState.SpectatingWhileBallIsResting || newState == ControllerState.WaitingForBallToLand)
      && !cameraController.IsFlying)
    {
      cameraController.Fly ();
    }
    else if (newState == ControllerState.HittingBall && cameraController.IsFlying)
    {
      cameraController.EndFly (transform.position + cameraOffset);
    }

    state = newState;
  }
}
