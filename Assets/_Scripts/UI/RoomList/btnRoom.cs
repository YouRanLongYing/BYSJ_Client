using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class btnRoom : MonoBehaviour {
    public int roomId;
    public string roomName;
    public string owner;
	// Use this for initialization
	void Start () 
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnClick()
    {

    }
}
