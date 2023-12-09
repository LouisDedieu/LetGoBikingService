package org.example;

import com.soap.ws.client.generated.Feature;
import com.soap.ws.client.generated.Itinary;
import com.soap.ws.client.generated.Segment;
import com.soap.ws.client.generated.Step;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.jxmapviewer.JXMapViewer;

import javax.jms.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import static org.example.Main.addRouteToMap;

public class MessageViewer {

    private MessageConsumer consumer;

    public JPanel createMessagePanel(JTextArea textArea) {
        JPanel panel = new JPanel(new BorderLayout());
        panel.add(new JScrollPane(textArea), BorderLayout.CENTER);

        JButton button = new JButton("Get Next Messages");
        panel.add(button, BorderLayout.SOUTH);

        try {
            // Configuration de la connexion ActiveMQ
            ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
            Connection connection = factory.createConnection();
            connection.start();
            Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            String queueName = Main.getResponse().get(0).getMetadata().getValue().getUuid().getValue();
            Queue queue = session.createQueue(queueName);

            consumer = session.createConsumer(queue);

            button.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    int messagesConsumed = 0;

                    try {
                        for (int i = 0; i < 10; i++) {
                            Message message = consumer.receiveNoWait();
                            if (message == null) {
                                break;
                            }
                            messagesConsumed++;

                            if (message instanceof TextMessage) {
                                TextMessage textMessage = (TextMessage) message;
                                String text = textMessage.getText();
                                textArea.append(text + "\n");
                            }
                        }
                        if (messagesConsumed > 0) {
                            Main.refreshMap(messagesConsumed);
                        } else {
                            textArea.append("Aucun nouveau message à traiter.\n");
                            Main.refreshMap(1);
                        }
                    } catch (JMSException ex) {
                        for (Itinary itinary : Main.getResponse()) {
                            textArea.append(itinary.toString() + "\n");
                        }
                    }
                }
            });
        } catch (JMSException e) {
            handleJMSException(textArea);
        }

        return panel;
    }

    private void handleJMSException(JTextArea textArea) {
        for (Itinary itinary : Main.getResponse()) {
            processItinary(itinary, textArea);
        }
    }

    private void processItinary(Itinary itinary, JTextArea textArea) {
        for (Feature feature : itinary.getFeatures().getValue().getFeature()) {
            for (Segment segment : feature.getProperties().getValue().getSegments().getValue().getSegment()) {
                for (Step step : segment.getSteps().getValue().getStep()) {
                    textArea.append(step.getInstruction().getValue() + "\n");
                }
            }
        }
    }

    private void updateGeoPositions(int messagesConsumed) {

    }


}
