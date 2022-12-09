using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioSystem))]
public class AudioSystemEditor : Editor
{
    private const string _audioFile = "AudioName";
    private const string _audioBaseName = "New Audio Name";
 
    private AudioSystem _audioSystem;
    private string _pathToEnumFile;

    private string _audioName = _audioBaseName;
    
    private void OnEnable()
    {
        _audioSystem = (AudioSystem)target;
        _pathToEnumFile = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(_audioFile)[0]);
        AudioSection.DeleteAudio += RemoveAudio;
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        _audioSystem.Audios = RefreshAudios(_audioSystem.Audios);
        AudioSection.Draw(_audioSystem.Audios);

        DrawNewAudioSection();
    }

    

    private void DrawNewAudioSection()
    {
        GUILayout.Space(15f);

        //GUILayout.BeginHorizontal();

        _audioName = EditorGUILayout.TextField("Name", _audioName);
        DrawAddButton();

        //GUILayout.EndHorizontal();
    }

    private void DrawAddButton()
    {
        if (GUILayout.Button("Add"))
        {
            AddAudio();
        }
    }

    private void AddAudio()
    {
        if(_audioName == string.Empty)
            return;
        
        if (!Regex.IsMatch(_audioName, @"^[a-zA-Z][a-zA-Z0-9_]*$"))
            return;

        Array array = Enum.GetValues(typeof(AudioName));
        if (array.Length != 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (_audioName == array.GetValue(i).ToString())
                {
                    Debug.LogError("A path with the same name has already been");
                    
                    return;
                }
            }
        }
        
        EnumEditor.WriteToFile(_audioName, _pathToEnumFile);
        Refresh();
        
        _audioName = _audioBaseName;
    }

    private void RemoveAudio(AudioName audioName)
    {
        if (!EnumEditor.TryRemoveFromFile(audioName.ToString(), _pathToEnumFile))
            return;

        Refresh();
    }

    private void Refresh()
    {
        Debug.Log("WAIT");
        var realivePath = _pathToEnumFile.Substring(_pathToEnumFile.IndexOf("Assets"));
        AssetDatabase.ImportAsset(realivePath);
    }
    
    
    private List<Audio> RefreshAudios(List<Audio> oldAudios)
    {
        int rountAudio = Enum.GetNames(typeof(AudioName)).Length;
        List<Audio> routes = new List<Audio>(rountAudio);

        for (int i = 0; i < rountAudio; i++)
        {
            AudioName audioName = (AudioName)i;
            Audio audio = TryRestoreAudio(oldAudios, audioName);

            if (audio == null)
                audio = CreateNewAudio(audioName);

            routes.Add(audio);
        }

        return routes;
    }

    private Audio TryRestoreAudio(List<Audio> oldAudios, AudioName audioName)
    {
        Audio audio;

        if (oldAudios.Count == 0)
            audio = CreateNewAudio(audioName);
        else
            audio =  oldAudios.FirstOrDefault(x => x.Name == audioName);

        return audio;
    }

    private Audio CreateNewAudio(AudioName audioName)
    {
        Audio route = new Audio
        {
            Name = audioName,
            Snippets = new List<Snippet> { new Snippet(1f, 1f) }
        };

        return route;
    }
}
