using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEffect : MonoBehaviour
{
    public virtual void Initialize(CommandTarget target, CommandArguments arguments) {}

    public virtual void Activate() {}

    public virtual void Destroy() {}
}
