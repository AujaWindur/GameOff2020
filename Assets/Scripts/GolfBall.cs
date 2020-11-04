using UnityEngine;

public class GolfBall : MonoBehaviour
{
  public BallState State { get; private set; }

  private new Rigidbody rigidbody;

  public enum BallState
  {
    //Is dropped onto the ground
    FallingToRest,
    //On the tee, resting on the grass, waiting to get hit, etc
    Resting,
    //After it gets hit
    Falling
  }

  private void Awake()
  {
    rigidbody = GetComponent<Rigidbody> ();
  }

  private void FixedUpdate()
  {
    if (State == BallState.FallingToRest)
    {
      if (rigidbody.velocity.y == float.Epsilon)
      {
        State = BallState.Resting;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.useGravity = false;
      }
    }
  }

  public void Reset(Transform parent, Vector3 position, float heightOffset)
  {
    transform.SetParent (parent);
    transform.position = position + new Vector3 (0f, heightOffset);
    State = BallState.FallingToRest;
    rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    rigidbody.useGravity = true;
  }

  public void Hit(Vector3 force)
  {
    transform.SetParent (null);
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.useGravity = true;
    rigidbody.velocity = force;
    State = BallState.Falling;
  }
}