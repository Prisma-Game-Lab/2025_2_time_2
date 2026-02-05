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

    private bool interpolateScale;
    private float currentTime;
    [SerializeField] private float interpolationTime = 2;

    private Coroutine activeDestructionCoroutine;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        if (interpolateScale)
        {
            currentTime += Time.deltaTime;
            InterpolateScale(initialScale, targetScale);
        }
    }

    public override void Initialize(CommandTarget target, CommandArguments arguments) 
    {
        this.target = target;
        if (startSize == TargetSize.Altering)
            startSize = target.GetTargetSize();

        Command commandScriptable = arguments.commandScriptable;
        List<string> parameters = arguments.parameters;

        int index;
        if (!commandScriptable.GetModifierValue(parameters[1], out modifierValue, out index))
            Debug.LogError("No Modifier Value found for Size");

        desiredSize = (TargetSize)index;

        if (target.GetTargetSize() == desiredSize)
            return;

        if (activeDestructionCoroutine != null)
        {
            StopCoroutine(activeDestructionCoroutine);
            activeDestructionCoroutine = null;
        }

        currentTime = 0;
        initialScale = transform.localScale;
        print(modifierValue);
        targetScale = Vector2.one * modifierValue;
        interpolateScale = true;

        target.SetTargetSize(TargetSize.Altering);
    }

    public void Initialize(CommandTarget target, TargetSize targetSize, Command commandScriptable)
    {
        this.target = target;
        if (startSize == TargetSize.Altering)
            startSize = target.GetTargetSize();

        if (!commandScriptable.GetModifierValue((int)targetSize, out modifierValue))
            Debug.LogError("No Modifier Value found for Size");

        if (activeDestructionCoroutine != null)
        {
            StopCoroutine(activeDestructionCoroutine);
            activeDestructionCoroutine = null;
        }

        transform.localScale = Vector3.one * modifierValue;
        target.SetTargetSize(targetSize);
    }

    public override void Destroy()
    {
        activeDestructionCoroutine = StartCoroutine(DestructionSequence());
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
        currentTime = 0;
        targetScale = startScale;
        initialScale = transform.localScale;
        desiredSize = startSize;
        interpolateScale = true;
        target.SetTargetSize(TargetSize.Altering);
    }

    private IEnumerator DestructionSequence()
    {
        ReturnToOriginalScale();
        yield return new WaitForSeconds(interpolationTime);
        //Destroy(this);
    }
}

public struct SizeMap 
{
    public string sizeName;
    public SizeEffect.Size sizeEnum;
}