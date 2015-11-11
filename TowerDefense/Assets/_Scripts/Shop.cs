using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public int money = 5;


    private GameObject UI;

    private Text currMoney;

	// Use this for initialization
	void Start () {
        UI = GameObject.Find("MoneyText");
        currMoney = UI.GetComponent<Text>();
        currMoney.text = money.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
        }

	    if (!currMoney.text.Equals(money.ToString()))
	    {
            currMoney.text = money.ToString();
	    }
	}

}
