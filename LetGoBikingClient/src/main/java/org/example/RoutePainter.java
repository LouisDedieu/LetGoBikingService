package org.example;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.GeoPosition;

import java.awt.*;
import java.awt.geom.Point2D;
import java.util.ArrayList;
import java.util.List;

public class RoutePainter implements Painter<JXMapViewer> {
    private List<GeoPosition> track;
    private Color color = Color.RED;
    private boolean antiAlias = true;

    public RoutePainter(List<GeoPosition> track) {
        // Copie la liste pour que les modifications de la liste originale n'aient aucun effet ici
        this.track = new ArrayList<>(track);
    }

    @Override
    public void paint(Graphics2D g, JXMapViewer map, int w, int h) {
        g = (Graphics2D) g.create();

        // Convertit du viewport au bitmap du monde
        Rectangle rect = map.getViewportBounds();
        g.translate(-rect.x, -rect.y);

        if (antiAlias) {
            g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        }

        // Dessine la route
        g.setColor(Color.BLACK);
        g.setStroke(new BasicStroke(4));

        drawStartAndEndMarkers(g, map);

        g.dispose();
    }

    private void drawStartAndEndMarkers(Graphics2D g, JXMapViewer map) {
        // Dessine le point de départ
        Point2D point = map.getTileFactory().geoToPixel(track.get(0), map.getZoom());
        int x = (int) point.getX();
        int y = (int) point.getY();
        g.setColor(Color.GREEN);
        g.fillOval(x - 5, y - 5, 10, 10);

        // Dessine le point d'arrivée
        point = map.getTileFactory().geoToPixel(track.get(track.size() - 1), map.getZoom());
        x = (int) point.getX();
        y = (int) point.getY();
        g.setColor(Color.RED);
        g.fillOval(x - 5, y - 5, 10, 10);
    }

    /*private void drawRoute(Graphics2D g, JXMapViewer map) {
        int lastX = 0;
        int lastY = 0;

        boolean first = true;

        for (GeoPosition gp : track) {
            // Convertit les coordonnées en pixels
            Point2D pt = map.getTileFactory().geoToPixel(gp, map.getZoom());
        }

        // Dessine une ligne entre les deux points
        if (first) {
            first = false;
        } else {
            g.drawLine(lastX, lastY, x, y);
        }

        // Stocke les coordonnées

    }*/
}
