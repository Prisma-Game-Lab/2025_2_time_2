using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLibrary : MonoBehaviour
{
    [SerializeField] private SFXSound[] SFXSounds;
    private Dictionary<string, List<SFXClip>> SFXDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    public void InitializeDictionary()
    {
        SFXDictionary = new Dictionary<string, List<SFXClip>>();
        foreach (SFXSound group in SFXSounds)
        {
            SFXDictionary[group.name] = group.clipVariations;
        }
    }

    public AudioClip GetClipRandomVariation(string clipName, ref float volume, ref float pitchModifier)
    {
        List<SFXClip> currentSFXGroup;

        SFXDictionary.TryGetValue(clipName, out currentSFXGroup);
        if (currentSFXGroup == null)
        {
            print($"No audio with name {clipName}");
            return null;
        }

        if (currentSFXGroup.Count > 0)
        {
            int randomClipNumber = UnityEngine.Random.Range(0, currentSFXGroup.Count);

            SFXClip currentSFX = currentSFXGroup[randomClipNumber];

            volume = currentSFX.volumeModifier;
            pitchModifier = UnityEngine.Random.Range(currentSFX.minPitchShift, currentSFX.maxPitchShift);

            return currentSFX.audioClip;
        }
        return null;
    }
}

[Serializable]
public class SFXSound
{
    public string name;
    public List<SFXClip> clipVariations;
}

[Serializable]
public class SFXClip 
{
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volumeModifier = 0.5f;
    public float minPitchShift;
    public float maxPitchShift;
}