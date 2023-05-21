using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class AnimationState : MonoBehaviour
{
    public enum AnimationStates
    {
        Idling,
        Blocking,
        Attacking,
        Damaging,
        Impact,
    }

    public AnimationStates Current = AnimationStates.Idling;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    [SerializeField]
    private CharacterController _controller;

    private void Start()
    {
        TryGetComponent(out _controller);
    }

    public void OnAnimationEvent(AnimationStates state)
    {
        Current = state;
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (!(animationEvent.animatorClipInfo.weight > 0.5f)) return;
        if (FootstepAudioClips.Length <= 0) return;
        var index = Random.Range(0, FootstepAudioClips.Length);
        AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller?.center ?? transform.position),
            FootstepAudioVolume);
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (!(animationEvent.animatorClipInfo.weight > 0.5f)) return;
        AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller?.center ?? transform.position),
            FootstepAudioVolume);
    }
}