using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    public bool hasStomp;
	public bool hasShield;
    public bool hasAirBomb;

    public Sprite stompIcon;
    public Sprite shieldIcon;
    public Sprite airBombIcon;

    public float stompForce = 1000;

    GameObject stomper;
    GameObject itemIcon;


    

	// Use this for initialization
	void Start () {
        stomper = transform.Find("Stomper").gameObject;
        itemIcon = transform.Find("ItemIcon").gameObject;
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
                itemIcon.GetComponent<SpriteRenderer>().enabled = false;
				//Initial sound effect and "windup" animation in jet boots
				Invoke("Stomp", .7f);
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (hasShield)
			{
				hasShield = false;
                itemIcon.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<SpriteRenderer> ().color = Color.blue;
				//Add invulnerability to getting hooked (probably setting a new bool in HookObject to check if current player is "hookable")
                //or getting git by bombs too?
				Invoke("ResetShield", 2f);
			}
		}
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (hasAirBomb)
            {
                hasAirBomb = false;
                itemIcon.GetComponent<SpriteRenderer>().enabled = false;
				Instantiate(Resources.Load("AirBomb"), new Vector2(transform.position.x - 1, transform.position.y), transform.rotation);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish") //Change this to a finally player tag, however we eventually tag players indivudally or in one group
        {
			this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, stompForce*1.5f));
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1*stompForce));
            stomper.GetComponent<BoxCollider2D>().enabled = false;
            //any any other item pickup deletion we need to do
        }
		if (other.gameObject.tag == "Stomper") {
            ClearInventory();

            itemIcon.GetComponent<SpriteRenderer>().enabled = true;
            itemIcon.GetComponent<SpriteRenderer>().sprite = stompIcon;
			hasStomp = true;
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "Shielder") {
            ClearInventory();

            itemIcon.GetComponent<SpriteRenderer>().enabled = true;
            itemIcon.GetComponent<SpriteRenderer>().sprite = shieldIcon;
            hasShield = true;
			Destroy (other.gameObject);
		}
        if (other.gameObject.tag == "AirBomber")
        {
            ClearInventory();

            itemIcon.GetComponent<SpriteRenderer>().enabled = true;
            itemIcon.GetComponent<SpriteRenderer>().sprite = airBombIcon;
            hasAirBomb = true;
            Destroy(other.gameObject);
        }
    }
    void ClearInventory(){
        hasStomp = false;
        hasShield = false;
        hasAirBomb = false;
    }
	void ResetShield(){
        //Whatever needs to be done to reset invulnerabilities to hooks (and bombs?)
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
