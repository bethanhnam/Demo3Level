using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IronPlate : MonoBehaviour
{
    [SerializeField] CircleCollider2D[] holes;
    public Vector3[] centerPoints;
	[SerializeField] private float radius;
	public bool result;

	private void Start()
	{
		centerPoints = new Vector3[holes.Length];
		
	}
	public void setPoint()
	{
		for(int i = 0; i < holes.Length; i++)
		{
			centerPoints[i] = holes[i].bounds.center;
		}
	}
	private void Update()
	{
		setPoint();
	}
	private void OnDrawGizmos()
	{
		foreach (var centerPoint in centerPoints)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(centerPoint, 0.2f);
		}
	}
	public bool checkHitPoint(Vector2 mousePosition)
	{
		result = false;
		radius = holes[1].radius/2;
		float reference = radius - 0.1f;
		Debug.Log("reference " +reference);
		for (int i = 0; i < centerPoints.Length; i++)
		{
			float distance = Vector2.Distance(mousePosition, centerPoints[i]);
			Debug.Log("distance " + distance);
			if(distance < reference)
			{
				result = true;
			}
		}
		return result;
	}
}
