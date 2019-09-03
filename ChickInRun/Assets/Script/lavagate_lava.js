#pragma strict

public var openMat : Material;
public var openNumber : int;
private var lavaNum : int;
private var lGate : GameObject;
//gate의 변수를 받아오기 위한 오브젝트

function Start () {
	lGate = GameObject.FindGameObjectWithTag("LavaGate");
}

function Update () {

	lavaNum = lGate.GetComponent(GateOpen).openNum;
	if(lavaNum == openNumber){
		GetComponent.<Renderer>().material = openMat;
	}
}