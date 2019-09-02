#pragma strict

private var lGate : GameObject;
public var lavaParticle : GameObject;


function Start(){
	lGate = GameObject.FindGameObjectWithTag("LavaGate");
}

function OnTriggerEnter (col : Collider){

	if(col.gameObject.tag == "Chick"){
		lGate.GetComponent(GateOpen).openNum++;
		Instantiate(lavaParticle, transform.position, transform.rotation);
		Destroy(gameObject);
		lGate.GetComponent(GateOpen).lsoundCheck = true;
	}
}

function Update(){

	transform.rotation *=Quaternion.AngleAxis(2, Vector3(0,0,1));

}