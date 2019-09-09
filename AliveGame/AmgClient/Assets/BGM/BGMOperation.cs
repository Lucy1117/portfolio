using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class BGMOperation : MonoBehaviour
    {
        public AudioClip startBgm;
        public AudioClip heratBeatBgm;
        public AudioClip gardenRainBgm;
        public AudioClip bMenuOpenBgm;
        public AudioClip horrorFirst;
        public AudioClip horrorSecond;
        public AudioClip horrorThird;
        public AudioClip tvArtBgm;
        public AudioClip horrorForth;
        public AudioClip horrorFifth;
        public AudioClip horrorSixth;
        public AudioClip horrorSeventh;

        public float soundVolume;

        private int bgmNum;

        private AudioSource bmenuObj;

        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("BMenuBackGround"))
            {
                bmenuObj = GameObject.Find("BMenuBackGround").GetComponent<AudioSource>();
            }
            else
            {
                bmenuObj = null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //--TVArt에서 필요
            if(bgmNum == 7)
            {
                if (!GetComponent<AudioSource>().isPlaying && !bmenuObj.isPlaying)
                {
                    GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 8;
                    SoundBGMPlay(8);
                    GetComponent<AudioSource>().loop = true;
                }
            }
            if(bgmNum == 6)
            {
                if (!GetComponent<AudioSource>().isPlaying && !bmenuObj.isPlaying)
                {
                    GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 7;
                    SoundBGMPlay(7);
                }
            }
            if (bgmNum == 11)
            {
                if (!GetComponent<AudioSource>().isPlaying && !bmenuObj.isPlaying)
                {
                    GameObject.Find("Initiate").GetComponent<InitInterface>().bgmSoundNum = 12;
                    SoundBGMPlay(12);
                }
            }
        }

        /// <summary>
        ///  BGM Play
        /// startBgm = 1, heratBeatBgm = 2, gardenRainBgm = 3, horrorFirst = 4
        /// </summary>
        /// <param name="num"></param>
        public void SoundBGMPlay(int num)
        {
            bgmNum = num;
            AudioSource audiosource = GetComponent<AudioSource>();
            audiosource.volume = soundVolume;
            
            switch (num)
            {
                case 1:
                    audiosource.clip = startBgm;
                    break;
                case 2:
                    audiosource.clip = heratBeatBgm;
                    break;
                case 3:
                    audiosource.clip = gardenRainBgm;
                    break;
                case 4:
                    audiosource.clip = bMenuOpenBgm;
                    break;
                case 5:
                    audiosource.clip = horrorFirst;
                    break;
                case 6:
                    audiosource.clip = tvArtBgm;
                    audiosource.loop = false;
                    break;
                case 7:
                    //한 번 실행
                    audiosource.clip = horrorSecond;
                    break;
                case 8:
                    audiosource.clip = horrorThird;
                    break;
                case 9:
                    audiosource.clip = startBgm;
                    GameObject.Find("EffectSound").SendMessage("EffectSoundPlay", 1);
                    break;
                case 10:
                    audiosource.clip = horrorFifth;
                    break;
                case 11:
                    audiosource.clip = horrorSixth;
                    audiosource.loop = false;
                    break;
                case 12:
                    audiosource.clip = horrorSeventh;
                    break;
                case 13:
                    audiosource.clip = horrorForth;
                    break;

            }
            audiosource.Play();
        }

        /// <summary>
        /// 배경음악을 멈추는.
        /// </summary>
        public void SoundBGMStop()
        {
            AudioSource audiosource = GetComponent<AudioSource>();
            audiosource.Stop();
        }
    }
}