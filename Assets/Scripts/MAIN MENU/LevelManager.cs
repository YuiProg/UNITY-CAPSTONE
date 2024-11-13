using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider progressBar;

    private float _target;
    
    public void Awake()
    {
        _loaderCanvas.SetActive(false); 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async void loadscene(string scenename)
    {
        _target = 0;
        progressBar.value = 0;
        var scene = SceneManager.LoadSceneAsync(scenename);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);
        while (scene.progress < 0.9f)
        {
            _target = scene.progress; 
            await Task.Delay(100);   
        }
        _target = 1.0f;
        await Task.Delay(500);
        scene.allowSceneActivation = true;
        while (!scene.isDone)
        {
            await Task.Delay(100); 
        }

        _loaderCanvas.SetActive(false);
    }

    private void Update()
    {
       
        progressBar.value = Mathf.MoveTowards(progressBar.value, _target, 3 * Time.deltaTime);
    }

}
