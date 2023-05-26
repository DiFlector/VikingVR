using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string type;
    public int count;

    public string getType()
    {
        return type;
    }

    public int getCount()
    {
        return count;
    }
}
