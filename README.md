# LetsGoBiking - Instructions d'installation et de configuration

Ce document fournit des instructions détaillées pour installer et configurer l'environnement nécessaire pour exécuter le projet LetGoBiking.

## Prérequis

Avant de lancer l'application, assurez-vous que votre système répond aux exigences suivantes :

### Privilèges administratifs
- Vous devez avoir des privilèges administratifs sur votre machine pour exécuter le script de démarrage.

### Installation de Maven
- Maven doit être installé sur votre machine. Téléchargez Maven depuis [Apache Maven Project](https://maven.apache.org/download.cgi).
- Assurez-vous que Maven est ajouté au PATH de votre système.

### Java Development Kit (JDK)
- Java 17 doit être installé et configuré correctement. Téléchargez et installez le JDK depuis [AdoptOpenJDK](https://adoptopenjdk.net/) ou un autre fournisseur.
- Configurez la variable d'environnement `JAVA_HOME` et mettez à jour le PATH.

### Connexion Internet
- Une connexion Internet active est nécessaire pour télécharger les dépendances externes spécifiées dans le fichier `pom.xml`.

### Service WSDL
- Le fichier `pom.xml` fait référence à un service WSDL à l'URL `http://localhost:8090/RouteService?wsdl`. Assurez-vous que ce service est opérationnel et accessible.

## Démarrage des Services

### Service Route

Le service Route est une application WCF écoutant à l'adresse `http://localhost:8090/RouteService`.

#### Pour démarrer le Service Route :
1. Le service est hébergé en utilisant `ServiceHost` dans une application console .NET.
2. Au démarrage, il affiche l'URL du service et attend une entrée utilisateur pour se fermer.
3. Pour arrêter le service, appuyez sur `<Enter>` dans la fenêtre de la console.

### Service Proxy

Le service Proxy est une application WCF écoutant à l'adresse `http://localhost:8091/ProxyCacheSOAP/Service1`.

#### Pour démarrer le Service Proxy :
1. Le service est hébergé via `ServiceHost` dans une application console .NET.
2. Il affiche un message indiquant qu'il est prêt et en attente d'interactions.
3. Pour arrêter le service, appuyez sur `<Enter>` dans sa fenêtre de console.

---

## Configuration d'ActiveMQ pour LetGoBiking

Bien qu'ActiveMQ ne soit pas essentiel au fonctionnement de base de LetGoBiking, son utilisation est recommandée pour améliorer l'expérience utilisateur, notamment pour la gestion des files d'attente des messages.

### Installation d'ActiveMQ

1. **Téléchargez ActiveMQ** : Rendez-vous sur le [site officiel d'ActiveMQ](https://activemq.apache.org/components/classic/download/) et téléchargez la dernière version stable d'ActiveMQ.

2. **Décompressez l'archive** : Extrayez les fichiers dans un répertoire de votre choix.

3. **Lancez ActiveMQ** : Naviguez dans le répertoire d'ActiveMQ et lancez-le. Sous Windows, vous pouvez exécuter `activemq start` depuis le sous-dossier `bin`.

### Configuration du Projet

- **URL du Broker ActiveMQ** : Le service ActiveMQ est configuré pour écouter sur `tcp://localhost:61616`. Assurez-vous que ce port est libre sur votre machine.

- **Nom de la Queue** : La queue est dynamiquement nommée d'après l'UUID des itinéraires dans votre application. Assurez-vous que la génération et la réception des noms de queue fonctionnent correctement.

### Dépannage

Si vous rencontrez des problèmes avec ActiveMQ, vérifiez les éléments suivants :

- ActiveMQ est-il correctement lancé et accessible ?
- Les ports nécessaires sont-ils ouverts et non bloqués par un pare-feu ?
- Les configurations de queue dans l'application correspondent-elles aux paramètres d'ActiveMQ ?

---

## Exécution du Script de Démarrage

1. Ouvrez une invite de commande en tant qu'administrateur.
2. Naviguez vers le répertoire racine de LetGoBiking.
3. Exécutez le script `start_app.bat`.

Le script démarrera automatiquement les services nécessaires et exécutera les commandes Maven pour construire et démarrer l'application.

## Utilisation de l'Application
Une fois l'application lancée, vous serez accueilli par la fenêtre "Travel Form" où vous devrez entrer une origine et une destination. Après avoir rempli ces champs et soumis le formulaire, une autre fenêtre s'ouvrira : le "JXMapViewer2 - Interactive Map Viewer". Cette fenêtre affichera l'itinéraire sur une carte ainsi que les instructions de navigation détaillées.

## Considérations importantes

- Assurez-vous que les ports `8090` et `8091` sont libres et accessibles.
- Les services doivent être démarrés avant toute tentative de connexion des clients.
- Vérifiez les configurations de sécurité, comme les pare-feu, qui pourraient bloquer les communications sur ces ports.
- Si vous utilisez ActiveMQ, assurez-vous que le service est lancé et accessible.


