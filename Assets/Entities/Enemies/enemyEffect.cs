using UnityEngine;
using System.Collections;

public class enemyEffect : MonoBehaviour {
	public float health = 150f;
	public float projectileSpeed = -10f;
	public GameObject projectile; 
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 1;
	
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	private ScoreKeeper scoreKeeper;
	
	// Use this for initialization
	void Start () {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		Projectile missle = col.gameObject.GetComponent<Projectile>();
		if(missle)
		{
			health -= missle.GetDamage();
			missle.Hit ();
			if (health <= 0)
			{
				Die ();
			}
		}
	}
	
	void Die () {
		Destroy (gameObject);
		scoreKeeper.Score(scoreValue);
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float probability = Time.deltaTime * shotsPerSecond *2/3;
		if(Random.value < probability)
		{
			Fire ();
		}
	}
	
	void Fire () {
		GameObject missle = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		missle.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
}
