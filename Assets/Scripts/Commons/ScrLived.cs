using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrLived : MonoBehaviour
{
    public float life;

    // Update is called once per frame
    public void checkLife()
    {
        if(this.life <= 0)
        {
            this.lifeGotZero();
        }
    }

    public abstract void lifeGotZero();
}
