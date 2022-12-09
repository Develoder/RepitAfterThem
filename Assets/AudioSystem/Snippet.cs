using System;

[Serializable]
public class Snippet
{
    public float Volume;
    public float Pitch;

    public float StartPlay;

    public Snippet(float volume, float pitch, float startPlay = 0f)
    {
        Volume = volume;
        Pitch = pitch;
        StartPlay = startPlay;
    } 
}