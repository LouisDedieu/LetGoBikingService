package org.example;

import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class MessageViewer {

    private MessageConsumer consumer;

    public JPanel createMessagePanel(JTextArea textArea) throws JMSException {
        // Configuration de la connexion ActiveMQ
        ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
        Connection connection = factory.createConnection();
        connection.start();
        Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
        Queue queue = session.createQueue("ItineraryQueue");

        consumer = session.createConsumer(queue);

        // Création du panneau principal pour les messages
        JPanel panel = new JPanel(new BorderLayout());

        panel.add(new JScrollPane(textArea), BorderLayout.CENTER);

        JButton button = new JButton("Get Next Message");
        panel.add(button, BorderLayout.SOUTH);

        button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                try {
                    Message message = consumer.receiveNoWait(); // Ne pas attendre indéfiniment pour un message
                    if (message instanceof TextMessage) {
                        TextMessage textMessage = (TextMessage) message;
                        String text = textMessage.getText();
                        textArea.append(text + "\n");
                    } else {
                        textArea.append("Aucun nouveau message.\n");
                    }
                } catch (JMSException ex) {
                    ex.printStackTrace();
                }
            }
        });

        return panel;
    }
}
