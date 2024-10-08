using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeHacking : MonoBehaviour
{
    public float staminaDrainTreeChopping = 10.0f;
    public float staminaDrainWoodChopping = 5.0f;

    public Animation anim;

    private AudioSource audioSource;

    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void startAnim()
    {
        if (anim != null)
        {
            if (anim)
            {
                anim.Play();
            }
        }
    }

    private void playSound()
    {
        if (audioSource != null)
        {
            int random = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[random];
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void treeChopping()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (
            Physics.Raycast(ray, out hit, 3.0f) /*, layersToIgnore*/
        )
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6)
            {
                // if InterActableObject was hit
                TreeChopping treeChoppingScript = hitGameObject.GetComponent<TreeChopping>();
                if (treeChoppingScript != null)
                {
                    if (treeChoppingScript.canBeChopped == true)
                    {
                        //if (StaminaController.canPerform(staminaDrainTreeChopping))
                        //{
                            //drain stamina
                            //StaminaController.UpdateStamina(-staminaDrainTreeChopping);

                            startAnim();
                            playSound();
                            treeChoppingScript.chopTree();
                        //}
                    }
                }
            }
        }
    }

    public void woodChopping()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (
            Physics.Raycast(ray, out hit, 3.0f) /*, layersToIgnore*/
        )
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (hitGameObject.layer == 6 || hitGameObject.layer == 11)
            {
                // if InterActableObject was hit
                WoodChopping woodChoppingScript = null;

                SnapToPlaceholder snapToPlaceholderScript =
                    hitGameObject.GetComponent<SnapToPlaceholder>();
                if (snapToPlaceholderScript != null)
                {
                    woodChoppingScript =
                        hitGameObject
                            .transform
                            .root
                            .GetComponentInChildren<WoodChopping>();
                }
                if (woodChoppingScript == null)
                {
                    woodChoppingScript =
                        hitGameObject.GetComponent<WoodChopping>();
                }

                if (woodChoppingScript != null)
                {
                    if (woodChoppingScript.isChoppable() == true)
                    {
                        /*if (StaminaController.canPerform(staminaDrainWoodChopping))
                        {
                            //drain stamina
                            StaminaController.UpdateStamina(-staminaDrainWoodChopping);

                            startAnim();
                            playSound(); */
                            woodChoppingScript.chopWood();
                        //}
                    }
                }
            }
        }
    }
}
