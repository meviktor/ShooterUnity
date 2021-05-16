using UnityEngine;

public class CrazyHorseController : MonoBehaviour
{
    public float interval = 2.0f;

    private bool headingToTransparent = false;
    private Material _horseBodyMaterial;

    void Start()
    {
        _horseBodyMaterial = GameObject.Find("HorseBody").GetComponent<MeshRenderer>().material;
        _horseBodyMaterial.SetFloat("_Alpha", 0f);
    }

    void Update()
    {
        float currentAlpha = _horseBodyMaterial.GetFloat("_Alpha");
        float alphaChange = (1 / interval) * Time.deltaTime;

        currentAlpha = (headingToTransparent ? currentAlpha - alphaChange : currentAlpha + alphaChange);
        
        if(currentAlpha < 0)
        {
            currentAlpha = 0;
            headingToTransparent = false;
        }

        if(currentAlpha > 1)
        {
            currentAlpha = 1;
            headingToTransparent = true;
        }

        _horseBodyMaterial.SetFloat("_Alpha", currentAlpha);
    }
}
