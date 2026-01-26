using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandTarget : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SizeEffect size;
    [SerializeField] private Command sizeScriptable;

    [Header("Variables")]
    [SerializeField] private string targetName;
    [SerializeField] private string colorName;
    [SerializeField] private TargetSize targetSize = TargetSize.Medium;
    private ColorInfo currentColor;

    [Header("Events")]
    [SerializeField] private UnityEvent<CommandEffectType> OnCommandEffectStart;
    [SerializeField] private UnityEvent<CommandEffectType> OnCommandEffectEnd;
    [SerializeField] private UnityEvent OnSizeChange;

    private void Start()
    {
        CommandsController.OnCommand.AddListener(ActivateCommand);

        StartUp();
    }

    private void OnValidate()
    {
        StartUp();
    }

    private void StartUp() 
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (size == null)
            size = GetComponent<SizeEffect>();

        ChangeCurrentColor(colorName);

        if (size != null)
        {
            size.Initialize(this, targetSize, sizeScriptable);
        }
    }

    public void ChangeCurrentColor(string newColor)
    {
        if (LevelColors.instance == null || newColor == null)
        {
            sr.color = Color.white;
            return;
        }

        ColorInfo newTargetColor = LevelColors.instance.GetTargetColor(newColor);

        if (newTargetColor != null)
        {
            currentColor = newTargetColor;
            colorName = newTargetColor.diplayName;
            sr.color = newTargetColor.RGB;
        }
        //else if (currentColor == null)
        //{
        //    newTargetColor = LevelColors.instance.GetDefaultColor();

        //    if (newTargetColor != null)
        //    {
        //        currentColor = newTargetColor;
        //        colorName = newTargetColor.diplayName;
        //        sr.color = newTargetColor.RGB;
        //    }
        //}
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

    public TargetSize GetTargetSize() 
    {
        return targetSize;
    }

    public void SetTargetSize(TargetSize newSize, bool invoke = true)
    {
        targetSize = newSize;
        if (invoke)
            OnSizeChange.Invoke();
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
                size.Initialize(this, arguments);
                OnSizeChange.Invoke();
                break;
            case CommandEffectType.Clear:
                //Just a test
                //Not final clear code
                //if (size != null)
                //    size.Destroy();
                break;
            case CommandEffectType.Color:
                ColorEffect color = GetComponent<ColorEffect>();
                if (color == null) 
                {
                    color = gameObject.AddComponent<ColorEffect>();
                }
                color.Initialize(this, arguments);
                break;
            case CommandEffectType.Stop:
                StopEffect stop = GetComponent<StopEffect>();
                if (stop == null)
                {
                    stop = gameObject.AddComponent<StopEffect>();
                }
                stop.Initialize(this, arguments);
                break;
        }

        OnCommandEffectStart.Invoke(commandEffect);
    }

    public void OnCommandEnd(CommandEffectType command) 
    {
        OnCommandEffectEnd.Invoke(command);
    }
}

public enum TargetSize 
{
    Small, Medium, Big, Altering
}
