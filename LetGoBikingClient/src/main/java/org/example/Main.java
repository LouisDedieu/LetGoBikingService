package org.example;

import com.soap.ws.client.generated.*;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.input.CenterMapListener;
import org.jxmapviewer.input.PanKeyListener;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCursor;
import org.jxmapviewer.painter.CompoundPainter;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.*;

import javax.jms.*;
import javax.jms.Queue;
import javax.swing.*;
import java.awt.*;
import java.util.*;
import java.util.List;

public class Main {
    private static final String WALKING = "foot-walking" ;
    private static final String CYCLING = "cycling-regular" ;

    public static void main(String[] args) {
        try {

            // Création du client SOAP
            RouteService service = new RouteService();
            IRouteService routeServiceClient = service.getBasicHttpBindingIRouteService();

            // Appel du service SOAP pour obtenir l'itinéraire
            String origin = "Cublize, 48 Rue de L Hôtel de ville";
            String destination = "Beauvoir-de-Marc, 1644 Rte de Lyon";
            List<Itinary> response = routeServiceClient.getItinerary(origin, destination).getItinary();

            SwingUtilities.invokeLater(() -> createAndShowMap(response));

            // Configuration de la connexion ActiveMQ
            ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
            Connection connection = factory.createConnection();
            connection.start();
            Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Queue queue = session.createQueue("ItineraryQueue");

            MessageConsumer consumer = session.createConsumer(queue);
            connection.start();

            Scanner scanner = new Scanner(System.in);

            System.out.println("Appuyez sur Entrée pour recevoir le prochain message...");

            // Boucle de réception des messages
            while (true) {
                System.out.println("En attente du prochain message (appuyez sur Entrée)...");
                scanner.nextLine(); // Attente de la pression sur la touche Entrée

                Message message = consumer.receive(1000); // Attendre le message pendant 1 seconde
                if (message instanceof TextMessage) {
                    TextMessage textMessage = (TextMessage) message;
                    System.out.println("Message reçu: " + textMessage.getText());
                } else if (message == null) {
                    break;
                }
            }
            consumer.close();
            session.close();
            connection.close();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }
    private static void createAndShowMap(List<Itinary> itinary) {
        // Création de la carte
        JXMapViewer mapViewer = new JXMapViewer();

        // Configuration de la tuile et de la position initiale
        OSMTileFactoryInfo info = new OSMTileFactoryInfo();
        DefaultTileFactory tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);

        // Activer le déplacement de la carte avec la souris
        PanMouseInputListener pannerPlugin = new PanMouseInputListener(mapViewer);
        mapViewer.addMouseListener(pannerPlugin);
        mapViewer.addMouseMotionListener(pannerPlugin);
        mapViewer.addMouseListener(new CenterMapListener(mapViewer));

        // Activer le zoom avec la molette de la souris
        mapViewer.addMouseWheelListener(new ZoomMouseWheelListenerCursor(mapViewer));

        // Activer le déplacement avec les touches du clavier
        mapViewer.addKeyListener(new PanKeyListener(mapViewer));

        // Assurez-vous que la carte est focusable pour recevoir les entrées du clavier
        mapViewer.setFocusable(true);
        mapViewer.requestFocusInWindow();

        // Ajouter l'itinéraire à la carte
        addRouteToMap(mapViewer, itinary);

        // Création et affichage du JFrame
        JFrame frame = new JFrame("JXMapViewer2 - Interactive Map Viewer");
        frame.getContentPane().add(mapViewer);
        frame.setSize(800, 600);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
    }
    private static void addRouteToMap(JXMapViewer mapViewer, List<Itinary> itinaries) {
        // Liste pour stocker les positions Geo
        List<GeoPosition> track = null;
        List<RoutePainter> routePainters = new ArrayList<RoutePainter>();
        Color c = Color.RED;

        for (Itinary itinary : itinaries) {
            track = new ArrayList<GeoPosition>();
            ArrayOfFeature arrayOfFeature = itinary.getFeatures().getValue();

            // Convertissez les coordonnées de l'itinéraire en GeoPositions
            for (Feature feature : arrayOfFeature.getFeature()) {
                ArrayOfArrayOfdouble coordinates = feature.getGeometry().getValue().getCoordinates().getValue();
                for (ArrayOfdouble coord : coordinates.getArrayOfdouble()) {
                    track.add(new GeoPosition(coord.getDouble().get(1), coord.getDouble().get(0))); // Latitude, Longitude
                }
            }
            if (itinary.getMetadata().getValue().getQuery().getValue().getProfile().getValue().equals(WALKING))
                c = Color.BLUE;
            else if (itinary.getMetadata().getValue().getQuery().getValue().getProfile().getValue().equals(CYCLING))
                c = Color.RED;



            RoutePainter routePainter = new RoutePainter(track, c);
            routePainters.add(routePainter);
        }

        // Set the focus
        mapViewer.zoomToBestFit(new HashSet<GeoPosition>(track), 0.7);

        Set<Waypoint> waypoints = new HashSet<Waypoint>(2);
        // Ajoutez les points de départ et d'arrivée
        waypoints.add(new DefaultWaypoint(itinaries.get(0).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(1), itinaries.get(0).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(0)));
        waypoints.add(new DefaultWaypoint(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().size()-1).getDouble().get(1), itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().size()-1).getDouble().get(0)));

        // Create a waypoint painter that takes all the waypoints
        WaypointPainter<Waypoint> waypointPainter = new WaypointPainter<Waypoint>();
        waypointPainter.setWaypoints(waypoints);

        // Create a compound painter that uses both the route-painter and the waypoint-painter
        List<Painter<JXMapViewer>> painters = new ArrayList<>(routePainters);
        painters.add(waypointPainter);

        CompoundPainter<JXMapViewer> painter = new CompoundPainter<JXMapViewer>(painters);
        mapViewer.setOverlayPainter(painter);
    }

}

