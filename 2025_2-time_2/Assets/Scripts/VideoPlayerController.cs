using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Video Variables")]
    [SerializeField] private string videoFileName;

    [Header("Events")]
    [SerializeField] private UnityEvent OnVideoEndEvent;

    private void Start()
    {
        PlayVideo();
    }

    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void PlayVideo() 
    {
        if (videoPlayer != null)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    private void OnVideoEnd(VideoPlayer vp) 
    {
        OnVideoEndEvent.Invoke();
    }
}
