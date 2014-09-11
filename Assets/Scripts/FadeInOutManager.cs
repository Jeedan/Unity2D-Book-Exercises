using UnityEngine;
using System.Collections;

public class FadeInOutManager : Singleton<FadeInOutManager>
{
    // singleton
    protected FadeInOutManager(){}

    // texture to display
    private Material fadeMaterial;
    //fading parameters
    private float fadeOutTime, fadeInTime;
    private Color fadeColor;

    // place holder for the level you will be navigating to 
    // (by name or index)
    private string navigateToLevelName = "";
    private int navigateToLevelIndex = 0;

    // state to control if a level is fading or not,
    // including public property if access through code 
    private bool fading = false;

    public static bool Fading { get { return Instance.fading; } }

    void Awake()
    {
        // setup a default blank texture for fading if none is supplied
        fadeMaterial = new Material ("Shader\"Plane/No zTest\" {" +
            "SubShader { Pass { " +
            " Blend SrcAlpha OneMinusSrcAlpha " +
            " Zwrite Off Cull Off Fog { Mode Off } " +
            " BindChannels { " + 
            "   Bind \"color\", color } " +
            "} } }");
 
    }

    public static void FadeToLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance.navigateToLevelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }

    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        fading= true;
        Instance.fadeOutTime = aFadeOutTime;
        Instance.fadeInTime = aFadeInTime;
        Instance.fadeColor = aColor;
        StopAllCoroutines();
        StartCoroutine("Fade");
    }

    private IEnumerator Fade()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
        if (navigateToLevelName != "")
            Application.LoadLevel(navigateToLevelName);
        else
            Application.LoadLevel(navigateToLevelIndex);
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
            DrawingUtilities.DrawQuad(fadeMaterial, fadeColor, t);
        }
        fading = false;
    }

}
