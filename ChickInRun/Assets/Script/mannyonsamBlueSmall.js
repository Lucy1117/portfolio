#pragma strict
public var manParticle : GameObject;
function Start () {

}

function Update () {

}

function OnTriggerEnter (col : Collider){
	if(col.gameObject.tag == "Chick"){
		col.gameObject.SendMessage("Small");
		Instantiate(manParticle, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}