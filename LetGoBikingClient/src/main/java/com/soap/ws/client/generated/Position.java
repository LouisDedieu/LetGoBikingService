
package com.soap.ws.client.generated;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour Position complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType name="Position"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="Lat" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="Lng" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Position", propOrder = {
    "lat",
    "lng"
})
public class Position {

    @XmlElement(name = "Lat")
    protected Double lat;
    @XmlElement(name = "Lng")
    protected Double lng;

    /**
     * Obtient la valeur de la propriété lat.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getLat() {
        return lat;
    }

    /**
     * Définit la valeur de la propriété lat.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setLat(Double value) {
        this.lat = value;
    }

    /**
     * Obtient la valeur de la propriété lng.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getLng() {
        return lng;
    }

    /**
     * Définit la valeur de la propriété lng.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setLng(Double value) {
        this.lng = value;
    }

}
