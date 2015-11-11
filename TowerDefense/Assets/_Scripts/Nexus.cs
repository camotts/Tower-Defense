using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class Nexus : MonoBehaviour {

	public int health;
	private GameObject UI;

    private Text currHealth;

	// Use this for initialization
	void Start () {
        UI = GameObject.Find("HealthText");
        currHealth = UI.GetComponent<Text>();
		health = 100;
		currHealth.text = health.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			health -= 5;
			currHealth.text = health.ToString ();

			Destroy (other.gameObject);
		}
	}
}
