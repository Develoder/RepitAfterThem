using System;
using System.Collections.Generic;
using UnityEngine;
    
[Serializable]
public class Audio
{
    [HideInInspector] public string ID;
    [HideInInspector] public AudioClip AudioClip;
    [HideInInspector] public AudioName Name;
    [HideInInspector] public List<Snippet> Snippets;

}
