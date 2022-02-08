using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound
{
    public enum Zvuk
    {
        Skok,
        Poen,
        kraj,
       
    }

    public static void pustiZvuk(Zvuk zvuk) {
        GameObject go = new GameObject("Sound", typeof(AudioSource));
        AudioSource audio = go.GetComponent<AudioSource>();
        audio.PlayOneShot(getZvuk(zvuk));
    }

    private static AudioClip getZvuk(Zvuk zvuk) {
        foreach (GameAssets.AudioZvuk zvukAudioKlipa in GameAssets.GetInstancu().nizZvukova) {
            if (zvukAudioKlipa.zvuk == zvuk) {
                return zvukAudioKlipa.audioklip;
            }
        }
        return null;
    }
}
