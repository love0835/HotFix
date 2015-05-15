using UnityEngine;
using System.Collections;

public class Pud_Mono : MonoBehaviour
{
    private static Pud_Mono _inst = null;
    public static Pud_Mono Inst
    {
        get { return _inst; }
    }

    void Awake()
    {
        _inst = this;
    }

    public void Bird()
    {
        Debug.Log("123");
    }
}
