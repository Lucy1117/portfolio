#pragma strict

function Start () {
    //animation.Play("Idle");
}

function Update () {
	GetComponent.<Animation>().Play("Idle");
}