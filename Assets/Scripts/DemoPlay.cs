using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayDemo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayDemo()
    {
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene("Demo");
    }
}
