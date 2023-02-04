using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Audio;
using static UnityEngine.Windows.WebCam.VideoCapture;

public class AudioManager : MonoBehaviour
{
    [Header("Debug Features")]
    public float audioDebugStart;   //Cambiar como variable en sound
    public float audioLatencyDelay;

    [Header("Sounds")]
    public Sound[] soundsRaw;

    [HideInInspector]
    public Dictionary<SoundType, Sound> sounds;

    void Awake()
    {
        sounds = new Dictionary<SoundType, Sound>();

        foreach(Sound sound in soundsRaw)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            //mainMusic.time = audioStart;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            sound.source.playOnAwake = false;

            sounds.Add(sound.type, sound);
        }
    }



    public void Play(SoundType type, bool degeneracy, float Time)
    {
        Sound sound;
        if(sounds.TryGetValue(type, out sound)) 
        {
            if (degeneracy)
            {
                sound.source.time = Time;
                sound.source.Play();
            }
            else
            {
                sound.source.time = 0;
                sound.source.Play();
            }
        }
    }
}
