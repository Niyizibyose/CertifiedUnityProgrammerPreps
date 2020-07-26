using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private float secondsBetweenShots;
	
	private Transform _transform;
	private float _lastShoot;
	private Transform _bulletAnchor;
		
	private void Awake()
	{
		_transform = transform;
		_bulletAnchor = new GameObject("Bullet Anchor").transform;
	}

	/// <summary>
	/// <para>Instantiates a bullet prefab with the same position and rotation as this transform</para>
	/// <para>Before instantiating it checks if the time passed between now and the list time it shoot is greater than
	/// the seconds between shots.</para>
	/// </summary>
	public void Fire()
	{
		var now = Time.time;
		if (now - _lastShoot < secondsBetweenShots) return;
		_lastShoot = Time.time;
		Instantiate(bulletPrefab, _transform.position, _transform.rotation, _bulletAnchor);
	}

	/// <summary>
	/// <para>
	/// Rotates toward the rotationPosition
	/// </para>
	/// </summary>
	/// <param name="rotationPosition"></param>
	public void Rotate(Vector3 rotationPosition)
	{
		transform.LookAt(rotationPosition, Vector3.back);
	}
}
