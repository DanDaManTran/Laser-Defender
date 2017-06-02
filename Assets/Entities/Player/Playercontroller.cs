using UnityEngine;
using System.Collections;

public class Playercontroller : MonoBehaviour {

	public float speed = 5.0f;
	public float padding = 1.0f; 
	public float projectileSpeed = 5.0f;
	public float fireRate = 0.2f;
	public float health = 250f;
	public GameObject projectile;
	public AudioClip fireSound;
	
	float xMin;
	float xMax;
	private ScoreKeeper scoreKeeper;
	
	
	// Use this for initialization
	void Start () {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
	}
	
	void Fire () {
		Vector3 offset = new Vector3 (0, 1 ,0);
		GameObject beam = Instantiate(projectile, transform.position + offset, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			InvokeRepeating("Fire", 0.00001f, fireRate);	
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			CancelInvoke("Fire");
		}
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
		}
		
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		Projectile missle = col.gameObject.GetComponent<Projectile>();
		if(missle)
		{
			health -= missle.GetDamage();
			missle.Hit ();
			if (health <= 0)
			{
				LevelManager man = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
				man.LoadLevel("Win Screen");
				Destroy (gameObject);
			}
		}
	}
}
