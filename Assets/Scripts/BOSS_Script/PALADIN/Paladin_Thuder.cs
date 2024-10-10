using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin_Thuder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Thunder1;
    [SerializeField] GameObject Thunder2;
    [SerializeField] GameObject Thunder3;
    [SerializeField] GameObject Thunder4;
    [SerializeField] GameObject Thunder5;
    [SerializeField] GameObject Thunder6;
    void Start()
    {
        Thunder1.SetActive(false);
        Thunder2.SetActive(false);
        Thunder3.SetActive(false);
        Thunder4.SetActive(false);
        Thunder5.SetActive(false);
        Thunder6.SetActive(false);
    }

    bool started = false;
    private void OnEnable()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(startThunder());
        }
    }

    private void OnDisable()
    {
        started = false;
    }

    IEnumerator startThunder()
    {
        CameraShake.Instance.ShakeCamera();
        Thunder1.SetActive (true);
        yield return new WaitForSeconds(1f);
        CameraShake.Instance.ShakeCamera();
        Thunder2.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        Thunder3.SetActive(true);
        Thunder2.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        CameraShake.Instance.ShakeCamera();
        Thunder1.SetActive(false);
        Thunder6.SetActive(true);
        yield return new WaitForSeconds(1f);
        CameraShake.Instance.ShakeCamera();
        Thunder5.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        CameraShake.Instance.ShakeCamera();
        Thunder6.SetActive (false);
        yield return new WaitForSeconds(1f);
        Thunder1.SetActive(false);
        Thunder2.SetActive(false);
        Thunder3.SetActive(false);
        Thunder4.SetActive(false);
        Thunder5.SetActive(false);
        Thunder6.SetActive(false);
        gameObject.SetActive(false);
    }
}
