using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour {
	public GameObject enemyPreFab;
	public float width = 10.0f;
	public float height = 5.0f;
	public float speed = 5.0f;
	public float createDelay = 0.5f;
	
	private bool movingRight = true;
	private float xMax;
	private float xMin;
	
	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
		
		xMax = rightEdge.x;
		xMin = leftEdge.x;
		
		CreateUntilFull();
	}
	
	void CreateEnemy () {
		foreach (Transform child in transform)
		{
			GameObject enemy = Instantiate(enemyPreFab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}
	
	void CreateUntilFull () {
		Transform freePosition = NextFreePosition();
		if(freePosition)
		{
			GameObject enemy = Instantiate(enemyPreFab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		
		if(freePosition)
		{
			Invoke ("CreateUntilFull", createDelay);
		}
		
	}
	
	public void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight)
		{
			transform.position += new Vector3 (speed * Time.deltaTime, 0);
		}
		else 
		{
			transform.position += new Vector3 (-speed * Time.deltaTime, 0);
		}
		
		float rightEgdeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfForamation = transform.position.x - (0.5f * width);
		if (rightEgdeOfFormation > xMax)
		{
			movingRight = false;
		} 
		else if (leftEdgeOfForamation < xMin)
		{
			movingRight = true;
		}
		
		if(AllMembersDead())
		{
			CreateUntilFull();	
		}
	}
	
	Transform NextFreePosition() {
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount == 0)
			{
				return childPositionGameObject;
			}
		}
		
		return null;
	}
	
	bool AllMembersDead() {
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount > 0)
			{
				return false;
			}
		}
		
		return true;
	}
}
