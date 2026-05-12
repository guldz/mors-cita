using UnityEngine;

public class StartCutscene : MonoBehaviour
{

    public Animator elevatordoorAnimator;
    public Collider2D doorCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //when triggered play "Start" animation freeze player and wait untill animation is finisheda and after its finished make a fade out in and screen
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
