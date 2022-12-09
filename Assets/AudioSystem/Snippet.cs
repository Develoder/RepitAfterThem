using System;

[Serializable]
public class Snippet
{
    public float Volume;
    public float Pitch;

    public float StartPlay;

    public Snippet(float volume, float pitch)
    {
        Volume = volume;
        Pitch = pitch;
    } 
}