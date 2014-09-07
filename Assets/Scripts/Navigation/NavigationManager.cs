using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NavigationManager
{
    public struct Route
    {
        public string RouteDescription;
        public bool CanTravel;
    }

    public static Dictionary<string, Route> RouteInformation = new Dictionary<string, Route>()
        {
            {"World", new Route{RouteDescription = "The big bad world", CanTravel = true}},
            {"Cave", new Route{RouteDescription = "The deep dark cave", CanTravel = false}},
        };

    public static string GetRouteInfo(string destination)
    {
        return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].RouteDescription : null;
    }

    public static bool CanNavigate(string destination)
    {
        return RouteInformation.ContainsKey(destination) ? RouteInformation[destination].CanTravel : false;
    }

    public static void NavigateTo(string destination)
    {
        //Application.LoadLevel(destination);
        Debug.Log("Navigate to " + destination);
    }
}
