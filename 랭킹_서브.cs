
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using 랭킹_메인네임스페이스;


namespace 랭킹_서브네임스페이스
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class 랭킹_서브 : UdonSharpBehaviour
    {
        [UdonSynced] public float 플레이어소지금;
        
        public 랭킹_메인 랭킹_메인데이터;
        string 정렬된스트링=string.Empty;
        string 정렬된스트링2 = string.Empty;




        public void 싱크요청()
        {
            if (Networking.IsOwner(랭킹_메인데이터.gameObject))
            {
                if (랭킹_메인데이터.로컬번호 == 랭킹_메인데이터.서브데이터.Length - 1)
                {
                    if (!Networking.IsOwner(this.gameObject))
                    {
                        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "싱크");
                    }
                    else
                    {
                        if (this.gameObject == 랭킹_메인데이터.서브데이터[랭킹_메인데이터.서브데이터.Length - 1].gameObject)
                        {
                            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "싱크");
                        }
                    }
                }
            }
            else
            {
                if (!Networking.IsOwner(this.gameObject))
                {
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "싱크");
                }
                else
                {
                    if (this.gameObject == 랭킹_메인데이터.서브데이터[랭킹_메인데이터.서브데이터.Length - 1].gameObject)
                    {
                        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "싱크");
                    }
                }
            }
           
          
          
        }
        public void 싱크()
        {
            플레이어소지금 = 랭킹_메인데이터.우동칩스데이터.money;
            RequestSerialization();
            정렬();
        }
 
        public override void OnDeserialization()
        {
            정렬();

        }

        public void 정렬()
        {

            VRCPlayerApi PlayerApiTemp;
            VRCPlayerApi Master= Networking.GetOwner(랭킹_메인데이터.gameObject);
            정렬된스트링 = "";
            정렬된스트링2 = "";
            int[] sort = new int[랭킹_메인데이터.서브데이터.Length];
            float[] money = new float[랭킹_메인데이터.서브데이터.Length];
            int[] 정렬된데이터 = new int[랭킹_메인데이터.서브데이터.Length];
            for (int i = 0; i < money.Length; i++)
            {
                sort[i] = i;
                money[i] = 랭킹_메인데이터.서브데이터[i].플레이어소지금;
            }
            int temp = 0;
            bool is_sort = false;
            for (int n = 0; n < sort.Length; n++)
            {
                is_sort = false;
                for (int i = 0; i < sort.Length - 1; i++)
                {
                    if (money[sort[i]] < money[sort[i + 1]])
                    {
                        temp = sort[i];
                        sort[i] = sort[i + 1];
                        sort[i + 1] = temp;

                        is_sort = true;
                    }
                }
                if (!is_sort) break;
            }
            정렬된데이터 = sort;



            int HalfSize = 랭킹_메인데이터.서브데이터.Length/2;

            for (int i = 0; i < 랭킹_메인데이터.서브데이터.Length; i++)
            {

                PlayerApiTemp = Networking.GetOwner(랭킹_메인데이터.서브데이터[정렬된데이터[i]].gameObject);
                if(정렬된데이터[i]==랭킹_메인데이터.서브데이터.Length-1)
                {
                    if (i < HalfSize)
                    {

                        정렬된스트링 = 정렬된스트링 + PlayerApiTemp.displayName + " " + 랭킹_메인데이터.서브데이터[정렬된데이터[i]].플레이어소지금 + "\n";
                    }
                    else
                    {
                        정렬된스트링2 = 정렬된스트링2 + PlayerApiTemp.displayName + " " + 랭킹_메인데이터.서브데이터[정렬된데이터[i]].플레이어소지금 + "\n";
                    }
                }
                else
                {
                    if (PlayerApiTemp != Master)
                    {
                        if (i < HalfSize)
                        {

                            정렬된스트링 = 정렬된스트링 + PlayerApiTemp.displayName + " " + 랭킹_메인데이터.서브데이터[정렬된데이터[i]].플레이어소지금 + "\n";
                        }
                        else
                        {
                            정렬된스트링2 = 정렬된스트링2 + PlayerApiTemp.displayName + " " + 랭킹_메인데이터.서브데이터[정렬된데이터[i]].플레이어소지금 + "\n";
                        }
                    }
                }
            }
            // 플레이어가 나가면 돈을 옮겨줘야함

            랭킹_메인데이터.게시판.text = 정렬된스트링;
            랭킹_메인데이터.게시판2.text = 정렬된스트링2;
        }
    }

}
