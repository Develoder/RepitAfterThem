using System;

[Serializable]
public class AudioSettingsCustom
{
    public float Volume;
    public float Pitch;

    public AudioSettingsCustom(float volume, float pitch)
    {
        Volume = volume;
        Pitch = pitch;
    } 
}