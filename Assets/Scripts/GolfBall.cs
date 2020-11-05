using UnityEngine;

public class GolfBall : MonoBehaviour
{
  public BallState State { get; private set; }
  public float Radius => collider.radius;

  private new Rigidbody rigidbody;
  private new SphereCollider collider;

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
    collider = GetComponent<SphereCollider> ();
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

  public void Reset(Vector3 position, float heightOffset)
  {
    transform.position = position + new Vector3 (0f, heightOffset);
    State = BallState.FallingToRest;
    rigidbody.velocity = Vector3.zero;
    rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    rigidbody.useGravity = true;
  }

  public void Hit(Vector3 force)
  {
    rigidbody.constraints = RigidbodyConstraints.None;
    rigidbody.useGravity = true;
    rigidbody.velocity = force;
    State = BallState.Falling;
  }
}