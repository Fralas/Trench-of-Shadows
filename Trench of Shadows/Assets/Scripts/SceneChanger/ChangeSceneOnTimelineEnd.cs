using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimelineEnd : MonoBehaviour
{
    public string sceneToLoad;  // Nome della scena da caricare
    private PlayableDirector timeline;

    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        if (timeline != null)
        {
            timeline.stopped += OnTimelineEnd;
        }
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        if (pd == timeline)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
