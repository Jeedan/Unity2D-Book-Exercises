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
            {"Home", new Route{RouteDescription = "Home sweet home", CanTravel = true}},
            {"Kirkidw", new Route{RouteDescription = "The grand city of Kirkidw", CanTravel = true}},
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
        FadeInOutManager.FadeToLevel(destination, 2f, 2f, Color.black);
        Debug.Log("Navigate to " + destination);
    }
}
