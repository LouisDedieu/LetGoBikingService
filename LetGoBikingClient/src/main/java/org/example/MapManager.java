package org.example;

import com.soap.ws.client.generated.*;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.input.CenterMapListener;
import org.jxmapviewer.input.PanKeyListener;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCursor;
import org.jxmapviewer.painter.CompoundPainter;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.*;

import javax.jms.JMSException;
import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import static org.example.Main.clientInputManager;
import static org.example.Main.routeServiceClient;

public class MapManager {
    private JXMapViewer mapViewer;
    private static final String WALKING = "foot-walking" ;
    private static final String CYCLING = "cycling-regular" ;
    private List<Itinary> response = null;
    private List<GeoPosition> track = null;
    public MapManager() {
        mapViewer = new JXMapViewer();
    }

    // Fonction principale pour créer et montrer la carte
    void createAndShowMap(List<Itinary> itinary) throws JMSException {
        response = itinary;
        mapViewer = initializeMapViewer(itinary);
        JTextArea textArea = new JTextArea();
        textArea.setEditable(false);
        setupUI(mapViewer, textArea);
    }

    // Initialisation et configuration de JXMapViewer
    private JXMapViewer initializeMapViewer(List<Itinary> itinary) {
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
    private void setupUI(JXMapViewer mapViewer, JTextArea textArea) throws JMSException {
        MessageViewer messageViewer = new MessageViewer(this);
        JPanel messagePanel = messageViewer.createMessagePanel(textArea);

        JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT, messagePanel, mapViewer);
        splitPane.setOneTouchExpandable(true);
        splitPane.setDividerLocation(250);

        // Créer le bouton de centrage
        JButton centerButton = new JButton("Nouvelle Recherche");

        // Panel pour contenir le bouton
        JPanel bottomPanel = new JPanel();
        bottomPanel.setLayout(new BorderLayout());
        bottomPanel.add(centerButton, BorderLayout.CENTER);

        // Créer et configurer la fenêtre principale
        JFrame frame = new JFrame("JXMapViewer2 - Interactive Map Viewer");
        frame.getContentPane().add(splitPane, BorderLayout.CENTER);
        frame.getContentPane().add(bottomPanel, BorderLayout.SOUTH); // Ajouter le panel du bouton en bas
        frame.setSize(1000, 600);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mapViewer.zoomToBestFit(new HashSet<GeoPosition>(track), 0.7);
        centerButton.addActionListener(e -> {
            clientInputManager.askOriginAndDestination(routeServiceClient, this);
            frame.setVisible(false);
        });
        frame.setVisible(true);
    }

    void addRouteToMap(JXMapViewer mapViewer, List<Itinary> itinaries) {
        // Liste pour stocker les positions Geo
        List<RoutePainter> routePainters = new ArrayList<RoutePainter>();
        Color c = Color.RED;
        Set<Waypoint> waypoints = new HashSet<Waypoint>();
        for (Itinary itinary : itinaries) {
            track = new ArrayList<GeoPosition>();
            ArrayOfFeature arrayOfFeature = itinary.getFeatures().getValue();
            waypoints.add(new DefaultWaypoint(arrayOfFeature.getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(1),
                    arrayOfFeature.getFeature().get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble().get(0).getDouble().get(0)));

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

    public void refreshMap(int messagesConsumed) {
        if (response == null) return;

        int i = 0;
        while (i < messagesConsumed && !response.isEmpty()) {
            Itinary firstItinary = response.get(0); // Obtenez le premier Itinary
            List<Feature> features = firstItinary.getFeatures().getValue().getFeature();

            if (!features.isEmpty()) {
                Feature firstFeature = features.get(0);
                List<Segment> segments = firstFeature.getProperties().getValue().getSegments().getValue().getSegment();

                if (!segments.isEmpty()) {
                    Segment firstSegment = segments.get(0);
                    List<Step> steps = firstSegment.getSteps().getValue().getStep();

                    if (!steps.isEmpty()) {
                        // Supprimez la première step
                        steps.remove(0);

                        // Supprimez également les coordonnées géographiques correspondantes
                        try{
                            int nbWaypointsToDelete = steps.get(0).getWayPoints().getValue().getInt().get(1)
                                    - steps.get(0).getWayPoints().getValue().getInt().get(0);
                            for (int j=0; j<nbWaypointsToDelete; j++) {
                                List<ArrayOfdouble> coordinates = features.get(0).getGeometry().getValue().getCoordinates().getValue().getArrayOfdouble();
                                if (!coordinates.isEmpty() && coordinates.size()>2) {
                                    coordinates.remove(0); // Supprimez la coordonnée correspondante
                                }
                                else {
                                    break;
                                }
                            }
                        } catch (Exception e) {
                            response.remove(0);
                            refreshRouteDisplay(mapViewer, response);
                            return;
                        }
                    } else {
                        segments.remove(0); // Si pas de steps, supprimez le Segment entier
                    }
                } else {
                    features.remove(0); // Si pas de segments, supprimez la Feature entière
                }
            } else {
                response.remove(0); // Si pas de features, supprimez l'Itinary entier
            }

            i++; // Incrémente i après chaque itération
        }

        // Après la mise à jour des itinéraires, rafraîchir l'affichage de la carte
        refreshRouteDisplay(mapViewer, response);
    }




    public void refreshRouteDisplay(JXMapViewer mapViewer, List<Itinary> itinaries) {
        // Effacez les anciens itinéraires
        mapViewer.setOverlayPainter(null);

        // Reconstruisez track et ajoutez les nouveaux itinéraires
        addRouteToMap(mapViewer, itinaries);

        // Redessiner la carte
        mapViewer.repaint();
    }

    public List<Itinary> getResponse() {
        return response;
    }
}
