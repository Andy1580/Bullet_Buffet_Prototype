using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musica;
    public Sound[] sfx;
    [Range(0f, 1f)] public float volumenMusica = 1f;
    [Range(0f, 1f)] public float volumenSFX = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InicializarSonidos(musica, volumenMusica);
        InicializarSonidos(sfx, volumenSFX);
    }

    private void InicializarSonidos(Sound[] soundsArray, float volumeGroup)
    {
        foreach (Sound s in soundsArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume * volumeGroup;
            s.source.pitch = s.pitch;

            Debug.Log($"Sonido inicializado: {s.nombre}, Volumen: {s.source.volume}, Loop: {s.source.loop}");
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(musica, sound => sound.nombre == name);
        if (s == null)
        {
            s = Array.Find(sfx, sound => sound.nombre == name);
        }

        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Sonido no encontrado: " + name);
        }
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(musica, sound => sound.nombre == name);
        if (s == null)
        {
            s = Array.Find(sfx, sound => sound.nombre == name);
        }

        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sonido no encontrado: " + name);
        }
    }

    public void AjustarVolumenMusica(float volumen)
    {
        volumenMusica = volumen;
        foreach (Sound s in musica)
        {
            s.source.volume = s.volume * volumenMusica;
        }
    }

    public void AjustarVolumenSFX(float volumen)
    {
        volumenSFX = volumen;
        foreach (Sound s in sfx)
        {
            s.source.volume = s.volume * volumenSFX;
        }
    }
}

