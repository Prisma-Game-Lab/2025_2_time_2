using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private MusicGroup[] musicList;
    private Dictionary<string, MusicGroup> musicDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    public void InitializeDictionary()
    {
        musicDictionary = new Dictionary<string, MusicGroup>();
        foreach (MusicGroup musicGroup in musicList)
        {
            musicDictionary[musicGroup.musicName] = musicGroup;
        }
    }

    public MusicGroup GetMusicClip(string musicName)
    {
        if (!musicDictionary.TryGetValue(musicName, out MusicGroup musicGroup))
        {
            print($"No Music named {musicName}");
            return null;
        }
        return musicGroup;
    }
}

[Serializable]
public class MusicGroup
{
    public string musicName;
    public AudioClip musicClip;
    [Range(0, 1)]
    public float volume = 0.5f;

    public MusicGroup(MusicGroup clone)
    {
        musicName = clone.musicName;
        musicClip = clone.musicClip;
        volume = clone.volume;
    }
}
