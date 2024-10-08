using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Campfire : MonoBehaviour
{

    public List<GameObject> woodLogs;
    public List<GameObject> particles;
    public Light light;

    [SerializeField] private int logsToInsert = 0;
    [SerializeField] private int logsInserted = 0;

    private AudioSource audioSource;


    [SerializeField] private List<SnapToPlaceholder> placeholderScripts;

    public bool canBeBurned = false;
    public bool isBurning = false;

    // Start is called before the first frame update
    void Start()
    {
        logsToInsert = woodLogs.Count;
        placeholderScripts = getPlaceholderScripts(woodLogs);
        playParticles(false);

        this.audioSource = GetComponent<AudioSource>();

        if (isBurning)
        {
            playParticles(true);
            playSound(true);
            StartCoroutine(toggleLight(true));
        }else{
            playParticles(false);
            playSound(false);
            StartCoroutine(toggleLight(false));
        }
    }

    private void playSound(bool play)
    {
        if (audioSource != null)
        {
            if (play && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (!play)
            {
                audioSource.Stop();
            }

        }
    }

    IEnumerator toggleLight(bool play)
    {
        float elapsedTime = 0;
        float start = 0;
        float end = 25;
        float animationTime = 2f;
        while (elapsedTime < animationTime)
        {
            float time = elapsedTime / animationTime;
            if (play)
            {
                light.intensity = Mathf.Lerp(start, end, time);
            }
            else
            {
                light.intensity = Mathf.Lerp(end, start, time);
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private List<SnapToPlaceholder> getPlaceholderScripts(List<GameObject> objects)
    {
        List<SnapToPlaceholder> placeholderScripts = new List<SnapToPlaceholder>();
        foreach (GameObject obj in objects)
        {
            SnapToPlaceholder snapToPlaceholderScript = obj.GetComponent<SnapToPlaceholder>();
            if (snapToPlaceholderScript != null)
            {
                placeholderScripts.Add(snapToPlaceholderScript);
            }
        }
        return placeholderScripts;
    }

    private void playParticles(bool play)
    {
        foreach (GameObject particle in particles)
        {
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                if (play && !ps.isPlaying)
                {
                    ps.Play();
                }
                else if (!play && !ps.isStopped)
                {
                    ps.Stop();
                }

            }
        }
    }

    private bool checkForLogs()
    {
        List<SnapToPlaceholder> snappedOn = new List<SnapToPlaceholder>();
        if (placeholderScripts != null)
        {
            foreach (SnapToPlaceholder placeholderScript in placeholderScripts)
            {
                if (placeholderScript.isSnappedOn == true)
                {
                    if (!snappedOn.Contains(placeholderScript))
                    {
                        snappedOn.Add(placeholderScript);
                    }
                }
            }
            logsInserted = snappedOn.Count;
            if (logsInserted == logsToInsert)
            {
                return true;
            }
        }
        return false;
    }

    public void startFire()
    {
        canBeBurned = checkForLogs();
        if (canBeBurned)
        {
            isBurning = true;
            playParticles(true);
            playSound(true);
            StartCoroutine(toggleLight(true));
        }
        else
        {
            isBurning = false;
            playParticles(false);
            playSound(false);
            StartCoroutine(toggleLight(false));
        }
    }

    public void stopFire()
    {
        playParticles(false);
        playSound(false);
        isBurning = false;
        StartCoroutine(toggleLight(false));
    }
}
