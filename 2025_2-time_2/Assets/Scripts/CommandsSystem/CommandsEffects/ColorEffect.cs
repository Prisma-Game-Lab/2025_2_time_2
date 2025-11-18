using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEffect : CommandEffect
{
    public override void Initialize(CommandTarget target, CommandArguments arguments)
    {
        Command commandScriptable = arguments.commandScriptable;
        List<string> parameters = arguments.parameters;

        target.ChangeCurrentColor(parameters[1]);
    }
}
