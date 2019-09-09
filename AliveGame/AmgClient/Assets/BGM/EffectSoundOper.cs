using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    /// <summary>
    /// 효과음 작동 cs
    /// </summary>
    public class EffectSoundOper : MonoBehaviour
    {

        public AudioClip littleGirl;

        public AudioClip alarmBuzzer;

        public float soundVolume;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// 버튼 이펙트 사운드.
        /// </summary>
        /// <param name="num"></param>
        public void EffectSoundPlay(int num)
        {
            AudioSource audiosource = GetComponent<AudioSource>();

            audiosource.volume = soundVolume;
            switch (num)
            {
                case 1:
                    audiosource.PlayOneShot(littleGirl, soundVolume);
                    break;
                case 2:
                    audiosource.PlayOneShot(alarmBuzzer, 0.5f);
                    break;
            }
        }
    }
}
