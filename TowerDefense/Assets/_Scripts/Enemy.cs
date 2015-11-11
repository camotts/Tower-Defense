using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int Health = 100;
    public int Value = 10;

    private GameObject player;

	// Use this for initialization
	void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (Health <= 0)
        {
            var playerMoney = player.GetComponent<Shop>();
            playerMoney.money += Value;
            Destroy(this.gameObject);
        }
	}
}
