using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{

    #if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Progress Bar")]
        public static void AddLinearProgressBar(){
            GameObject obj = Instantiate(Resources.Load<GameObject>("UI_Elements/Linear Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }
    #endif

    #if UNITY_EDITOR
    [MenuItem("GameObject/UI/Radial Progress Bar")]
        public static void AddRadialProgressBar(){
            GameObject obj = Instantiate(Resources.Load<GameObject>("UI_Elements/Radial Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }
    #endif

    public float minimum;
    public float maximum;
    public float current;
    public Image mask;
    public Image fill;
    public Color color;
    private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetCurrentFill();
    }

    void GetCurrentFill(){
        float currentFillAmount = mask.fillAmount;
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float nextFillAmount = currentOffset / maximumOffset;
        //mask.fillAmount = fillAmount;
        mask.fillAmount = Mathf.Lerp(currentFillAmount, nextFillAmount, Time.deltaTime * speed);

        fill.color = color;
    }
}
