using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : CommandEffect
{
     private CommandTarget target1;


     public override void Initialize(CommandTarget target1, CommandArguments arguments)
    {
        this.target1 = target1;
        
        Switch();
    }

    private void Switch()
    {
        // Switching with the player for now, but this command should have 2 targets instead of 1
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos1 = target1.transform.position;
        Vector3 pos2 = player.transform.position;

        target1.transform.position = pos2;
        player.transform.position = pos1;
        
    }
}
