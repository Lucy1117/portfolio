using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class ButtonSoundOper : MonoBehaviour
    {
        public AudioClip buttonMove;
        public AudioClip buttonClick;
        public AudioClip inventoryOpen;
        public AudioClip bMenuOpen;

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
        /// buttonMove = 1, buttonClick = 2, inventoryOpen = 3, bMenuOpen = 4
        /// </summary>
        /// <param name="num"></param>
        public void ButtonEffectPlay(int num)
        {
            AudioSource audiosource = GetComponent<AudioSource>();

            audiosource.volume = soundVolume;
            switch (num)
            {
                case 1:
                    audiosource.PlayOneShot(buttonMove, soundVolume);
                    break;
                case 2:
                    audiosource.PlayOneShot(buttonClick, soundVolume);
                    break;
                case 3:
                    audiosource.PlayOneShot(inventoryOpen, soundVolume);
                    break;
                case 4:
                    audiosource.PlayOneShot(bMenuOpen, soundVolume);
                    break;
            }
        }
    }
}
