package org.example;

import com.soap.ws.client.generated.IRouteService;
import com.soap.ws.client.generated.Itinary;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.util.List;

public class ClientInputManager {

    private JTextField originField;
    private JTextField destinationField;
    private JFrame frame;

    public ClientInputManager() {
        frame = new JFrame("Travel Form");
        frame.setSize(400, 200);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setLayout(null);
    }

    public void askOriginAndDestination(IRouteService routeServiceClient, MapManager mapManager) {
        //Add components to the frame
        addComponents(routeServiceClient, mapManager);

        //Display the window.
        frame.setVisible(true);
    }

    private void addComponents(IRouteService routeServiceClient, MapManager mapManager) {
        // Create the labels
        JLabel originLabel = new JLabel("Origin:");
        originLabel.setBounds(10, 20, 80, 25);
        frame.add(originLabel);

        JLabel destinationLabel = new JLabel("Destination:");
        destinationLabel.setBounds(10, 50, 80, 25);
        frame.add(destinationLabel);

        // Create the text fields
        originField = new JTextField(20);
        originField.setBounds(100, 20, 165, 25);
        frame.add(originField);

        destinationField = new JTextField(20);
        destinationField.setBounds(100, 50, 165, 25);
        frame.add(destinationField);

        // Create the submit button
        JButton submitButton = new JButton("Submit");
        submitButton.setBounds(100, 80, 80, 25);
        frame.add(submitButton);

        // Add the action listener
        submitButton.addActionListener((ActionEvent e) -> {
            String origin = originField.getText();
            String destination = destinationField.getText();

            System.out.println("Origin: " + origin + ", Destination: " + destination);
            frame.setVisible(false);

            List<Itinary> response = routeServiceClient.getItinerary(origin, destination).getItinary();
            SwingUtilities.invokeLater(() -> {
                try {
                    mapManager.createAndShowMap(response);
                } catch (Exception ex) {
                    throw new RuntimeException("Error while creating map");
                }
            });
        });
    }
}
