using System.Collections;
using System.Linq;
using Meta.Net.NativeWebSocket;
using UnityEngine;

public class ElementReactionAnimator : MonoBehaviour
{
    public bool IsPaused => isPaused;

    private Animator animator;

    private string animationClip;
    private AnimationClip clip;

    private bool isPaused = true;
    private float _animationProgress = 0f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FindClip();
    }

    private void Update()
    {
        if (!IsPaused)
        {
            _animationProgress += Time.deltaTime;
        }
    }

    public void PlayAnimation()
    {
        if (isPaused)
        {
            float normalizedTime = 0f;

            if (_animationProgress > 0f)
                normalizedTime = _animationProgress / clip.length;

            animator.speed = 1f;
            animator.Play(animationClip, 0, normalizedTime);

            isPaused = false;
        }
    }

    public void StopAnimation()
    {
        if (!isPaused)
        {
            animator.speed = 0f;

            isPaused = true;
        }
    }

    public bool GetClipIsFinishedPlaying() => clip.length <= _animationProgress;

    public void SetAnimationClip(string clip) => animationClip = clip;

    private void FindClip()
    {
        var clips = animator.runtimeAnimatorController.animationClips;

        foreach (var c in clips)
        {
            if (c.name == animationClip)
            {
                Debug.Log(c.name);
                clip = c;
            }
        }
    }

}
