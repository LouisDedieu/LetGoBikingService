package org.example;

import com.soap.ws.client.generated.Feature;
import com.soap.ws.client.generated.Itinary;
import com.soap.ws.client.generated.Segment;
import com.soap.ws.client.generated.Step;
import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.*;
import javax.swing.*;
import java.awt.*;

public class MessageViewer {

    private MapManager mapManager;
    private static final String BROKER_URL = "tcp://localhost:61616";

    public MessageViewer(MapManager mapManager) {
        this.mapManager = mapManager;
    }

    public JPanel createMessagePanel(JTextArea textArea) {
        JPanel panel = new JPanel(new BorderLayout());
        panel.add(new JScrollPane(textArea), BorderLayout.CENTER);
        JButton button = new JButton("Get Next Instructions");
        panel.add(button, BorderLayout.SOUTH);

        try {
            setupActiveMQConnection(textArea, button);
        } catch (JMSException e) {
            handleJMSException(textArea, button);
        }

        return panel;
    }

    private void setupActiveMQConnection(JTextArea textArea, JButton button) throws JMSException {
        // ActiveMQ configuration
        ConnectionFactory factory = new ActiveMQConnectionFactory(BROKER_URL);
        Connection connection = factory.createConnection();
        connection.start();
        Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
        String queueName = mapManager.getResponse().get(0).getMetadata().getValue().getUuid().getValue();
        Queue queue = session.createQueue(queueName);
        MessageConsumer consumer = session.createConsumer(queue);

        button.addActionListener(e -> processWithActiveMQ(consumer, textArea));
    }

    private void processWithActiveMQ(MessageConsumer consumer, JTextArea textArea) {
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
                mapManager.refreshMap(messagesConsumed);
            } else {
                textArea.append("Aucun nouveau message à traiter.\n");
                if(!mapManager.getResponse().isEmpty())
                    mapManager.refreshMap(10);
            }
        } catch (JMSException ex) {
            textArea.append("Erreur lors de la réception des messages.\n");
        }
    }

    private void handleJMSException(JTextArea textArea, JButton button) {
        textArea.append("Connexion ActiveMQ échouée. Affichage des données disponibles.\n");
        for (Itinary itinary : mapManager.getResponse()) {
            processItinary(itinary, textArea);
        }
        button.addActionListener(e -> processWithoutActiveMQ());
    }

    private void processWithoutActiveMQ() {
        if(!mapManager.getResponse().isEmpty())
            mapManager.refreshMap(10);
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
}
