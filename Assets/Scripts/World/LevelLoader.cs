using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoSingleton<LevelLoader>
{
    [SerializeField] string _firstLevelName = "";
    int _currentSceneIndex = 0;
    Scene _currentScene;

    public delegate void SceneIsLoading(bool cond);
    public event SceneIsLoading OnSceneIsLoading;



    public void LoadFirstLevel()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        if (_firstLevelName.Equals(""))
        {
             _currentScene = SceneManager.LoadScene(1, parameters);
        }
        else
        {
            _currentScene = SceneManager.LoadScene(_firstLevelName, parameters);
        }
        _currentSceneIndex = _currentScene.buildIndex;
    }
    public void LoadNextLevel()
    {
        OnSceneIsLoading?.Invoke(true);
        /// Mandatory the victory scene ends the sequence
        SceneManager.LoadScene(++_currentSceneIndex);
        OnSceneIsLoading?.Invoke(false);
    }
}
