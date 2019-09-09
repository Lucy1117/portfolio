using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace AmgClient
{
    public class Marshaler : Nettention.Proud.Marshaler
    {
        //Nettention.Proud.Marshaler 안에 기본적인 Marshaler함수가 정의되어 있음.
        //분산 환경의 응용 프로그램에는 응용 프로그램 데이터와 네트워크 스트림을 조합하거나 분해하기 위한 통신 계층이 필요하다. 
        //마이크로소프트에서는 이 계층을 공식 명칭으로 '마샬로(marshaler)라고 한다.
        //Marshaler는 Read함수와 Write함수를 가짐.

        public static void Write(Nettention.Proud.Message msg, UnityEngine.Vector3 b)
        {
            //3d 벡터 타입을 위한 Marshaler 함수를 추가.
            msg.Write(b.x);
            msg.Write(b.y);
            msg.Write(b.z);
        }

        public static void Read(Nettention.Proud.Message msg, out UnityEngine.Vector3 b)
        {
            b = new UnityEngine.Vector3();
            msg.Read(out b.x);
            msg.Read(out b.y);
            msg.Read(out b.z);

        }
    }
}