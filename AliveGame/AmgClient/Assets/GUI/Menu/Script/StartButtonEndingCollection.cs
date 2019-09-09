using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button_Ending_Collect에 적용
/// </summary>
public class StartButtonEndingCollection : MonoBehaviour {
	public GameObject EndingCollectObject;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    /// <summary>
    /// EndingCollection을 눈에 보이게 할 때 사용.
    /// EndingCollection버튼을 눌렀을 때, SendMessage에 의해 함수가 적용된다.
    /// </summary>
	public void Collect()
    {
		EndingCollectObject.SetActive (true);

        //자식인 Background에 지정된 스크립트 내의 함수 ecOnFun이므로 Broadcast로 메시지를 보냄.
        EndingCollectObject.BroadcastMessage("ecOnFun");
	}
}
