using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    public bool round = false;

    public GameObject roundMinimap;
    public GameObject cubicMinimap;

    private void OnValidate() {
        roundMinimap.SetActive(round);
        cubicMinimap.SetActive(!round);
    }
}
