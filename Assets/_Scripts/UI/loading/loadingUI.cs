using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class loadingUI : MonoBehaviour {

    private Slider slider;

    public float loadingPercent = 0f;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        StartCoroutine(loading_AA());
	}
	
	// Update is called once per frame
	void Update () 
    {
        slider.value = loadingPercent;
        if (slider.value>=1f)
        {
            //if (Login.isConnected)
            {
                Application.LoadLevel(2);
            }
            
        }
        
	}

    IEnumerator loading_AA()
    {
        yield return null;
        while (loadingPercent < 1f)
        {
            loadingPercent += Time.deltaTime;
            yield return null;
        }
    }
}
