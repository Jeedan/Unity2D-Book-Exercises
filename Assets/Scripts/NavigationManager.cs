using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NavigationManager
{
    public static Dictionary<string, string> RouteInformation = new Dictionary<string, string>()
        {
            {"World", "The big bad world"},
            {"Cave", "The deep dark cave"},
        };

    public static string GetRouteInfo(string destination)
    {
        return RouteInformation.ContainsKey(destination) ? RouteInformation[destination] : null;
    }

    public static bool CanNavigate(string destination)
    {
        return true;
    }

    public static void NavigateTo(string destination)
    {
        //Application.LoadLevel(destination);
        Debug.Log("Navigate to " + destination);
    }
}
