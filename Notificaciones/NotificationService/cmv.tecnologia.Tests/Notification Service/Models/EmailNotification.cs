﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace cmv.tecnologia.Tests.NotificationService.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class EmailNotification
    {
        /// <summary>
        /// Initializes a new instance of the EmailNotification class.
        /// </summary>
        public EmailNotification() { }

        /// <summary>
        /// Initializes a new instance of the EmailNotification class.
        /// </summary>
        public EmailNotification(string destinationEmail = default(string), string body = default(string), string subject = default(string), IList<ArchivoAdjunto> adjuntos = default(IList<ArchivoAdjunto>), string imagenFirmaBase64 = default(string))
        {
            DestinationEmail = destinationEmail;
            Body = body;
            Subject = subject;
            Adjuntos = adjuntos;
            ImagenFirmaBase64 = imagenFirmaBase64;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DestinationEmail")]
        public string DestinationEmail { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Adjuntos")]
        public IList<ArchivoAdjunto> Adjuntos { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ImagenFirmaBase64")]
        public string ImagenFirmaBase64 { get; set; }

    }
}
