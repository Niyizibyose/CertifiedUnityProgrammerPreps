using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
	/// <summary>
	/// <para>Invoked every frame with the X as the "Horizontal" axis and the Y as the "Vertical" axis</para>
	/// </summary>
	public event Action<Vector2> OnMove;
	/// <summary>
	/// <para>Invoked when the "Fire1" button is pressed</para>
	/// </summary>
	public event Action OnFire;
	/// <summary>
	/// <para>Invoked with the mouse in world position. It is invoked every frame</para>
	/// </summary>
	public event Action<Vector3> OnRotate; 

	private Camera _camera;

	private void Awake()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		CheckMovement();
		CheckIfFiring();
		CheckRotation();
	}
	
	/// <summary>
	/// <para>With the mouse position and camera position it returns the position of the mouse in world position.</para>
	/// </summary>
	/// <returns></returns>
	private Vector3 GetMouseInWorldPosition()
	{
		return _camera.ScreenToWorldPoint(Input.mousePosition
		                                      + Vector3.back * _camera.transform.position.z);
	}

	/// <summary>
	/// <para>Checks if the "Fire1" button is pressed. If it is pressed then the OnFire event is invoked</para>
	/// </summary>
	private void CheckIfFiring()
	{
		if (Input.GetButton("Fire1") && OnFire != null) OnFire();
	}

	/// <summary>
	/// <para>Invokes the OnRotate event with the mouse in world position</para>
	/// </summary>
	private void CheckRotation()
	{
		if (OnRotate != null) OnRotate(GetMouseInWorldPosition());
	}

	/// <summary>
	/// <para>Creates a vector with the x as the "Horizontal" axis and the y as the "Vertical" axis. It invokes the
	/// OnMove event with this vector</para>
	/// </summary>
	private void CheckMovement()
	{
		var movement = Vector2.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");
		if (OnMove != null) OnMove(movement);
	}
}
