using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(APlayerShip))]
public class Jumper : MonoBehaviour
{
	[SerializeField] private float secondsBetweenJumps = 2f;
	[SerializeField] private float maximumDistanceFromAsteroids = 2f;
	[SerializeField] private int quantityOfJumps = 3;

	public delegate void JumpUsed(int remainingJumps);
	/// <summary>
	/// <para>Invoked when the player is destroyed and it jumps</para>
	/// </summary>
	public event JumpUsed OnJumpUsed;
	public float JumpsRemaining
	{
		get { return quantityOfJumps; }
	}
	
	private WaitForSeconds _waitingTime;
	private Func<IEnumerator> _jumpCoroutine;
	private Collider[] _hits;
	private Transform _transform;
	private Bounds _bounds;
	private int _asteroidsLayer;
	private APlayerShip _playerShip;

	private void Awake()
	{
		_playerShip = GetComponent<APlayerShip>();
		_waitingTime = new WaitForSeconds(secondsBetweenJumps);
		_hits = new Collider[10];
		_jumpCoroutine = JumpCoroutine;
		_bounds = MapLimit.GetCameraBounds(Camera.main);
		_asteroidsLayer = LayerMask.GetMask("Asteroid");
		_playerShip.Stats.OnDie += StartCoroutineForJumping;
		_transform = transform;
	}

	/// <summary>
	/// <para>Reduces the quantity of jumps by one and disables the player ship. If the quantity of jumps is greater
	/// than zero then is starts the jump coroutine.</para>
	/// <para>It also invokes the OnJumpUsed events with the remaining jumps.</para>
	/// <remarks>This method is executed when the health of the player reaches zero</remarks>
	/// </summary>
	private void StartCoroutineForJumping()
	{
		if (quantityOfJumps > 0)
		{
			StartCoroutine(_jumpCoroutine());
		}
		quantityOfJumps--;
		_playerShip.Disable();
		if (OnJumpUsed != null) OnJumpUsed(quantityOfJumps);
	}

	/// <summary>
	/// <para>Waits the time established by the variable secondsBetweenJumps and the it calls the method Jump</para>
	/// </summary>
	/// <returns></returns>
	private IEnumerator JumpCoroutine()
	{
		yield return _waitingTime;
		Jump();
	}
	
	/// <summary>
	/// <para>It gets a random position inside the screen and check if there are no asteroids nearby by. If there are
	/// asteroids near by then it check again until there are no asteroids near by</para>
	/// <para>Sets the new random position to the player and enables it.</para>
	/// </summary>
	private void Jump()
	{
		var size = 1;
		var position = Vector3.zero;
		while (size > 0)
		{
			position = GetRandomPosition();
			size = Physics.OverlapSphereNonAlloc(position, maximumDistanceFromAsteroids, _hits, _asteroidsLayer);
		}

		_transform.position = position;
		_playerShip.Enable();
	}

	/// <summary>
	/// <para>Returns a random position inside the bounds of the camera</para>
	/// </summary>
	/// <returns></returns>
	private Vector3 GetRandomPosition()
	{
		var x = Random.Range(_bounds.min.x, _bounds.max.x);
		var y = Random.Range(_bounds.min.y, _bounds.max.y);
		return new Vector3(x, y, _transform.position.z);
	}

	
}
