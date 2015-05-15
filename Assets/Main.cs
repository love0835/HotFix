using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
    private string classID;
    private string c2;
    void Awake()
    {
        HotFixMgr.Inst.Init();
    }
	void Start ()
	{
        classID = HotFixMgr.Inst.NewClass("CallMember");
        c2 = HotFixMgr.Inst.NewClass("CallMember");
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("Add"))
        {
            HotFixMgr.Inst.CallMethod(classID, "Add", null ,null);
            HotFixMgr.Inst.CallMethod(classID, "Log", new System.Type[] { typeof(string) }, new object[] { classID });
        }
        if (GUILayout.Button("Add2"))
        {
            HotFixMgr.Inst.CallMethod(c2, "Add", null, null);
            HotFixMgr.Inst.CallMethod(c2, "Log", new System.Type[] { typeof(string) }, new object[] { c2 });
        }
    }
}
