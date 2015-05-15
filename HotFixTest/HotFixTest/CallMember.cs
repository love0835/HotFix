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
            Pud
        }

        public IEnumerator co_Test()
        {
            yield return new WaitForSeconds(3);
            Debug.Log("BBB");
        }


        public void Update()
        {
            Debug.Log("abc");
        }
    }
}