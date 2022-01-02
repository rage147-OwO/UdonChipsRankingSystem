
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UCS;
using System;
using 랭킹_서브네임스페이스;
using UnityEngine.UI;

namespace 랭킹_메인네임스페이스
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class 랭킹_메인 : UdonSharpBehaviour
    {
       public float 동기간격기본은2 = 2; //1초에 한명씩 동기함, 플레이어가 40명이면 한명씩 40초가 걸림,소수점으로 설정 가능함  //기본값 2 




        public byte 로컬번호;
        public UdonChips 우동칩스데이터;
        public 랭킹_서브[] 서브데이터;
        public Text 게시판;
        public Text 게시판2;
        float time=0f;
        byte timeIndex = 0;

        public override void Interact()
        {
            for(int i= 0; i < 서브데이터.Length; i++)
            {
                서브데이터[i].싱크요청();
            }
        }

        private void FixedUpdate()
        {
            if (Networking.IsOwner(this.gameObject))
            {
                time = 0.02f + time;
                if (time > 동기간격기본은2)
                {
                    if(timeIndex >= 서브데이터.Length)
                    {
                        timeIndex = 0;
                    }
                    서브데이터[timeIndex].싱크요청();
                    timeIndex++;
                    time = 0;
                }
            }
        }
        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (Networking.IsOwner(this.gameObject))
            {
                Debug.Log(로컬번호);
                if (로컬번호 != 서브데이터.Length - 1)   //마스터자리가 아닐때
                {
                    서브데이터[서브데이터.Length-1].플레이어소지금 = 서브데이터[로컬번호].플레이어소지금;
                    서브데이터[서브데이터.Length - 1].RequestSerialization();
                    서브데이터[로컬번호].플레이어소지금 = 0;
                    서브데이터[로컬번호].RequestSerialization();
                    로컬번호 = ((byte)(서브데이터.Length - 1));
                }
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (player == Networking.LocalPlayer)
            {
                VRCPlayerApi 오너 = Networking.GetOwner(this.gameObject);

                if (Networking.IsOwner(this.gameObject))
                {
                    로컬번호 = ((byte)(서브데이터.Length - 1));
                    Networking.SetOwner(Networking.LocalPlayer, 서브데이터[서브데이터.Length - 1].gameObject);
                    서브데이터[로컬번호].플레이어소지금 = 0;
                    서브데이터[로컬번호].RequestSerialization();
                }
                else
                {
                    for (byte i = 0; i < 서브데이터.Length - 1; i++)
                    {
                        if (Networking.GetOwner(서브데이터[i].gameObject) == 오너)
                        {
                            로컬번호 = i;
                            Networking.SetOwner(Networking.LocalPlayer, 서브데이터[i].gameObject);
                            서브데이터[로컬번호].플레이어소지금 = 0;
                            서브데이터[로컬번호].RequestSerialization();
                            break;
                        }
                    }
                }
            }
        }
    }
}


