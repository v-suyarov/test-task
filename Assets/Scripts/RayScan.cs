using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScan : MonoBehaviour
{
	public string targetTag = "Player";
	public int rays = 6;
	public float distance = 2.15f;
	public float angle = 20;
	public float width = 0.8f;
	public float offsetY;
	public Transform startRay;
	public LayerMask LayerMask;
	
	

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
    {
		

		//Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * distance, Color.yellow);
		float nexRotate = -width;
		while(nexRotate<=width)
        {
			float cs = Mathf.Cos(nexRotate);
			float sn = Mathf.Sin(nexRotate);
			Vector2 nextRay = Vector2.zero;
			nextRay.x = Vector3.down.x * cs - Vector3.down.y * sn;
			nextRay.y = Vector3.down.x * sn + Vector3.down.y * cs;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(nextRay), distance, LayerMask);
			Debug.DrawRay(transform.position, transform.TransformDirection(nextRay) * distance, Color.black);
			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("Player"))
					transform.GetComponent<EnemyBehaviour>().EnableFollowingMod();
			}
			nexRotate += 0.1f;
		}
		
		
		/*if (hit.collider != null)
		{
			Debug.DrawLine(transform.position, hit.collider.transform.position,Color.yellow);
            if (hit.collider.CompareTag("Player"))
                Debug.Log("hit");
        }*/

		
		
    }
}
