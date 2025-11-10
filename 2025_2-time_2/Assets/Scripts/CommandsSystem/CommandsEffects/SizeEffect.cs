using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SizeEffect : CommandEffect
{
    private float modifierValue;

    private Vector2 startScale;
    private Vector2 initialScale;
    private Vector2 targetScale;

    private bool interpolateScale;
    private float currentTime;
    private float interpolationTime = 2;

    private Coroutine activeDestructionCoroutine;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (interpolateScale)
        {
            currentTime += Time.deltaTime;
            InterpolateScale(initialScale, targetScale);
        }
    }

    public override void Initialize(CommandArguments arguments) 
    {
        Command commandScriptable = arguments.commandScriptable;
        List<string> parameters = arguments.parameters;

        if (!commandScriptable.GetModifierValue(parameters[1], out modifierValue))
            Debug.LogError("No Modifier Value found for Size");

        if (activeDestructionCoroutine != null)
        {
            StopCoroutine(activeDestructionCoroutine);
            activeDestructionCoroutine = null;
        }

        currentTime = 0;
        initialScale = transform.localScale;
        targetScale = Vector2.one * modifierValue;
        interpolateScale = true;
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
            interpolateScale = false;
    }

    private void ReturnToOriginalScale()
    {
        currentTime = 0;
        targetScale = startScale;
        initialScale = transform.localScale;
        interpolateScale = true;
    }

    private IEnumerator DestructionSequence()
    {
        ReturnToOriginalScale();
        yield return new WaitForSeconds(interpolationTime);
        Destroy(this);
    }
}
