
package com.soap.ws.client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour Itinerary complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType name="Itinerary"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="EndLocation" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *         &lt;element name="StartLocation" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Itinerary", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", propOrder = {
    "endLocation",
    "startLocation"
})
public class Itinerary {

    @XmlElementRef(name = "EndLocation", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<String> endLocation;
    @XmlElementRef(name = "StartLocation", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<String> startLocation;

    /**
     * Obtient la valeur de la propriété endLocation.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getEndLocation() {
        return endLocation;
    }

    /**
     * Définit la valeur de la propriété endLocation.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setEndLocation(JAXBElement<String> value) {
        this.endLocation = value;
    }

    /**
     * Obtient la valeur de la propriété startLocation.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getStartLocation() {
        return startLocation;
    }

    /**
     * Définit la valeur de la propriété startLocation.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setStartLocation(JAXBElement<String> value) {
        this.startLocation = value;
    }

}
