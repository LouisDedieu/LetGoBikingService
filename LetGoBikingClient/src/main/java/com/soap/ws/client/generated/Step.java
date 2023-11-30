
package com.soap.ws.client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour Step complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType name="Step"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="Distance" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="Duration" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="EndLatitude" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="EndLongitude" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="Instruction" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *         &lt;element name="Name" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *         &lt;element name="StartLatitude" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="StartLongitude" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/&gt;
 *         &lt;element name="way_points" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfint" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Step", propOrder = {
    "distance",
    "duration",
    "endLatitude",
    "endLongitude",
    "instruction",
    "name",
    "startLatitude",
    "startLongitude",
    "wayPoints"
})
public class Step {

    @XmlElement(name = "Distance")
    protected Double distance;
    @XmlElement(name = "Duration")
    protected Double duration;
    @XmlElement(name = "EndLatitude")
    protected Double endLatitude;
    @XmlElement(name = "EndLongitude")
    protected Double endLongitude;
    @XmlElementRef(name = "Instruction", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<String> instruction;
    @XmlElementRef(name = "Name", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<String> name;
    @XmlElement(name = "StartLatitude")
    protected Double startLatitude;
    @XmlElement(name = "StartLongitude")
    protected Double startLongitude;
    @XmlElementRef(name = "way_points", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<ArrayOfint> wayPoints;

    /**
     * Obtient la valeur de la propriété distance.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getDistance() {
        return distance;
    }

    /**
     * Définit la valeur de la propriété distance.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setDistance(Double value) {
        this.distance = value;
    }

    /**
     * Obtient la valeur de la propriété duration.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getDuration() {
        return duration;
    }

    /**
     * Définit la valeur de la propriété duration.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setDuration(Double value) {
        this.duration = value;
    }

    /**
     * Obtient la valeur de la propriété endLatitude.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getEndLatitude() {
        return endLatitude;
    }

    /**
     * Définit la valeur de la propriété endLatitude.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setEndLatitude(Double value) {
        this.endLatitude = value;
    }

    /**
     * Obtient la valeur de la propriété endLongitude.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getEndLongitude() {
        return endLongitude;
    }

    /**
     * Définit la valeur de la propriété endLongitude.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setEndLongitude(Double value) {
        this.endLongitude = value;
    }

    /**
     * Obtient la valeur de la propriété instruction.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getInstruction() {
        return instruction;
    }

    /**
     * Définit la valeur de la propriété instruction.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setInstruction(JAXBElement<String> value) {
        this.instruction = value;
    }

    /**
     * Obtient la valeur de la propriété name.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getName() {
        return name;
    }

    /**
     * Définit la valeur de la propriété name.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setName(JAXBElement<String> value) {
        this.name = value;
    }

    /**
     * Obtient la valeur de la propriété startLatitude.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getStartLatitude() {
        return startLatitude;
    }

    /**
     * Définit la valeur de la propriété startLatitude.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setStartLatitude(Double value) {
        this.startLatitude = value;
    }

    /**
     * Obtient la valeur de la propriété startLongitude.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getStartLongitude() {
        return startLongitude;
    }

    /**
     * Définit la valeur de la propriété startLongitude.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setStartLongitude(Double value) {
        this.startLongitude = value;
    }

    /**
     * Obtient la valeur de la propriété wayPoints.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     *     
     */
    public JAXBElement<ArrayOfint> getWayPoints() {
        return wayPoints;
    }

    /**
     * Définit la valeur de la propriété wayPoints.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     *     
     */
    public void setWayPoints(JAXBElement<ArrayOfint> value) {
        this.wayPoints = value;
    }

}
