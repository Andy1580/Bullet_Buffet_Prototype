using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public Sound[] musica;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach (Sound s in musica)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

    }

    public void PlaySound(string name)
    {

        foreach(Sound s in musica)
        {
            if (s.nombre == name)
            {
                s.source.Play();
                return;
            }
        }

        Debug.LogWarning("No hay ningun sonido");
    }

    public void StopSound(string name)
    {

        foreach (Sound s in musica)
        {
            if (s.nombre == name)
            {
                s.source.Stop();
                return;
            }
        }

        Debug.LogWarning("No hay ningun sonido");
    }

}
