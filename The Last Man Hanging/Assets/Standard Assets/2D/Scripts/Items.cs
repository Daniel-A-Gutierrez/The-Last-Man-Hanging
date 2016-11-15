using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public bool hasStomp;
	public bool hasShield;
    public float stompForce = 1000;

    GameObject stomper;
    

	// Use this for initialization
	void Start () {
        stomper = transform.Find("Player1Stomper").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForInputs ();
	}

	void CheckForInputs(){
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (hasStomp)
			{
				hasStomp = false;
				//Initial sound effect and "windup" animation in jet boots
				Invoke("Stomp", .7f);
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (hasShield)
			{
				hasShield = false;
				this.GetComponent<SpriteRenderer> ().color = Color.yellow;
				//Add invulnerability to getting hooked (probably setting a new bool in HookObject to check if current player is "hookable")
				Invoke("ResetShield", 2f);
			}
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
			this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, stompForce*1.5f));
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1*stompForce));
            stomper.GetComponent<BoxCollider2D>().enabled = false;
            //any any other item pickup deletion we need to do
        }
		if (other.gameObject.tag == "Stomper") {
			hasStomp = true;
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "Shielder") {
			hasShield = true;
			Destroy (other.gameObject);
		}
    }
	void ResetShield(){
		this.GetComponent<SpriteRenderer> ().color = Color.white;
	}
	void Stomp(){
		stomper.GetComponent<SpriteRenderer> ().enabled = true;
		stomper.GetComponent<BoxCollider2D> ().enabled = true;
		Invoke ("ResetStomp", .3f);
	}
	void ResetStomp(){
		stomper.GetComponent<SpriteRenderer> ().enabled = false;
		stomper.GetComponent<BoxCollider2D> ().enabled = false;
	}
}
