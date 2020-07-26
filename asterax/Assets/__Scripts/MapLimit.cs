using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MapLimit : MonoBehaviour
{
	[SerializeField] private BoxCollider extraBound;
	
	private BoxCollider _boxCollider;
	private Bounds _bounds;
	private void Awake()
	{
		_boxCollider = GetComponent<BoxCollider>();
	}

	private void Start()
	{
		_bounds = GetCameraBounds(Camera.main);
		_boxCollider.size = _bounds.size;
		extraBound.size = _bounds.size;
	}

	/// <summary>
	/// <para>
	/// Returns the bounds of the camera taking into account the size and the aspect of the camera.
	/// </para>
	/// </summary>
	/// <param name="myCamera"></param>
	/// <returns></returns>
	public static Bounds GetCameraBounds(Camera myCamera)
	{
		var verticalHeightSeen = myCamera.orthographicSize * 2.0f;
		var verticalWidthSeen = verticalHeightSeen * myCamera.aspect;
		
		return new Bounds(myCamera.transform.position, 
			new Vector3(verticalWidthSeen, verticalHeightSeen, 10));
	}
	

	/// <summary>
	/// <para>
	/// If the GameObject has a SpaceObject component then checks on which side of the bounds it is and moves it to the
	/// opposite side.
	/// </para>
	/// </summary>
	/// <param name="other">The Collider that has gone out of the map limits</param>
	private void OnTriggerExit(Collider other)
	{
		var spaceObject = other.GetComponent<SpaceObject>();
		if (spaceObject == null || !spaceObject.enabled) return;
		var otherTransform = other.transform;
		var otherBounds = other.bounds;
		var width = otherBounds.size.x;
		var height = otherBounds.size.y;
		var otherPosition = otherTransform.position;

		if (otherPosition.x > _bounds.max.x) otherPosition.x = _bounds.min.x - width;
		else if (otherPosition.x < _bounds.min.x) otherPosition.x = _bounds.max.x + width;
		
		if (otherPosition.y > _bounds.max.y) otherPosition.y = _bounds.min.y - height;
		else if (otherPosition.y < _bounds.min.y) otherPosition.y = _bounds.max.y + height;
		
		otherTransform.position = otherPosition;
	}
}
