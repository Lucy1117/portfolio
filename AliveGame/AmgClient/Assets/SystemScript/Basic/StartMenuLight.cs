using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartMenuLight : MonoBehaviour {

	//기본 시간 범위
	public float duration = 0.0f;

	//사용 될 조명
	public Light lt;

	//랜덤 시간
	private float rndDuration = 1.0f;
	//랜덤 시간 최소, 최대
	private float minDuration = 0.2f;
	private float maxDuration = 1.0f;

	//랜덤 조명 값
	public float rndIntensity = 2.0f;
	//랜덤 조명 최소, 최대
	private float minIntensity = 0.7f;
	private float maxIntensity = 3.0f;

	//조명이 아예 0이 안되고 최소한의 조명을 보장하고 싶다면
	//지금은 0이지만 최소 조명 보장하고 싶으면 0.5f정도로 주면 됨!
	private float notzeroIntensity = 0.0f;

	// Use this for initialization
	void Start () {
		lt = GetComponent<Light>();
		//초기화
		rndDuration = minDuration;
        //Debug.Log(rndDuration);
    }

    void LightOnOff()
    {
        //duration이 rndDuration 값과 같아지면
        //시간 랜덤
        rndDuration = Random.Range(minDuration, maxDuration);
        if (rndDuration < minDuration)
        {
            rndDuration = minDuration;
            rndDuration = Mathf.Round(rndDuration / 0.01f) * .01f;
            //Debug.Log("랜덤1 " + rndDuration + "   조명값 " + rndIntensity);
            duration = 0.0f;
            //조명 랜덤
            rndIntensity = Random.Range(minIntensity, maxIntensity);
            rndIntensity = Mathf.Round(rndIntensity / 0.01f) * .01f;
            if (rndDuration < 0.5f)
            {
                minIntensity = 0.3f;
                maxIntensity = 0.7f;
            }
            else
            {
                minIntensity = 0.7f;
                maxIntensity = 3.0f;
            }
        }
        else
        {
            rndDuration = Random.Range(minDuration, maxDuration);
            rndDuration = Mathf.Round(rndDuration / 0.01f) * .01f;
            //Debug.Log("랜덤2 " + rndDuration + "   조명값 " + rndIntensity);
            duration = 0.0f;
            //조명 랜덤
            rndIntensity = Random.Range(minIntensity, maxIntensity);
            rndIntensity = Mathf.Round(rndIntensity / 0.01f) * .01f;
            if (rndDuration < 0.5f)
            {
                minIntensity = 0.3f;
                maxIntensity = 0.7f;
            }
            else
            {
                minIntensity = 0.7f;
                maxIntensity = 3.0f;
            }
        }

    }
	// Update is called once per frame
	void Update () {

        if (duration == rndDuration)
        {
            LightOnOff();
            //Debug.Log("1");
        }
		else if (duration > rndDuration)
		{
            //rndDuration값이 0.01f씩 오르다가 갑자기 0.099999999999가 더해지는 경우가 생겨서
            //분기를 하나 더 둠. duration 최댓값보다 높아지면 다시 랜덤 값으로.
            LightOnOff();
           // Debug.Log("2");
        }
		else
		{
			duration = duration + 0.01f;
            //Debug.Log("3   "+ duration);
        }
        //조명의 변화량을 자연스럽게 바꿔주는 함수. unityAPI의 Light API 참조.
        float phi = Time.time / rndDuration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * rndIntensity + notzeroIntensity;
		lt.intensity = amplitude;
	}
}
