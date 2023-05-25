using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelect : MonoBehaviour
{
    public Animator animator;

    public void SetAnim(string animName)
    {
        print("gay");
        animator.SetTrigger(animName);
    }
}
