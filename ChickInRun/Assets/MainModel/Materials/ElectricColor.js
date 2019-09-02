#pragma strict

private var electDeath : GameObject;
private var elecVar : int;
public var darkMatA : Material;
public var darkMatB : Material;
public var originMat : Material;

function Start () {
	electDeath = GameObject.FindGameObjectWithTag("Chick");
	//originMat = renderer.material;
}

function Update () {
	elecVar = electDeath.GetComponent(PlayerInst).electricVar;
	if(elecVar == 1){
		GetComponent.<Renderer>().material = darkMatA;
	}
	else if(elecVar == 2){
		GetComponent.<Renderer>().material = darkMatB;
	}
	else if(elecVar ==0){
	
		GetComponent.<Renderer>().material = originMat;
	}
}