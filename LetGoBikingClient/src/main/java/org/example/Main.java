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
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.*;
import java.util.List;

public class Main {
    private static final String WALKING = "foot-walking" ;
    private static final String CYCLING = "cycling-regular" ;
    private static String origin;
    private static String destination;
    private static IRouteService routeServiceClient;

    public static void main(String[] args) {

        try {

            // Création du client SOAP
            RouteService service = new RouteService();
            routeServiceClient = service.getBasicHttpBindingIRouteService();
            askOriginAndDestination();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static void askOriginAndDestination() {
        // Créer la fenêtre
        JFrame frame = new JFrame("Travel Form");
        frame.setSize(400, 200);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setLayout(null);

        // Ajouter les composants
        addComponents(frame);

        // Afficher la fenêtre
        frame.setVisible(true);
    }

    private static void addComponents(JFrame frame) {
        // Créer les labels
        JLabel originLabel = new JLabel("Origin:");
        originLabel.setBounds(10, 20, 80, 25);
        frame.add(originLabel);

        JLabel destinationLabel = new JLabel("Destination:");
        destinationLabel.setBounds(10, 50, 80, 25);
        frame.add(destinationLabel);

        // Créer les champs de texte
        JTextField originField = new JTextField(20);
        originField.setBounds(100, 20, 165, 25);
        frame.add(originField);

        JTextField destinationField = new JTextField(20);
        destinationField.setBounds(100, 50, 165, 25);
        frame.add(destinationField);

        // Créer le bouton
        JButton submitButton = new JButton("Submit");
        submitButton.setBounds(100, 80, 80, 25);
        frame.add(submitButton);

        // Ajouter l'action du bouton
        submitButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                origin = originField.getText();
                destination = destinationField.getText();

                // Vous pouvez initialiser vos variables ici
                System.out.println("Origin: " + origin + ", Destination: " + destination);
                frame.setVisible(false);

                List<Itinary> response = routeServiceClient.getItinerary(origin, destination).getItinary();

                SwingUtilities.invokeLater(() -> {
                    try {
                        createAndShowMap(response);
                    } catch (JMSException ex) {
                        throw new RuntimeException(ex);
                    }
                });
            }
        });
    }

    // Fonction principale pour créer et montrer la carte
    private static void createAndShowMap(List<Itinary> itinary) throws JMSException {
        JXMapViewer mapViewer = initializeMapViewer(itinary);
        JTextArea textArea = new JTextArea();
        textArea.setEditable(false);
        setupUI(mapViewer, textArea);
    }

    // Initialisation et configuration de JXMapViewer
    private static JXMapViewer initializeMapViewer(List<Itinary> itinary) {
        JXMapViewer mapViewer = new JXMapViewer();
        OSMTileFactoryInfo info = new OSMTileFactoryInfo();
        DefaultTileFactory tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);

        PanMouseInputListener pannerPlugin = new PanMouseInputListener(mapViewer);
        mapViewer.addMouseListener(pannerPlugin);
        mapViewer.addMouseMotionListener(pannerPlugin);
        mapViewer.addMouseListener(new CenterMapListener(mapViewer));
        mapViewer.addMouseWheelListener(new ZoomMouseWheelListenerCursor(mapViewer));
        mapViewer.addKeyListener(new PanKeyListener(mapViewer));
        mapViewer.setFocusable(true);
        mapViewer.requestFocusInWindow();

        addRouteToMap(mapViewer, itinary);
        return mapViewer;
    }

    // Configuration de l'interface utilisateur
    private static void setupUI(JXMapViewer mapViewer, JTextArea textArea) throws JMSException {
        MessageViewer messageViewer = new MessageViewer();
        JPanel messagePanel = messageViewer.createMessagePanel(textArea);

        JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT, messagePanel, mapViewer);
        splitPane.setOneTouchExpandable(true);
        splitPane.setDividerLocation(250);

        JFrame frame = new JFrame("JXMapViewer2 - Interactive Map Viewer");
        frame.getContentPane().add(splitPane, BorderLayout.CENTER);
        frame.setSize(1000, 600);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
    }

    private static void addRouteToMap(JXMapViewer mapViewer, List<Itinary> itinaries) {
        // Liste pour stocker les positions Geo
        List<GeoPosition> track = null;
        List<RoutePainter> routePainters = new ArrayList<RoutePainter>();
        Color c = Color.RED;
        Set<Waypoint> waypoints = new HashSet<Waypoint>(2);

        for (Itinary itinary : itinaries) {
            track = new ArrayList<GeoPosition>();
            ArrayOfFeature arrayOfFeature = itinary.getFeatures().getValue();

            // Convertissez les coordonnées de l'itinéraire en GeoPositions
            for (Feature feature : arrayOfFeature.getFeature()) {
                ArrayOfArrayOfdouble coordinates = feature.getGeometry().getValue().getCoordinates().getValue();
                for (ArrayOfdouble coord : coordinates.getArrayOfdouble()) {
                    track.add(new GeoPosition(coord.getDouble().get(1), coord.getDouble().get(0))); // Latitude, Longitude
                    waypoints.add(new DefaultWaypoint(coord.getDouble().get(1), coord.getDouble().get(0)));
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

        // Ajoutez les points de départ et d'arrivée
        Set<Waypoint> startEnd = new HashSet<Waypoint>(2);
        startEnd.add(new DefaultWaypoint(itinaries.get(0).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(1), itinaries.get(0).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(0)));
        startEnd.add(new DefaultWaypoint(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().size()-1).getDouble().get(1), itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(itinaries.get(itinaries.size()-1).getFeatures().getValue().getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().size()-1).getDouble().get(0)));

        // Create a waypoint painter that takes all the waypoints
        WaypointPainter<Waypoint> waypointPainter = new WaypointPainter<Waypoint>();
        waypointPainter.setWaypoints(startEnd);

        // Create a compound painter that uses both the route-painter and the waypoint-painter
        List<Painter<JXMapViewer>> painters = new ArrayList<>(routePainters);
        painters.add(waypointPainter);

        CompoundPainter<JXMapViewer> painter = new CompoundPainter<JXMapViewer>(painters);
        mapViewer.setOverlayPainter(painter);

        //center map on the first waypoint
        mapViewer.setZoom(5);
        mapViewer.setAddressLocation(startEnd.stream().findFirst().get().getPosition());
    }

}

