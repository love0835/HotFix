using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HotFixMgr
{

    private static HotFixMgr _inst = null;

    private static readonly object syncObj = new object();

    HotFixMgr() { }

    public static HotFixMgr Inst
    {
        get
        {
            if (null == _inst)
            {
                lock (syncObj)
                {
                    if (null == _inst)
                    {
                        _inst = new HotFixMgr();
                    }
                }

            }
            return _inst;
        }
    }

    public class ClassMethod
    {
        public CLRSharp.ICLRType type;
        public CLRSharp.IMethod methodctor;
        public object typeObj;
        
    }

    private string file = "HotFixTest";
    private CLRSharp.CLRSharp_Environment env;
    private CLRSharp.ThreadContext context;
    private Dictionary<string, ClassMethod> methods = new Dictionary<string, ClassMethod>();
    private Dictionary<string, int> classCounts = new Dictionary<string, int>();
    public void Init()
    {
        env = new CLRSharp.CLRSharp_Environment(new Logger());
        //加载L#模块
        TextAsset dll = Resources.Load(file + ".dll") as TextAsset;
        TextAsset pdb = Resources.Load(file + ".pdb") as TextAsset;
        System.IO.MemoryStream msDll = new System.IO.MemoryStream(dll.bytes);
        System.IO.MemoryStream msPdb = new System.IO.MemoryStream(pdb.bytes);
        //env.LoadModule (msDll);//如果无符号是pdb的话，第二个参数传null
        env.LoadModule(msDll, msPdb, new Mono.Cecil.Pdb.PdbReaderProvider());//Pdb
        //env.LoadModule(msDll, msMdb, new Mono.Cecil.Mdb.MdbReaderProvider());//如果符号是Mdb格式
        Debug.Log(string.Format("LoadModule {0}.dll done.", file));
        //step01建立一个线程上下文，用来模拟L#的线程模型，每个线程创建一个即可。
        context = new CLRSharp.ThreadContext(env);
        Debug.Log("Create ThreadContext for L#.");
    }

    public string NewClass(string className)
    {
        //step02取得想要调用的L#类型
        CLRSharp.ICLRType wantType = env.GetType(file+"."+className);//用全名称，包括命名空间
        CLRSharp.IMethod methodctor = wantType.GetMethod(".ctor", CLRSharp.MethodParamList.constEmpty());//取得构造函数
        object typeObj = methodctor.Invoke(context, null, null);//执行构造函数
        Debug.Log("GetType:" + wantType.Name);
        ClassMethod method = new ClassMethod();
        method.type = wantType;
        method.methodctor = methodctor;
        method.typeObj = typeObj;

        if (!classCounts.ContainsKey(className))
            classCounts.Add(className, 0);

        classCounts[className]++;
        string id = string.Format("{0}.{1}", className, classCounts[className]);

        if (!methods.ContainsKey(id))
            methods.Add(id, method);
        else
        {
            methods[id] = null;
            methods[id] = method;
        }
        return id;
    }
    public void CallMethod(string classID, string methodName, System.Type[] types, object[] list)
    {
        
        if (!methods.ContainsKey(classID))
            return;
        ClassMethod method = methods[classID];
        CLRSharp.IMethod method02;
        if (types != null && list != null)
        {
            CLRSharp.ICLRType[] _rtType = new CLRSharp.ICLRType[types.Length];
            for (int n = 0; n < types.Length; n++)
            {
                _rtType[n] = env.GetType(types[n]);
            }
            var typeList = CLRSharp.MethodParamList.Make(_rtType);
            method02 = method.type.GetMethod(methodName, typeList);
        }
        else
        {
            method02 = method.type.GetMethod(methodName, CLRSharp.MethodParamList.constEmpty());
        }
        
        
        method02.Invoke(context, method.typeObj, list);
    }
    /*
    public void CallMethod_Static(string className, string methodName, System.Type[] types, object[] list)
    {
        //step02取得想要调用的L#类型
        CLRSharp.ICLRType wantType = env.GetType(file + "." + className);//用全名称，包括命名空间
        //和反射代码中的Type.GetType相对应

        CLRSharp.ICLRType[] _rtType = new CLRSharp.ICLRType[types.Length];
        for (int n = 0; n < types.Length; n++)
        {
            _rtType[n] = env.GetType(types[n]);
        }
        var typeList = CLRSharp.MethodParamList.Make(_rtType);
        //得到类型上的一个函数，第一个参数是函数名字，第二个参数是函数的参数表，这是一个没有参数的函数
        CLRSharp.IMethod method01 = wantType.GetMethod(methodName, typeList);
        method01.Invoke(context, null, list);
    }
    */
    /*
     * 还找不到实现的方法
    public object GetField(string classID, string fieldName)
    {
        if (!methods.ContainsKey(classID))
            return null;
        ClassMethod method = methods[classID];
        CLRSharp.IField feild =  method.type.GetField(fieldName);
        return feild.Get(feild);
        //return feild.Get(context);
    }
    */
    public void Destroy(string classID)
    {
        if (!methods.ContainsKey(classID))
            return;
        ClassMethod method = methods[classID];

    }

    public class Logger : CLRSharp.ICLRSharp_Logger//实现L#的LOG接口
    {
        public void Log(string str)
        {
            Debug.Log(str);
        }

        public void Log_Error(string str)
        {
            Debug.LogError(str);
        }

        public void Log_Warning(string str)
        {
            Debug.LogWarning(str);
        }
    }
}
