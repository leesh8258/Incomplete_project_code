using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("Dissovle Setting")]
    [SerializeField] private float dissolveTime = 0.75f;
    [SerializeField] private Transform coreBone;
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Material[] materials;
    
    private int dissovleAmount = Shader.PropertyToID("_DissolveThreshold");
    private int dissovleEdgeAmount = Shader.PropertyToID("_DissolveEdgeWidth");

    [Header("Sprite 2방위 / 4방위")]
    [SerializeField] private bool IsFourCardinalPoints;
    private int cardinalPoint;

    private GameObject spritePrefab;
    private Animator animator;
    private Coroutine FlashRedCoroutine;

    private Dictionary<ParticleProperty.ParticleState, ParticleSystem[]> particleDictionary = new Dictionary<ParticleProperty.ParticleState, ParticleSystem[]>();
    [SerializeField] private List<ParticleProperty> particleProperties = new List<ParticleProperty>();

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        materials = new Material[spriteRenderers.Length];
        spritePrefab = GetComponentInChildren<Animator>().gameObject;
        animator = spritePrefab.GetComponent<Animator>();

        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }

        foreach (var particle in particleProperties)
        {
            particleDictionary.Add(particle.stateType, particle.particleList);
        }
    }

    public void FlashDamageEffect()
    {
        if (FlashRedCoroutine != null) return;
        FlashRedCoroutine = StartCoroutine(FlashRed());
    }

    public IEnumerator Dissolve()
    {
        float elapsedTime = 0f;
        while(elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0f, 1f, (elapsedTime / dissolveTime));

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat(dissovleAmount, lerpedDissolve);
                materials[i].SetFloat(dissovleEdgeAmount, lerpedDissolve);
            }

            yield return null;
        }
    }

    public void Appear()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat(dissovleAmount, 0f);
            materials[i].SetFloat(dissovleEdgeAmount, 0f);
        }
    }

    private IEnumerator FlashRed()
    {
        // 원래 색상을 백업
        List<Color> originalColors = new List<Color>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors.Add(spriteRenderers[i].color);
        }

        float duration = 0.1f;
        float timer = 0f;

        // 서서히 붉은색으로 전환
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                // 원래 색에서 빨간색으로 보간
                spriteRenderers[i].color = Color.Lerp(originalColors[i], Color.red, t);
            }
            yield return null;
        }

        // 확실하게 붉은색으로 고정
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.red;
        }

        // 타이머 리셋 후 원래 색으로 복귀하는 보간 진행
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                // 붉은색에서 원래 색으로 보간
                spriteRenderers[i].color = Color.Lerp(Color.red, originalColors[i], t);
            }
            yield return null;
        }

        // 최종적으로 원래 색으로 설정
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }

        FlashRedCoroutine = null;
    }

    public int GetCardinal()
    {
        return cardinalPoint; 
    }

    public void GetCardinalPoints(Vector3 direction)
    {

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360f;

        if(IsFourCardinalPoints)
        {
            if (angle >= 315f || angle < 45f) cardinalPoint = 1; // 북 (315° ~ 45°)
            else if (angle >= 45f && angle < 135f) cardinalPoint = 2;  // 동 (45° ~ 135°)
            else if (angle >= 135f && angle < 225f) cardinalPoint = 3; // 남 (135° ~ 225°)
            else if (angle >= 225f && angle < 315f) cardinalPoint = 4;  // 서 (225° ~ 315°)
        }

        else
        {
            if (angle >= 0f && angle < 180f) cardinalPoint = 2; // 동 (0° ~ 180°)
            else if (angle >= 180f && angle < 360f) cardinalPoint = 4; // 서 (180° ~ 360°)
        }

        animator.SetInteger("Direction", cardinalPoint);
        SetSpriteDirection();
    }

    private void SetSpriteDirection()
    {
        if (coreBone == null) return;

        Vector3 currentEuler = coreBone.localEulerAngles;

        if (cardinalPoint == 2) currentEuler.y = 180f;
        else if (cardinalPoint == 4) currentEuler.y = 0f;

        coreBone.localEulerAngles = currentEuler;
    }

    public void PlayAnimationToTrigger(string animationTrigger)
    {
        if (animator != null)
        {
            ResetAllTrigger();
            animator.SetTrigger(animationTrigger);
        }
    }

    public void PlayAnimationToBoolean(string animationBoolean, bool boolean)
    {
        if(animator != null)
        {
            animator.SetBool(animationBoolean, boolean);
        }
    }
    public void ResetAllTrigger()
    {
        if (animator != null)
        {
            foreach (var param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(param.name);
                }
            }
        }
    }

    public void StartParticleSystems(string name)
    {
        if(Enum.TryParse<ParticleProperty.ParticleState>(name, out ParticleProperty.ParticleState state) && particleDictionary.ContainsKey(state) )
        {
            foreach(var param in particleDictionary[state])
            {
                param.Play();
            }
        }
    }

    public void StopParticleSystems(string name)
    {
        if (Enum.TryParse<ParticleProperty.ParticleState>(name, out ParticleProperty.ParticleState state) && particleDictionary.ContainsKey(state))
        {
            foreach (var param in particleDictionary[state])
            {
                param.Stop();
            }
        }
    }
}
