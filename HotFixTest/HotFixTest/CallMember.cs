using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFixTest
{
    public class CallMember
    {

        public int num = 0;

        public static void Test1()
        {
            Debug.Log("TEST1");

        }
        public void Add()
        {
            num++;
            Debug.Log("Add");
        }
        public void Log(string classID)
        {
            Debug.Log( string.Format("ClassID = {0}  Num = {1}",classID, num));
        }

        public void Test3()
        {
            Pud_Mono.Inst.StartCoroutine(co_Test());
        }

        public IEnumerator co_Test()
        {
            Debug.Log("Start Wait");
            while (!Pud_Mono.Inst.isTrue)
            {
                yield return 0;
            }
            Debug.Log("BBB");
        }

    }
}