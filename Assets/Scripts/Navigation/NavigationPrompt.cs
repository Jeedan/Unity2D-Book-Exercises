using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavigationPrompt : MonoBehaviour
{
    public GameObject navigationContainer; // container which has the GUI elements
    public bool showDialog = false;

    public Text travelText;
    public Button travelButton;
    public Button stayButton;
    public string travelString = "Do you want to travel to ";

    void Awake()
    {
        if (!travelText)
            Debug.LogError("no travel text found or is unassigned!");

        if (!travelButton)
            Debug.LogError("no travel Button found or is unassigned!");

        if (!stayButton)
            Debug.LogError("no stay Button found or is unassigned!");
    }

    // Update is called once per frame
    void Update()
    {
        if (showDialog)
        {
            //travelText.text = travelString + this.tag + "?";
            travelText.text = travelString + NavigationManager.GetRouteInfo(this.tag) + "?";
            navigationContainer.SetActive(true);
        }
        else
        {
            navigationContainer.SetActive(false);
        }
    }

    public void OnTravelClicked()
    {
        NavigationManager.NavigateTo(this.tag);
        showDialog = false;
        // Application.LoadLevel(1);
    }

    public void OnStayClicked()
    {
        Debug.Log("Stay in " + this.tag);
        showDialog = false;
        // navigationContainer.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // only allow the player to travel if allowed
        if (NavigationManager.CanNavigate(this.tag))
        {

            showDialog = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("exit");
        showDialog = false;
    }
}
