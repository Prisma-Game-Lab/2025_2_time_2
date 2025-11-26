using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTarget : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer sr;

    [Header("Variables")]
    [SerializeField] private string targetName;
    [SerializeField] private string colorName;
    private TargetColor currentColor;

    private SizeEffect size;

    private void Start()
    {
        CommandsController.OnCommand.AddListener(ActivateCommand);

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        ChangeCurrentColor(colorName);
    }

    private void OnValidate()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        ChangeCurrentColor(colorName);
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
                size.Initialize(this, arguments);
                break;
            case CommandEffectType.Clear:
                //Just a test
                //Not final clear code
                if (size != null)
                    size.Destroy();
                break;
            case CommandEffectType.Color:
                ColorEffect color = gameObject.AddComponent<ColorEffect>();
                color.Initialize(this, arguments);
                break;
        }
    }

    public string GetDisplayName() 
    {
        if (colorName != null)
        {
            string displayName = colorName + " " + targetName;
            return displayName;
        }
        return targetName;
    }

    public void ChangeCurrentColor(string newColor) 
    {
        if (LevelColors.instance == null || newColor == null) 
        {
            sr.color = Color.white;
            return;
        }

        TargetColor newTargetColor = LevelColors.instance.GetTargetColor(newColor);

        if (newTargetColor != null)
        {
            currentColor = newTargetColor;
            colorName = newTargetColor.diplayName;
            sr.color = newTargetColor.RGB;
        }
        else if (currentColor == null)
        {
            newTargetColor = LevelColors.instance.GetDefaultColor();

            if (newTargetColor != null)
            {
                currentColor = newTargetColor;
                colorName = newTargetColor.diplayName;
                sr.color = newTargetColor.RGB;
            }
        }
    }
}
