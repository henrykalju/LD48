using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetMasterSound(float soundLevel)
    {
    masterMixer.SetFloat ("Master", soundLevel);
    }

    public void SetMusicSound(float soundLevel)
    {
    masterMixer.SetFloat ("Music", soundLevel);
    }

    public void SetAmbientSound(float soundLevel)
    {
    masterMixer.SetFloat ("Ambient", soundLevel);
    }

    public void SetEffectsSound(float soundLevel)
    {
    masterMixer.SetFloat ("Effects", soundLevel);
    }
}
