package org.example;

import com.soap.ws.client.generated.*; // Remplacez ceci par votre package généré
import org.apache.activemq.ActiveMQConnectionFactory;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.input.CenterMapListener;
import org.jxmapviewer.input.PanKeyListener;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCursor;
import org.jxmapviewer.painter.CompoundPainter;
import org.jxmapviewer.viewer.DefaultTileFactory;
import org.jxmapviewer.viewer.GeoPosition;

import javax.jms.*;
import javax.swing.*;
import java.util.ArrayList;
import java.util.List;

public class Main {
    public static void main(String[] args) {
        try {

            // Création du client SOAP
            RouteService service = new RouteService(); // Remplacez par votre classe de service générée
            IRouteService routeServiceClient = service.getBasicHttpBindingIRouteService(); // Remplacez par le nom correct de la méthode

            // Appel du service SOAP pour obtenir l'itinéraire
            String origin = "lyon, place bellecour";
            String destination = "lyon, 5 rue Félix Rollet";
            Itinary response = routeServiceClient.getItinerary(origin, destination); // Ajustez selon votre méthode générée
            System.out.println("Itinéraire reçu: " + response); // Affichage basique, à personnaliser

            // Configuration de la connexion ActiveMQ
            ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
            Connection connection = factory.createConnection();
            connection.start();
            Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Queue queue = session.createQueue("ItineraryQueue");

            MessageConsumer consumer = session.createConsumer(queue);
            connection.start();

            System.out.println("En attente de messages...");
            // Boucle de réception des messages
            while (true) {
                Message message = consumer.receive(1000); // Timeout de 1 seconde
                if (message instanceof TextMessage) {
                    TextMessage textMessage = (TextMessage) message;
                    System.out.println("Message reçu: " + textMessage.getText());
                    // Ajoutez ici la logique pour traiter le message
                } else if (message == null) {
                    break; // Sortie de la boucle si aucun message n'est reçu
                }
            }

            SwingUtilities.invokeLater(() -> createAndShowMap(response));

            consumer.close();
            session.close();
            connection.close();
            } catch (Exception e) {
            e.printStackTrace();
        }
    }
    private static void createAndShowMap(Itinary itinary) {
        // Création de la carte
        JXMapViewer mapViewer = new JXMapViewer();

        // Configuration de la tuile et de la position initiale
        OSMTileFactoryInfo info = new OSMTileFactoryInfo();
        DefaultTileFactory tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);
        GeoPosition lyon = new GeoPosition(45.764043, 4.835659); // Lyon, France
        mapViewer.setZoom(7);
        mapViewer.setAddressLocation(lyon);

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
    private static void addRouteToMap(JXMapViewer mapViewer, Itinary itinary) {
        // Liste pour stocker les positions Geo
        List<GeoPosition> track = new ArrayList<>();

        ArrayOfFeature arrayOfFeature = itinary.getFeatures().getValue();

        // Convertissez les coordonnées de l'itinéraire en GeoPositions
        for (Feature feature : arrayOfFeature.getFeature()) {
            ArrayOfArrayOfdouble coordinates = feature.getGeometry().getValue().getCoordinates().getValue();
            for (ArrayOfdouble coord : coordinates.getArrayOfdouble()) {
                track.add(new GeoPosition(coord.getDouble().get(1), coord.getDouble().get(0))); // Latitude, Longitude
            }
        }

        // Créer un RoutePainter pour dessiner l'itinéraire
        RoutePainter routePainter = new RoutePainter(track);

        // Ajouter le RoutePainter à la carte
        List<CompoundPainter<?>> painters = new ArrayList<>();
/*        painters.add(routePainter);
        CompoundPainter<?> painter = new CompoundPainter<>(painters);
        mapViewer.setOverlayPainter(painter);

        // Ajouter les marqueurs de début et de fin
        routePainter.drawStartAndEndMarkers(mapViewer);*/

    }

}

