using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    #region

    static Dictionary<AudioClipNames, AudioClip> sound = new Dictionary<AudioClipNames, AudioClip>();
    public static AudioSource audioSource;
    static bool initialized = false;
    static bool haptics = true;

    #endregion

    #region Properties

    public static bool Initialized 
    { get { return initialized; } }
    
    public static bool Haptics 
    { 
        get { return haptics; }
        set { haptics = value; }
    }

    #endregion

    #region Methods

    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;
        sound.Add(AudioClipNames.Clear, Resources.Load(@"Audio\clear") as AudioClip);
        sound.Add(AudioClipNames.Fail, Resources.Load(@"Audio\fail") as AudioClip);
        sound.Add(AudioClipNames.Success, Resources.Load(@"Audio\success") as AudioClip);
        sound.Add(AudioClipNames.LevelComplete, Resources.Load(@"Audio\level_complete") as AudioClip);
        sound.Add(AudioClipNames.Button, Resources.Load(@"Audio\button") as AudioClip);
        sound.Add(AudioClipNames.Applause, Resources.Load(@"Audio\applause") as AudioClip);
    }

    public static void Play(AudioClipNames name)
    {
        if(sound.ContainsKey(name) && haptics)
        {
            audioSource.PlayOneShot(sound[name]);
        }
    }

    #endregion
}
