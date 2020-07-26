using UnityEngine;

[RequireComponent(typeof(Stats), typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float tiltWhenMoving;

    private Stats _stats;
    private Rigidbody _rigidbody;
    private Vector2 _direction;
    private Transform _transform;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    /// <summary>
    /// <para>Sets the velocity and the rotation of the RigidBody including its tilt</para>
    /// </summary>
    private void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _stats.MovementSpeed;
        
        _transform.rotation =
            Quaternion.Euler(new Vector3(_direction.y * tiltWhenMoving, _direction.x * tiltWhenMoving * -1, 0));
    }

    /// <summary>
    /// <para>Sets the member variable _direction of the direction parameter</para>
    /// <para>If the magnitude of the direction is higher than one then it normalizes the direction. This is made to
    /// prevent faster velocities when going in a diagonal direction</para>
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        _direction = direction;
    }
}