using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PALADIN_AOE : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject AOEATTACKL1;
    [SerializeField] GameObject AOEATTACKL2;
    [SerializeField] GameObject AOEATTACKL3;
    [SerializeField] GameObject AOEATTACKL4;
    [SerializeField] GameObject AOEATTACKL5;
    [SerializeField] GameObject AOEATTACKR1;
    [SerializeField] GameObject AOEATTACKR2;
    [SerializeField] GameObject AOEATTACKR3;
    [SerializeField] GameObject AOEATTACKR4;
    [SerializeField] GameObject AOEATTACKR5;
    void Start()
    {
        AOEATTACKL1.SetActive(false);
        AOEATTACKL2.SetActive(false);
        AOEATTACKL3.SetActive(false);
        AOEATTACKL4.SetActive(false);
        AOEATTACKL5.SetActive(false);
        AOEATTACKR1.SetActive(false);
        AOEATTACKR2.SetActive(false);
        AOEATTACKR3.SetActive(false);
        AOEATTACKR4.SetActive(false);
        AOEATTACKR5.SetActive(false);
        
    }
    Coroutine start;
    private bool started = false;
    private void OnEnable()
    {
        if (!started)
        {
            started = true;
            Debug.Log("STARTED");
            StartCoroutine(AOEWAVE());
        }
    }
    private void OnDisable()
    {
        started = false;
    }

    IEnumerator AOEWAVE()
    {
        Debug.Log("WAVE STARTED");
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL1.SetActive (true);
        AOEATTACKR1.SetActive (true);   
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL1.SetActive(false);
        AOEATTACKR1.SetActive(false);
        AOEATTACKL2.SetActive(true);
        AOEATTACKR2.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL2.SetActive(false);
        AOEATTACKR2.SetActive(false);
        AOEATTACKL3.SetActive(true);
        AOEATTACKR3.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL3.SetActive(false);
        AOEATTACKR3.SetActive(false);
        AOEATTACKL4.SetActive(true);
        AOEATTACKR4.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL4.SetActive(false);
        AOEATTACKR4.SetActive(false);
        AOEATTACKL5.SetActive(true);
        AOEATTACKR5.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        CameraShake.Instance.ShakeCamera();
        AOEATTACKL5.SetActive(false);
        AOEATTACKR5.SetActive(false);
        gameObject.SetActive(false);
    }
}
