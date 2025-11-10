using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTarget : MonoBehaviour
{
    [SerializeField] private string targetName;
    [SerializeField] private string color;

    private SizeEffect size;

    private void Start()
    {
        CommandsController.OnCommand.AddListener(ActivateCommand);    
    }

    private void ActivateCommand(CommandArguments arguments)
    {
        CommandEffectType commandEffect = arguments.commandScriptable.effect;

        if (arguments.commandScriptable.hasTarget)
        { 
            if (arguments.parameters.Count == 0)
                return;
            if (arguments.parameters[0] != GetDisplayName())
            {
                if (commandEffect == CommandEffectType.Size && size != null)
                    size.Destroy();
                return;
            }
        }

        switch (commandEffect)
        {
            case CommandEffectType.Size:
                if (size == null)
                { 
                    size = gameObject.AddComponent<SizeEffect>();
                }
                size.Initialize(arguments);
                break;
            case CommandEffectType.Clear:
                //Just a test
                //Not final clear code
                if (size != null)
                    size.Destroy();
                break;
        }
    }

    public string GetDisplayName() 
    {
        if (color != null)
        {
            string displayName = color + " " + targetName;
            return displayName;
        }
        return targetName;
    }
}
