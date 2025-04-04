using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOLA_SERVER.API.Controllers.Policies.v1
{
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        /// <summary>
        /// Retorna las políticas de uso de la aplicación en formato HTML
        /// </summary>
        /// <returns>Contenido HTML con las políticas de uso</returns>
        [HttpGet("html")]
        [Produces("text/html")]
        public ContentResult GetPoliciesHtml()
        {
            var html = @"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Políticas de Uso - LOLA</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        max-width: 800px;
                        margin: 0 auto;
                        padding: 20px;
                    }
                    h1 {
                        color: #2c3e50;
                        border-bottom: 1px solid #eee;
                        padding-bottom: 10px;
                    }
                    h2 {
                        color: #3498db;
                        margin-top: 30px;
                    }
                    p {
                        margin-bottom: 16px;
                    }
                    .date {
                        color: #7f8c8d;
                        font-style: italic;
                    }
                </style>
            </head>
            <body>
                <h1>Políticas de Uso - LOLA</h1>
                <p class='date'>Última actualización: 3 de Abril, 2025</p>
    
                <h2>1. Introducción</h2>
                <p>
                    Bienvenido a LOLA. Estas políticas de uso establecen los términos y condiciones
                    para el uso de nuestros servicios. Al acceder o utilizar nuestra aplicación,
                    usted acepta estar sujeto a estos términos.
                </p>
    
                <h2>2. Uso Aceptable</h2>
                <p>
                    Usted acepta utilizar nuestra aplicación solo para propósitos legales y de una manera que
                    no infrinja los derechos de otros usuarios o restrinja su uso de la aplicación.
                </p>
    
                <h2>3. Privacidad de Datos</h2>
                <p>
                    Protegemos su información personal de acuerdo con nuestra Política de Privacidad.
                    Solo recopilamos la información necesaria para proporcionar y mejorar nuestros servicios.
                </p>
    
                <h2>4. Propiedad Intelectual</h2>
                <p>
                    Todo el contenido presente en nuestra aplicación, incluyendo pero no limitado a textos,
                    gráficos, logotipos, iconos y software, está protegido por leyes de propiedad intelectual.
                </p>
    
                <h2>5. Terminación</h2>
                <p>
                    Nos reservamos el derecho de terminar o suspender su acceso a nuestra aplicación en caso
                    de violación de estas políticas de uso.
                </p>
    
                <h2>6. Cambios en las Políticas</h2>
                <p>
                    Podemos actualizar estas políticas ocasionalmente. Le notificaremos cualquier cambio
                    significativo a través de nuestra aplicación o por correo electrónico.
                </p>
    
                <h2>7. Contacto</h2>
                <p>
                    Si tiene preguntas sobre estas políticas, contáctenos a través de los canales oficiales
                    de soporte proporcionados en la aplicación.
                </p>
            </body>
            </html>";

            return Content(html, "text/html");
        }

        /// <summary>
        /// Retorna las políticas de uso de la aplicación en formato texto plano
        /// </summary>
        /// <returns>Contenido de texto con las políticas de uso</returns>
        [HttpGet("text")]
        [Produces("text/plain")]
        public ContentResult GetPoliciesText()
        {
            var text = @"POLÍTICAS DE USO - LOLA
                Última actualización: 3 de Abril, 2025

                1. INTRODUCCIÓN
                Bienvenido a LOLA. Estas políticas de uso establecen los términos y condiciones para el uso de nuestros servicios.

                2. USO ACEPTABLE
                Usted acepta utilizar nuestra aplicación solo para propósitos legales y de una manera que no infrinja los derechos de otros usuarios.

                3. PRIVACIDAD DE DATOS
                Protegemos su información personal de acuerdo con nuestra Política de Privacidad.

                4. PROPIEDAD INTELECTUAL
                Todo el contenido presente en nuestra aplicación está protegido por leyes de propiedad intelectual.

                5. TERMINACIÓN
                Nos reservamos el derecho de terminar o suspender su acceso a nuestra aplicación en caso de violación de estas políticas.

                6. CAMBIOS EN LAS POLÍTICAS
                Podemos actualizar estas políticas ocasionalmente.

                7. CONTACTO
                Si tiene preguntas, contáctenos a través de los canales oficiales de soporte proporcionados en la aplicación.";

            return Content(text, "text/plain");
        }

        /// <summary>
        /// Retorna las políticas de uso de la aplicación en formato JSON
        /// </summary>
        /// <returns>Objeto JSON con las políticas de uso</returns>
        [HttpGet("json")]
        [Produces("application/json")]
        public IActionResult GetPoliciesJson()
        {
            var policies = new
            {
                Title = "Políticas de Uso - LOLA",
                LastUpdated = "3 de Abril, 2025",
                Sections = new[]
                {
                    new {
                        Title = "Introducción",
                        Content = "Bienvenido a LOLA. Estas políticas de uso establecen los términos y condiciones para el uso de nuestros servicios."
                    },
                    new {
                        Title = "Uso Aceptable",
                        Content = "Usted acepta utilizar nuestra aplicación solo para propósitos legales y de una manera que no infrinja los derechos de otros usuarios."
                    },
                    new {
                        Title = "Privacidad de Datos",
                        Content = "Protegemos su información personal de acuerdo con nuestra Política de Privacidad."
                    },
                    new {
                        Title = "Propiedad Intelectual",
                        Content = "Todo el contenido presente en nuestra aplicación está protegido por leyes de propiedad intelectual."
                    },
                    new {
                        Title = "Terminación",
                        Content = "Nos reservamos el derecho de terminar o suspender su acceso a nuestra aplicación en caso de violación de estas políticas."
                    },
                    new {
                        Title = "Cambios en las Políticas",
                        Content = "Podemos actualizar estas políticas ocasionalmente."
                    },
                    new {
                        Title = "Contacto",
                        Content = "Si tiene preguntas, contáctenos a través de los canales oficiales de soporte proporcionados en la aplicación."
                    }
                }
            };

            return Ok(policies);
        }
    }
}