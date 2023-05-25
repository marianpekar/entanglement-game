using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSwapper : MonoBehaviour
{
    [SerializeField]
    GameObject cube;

    [SerializeField]
    private Material materialToSwap;

    [SerializeField]
    private string tagToSwap;

    [SerializeField]
    private string nameToSwap;

    [SerializeField]
    private Color lightColorToSwap;

    [SerializeField]
    private string acceptTag;

    private float initialScale;
    private readonly float scaleStep = 100f;

    private bool isLocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(acceptTag))
            return;

        cube = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Contains("Cube"))
            return;

        cube = null;
    }

    public void Swap()
    {
        if (cube == null || isLocked)
            return;

        isLocked = true;
        initialScale = cube.transform.localScale.x;

        StartCoroutine(ShrinkAndSwap());
    }

    private IEnumerator ShrinkAndSwap()
    {
        while (cube.transform.localScale.x > 0.001f)
        {
            float scale = cube.transform.localScale.x - scaleStep * Time.deltaTime;
            cube.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForEndOfFrame();
        }

        SwapProperties();
        StartCoroutine(Enlarge());
    }

    private void SwapProperties()
    {
        Material tempMaterial = cube.GetComponent<MeshRenderer>().material;
        Color tempLightColor = cube.GetComponent<Light>().color;
        string tempTag = cube.tag;
        string tempName = cube.name;

        cube.GetComponent<MeshRenderer>().material = materialToSwap;
        cube.GetComponent<Light>().color = lightColorToSwap;
        cube.tag = tagToSwap;
        cube.name = nameToSwap;
        acceptTag = tagToSwap;

        materialToSwap = tempMaterial;
        lightColorToSwap = tempLightColor;
        tagToSwap = tempTag;
        nameToSwap = tempName;
    }

    private IEnumerator Enlarge()
    {
        while (cube.transform.localScale.x < initialScale)
        {
            float scale = cube.transform.localScale.x + scaleStep * Time.deltaTime;
            cube.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForEndOfFrame();
        }

        isLocked = false;
    }

}
