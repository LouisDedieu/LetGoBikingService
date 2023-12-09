package org.example;

import com.soap.ws.client.generated.*;

public class Main {
    private static IRouteService routeServiceClient;

    public static void main(String[] args) {
        ClientInputManager clientInputManager = new ClientInputManager();
        RouteService service = new RouteService();
        routeServiceClient = service.getBasicHttpBindingIRouteService();
        MapManager mapManager = new MapManager();

        clientInputManager.askOriginAndDestination(routeServiceClient, mapManager);
    }
}

