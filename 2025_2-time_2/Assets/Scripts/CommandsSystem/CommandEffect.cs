using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEffect : MonoBehaviour
{
    protected string parameter;
    protected float[] strength;

    public virtual void Activate() { }

    public void SetParameter(string parameter) 
    {
        this.parameter = parameter;
    }

    public void SetStrength(float[] strength)
    {
        this.strength = strength;
    }
}
