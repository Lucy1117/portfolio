using UnityEngine;
using System.Collections;

public class CameraChase : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	public float yPos;
	public float zPos;
	//public Transform playertrans;
	//private float distance = 10.0f;

	// Use this for initialization
	void Start () {
		//offset = transform.position - player.transform.position;
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y+yPos, player.transform.position.z-zPos);

	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = player.transform.position + offset;
        if(player)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yPos, player.transform.position.z - zPos);

        }
    }
}
