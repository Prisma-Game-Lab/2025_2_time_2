using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEffect : CommandEffect
{
    private CommandTarget targetScript;
    private Rigidbody2D targetRb;
    private RigidbodyType2D targetPreviousBodyType;
    private Vector2 originalVelocity;

    public override void Initialize(CommandTarget target, CommandArguments arguments)
    {
        Command commandScriptable = arguments.commandScriptable;
        List<string> parameters = arguments.parameters;

        targetScript = target;
        targetRb = target.gameObject.GetComponent<Rigidbody2D>();

        ApplyEffect();
        StartCoroutine(EndEffectTimer(commandScriptable.strength));
    }

    private void ApplyEffect() 
    {
        targetPreviousBodyType = targetRb.bodyType;
        originalVelocity = targetRb.velocity;
        targetRb.bodyType = RigidbodyType2D.Static;
    }

    private void EndEffect()
    {
        targetRb.bodyType = targetPreviousBodyType;
        targetRb.velocity = originalVelocity;
        targetScript.OnCommandEnd(CommandEffectType.Stop);
    }

    private IEnumerator EndEffectTimer(float duration) 
    {
        yield return new WaitForSeconds(duration);
        EndEffect();
    } 
}
