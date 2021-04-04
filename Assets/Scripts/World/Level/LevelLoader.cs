using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoSingleton<LevelLoader>
{
    string _firstLevelName = "SteveScene2";
    int _scoringSceneIndex = 1;
    int _currentSceneIndex = 0;
    Scene _currentScene;

    public delegate void SceneIsLoading(bool cond);
    public event SceneIsLoading OnSceneIsLoading;

    private bool _isScoringScene = false;

    private void Start()
    {
       
    }

    public void LoadFirstLevel()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        if (_firstLevelName.Equals(""))
        {
             _currentScene = SceneManager.LoadScene(_scoringSceneIndex+1, parameters);
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
        if (!_isScoringScene)
        {
            _isScoringScene = true;
            SceneManager.LoadScene(_scoringSceneIndex);
        }
        else
        {
            _isScoringScene = false;
            /// Mandatory the victory scene ends the sequence
            SceneManager.LoadScene(++_currentSceneIndex);
        }
        OnSceneIsLoading?.Invoke(false);
    }
}
