#pragma strict
public var manParticle : GameObject;

function Start(){
}

function OnCollisionEnter (col : Collision){

	if(col.gameObject.tag == "Chick"){
		col.gameObject.SendMessage("Union");
		Instantiate(manParticle, transform.position, transform.rotation);
		Destroy(gameObject);
		col.gameObject.SendMessage("Big");
		col.gameObject.SendMessage("Origin");
	}
}

