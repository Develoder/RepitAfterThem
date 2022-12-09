using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        Queue<SnippetConfig> snippetQueue = new Queue<SnippetConfig>(snippetConfigs.OrderBy(x => x.Snippet.StartPlay));

        while (snippetQueue.Count != 0)
        {
            var snippet = snippetQueue.Dequeue();
            CreateAudioSource(snippet, compositor.transform);
            
            if (snippetQueue.Count == 0)
                break;
            
            yield return new WaitForSeconds(snippetQueue.Peek().Snippet.StartPlay - snippet.Snippet.StartPlay);
        }
        
        Debug.Log("End composition!!!");
    }

    private void CreateAudioSource(SnippetConfig snippet, Transform parentTransform)
    {
        GameObject audioObj = new GameObject($"{snippet.AudioClip.name} ({snippet.Snippet.StartPlay} to {snippet.AudioClip.length})");
        audioObj.transform.SetParent(parentTransform);
        audioObj.transform.position = Camera.main.transform.position;
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();

        audioSource.volume = snippet.Snippet.Volume;
        audioSource.pitch = snippet.Snippet.Pitch;
        audioSource.clip = snippet.AudioClip;
        
        audioSource.Play();

        StartCoroutine(DestroyAudioSource(audioSource));
    }

    IEnumerator DestroyAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource.gameObject);
    }
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