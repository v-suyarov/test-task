using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScan : MonoBehaviour
{
	[SerializeField] private string targetTag = "Player";
	
	[SerializeField] private float distance = 2.15f;
	//ширина триггерной зоны в радианах*2
	[SerializeField] public float width = 0.8f;
	public LayerMask LayerMask;
	
	
	void Update()
    {
		
		//испускает лучи в посиках игрока
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
				//если луч попадет в игрока, то включаем режим преследования
				if (hit.collider.CompareTag("Player"))
				{
					GameManager.Instance.GetEnemy(0).GetComponent<EnemyBehaviour>().EnableFollowingMod();
					GameManager.Instance.GetEnemy(1).GetComponent<EnemyBehaviour>().EnableFollowingMod();
				}
			}
			nexRotate += 0.1f;
		}	
		
    }
}
