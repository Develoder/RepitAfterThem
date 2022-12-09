using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public List<Audio> Audios;

    private void Start()
    {
        StartCoroutine(PlayComposition());
    }

    IEnumerator PlayComposition()
    {
        GameObject compositor = new GameObject("Compositor");

        List<SnippetConfig> snippetConfigs = new List<SnippetConfig>();

        foreach (var audio in Audios)
            foreach (var snippet in audio.Snippets)
                snippetConfigs.Add(new SnippetConfig(audio.AudioClip, snippet));

        snippetConfigs.OrderBy(x => x.Snippet.StartPlay);

        

        yield return null;

    }

    public struct SnippetConfig
    {
        public AudioClip AudioClip;
        public Snippet Snippet;

        public SnippetConfig(AudioClip audioClip, Snippet snippet)
        {
            AudioClip = audioClip;
            Snippet = snippet;
        }
    }
}
