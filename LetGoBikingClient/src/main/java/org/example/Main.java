package org.example;

import com.soap.ws.client.generated.*;

public class Main {
    public static IRouteService routeServiceClient;
    public static ClientInputManager clientInputManager;

    public static void main(String[] args) {
        clientInputManager = new ClientInputManager();
        RouteService service = new RouteService();
        routeServiceClient = service.getBasicHttpBindingIRouteService();
        MapManager mapManager = new MapManager();

        clientInputManager.askOriginAndDestination(routeServiceClient, mapManager);
    }
}

