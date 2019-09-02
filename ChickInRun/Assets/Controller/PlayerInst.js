#pragma strict
public var chickPrefab : GameObject;
public var bloodPrefabA : GameObject;
public var bloodPrefabB : GameObject;

private var deathBGMA : GameObject;
private var deathBGMB : GameObject;
private var deathBGMC : GameObject;

public var crashVar : int = 0;
public var electricVar : int = 0;
//족제비 부딪히는 횟수, 전기 통과하는 횟수 

private var anObjectA : GameObject;
private var anObjectB : GameObject;
private var anObjectC : GameObject;
//생성된 prefab의 이름
private var insPos : Vector3;
//instantiate 위치
private var insRot : Quaternion;

private var direcA : float = 0.8;
private var direcB : float = 0.5;
private var direcC : float = 0;
private var direcD : float = 0.2;

//instantiate 떨어진 거리

private var chickTransform : Transform;

private var smallCheck : boolean;

public var manSound : AudioClip;
public var damageSound : AudioClip;

function Start () {

	chickTransform = this.transform;
	deathBGMA = GameObject.FindGameObjectWithTag("Backgroundmusic1");
	deathBGMB = GameObject.FindGameObjectWithTag("Backgroundmusic2");
	deathBGMC = GameObject.FindGameObjectWithTag("Backgroundmusic3");

}


function Update () {

}

function Union(){
	GetComponent.<AudioSource>().PlayOneShot(manSound);
	if(crashVar ==1){
		this.transform.localScale *=4/3f;
		Destroy(anObjectA);
		crashVar=0;
	}
	else if(crashVar > 1){
		this.transform.localScale *=16/9f;
		Destroy(anObjectA);
		Destroy(anObjectB);
		Destroy(anObjectC);
		crashVar=0;
	}


}

function Split(){
	GetComponent.<AudioSource>().PlayOneShot(damageSound);
	if(crashVar == 1){
		this.transform.localScale *=0.75f;
		insPos = this.transform.position;
		anObjectA = Instantiate(chickPrefab, insPos, transform.rotation);
		anObjectA.transform.position = insPos;
		anObjectA.transform.Translate(Vector3(0,0,-direcA));
		anObjectA.transform.localRotation = this.transform.localRotation; 
		anObjectA.transform.localScale = this.transform.localScale;
		}
	else if(crashVar ==2){
		anObjectA.transform.Translate(Vector3(0,0,direcA));
		this.transform.localScale *=0.75f;
		anObjectA.transform.Translate(Vector3(0,0,-direcB));
		insPos = this.transform.position;
		anObjectB = Instantiate(chickPrefab, insPos, transform.rotation);
		anObjectB.transform.position = insPos;
		anObjectB.transform.Translate(Vector3(0,0,-(direcB*2)));
		anObjectB.transform.localRotation = this.transform.localRotation; 
		anObjectB.transform.localScale = this.transform.localScale;
		
		anObjectC = Instantiate(chickPrefab, insPos, transform.rotation);
		anObjectC.transform.position = insPos;
		anObjectC.transform.Translate(Vector3(0,0,-(direcB*3)));
		anObjectC.transform.localRotation = this.transform.localRotation; 
		anObjectC.transform.localScale = this.transform.localScale;

	
	}
	else if(crashVar ==3){
		Death();
	}
}


function Small(){
	this.transform.localScale *=0.5f;
	smallCheck = true;
}

function Big(){
	if(smallCheck){
		this.transform.localScale *=2.0f;
		smallCheck = false;
	}
}

function ElecDeath(){
	if(electricVar == 1){
	
	}
	else if(electricVar == 2){
	
	}
	else{
		Death();
	}
}

function Origin(){
	electricVar = 0;
}

function Death(){
	deathBGMA.GetComponent(bgmoper).deathCheck = true;
	deathBGMB.GetComponent(bgmoper).deathCheck = true;
	deathBGMC.GetComponent(bgmoper).deathCheck = true;
	
	Instantiate(bloodPrefabA, transform.position, transform.rotation);
	Instantiate(bloodPrefabB, transform.position, transform.rotation);
	Destroy(gameObject);
		if(crashVar ==1){
		Destroy(anObjectA);
		crashVar=0;
	}
	else if(crashVar > 1){
		Destroy(anObjectA);
		Destroy(anObjectB);
		Destroy(anObjectC);
		crashVar=0;
	}
	
}
