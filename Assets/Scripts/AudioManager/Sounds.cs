﻿using System;
using UnityEngine;

[Serializable]
public class Sounds {
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(0.0f, 1f)] public float pitch;

    public bool loop;

    [HideInInspector] public AudioSource source;
}