using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public static class AudioSection
{
    public static event Action<AudioName> DeleteAudio;

    private static List<AudioName> _hideAudioName = new List<AudioName>();

    public static void Draw(List<Audio> audios)
    {
        foreach (var audio in audios)
        {
            string name = audio.Name.ToString();
            bool isHide = _hideAudioName.Contains(audio.Name);

            GUILayout.BeginVertical(GUI.skin.box);

            GUILayout.BeginHorizontal();

            DrawDeleteAudio(audio);
            EditorGUILayout.LabelField(name);
            DrawPlayAudio(audio);
            if (isHide)
                DrawShowButton(audio.Name);
            else
                DrawHideButton(audio.Name);

            GUILayout.EndHorizontal();


            if(!isHide)
            {
                audio.AudioClip = (AudioClip)EditorGUILayout.ObjectField("Audio", audio.AudioClip, typeof(AudioClip));
                DrawSnipets(audio);
            }

            GUILayout.EndVertical();
        }
    }


    private static void DrawSnipets(Audio audio)
    {
        for (int i = 0; i < audio.Snippets.Count; i++)
        {
            Snippet snippet = audio.Snippets[i];

            GUILayout.BeginVertical(GUI.skin.window);

            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"Snippet {i + 1}");
            DrawDeleteSnippet(audio, i);
            DrawAddSnippet(audio, i);

            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            snippet.Volume = EditorGUILayout.Slider("Volume", snippet.Volume, 0, 1);
            snippet.Pitch = EditorGUILayout.Slider("Pitch", snippet.Pitch, 0, 1);

            if (EditorGUI.EndChangeCheck())
            {
                //Debug.Log($"volume = {audio.AudioSettingsCustom.Volume}, pitch = {audio.AudioSettingsCustom.Pitch}");
            }

            snippet.StartPlay = EditorGUILayout.FloatField("StartPay", snippet.StartPlay);


            GUILayout.EndVertical();
        }

    }

    private static void DrawAddSnippet(Audio audio, int index)
    {
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            AddSnippet(audio, index);
        }
    }

    private static void DrawDeleteSnippet(Audio audio, int index)
    {
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            DeleteSnippet(audio, index);
        }
    }


    private static void DrawShowButton(AudioName audioName)
    {
        if (GUILayout.Button("⇒", GUILayout.Width(20), GUILayout.Height(20)))
        {
            _hideAudioName.Remove(audioName);
        }
    }

    private static void DrawHideButton(AudioName audioName)
    {
        if (GUILayout.Button("⇓", GUILayout.Width(20), GUILayout.Height(20)))
        {
            _hideAudioName.Add(audioName);
        }
    }

    private static void DrawPlayAudio(Audio audio)
    {
        if (GUILayout.Button("►", GUILayout.Width(20), GUILayout.Height(20)))
        {
            if (audio.AudioClip == null)
                return;

            EditorSFX.StopAllClips();
            EditorSFX.PlayClip(audio.AudioClip);
        }
    }

    private static void DrawDeleteAudio(Audio audio)
    { 
        if (GUILayout.Button("×", GUILayout.Width(15), GUILayout.Height(15)))
        {
            DeleteAudio?.Invoke(audio.Name);
        }
    }
    private static void DeleteSnippet(Audio audio, int index)
    {
        if (audio.Snippets.Count <= 1)
            return;

        audio.Snippets.RemoveAt(index);
    }    
    
    private static void AddSnippet(Audio audio, int index)
    {
        audio.Snippets.Insert(index + 1, new Snippet(1f, 1f));
    }

}
