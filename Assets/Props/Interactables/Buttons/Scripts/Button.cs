using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    private readonly float pushOffset = 0.2f;
    private readonly float movementSpeed = 1.5f;

    private bool isPressed;

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    [SerializeField]
    private string[] pushableOnlyByTags;
    private HashSet<string> pushableOnlyByTagsHashSet = new();

    [SerializeField]
    private Transform buttonTop;

    private void Awake()
    {
        foreach (string tag in pushableOnlyByTags)
        {
            pushableOnlyByTagsHashSet.Add(tag);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (pushableOnlyByTagsHashSet.Count > 0 && !pushableOnlyByTagsHashSet.Contains(col.gameObject.tag))
            return;

        Press();
    }

    private void OnCollisionExit(Collision col)
    {
        if (pushableOnlyByTagsHashSet.Count > 0 && !pushableOnlyByTagsHashSet.Contains(col.gameObject.tag))
            return;

        Release();
    }

    public void Press()
    {
        if (isPressed)
            return;

        isPressed = true;

        Vector3 targetPosition = new(buttonTop.transform.position.x, buttonTop.transform.position.y - pushOffset, buttonTop.transform.position.z);
        StartCoroutine(MoveTo(targetPosition, OnPressed));
    }

    public void Release()
    {
        if (!isPressed)
            return;

        isPressed = false;

        Vector3 targetPosition = new(buttonTop.transform.position.x, buttonTop.transform.position.y + pushOffset, buttonTop.transform.position.z);
        StartCoroutine(MoveTo(targetPosition, OnReleased));
    }

    private IEnumerator MoveTo(Vector3 position, UnityEvent onCompleted)
    {
        while (Vector3.Distance(buttonTop.transform.position, position) > 0.001f)
        {
            buttonTop.transform.position = Vector3.MoveTowards(buttonTop.transform.position, position, movementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        onCompleted?.Invoke();
    }
}