#pragma strict

public var openMat : Material;
public var originMat : Material;
private var level : int;
public var order : int;
public var orderA : int;
public var orderB : int;
//gate의 변수를 받아오기 위한 오브젝트

function Start () {
	level = 0;
}

function Update () {
	level++;
	if(level ==orderA){
		GetComponent.<Renderer>().material = openMat;
	}
	else if(level ==orderB){
		GetComponent.<Renderer>().material = originMat;
		level=order;
	}
}