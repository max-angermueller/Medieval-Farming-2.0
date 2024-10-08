using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float globalVolume = 1.0f;

    void Awake()
    {
        AudioListener.volume = globalVolume;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnValidate()
    {
        AudioListener.volume = globalVolume;
    }
}
