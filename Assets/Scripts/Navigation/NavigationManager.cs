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

    private static string PreviousLocation;

    public static Dictionary<string, Route> RouteInformation = new Dictionary<string, Route>()
        {
            {"World", new Route{RouteDescription = "The big bad world", CanTravel = true}},
            {"Cave", new Route{RouteDescription = "The deep dark cave", CanTravel = false}},
            {"Home", new Route{RouteDescription = "Home sweet home", CanTravel = true}},
            {"Kirkidw", new Route{RouteDescription = "The grand city of Kirkidw", CanTravel = true}},
            {"Shop", new Route{RouteDescription = "", CanTravel = true}},
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
        PreviousLocation = Application.loadedLevelName;
        if (destination == "Home")
            GameState.PlayerReturningHome = false;
        FadeInOutManager.FadeToLevel(destination, 2f, 2f, Color.black);
    }

    public static void GoBack()
    {
        var backLocation = PreviousLocation;
        PreviousLocation = Application.loadedLevelName;
        FadeInOutManager.FadeToLevel(backLocation, 2f, 2f, Color.black);
    }
}
