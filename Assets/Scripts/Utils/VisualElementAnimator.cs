using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
public class VisualElementAnimator : MonoBehaviour
{
    public bool isActive;
    [SerializeField] protected bool hideOnStart = true;
    [System.Serializable]
    public class AnimationParameters
    {
        public bool useFade = false;
        public float fadeTarget = 1f;
        public bool useMove = false;
        public Vector2 moveTarget = Vector2.zero;

        public bool useScale = false;
        public Vector2 scaleTarget = Vector2.one;

        public float duration = 1f;
        public Ease easeType = Ease.InOutQuad;
    }

    public AnimationParameters appearParams = new AnimationParameters();
    public AnimationParameters disappearParams = new AnimationParameters();

    protected Transform spriteTransform;
    [SerializeField] protected Transform contentsRoot;
    private List<PropertyFilter> propertyFilters = new List<PropertyFilter>();
    public bool useLocalPosition = true;
    Sequence sequence;
    protected virtual void Awake()
    {
        spriteTransform = transform;

        if (contentsRoot == null)
        {
            contentsRoot = transform.Find("Contents");
        }
        InitializePropertyFilters();

        if (hideOnStart)
        {
            isActive = false;
            if (contentsRoot != null) contentsRoot.gameObject.SetActive(false);
            InitFilterAlpha(true);
            if (disappearParams.useMove)
            {
                if (useLocalPosition)
                {
                    Vector3 newPos = spriteTransform.localPosition;
                    newPos.x = disappearParams.moveTarget.x;
                    newPos.y = disappearParams.moveTarget.y;
                    spriteTransform.localPosition = newPos;
                }
                else
                {
                    Vector3 newPos = spriteTransform.position;
                    newPos.x = disappearParams.moveTarget.x;
                    newPos.y = disappearParams.moveTarget.y;
                    spriteTransform.position = newPos;
                }
            }
            if (disappearParams.useScale) spriteTransform.localScale = disappearParams.scaleTarget;
        }
        else {
            isActive = true;
            if (contentsRoot != null) {
                contentsRoot.gameObject.SetActive(true);
                //Debug.Log(gameObject.name + " activate contentsRoot");
            }
            InitFilterAlpha(false);
            if (appearParams.useMove)
            {
                if (useLocalPosition)
                {
                    Vector3 newPos = spriteTransform.localPosition;
                    newPos.x = appearParams.moveTarget.x;
                    newPos.y = appearParams.moveTarget.y;
                    spriteTransform.localPosition = newPos;
                }
                else
                {
                    Vector3 newPos = spriteTransform.position;
                    newPos.x = appearParams.moveTarget.x;
                    newPos.y = appearParams.moveTarget.y;
                    spriteTransform.position = newPos;
                }
            }
            if (appearParams.useScale) spriteTransform.localScale = appearParams.scaleTarget;
        }
    }

    void InitializePropertyFilters()
    {
        AddPropertyFiltersRecursively(transform);
    }

    void AddPropertyFiltersRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            PropertyFilterAggregator aggregator = child.gameObject.GetComponent<PropertyFilterAggregator>();
            if (aggregator == null) aggregator = child.gameObject.AddComponent<PropertyFilterAggregator>();
            PropertyFilter filter = child.gameObject.AddComponent<PropertyFilter>();
            aggregator.Save();
            aggregator.AddFilter(filter);
            propertyFilters.Add(filter);

            AddPropertyFiltersRecursively(child);
        }
    }

    public virtual void Appear()
    {
        
        if (isActive) return;
        //Debug.Log(gameObject.name + " Appear");
        isActive = true;
        //Debug.Log(gameObject.name + " before kill " + sequence.IsActive());
        DOTween.Kill(this);
        sequence.Kill();
        //Debug.Log(gameObject.name + " after kill " + sequence.IsActive());
        InitFilterAlpha(true);
        Animate(appearParams, true);
    }

    public virtual void Disappear()
    {
        if (!isActive) return;
        //Debug.Log(gameObject.name + " Disappear");
        DOTween.Kill(this);
        sequence.Kill();
        InitFilterAlpha(false);
        Animate(disappearParams, false);
    }

    private void InitFilterAlpha(bool setToHiddenState)
    {
        foreach (PropertyFilter filter in propertyFilters)
        {
            if (setToHiddenState)
            {
                if (appearParams.useFade)
                {
                    //Debug.Log(filter.gameObject.name + " set to " + disappearParams.fadeTarget);
                    filter.SetAlphaFilter(disappearParams.fadeTarget);
                }
                else
                {
                    filter.SetAlphaFilter(1);
                }
            }
            else
            {
                if (disappearParams.useFade)
                {
                    filter.SetAlphaFilter(appearParams.fadeTarget);
                }
                else
                {
                    filter.SetAlphaFilter(1);
                }
            }
            filter.UpdateAlpha();
        }
    }

    protected virtual void Animate(AnimationParameters parameters, bool isAppearing)
    {
        //yield return null; // Wait for the next frame

        if (isAppearing) {
            if (contentsRoot != null) contentsRoot.gameObject.SetActive(true);
        }
        if (!isAppearing) {
            foreach(PropertyFilter filter in propertyFilters) {
                filter.Save();
            }
        }
        sequence = DOTween.Sequence();

        if (parameters.useFade)
        {
            foreach (var filter in propertyFilters)
            {
                float endAlpha = parameters.fadeTarget;
                sequence.Join(DOTween.To(() => filter.alphaFilter, x => filter.SetAlphaFilter(x), endAlpha, parameters.duration).SetEase(parameters.easeType));
            }
        }

        if (parameters.useMove)
        {
            if (useLocalPosition)
            {
                Vector3 currentPos = spriteTransform.localPosition;
                Vector3 targetPos = new Vector3(parameters.moveTarget.x, parameters.moveTarget.y, currentPos.z);
                sequence.Join(spriteTransform.DOLocalMove(targetPos, parameters.duration).SetEase(parameters.easeType));
            }
            else
            {
                Vector3 currentPos = spriteTransform.position;
                Vector3 targetPos = new Vector3(parameters.moveTarget.x, parameters.moveTarget.y, currentPos.z);
                sequence.Join(spriteTransform.DOMove(targetPos, parameters.duration).SetEase(parameters.easeType));
            }
        }

        if (parameters.useScale)
        {
            Vector3 endScale = parameters.scaleTarget;
            sequence.Join(spriteTransform.DOScale(endScale, parameters.duration).SetEase(parameters.easeType));
        }

        sequence.SetTarget(this);
        //Debug.Log(gameObject.name + " sequence.status:" + sequence.IsActive());
        if (!isAppearing)
        {
            sequence.OnComplete(() => {
                if (contentsRoot != null) contentsRoot.gameObject.SetActive(false);
                isActive = false;
                //Debug.Log(gameObject.name + " sequence.oncomplete");
            });
        }
    }
    public void Appear(float duration)
    {
        appearParams.duration = duration;
        Appear();
    }
    public void Disappear(float duration)
    {
        disappearParams.duration = duration;
        Disappear();
    }
}