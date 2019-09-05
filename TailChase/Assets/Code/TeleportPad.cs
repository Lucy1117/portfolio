using UnityEngine;
using System.Collections;

public class TeleportPad : MonoBehaviour {
	
	
	public int code;
	float disableTimer=0;
	
	//public tele;
	
	void Update () {
		if (disableTimer > 0)
			disableTimer -= Time.deltaTime;
	}
	
	
	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "Character_prefab" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Blue_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Green_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Orange_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Purple_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Red_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Sky_prefab(Clone)" && disableTimer <= 0 || 
		    collider.gameObject.name == "AI_Yellow_prefab(Clone)" && disableTimer <= 0 ) {

            //Debug.Log("텔레포트" + this.gameObject.name);

            foreach (TeleportPad tp in FindObjectsOfType<TeleportPad>()) {
                if (tp.code == code && tp != this)
                {
                    tp.disableTimer = 2;
                    Vector3 position = tp.gameObject.transform.position;
                    position.x += 2;
                    collider.gameObject.transform.position = position;
                    Debug.Log(gameObject.name + "텔레포트 사용" + collider.gameObject.name);
                    if (collider.gameObject.name != "Character_prefab")
                    {
                        if(code == 12)
                        {
                            collider.gameObject.SendMessage("SpringToSummer");
                        }
                        else if(code == 14)
                        {
                            collider.gameObject.SendMessage("SpringToWinter");
                        }
                        else if (code == 23)
                        {
                            collider.gameObject.SendMessage("SummerToFall");
                        }
                        else if (code == 34)
                        {
                            collider.gameObject.SendMessage("FallToWinter");
                        }
                    }
                }
            }

        }
    }



    // Use this for initialization
    void Start () {
		
	}
	
	
	
	// Update is called once per frame
	
}


