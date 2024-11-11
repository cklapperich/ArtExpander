using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HarmonyLib;
using ArtExpander.Patches;
namespace ArtExpander.Core
{
    public class GhostCardAnimatedRenderer : MonoBehaviour
    {
        private Image mainImage;
        private Image maskImage;
        private Image glowImage;
        private Sprite[] frames;
        private Coroutine animationCoroutine;
        private float frameDelay = 0.1f;
        private CardUI parentCardUI;
        private bool wasAnimating = false;
        private string componentId;
        private bool pendingStart = false;

        private void Awake()
        {
            componentId = System.Guid.NewGuid().ToString().Substring(0, 8);
            // Plugin.Logger.LogInfo($"[Animation {componentId}] Awake Called\n" +
            //     $"  GameObject Active: {gameObject.activeInHierarchy}\n" +
            //     $"  Has Parent CardUI: {GetComponentInParent<CardUI>() != null}\n" + 
            //     $"  Component Count: {gameObject.GetComponents<GhostCardAnimatedRenderer>().Length}");
        }

        private void LogAnimationState(string eventName)
        {
            // Plugin.Logger.LogInfo($"[Animation {componentId}] {eventName}\n" +
            //     $"  HasFrames: {(frames != null ? frames.Length : 0)} frames\n" +
            //     $"  HasCoroutine: {animationCoroutine != null}\n" +
            //     $"  WasAnimating: {wasAnimating}\n" +
            //     $"  PendingStart: {pendingStart}\n" +
            //     $"  GameObject Active: {gameObject.activeInHierarchy}\n" +
            //     $"  MainImage: {(mainImage?.sprite != null ? "Has Sprite" : "No Sprite")}");
        }

        public void Initialize(Image mainImage, Image maskImage, Image glowImage, Sprite[] frames)
        {
            // Clean up other animators first
            var others = GetComponents<GhostCardAnimatedRenderer>();
            if (others.Length > 1)
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] Found {others.Length} animators on object, cleaning up extras");
                foreach(var other in others)
                {
                    if (other != this)
                    {
                        other.StopAnimation();
                        Destroy(other);
                    }
                }
            }

            LogAnimationState("Initialize");
            
            this.mainImage = mainImage;
            this.maskImage = maskImage;
            this.glowImage = glowImage;
            this.frames = frames;
            this.pendingStart = false;
                    
            parentCardUI = GetComponentInParent<CardUI>();
            
            StopAnimation();
            
            // Don't try to start if inactive
            if (gameObject.activeInHierarchy)
            {
                StartAnimation();
            }
            else
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] GameObject inactive during Initialize, marking for pending start");
                pendingStart = true;
            }
        }

        public void StartAnimation()
        {
            LogAnimationState("StartAnimation Called");
            
            if (!gameObject.activeInHierarchy)
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] Cannot start - GameObject inactive");
                pendingStart = true;
                return;
            }
            
            if (frames == null || frames.Length == 0)
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] Cannot start - no frames");
                return;
            }
            if (animationCoroutine != null)
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] Cannot start - already running");
                return;
            }

            pendingStart = false;
            animationCoroutine = StartCoroutine(AnimateSprites());
            LogAnimationState("Animation Started");
        }

        public void StopAnimation()
        {   
            LogAnimationState("StopAnimation Called");

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
                LogAnimationState("Animation Stopped");
            }
        }

        private IEnumerator AnimateSprites()
        {
            LogAnimationState("Animation Coroutine Started");
            int currentFrame = 0;
            
            while (true) {
                mainImage.sprite = frames[currentFrame];
                maskImage.sprite = frames[currentFrame];
                if (glowImage != null) {
                    glowImage.sprite = frames[currentFrame];
                }
                
                currentFrame = (currentFrame + 1) % frames.Length;
                yield return new WaitForSeconds(frameDelay);
            }
        }

        private void OnDisable()
        {
            LogAnimationState("OnDisable");
            wasAnimating = (animationCoroutine != null);
            StopAnimation();
        }

        private void OnEnable()
        {
            LogAnimationState("OnEnable");
            if (wasAnimating || pendingStart)
            {
                StartAnimation();
            }
        }

        private void OnDestroy()
        {
            LogAnimationState("OnDestroy");
            StopAnimation();
        }

        public void ResetAnimation()
        {
            LogAnimationState("ResetAnimation Called");
            StopAnimation();
            StartAnimation();
        }
    }
}