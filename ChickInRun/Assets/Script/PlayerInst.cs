using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInst : MonoBehaviour
{
    public GameObject chickPrefab;
    public GameObject bloodPrefabA;
    public GameObject bloodPrefabB;

    private GameObject deathBGMA;
    private GameObject deathBGMB;
    private GameObject deathBGMC;

    public int crashVar = 0;
    public int electricVar = 0;
    //족제비 부딪히는 횟수, 전기 통과하는 횟수 
    private GameObject anObjectA;
    private GameObject anObjectB;
    private GameObject anObjectC;
    //생성된 prefab의 이름
    private Vector3 insPos;
    //instantiate 위치
    private Quaternion insRot;

    private float directA = 0.8f;
    private float directB = 0.5f;
    //private float directC = 0.0f;
    //private float directD = 0.2f;
    //instantiate 떨어진 거리

    //private Transform chickTransform;

    private bool smallCheck;
    public AudioClip manSound;
    public AudioClip damageSound;

    // Start is called before the first frame update
    void Start()
    {
        //chickTransform = this.transform;
        deathBGMA = GameObject.FindGameObjectWithTag("Backgroundmusic1");
        deathBGMB = GameObject.FindGameObjectWithTag("Backgroundmusic2");
        deathBGMC = GameObject.FindGameObjectWithTag("Backgroundmusic3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Union()
    {
        GetComponent<AudioSource>().PlayOneShot(manSound);
        if (crashVar == 1)
        {
            this.transform.localScale *= 4 / 3f;
            Destroy(anObjectA);
            crashVar = 0;
        }
        else if (crashVar > 1)
        {
            this.transform.localScale *= 16 / 9f;
            Destroy(anObjectA);
            Destroy(anObjectB);
            Destroy(anObjectC);
            crashVar = 0;
        }
    }

    public void Split()
    {
        crashVar++;
        GetComponent<AudioSource>().PlayOneShot(damageSound);
        if (crashVar == 1)
        {
            this.transform.localScale *= 0.75f;
            insPos = this.transform.position;
            anObjectA = Instantiate(chickPrefab, insPos, transform.rotation);
            anObjectA.transform.position = insPos;
            anObjectA.transform.Translate(0.0f, 0.0f, -directA);
            anObjectA.transform.localRotation = this.transform.localRotation;
            anObjectA.transform.localScale = this.transform.localScale;
            if(electricVar>0)
            {
                GameObject[] childChick = GameObject.FindGameObjectsWithTag("ElectricChick");
                for (int i = 0; i < childChick.Length; i++)
                {
                    childChick[i].SendMessage("ElecChange", electricVar);
                }
            }
        }
        else if (crashVar == 2)
        {
            anObjectA.transform.Translate(0.0f, 0.0f, directA);
            this.transform.localScale *= 0.75f;
            anObjectA.transform.Translate(0.0f, 0.0f, -directB);
            insPos = this.transform.position;
            anObjectB = Instantiate(chickPrefab, insPos, transform.rotation);
            anObjectB.transform.position = insPos;
            anObjectB.transform.Translate(0.0f, 0.0f, -(directB * 2));
            anObjectB.transform.localRotation = this.transform.localRotation;
            anObjectB.transform.localScale = this.transform.localScale;

            anObjectC = Instantiate(chickPrefab, insPos, transform.rotation);
            anObjectC.transform.position = insPos;
            anObjectC.transform.Translate(0.0f, 0.0f, -(directB * 3));
            anObjectC.transform.localRotation = this.transform.localRotation;
            anObjectC.transform.localScale = this.transform.localScale;
            if (electricVar > 0)
            {
                GameObject[] childChick = GameObject.FindGameObjectsWithTag("ElectricChick");
                for (int i = 0; i < childChick.Length; i++)
                {
                    childChick[i].SendMessage("ElecChange", electricVar);
                }
            }
        }
        else if (crashVar == 3)
        {
            Death();
        }
    }

    public void Small()
    {
        this.transform.localScale *= 0.4f;
        smallCheck = true;
    }

    public void Big()
    {
        if (smallCheck)
        {
            this.transform.localScale *= 2.0f;
            smallCheck = false;
        }
    }

    public void ElecDeath()
    {
        ///2019///
        GameObject[] childChick = GameObject.FindGameObjectsWithTag("ElectricChick");
        for (int i = 0; i < childChick.Length; i++)
        {
            childChick[i].SendMessage("ElecChange", electricVar);
        }
        if (electricVar > 2)
        {
            Death();
        }
        /////////
    }

    public void Origin()
    {
        electricVar = 0;
        GameObject[] childChick = GameObject.FindGameObjectsWithTag("ElectricChick");
        for (int i = 0; i < childChick.Length; i++)
        {
            childChick[i].SendMessage("ElecChange", electricVar);
        }
    }

    public void Death()
    {
        deathBGMA.GetComponent<BgmOper>().deathCheck = true;
        deathBGMB.GetComponent<BgmOper>().deathCheck = true;
        deathBGMC.GetComponent<BgmOper>().deathCheck = true;

        Instantiate(bloodPrefabA, transform.position, transform.rotation);
        Instantiate(bloodPrefabB, transform.position, transform.rotation);
        Destroy(gameObject);
        if (crashVar == 1)
        {
            Destroy(anObjectA);
            crashVar = 0;
        }
        else if (crashVar > 1)
        {
            Destroy(anObjectA);
            Destroy(anObjectB);
            Destroy(anObjectC);
            crashVar = 0;
        }

    }

}
