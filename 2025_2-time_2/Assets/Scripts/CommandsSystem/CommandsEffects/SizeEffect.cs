using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    public enum Size 
    {
        Small, Medium, Big
    }

    private CommandTarget target;

    private float modifierValue;

    private Vector2 startScale;
    private TargetSize startSize = TargetSize.Altering;
    private Vector2 initialScale;
    private Vector2 targetScale;
    private TargetSize desiredSize;

    private int skipFrame;
    private bool interpolateScale;
    private float currentTime;
    [SerializeField] private Command sizeScriptable;
    [SerializeField] private float interpolationTime = 2;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (interpolateScale)
        {
            currentTime += Time.deltaTime;
            if (skipFrame != 1) 
            {
                InterpolateScale(initialScale, targetScale);
                skipFrame++;
            }
            else 
            {
                skipFrame = 0;
            }
        }
    }

    public override void Initialize(CommandTarget target, CommandArguments arguments) 
    {
        this.target = target;
        if (startSize == TargetSize.Altering)
            startSize = target.GetTargetSize();

        List<string> parameters = arguments.parameters;

        int index;
        if (!sizeScriptable.GetModifierValue(parameters[1], out modifierValue, out index))
            Debug.LogError("No Modifier Value found for Size");

        desiredSize = (TargetSize)index;

        if (target.GetTargetSize() == desiredSize)
            return;

        currentTime = 0;
        initialScale = transform.localScale;
        targetScale = Vector2.one * modifierValue;
        interpolateScale = true;

        target.SetTargetSize(TargetSize.Altering);
    }

    public void Initialize(CommandTarget target, TargetSize targetSize)
    {
        this.target = target;
        if (startSize == TargetSize.Altering)
            startSize = target.GetTargetSize();

        if (!sizeScriptable.GetModifierValue((int)targetSize, out modifierValue))
            Debug.LogError("No Modifier Value found for Size");

        transform.localScale = Vector3.one * modifierValue;
        target.SetTargetSize(targetSize);
    }

    public override void Destroy()
    {
        ReturnToOriginalScale();
    }

    private void InterpolateScale(Vector2 startScale, Vector2 targetScale) 
    {
        float ratio = currentTime / interpolationTime;

        transform.localScale = Vector2.Lerp(startScale, targetScale, ratio);

        if (ratio >= 1)
        {
            interpolateScale = false;
            target.SetTargetSize(desiredSize);
        }
    }

    private void ReturnToOriginalScale()
    {
        if (startSize == target.GetTargetSize())
            return;

        currentTime = 0;
        targetScale = startScale;
        initialScale = transform.localScale;
        desiredSize = startSize;
        interpolateScale = true;
        target.SetTargetSize(TargetSize.Altering);
    }
}

public struct SizeMap 
{
    public string sizeName;
    public SizeEffect.Size sizeEnum;
}