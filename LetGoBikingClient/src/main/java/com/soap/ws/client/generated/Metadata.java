
package com.soap.ws.client.generated;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Classe Java pour Metadata complex type.
 * 
 * <p>Le fragment de schéma suivant indique le contenu attendu figurant dans cette classe.
 * 
 * <pre>
 * &lt;complexType name="Metadata"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="query" type="{http://schemas.datacontract.org/2004/07/LetGoBikingService.Models}Query" minOccurs="0"/&gt;
 *         &lt;element name="uuid" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Metadata", propOrder = {
    "query",
    "uuid"
})
public class Metadata {

    @XmlElementRef(name = "query", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<Query> query;
    @XmlElementRef(name = "uuid", namespace = "http://schemas.datacontract.org/2004/07/LetGoBikingService.Models", type = JAXBElement.class, required = false)
    protected JAXBElement<String> uuid;

    /**
     * Obtient la valeur de la propriété query.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link Query }{@code >}
     *     
     */
    public JAXBElement<Query> getQuery() {
        return query;
    }

    /**
     * Définit la valeur de la propriété query.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link Query }{@code >}
     *     
     */
    public void setQuery(JAXBElement<Query> value) {
        this.query = value;
    }

    /**
     * Obtient la valeur de la propriété uuid.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getUuid() {
        return uuid;
    }

    /**
     * Définit la valeur de la propriété uuid.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setUuid(JAXBElement<String> value) {
        this.uuid = value;
    }

}
