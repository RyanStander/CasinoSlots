using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private string levelName;

    public void Load()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
