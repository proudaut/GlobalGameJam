using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public enum MusicID : int
{
    NoMusic,
    Menu,
    Level1,
    Level2,
}


public class MusicManager : MonoBehaviour
{
    static public MusicManager instance;
    public AudioSource mMusicSource;
    public MusicID mCurrentSong = MusicID.NoMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    AudioClip GetResourceFromSource(string assetName)
    {
        return Resources.Load(assetName) as AudioClip;
    }

    AudioClip GetMusicFromResources(MusicID musicID)
    {
        AudioClip clip = null;
        switch (musicID)
        {
            case MusicID.Menu:
                clip = GetResourceFromSource("menu") as AudioClip;
                break;
            case MusicID.Level1:
                clip = GetResourceFromSource("Chorus1") as AudioClip;
                break;
            case MusicID.Level2:
                clip = GetResourceFromSource("Chorus2") as AudioClip;
                break;
            default:
                clip = null;
                break;
        }
        return clip;
    }

    // if _Id < 0, the current music clip will be played
    static public void PlayMusique(MusicID musicID)
    {
        if (instance != null)
        {
            if ((MusicID)musicID != MusicID.NoMusic)
            {
                if (instance.mCurrentSong != (MusicID)musicID)
                {
                    instance.mCurrentSong = (MusicID)musicID;
                    instance.mMusicSource.clip = instance.GetMusicFromResources(instance.mCurrentSong);
                    instance.mMusicSource.Play();
                }
            }
            else if (instance.mCurrentSong != MusicID.NoMusic)
            {
                instance.mMusicSource.clip = instance.GetMusicFromResources(instance.mCurrentSong);
                instance.mMusicSource.Play();
            }
        }
    }

    static public void StopMusique()
    {
        if (instance != null)
        {
            instance.mMusicSource.Stop();
        }
    }


}
