using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace LOLA_SERVER.API.Controllers.Policies.v1
{
    /// <summary>
    /// Controlador para manejar las políticas de la aplicación en diferentes formatos
    /// </summary>
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        // Definiciones de tipos para evitar problemas con dynamic
        public class Seccion
        {
            public int id { get; set; }
            public string titulo { get; set; }
            public string contenido { get; set; }
        }

        public class Politicas
        {
            public string titulo { get; set; }
            public string ultima_actualizacion { get; set; }
            public Seccion[] secciones { get; set; }
        }

        public class TerminosServicio
        {
            public string titulo { get; set; }
            public string vigencia { get; set; }
            public Seccion[] secciones { get; set; }
        }

        public class TerminosServicioCompletos
        {
            public string titulo { get; set; }
            public string vigencia { get; set; }
            public string contenido_completo { get; set; }
        }

        public class Contacto
        {
            public string email { get; set; }
            public string telefono { get; set; }
            public string direccion { get; set; }
        }

        public class Esquema
        {
            public Politicas politicas { get; set; }
            public TerminosServicio terminos_servicio { get; set; }
            public TerminosServicioCompletos terminos_servicio_completos { get; set; }
            public Contacto contacto { get; set; }
        }

        // El esquema es nuestra "base de datos" con toda la información de políticas
        private readonly Esquema _politicasEsquema;

        public PoliciesController()
        {
            // Inicializamos el esquema con la estructura y contenido usando clases tipadas
            _politicasEsquema = new Esquema
            {
                politicas = new Politicas
                {
                    titulo = "Políticas de Uso - LOLA",
                    ultima_actualizacion = "3 de Abril, 2025",
                    secciones = new Seccion[]
                    {
                        new Seccion {
                            id = 1,
                            titulo = "Introducción",
                            contenido = "Bienvenido a LOLA. Estas políticas de uso establecen los términos y condiciones para el uso de nuestros servicios. Al acceder o utilizar nuestra aplicación, usted acepta estar sujeto a estos términos."
                        },
                        new Seccion {
                            id = 2,
                            titulo = "Uso Aceptable",
                            contenido = "Usted acepta utilizar nuestra aplicación solo para propósitos legales y de una manera que no infrinja los derechos de otros usuarios o restrinja su uso de la aplicación."
                        },
                        new Seccion {
                            id = 3,
                            titulo = "Privacidad de Datos",
                            contenido = "Protegemos su información personal de acuerdo con nuestra Política de Privacidad. Solo recopilamos la información necesaria para proporcionar y mejorar nuestros servicios."
                        },
                        new Seccion {
                            id = 4,
                            titulo = "Propiedad Intelectual",
                            contenido = "Todo el contenido presente en nuestra aplicación, incluyendo pero no limitado a textos, gráficos, logotipos, iconos y software, está protegido por leyes de propiedad intelectual."
                        },
                        new Seccion {
                            id = 5,
                            titulo = "Terminación",
                            contenido = "Nos reservamos el derecho de terminar o suspender su acceso a nuestra aplicación en caso de violación de estas políticas de uso."
                        },
                        new Seccion {
                            id = 6,
                            titulo = "Cambios en las Políticas",
                            contenido = "La recopilación y el uso de su información personal en el Servicio LOLA se describen en nuestra Declaración de Privacidad. Al acceder o utilizar el Servicio LOLA, usted reconoce haber leído y comprendido la Declaración de Privacidad."
                        },
                        new Seccion {
                            id = 7,
                            titulo = "Contacto",
                            contenido = "Si tiene preguntas sobre estas políticas, contáctenos a través de los canales oficiales de soporte proporcionados en la aplicación."
                        }
                    }
                },
                terminos_servicio = new TerminosServicio
                {
                    titulo = "Condiciones de servicio",
                    vigencia = "27 de marzo de 2025",
                    secciones = new Seccion[]
                    {
                        new Seccion {
                            id = 1,
                            titulo = "Aceptación de los Términos; Modificaciones",
                            contenido="Estos Términos de Servicio (los “ Términos ”) constituyen un acuerdo legal vinculante entre usted y A LOLA CUIDA TU MASCOTA S.A.S, una empresa constituida bajo las leyes de colombianas con domicilio en Calle 2b # 81ª 460 Medellín, (“ LOLA ”, “ nosotros ”, “ nos ” y “ nuestro ”). Los Términos rigen el uso que usted haga de nuestras aplicaciones de software, recursos y servicios para que los dueños de mascotas y los proveedores de servicios para mascotas se encuentren, se comuniquen y organicen la prestación de servicios de cuidado de mascotas, y cualquier otro servicio o producto que podamos ofrecer ocasionalmente (colectivamente, nuestro “ Servicio LOLA ”). Los Términos rigen todo uso del Servicio Lola, ya sea que acceda a él desde nuestro sitio web en https://www.lolacuidatumascota.com (incluido cualquier subdominio o versión localizada) (el “ Sitio ”), nuestras aplicaciones móviles y sitios web móviles , nuestra aplicación de Facebook, nuestras ofertas de soporte en línea o telefónico, o cualquier otro punto de acceso que pongamos a su disposición. Nuestros TERMINOS DE GARANTIA LOLA ,LA POLITICA DE PROTECCION DE RESERVAS y otras Políticas aplicables a su uso del Servicio Lola se incorporan por referencia a estos Términos de Servicio. AL ACEPTAR ESTOS TÉRMINOS DURANTE EL PROCESO DE REGISTRO DE CUENTA O AL ACCEDER O USAR EL SERVICIO LOLA SIN UNA CUENTA, USTED ACEPTA ESTOS TÉRMINOS. SI NO ESTÁ DE ACUERDO CON ESTOS TÉRMINOS, NO DEBE ACEPTARLOS, EN CUYO CASO NO TENDRÁ DERECHO A USAR EL SERVICIO LOLA.\r\nUsted comprende y acepta que podemos modificar los Términos ocasionalmente, y que dichos cambios entrarán en vigor cuando publiquemos los Términos modificados en el Servicio LOLA, a menos que la legislación aplicable exija lo contrario. Su acceso y uso continuado del Servicio LOLA después de la publicación de los Términos modificados constituirá su consentimiento a quedar sujeto a los Términos modificados.\r\n"
                        },
                        new Seccion {
                            id = 2,
                            titulo = "LEA ESTOS TÉRMINOS DETENIDAMENTE",
                            contenido="2.1 Naturaleza del Servicio LOLA . El Servicio LOLA consta de una aplicación web de escritorio, aplicaciones móviles y otras herramientas, soporte y servicios relacionados que los dueños de mascotas (« Dueños de Mascotas ») y los proveedores de servicios relacionados con mascotas (« Proveedores de Servicios ») pueden utilizar para encontrarse, comunicarse e interactuar entre sí. El Servicio LOLA incluye nuestros servicios de asistencia en caso de emergencia, materiales educativos para proveedores de servicios y otros servicios. Cobramos por algunos aspectos del Servicio LOLA, como se describe más adelante en la Sección 9.\r\n2.2 LOLA no ofrece Servicios de Cuidado de Mascotas . LOLA es un foro neutral para Proveedores de Servicios y Dueños de Mascotas. LOLA no es un Proveedor de Servicios y, salvo la asistencia telefónica de emergencia y otros recursos y asistencia descritos específicamente en el Servicio LOLA, no ofrece servicios de cuidado de mascotas. No realizamos declaraciones ni garantías sobre la calidad del alojamiento, cuidado de mascotas, paseo de perros, cuidado de casas u otros servicios prestados por los Proveedores de Servicios (\" Servicios de Cuidado de Mascotas \"), ni sobre sus interacciones y tratos con los usuarios. Los Proveedores de Servicios que aparecen en LOLA no están bajo la dirección ni el control de LOLA, y estos determinan a su propia discreción cómo prestar los Servicios de Cuidado de Mascotas. Si bien en nuestro Sitio proporcionamos orientación general a los Proveedores de Servicios sobre seguridad y cuidado de mascotas, y a los Dueños de Mascotas sobre la selección y contratación de Proveedores de Servicios, LOLA no emplea, recomienda ni avala a los Proveedores de Servicios ni a los Dueños de Mascotas y, en la medida máxima permitida por la legislación aplicable, no seremos responsables del desempeño o la conducta de los Proveedores de Servicios ni de los Dueños de Mascotas, ya sea en línea o fuera de línea. Realizamos una revisión inicial de los perfiles de los Proveedores de Servicios y, según lo permita la ley, facilitamos Verificaciones de Antecedentes o de Identidad (cada una de ellas como se describe en la Sección 10, más adelante) realizadas por un tercero. Sin embargo, salvo que se especifique explícitamente en el Servicio LOLA (y solo en la medida especificada), no realizamos evaluaciones adicionales de los Proveedores de Servicios ni de los Dueños de Mascotas. Le recomendamos tener precaución y usar su criterio independiente antes de contratar a un Proveedor de Servicios, prestar Servicios de Cuidado de Mascotas o interactuar con usuarios a través del Servicio LOLA. Los Dueños de Mascotas y los Proveedores de Servicios son los únicos responsables de tomar decisiones que beneficien su propio bienestar y el de sus mascotas. Por ejemplo, cada usuario del Servicio LOLA es responsable de mantener al día las vacunas de su mascota, y no nos responsabilizamos por el incumplimiento de las vacunas de ninguna persona.\r\n2.3 Exención . De conformidad con la Sección 16 a continuación, LOLA no se responsabiliza de ninguna reclamación, lesión, pérdida, daño o perjuicio derivado o relacionado de cualquier manera con sus interacciones o tratos con otros usuarios, ni con las acciones u omisiones de los Proveedores de Servicios y Dueños de Mascotas, ya sea en línea o fuera de línea. Usted reconoce y acepta que, en la medida máxima permitida por la legislación aplicable, SU USO O PRESTACIÓN DE SERVICIOS DE CUIDADO DE MASCOTAS ES BAJO SU PROPIO Y EXCLUSIVO RIESGO. (Cualquier obligación financiera que LOLA pueda tener con sus usuarios en relación con su conducta se limita a las obligaciones de reembolso establecidas en la GARANTIA LOLA ).\r\n2.4 Las transacciones se realizan entre los dueños de mascotas y los proveedores de servicios . El Servicio LOLA puede utilizarse para buscar y ofrecer servicios de cuidado de mascotas y para facilitar el pago, pero todas las transacciones realizadas a través del Servicio LOLA se realizan entre los dueños de mascotas y los proveedores de servicios. Salvo los reembolsos limitados y la «Protección de la reserva» especificados en la Sección 9.6 y la  GARANTIA LOLA , usted acepta que LOLA no se responsabiliza por los daños asociados con los servicios de cuidado de mascotas (que pueden incluir lesiones corporales o la muerte de una mascota) ni por cualquier otra transacción entre usuarios del Servicio LOLA.\r\n2.5 Reservas . Los dueños de mascotas y los proveedores de servicios realizan transacciones entre sí en el Servicio LOLA cuando ambos acuerdan una \"reserva\" que especifica las tarifas, el período de tiempo, la política de cancelación y otros términos para la prestación de los servicios de cuidado de mascotas a través del mecanismo de reserva proporcionado en el Servicio LOLA (una \" Reserva \"). Una reserva puede ser iniciada por un proveedor de servicios o un dueño de mascota seleccionando el tipo de servicios de cuidado de mascotas que se proporcionarán y luego siguiendo las instrucciones que aparecen en pantalla. Si usted es dueño de mascota e inicia una reserva, acepta pagar por los servicios de cuidado de mascotas descritos en la reserva cuando haga clic en \"Solicitar reserva\". Si usted es dueño de mascota y un proveedor de servicios inicia una reserva, acepta pagar por los servicios de cuidado de mascotas descritos en la reserva cuando haga clic en \"Pagar ahora\". Todas las solicitudes están sujetas a la aceptación de la parte receptora. La parte receptora no está obligada a aceptar su (ni ninguna) solicitud y puede, a su discreción, rechazarla por cualquier motivo. Una vez que complete una reserva, usted acepta respetar el precio y otros términos de esa reserva, como se reconoce en la confirmación de la reserva.\r\n2.6 Los dueños de mascotas son los únicos responsables de evaluar a los proveedores de servicios . Los dueños de mascotas son los únicos responsables de evaluar la idoneidad de los proveedores de servicios para los servicios que ofrecen. Si bien LOLA realiza una revisión limitada de los perfiles de los proveedores de servicios y facilita la verificación de antecedentes o identidad de estos por parte de terceros, dicha revisión es limitada y LOLA no garantiza que sea precisa, completa, concluyente ni esté actualizada. Asimismo, LOLA no avala las reseñas de proveedores de servicios realizadas por otros dueños de mascotas que puedan estar disponibles a través del Servicio LOLA, y no se compromete a que dichas reseñas sean precisas o legítimas.\r\n2.7 Mascotas Abandonadas; Reubicación . Los dueños de mascotas que contraten Servicios de Cuidado de Mascotas y no recuperen a su mascota después del período de servicio indicado en una Reserva aceptan que LOLA (o el Proveedor de Servicios) podrá, a su entera discreción, colocar a la mascota en un hogar de acogida, transferir su cuidado a control animal u otras autoridades policiales, o buscar un cuidado alternativo. El dueño de la mascota se compromete a reembolsar a LOLA y/o al Proveedor de Servicios todos los costos y gastos asociados con dichas acciones. Además, LOLA se reserva expresamente el derecho, a su entera discreción, de retirar la mascota del cuidado del Proveedor de Servicios si LOLA lo considera necesario para la seguridad de la mascota, del Proveedor de Servicios o de cualquier persona que conviva con él. Antes de retirar una mascota del cuidado del Proveedor de Servicios, LOLA hará todo lo razonablemente posible durante su horario laboral para contactar al dueño de la mascota y/o a su contacto de emergencia (si se proporciona) para organizar un cuidado alternativo. Si LOLA no logra contactar al dueño de la mascota o al contacto de emergencia, hará todo lo posible para encontrar un cuidado alternativo hasta que el dueño pueda recuperarla. Si usted es dueño de una mascota, autoriza a su(s) veterinario(s) a compartir su historial veterinario con LOLA en relación con cualquier reubicación o realojamiento. Además, usted es responsable y acepta pagar todos los costos y gastos incurridos por LOLA en relación con dicho traslado.\r\n2.8 Emergencias . Recomendamos a los Dueños de Mascotas que proporcionen a sus Proveedores de Servicios la información de contacto para que puedan ser localizados en caso de que sea necesaria la atención médica para una mascota, y que proporcionen un contacto de emergencia en el perfil del Dueño de la Mascota que haya consentido la divulgación de su información. Los Proveedores de Servicios se comprometen a contactar inmediatamente a los Dueños de Mascotas en caso de que dicha atención sea necesaria o, si el Dueño de la Mascota no está disponible, a contactar a LOLA al número de teléfono o dirección de correo electrónico correspondiente que figura en la tabla al final de estos Términos. Si usted es Dueño de una Mascota, por la presente autoriza a su Proveedor de Servicios, a su contacto de emergencia y/o a LOLA a obtener y autorizar la prestación de atención veterinaria para su mascota si no puede ser localizado para autorizar la atención usted mismo en una situación de emergencia. En tal caso, también autoriza al/los veterinario(s) de su mascota a divulgar el historial veterinario de su mascota a LOLA y a su Proveedor de Servicios. Usted exime al Proveedor de Servicios y a LOLA de cualquier lesión, daño o responsabilidad derivada de la prestación de atención de emergencia o de la falta de solicitud de dicha atención conforme a esta sección, incluyendo el reembolso que de otro modo podría haber estado disponible bajo la GARANTIA LOLA . Los dueños de mascotas son responsables de los costos de cualquier tratamiento médico para sus mascotas y, si usted es dueño de una mascota, por la presente autoriza a LOLA a cargar dichos costos a su tarjeta de crédito u otro método de pago. En ciertas circunstancias, un dueño de mascota puede tener derecho a un reembolso bajo la GARANTIA LOLA . LOLA recomienda que todos los usuarios cuenten con un seguro para mascotas adecuado para cubrir los costos de la atención veterinaria, adicional a cualquier otro seguro que LOLA pueda brindar o promocionar.\r\n2.9 Servicios de Consulta . LOLA puede ofrecer a los Dueños de Mascotas y Proveedores de Servicios servicios de consulta veterinaria de terceros por teléfono, chat o correo electrónico para brindarles un recurso educativo que les ayude a tomar decisiones sobre sus mascotas o las mascotas bajo su cuidado. Estos servicios de consulta son proporcionados por un tercero y no forman parte del Servicio LOLA. Si utiliza estos servicios de consulta de terceros, debe usarlos únicamente junto con la atención veterinaria profesional, y no como sustitutos de esta. Usted acepta recurrir únicamente al servicio de consulta de terceros correspondiente en caso de cualquier reclamación derivada de sus servicios.\r\n2.10 Google Maps . El uso del Servicio LOLA requiere el uso de las funciones y el contenido de Google Maps, que están sujetos a las (1) Condiciones de Servicio Adicionales de Google Maps/Google Earth vigentes, disponibles en https://maps.google.com/help/terms_maps.html (incluida la Política de Uso Aceptable en https://cloud.google.com/maps-platform/terms/aup/ ); y a la (2) Política de Privacidad de Google, disponible en https://www.google.com/policies/privacy/ (en conjunto, las \" Condiciones de Google \"). Al utilizar el Servicio LOLA, usted reconoce y acepta las Condiciones de Google aplicables (por ejemplo, como \" Usuario Final \"). Cualquier uso no autorizado de las funciones y el contenido de Google Maps puede resultar en la suspensión o cancelación de su suscripción al Servicio LOLA.\r\n"
                            },
                        new Seccion {
                            id = 3,
                            titulo = "Servicio LOLA",
                            contenido="Al acceder y utilizar el Servicio LOLA, usted certifica que: (1) tiene al menos 18 años de edad o la mayoría de edad en su jurisdicción, lo que sea mayor, y (2) cumplirá con todas las leyes y reglamentaciones aplicables a sus actividades realizadas a través de, o relacionadas con, el Servicio LOLA.\r\n•\tPara los dueños de mascotas, esto significa, entre otras cosas, que usted se asegurará de que sus mascotas estén vacunadas, autorizadas, tengan una etiqueta de identificación y/o microchip según lo requieran las leyes o regulaciones locales; que ha obtenido y mantendrá todas las pólizas de seguro obligatorias con respecto a las mascotas cuyo cuidado confía a los proveedores de servicios (y que dichas pólizas beneficiarán a terceros, incluidos los proveedores de servicios, en la misma medida en que lo benefician a usted).\r\n•\tPara los proveedores de servicios, esto significa, entre otras cosas, que usted certifica que es legalmente elegible para proporcionar servicios de cuidado de mascotas en la jurisdicción donde proporciona dichos servicios; que ha cumplido y cumplirá con todas las leyes y reglamentaciones que le sean aplicables; que ha obtenido todas las licencias comerciales, registros de impuestos comerciales, licencias comerciales y permisos necesarios para proporcionar legalmente servicios de cuidado de mascotas; y que, al proporcionar servicios de cuidado de mascotas, cumplirá con las leyes aplicables sobre correas, eliminación de desechos de mascotas y leyes similares.\r\nUsted reconoce que LOLA tiene derecho a confiar en estas certificaciones suyas, no es responsable de garantizar que todos los usuarios hayan cumplido con las leyes y regulaciones aplicables y no será responsable por el incumplimiento de un usuario al hacerlo.\r\n"                        },
                        new Seccion {
                            id = 4,
                            titulo = "LOLA no ofrece Servicios de Cuidado de Mascotas",
                            contenido="4.1 Su conducta en el Servicio LOLA . Al utilizar el Servicio LOLA, usted acepta:\r\n•\tUtilizar el Servicio LOLA únicamente de forma legal y únicamente para los fines previstos.\r\n•\tNo utilizar el Servicio LOLA para organizar el cuidado de: (a) mascotas exóticas o inherentemente peligrosas, como serpientes venenosas o constrictoras, primates, lobos o híbridos de lobo, gatos no domesticados, caimanes, caballos u otro ganado; (b) cualquier animal cuya propiedad o cuidado por parte de terceros esté prohibido por la ley aplicable; o (c) cualquier animal que tenga antecedentes de, o que haya sido entrenado para, ataques a mascotas o personas.\r\n•\tNo enviar virus ni otros códigos maliciosos al Servicio LOLA ni a través de él.\r\n•\tNo utilizar el Servicio LOLA, ni interactuar con otros usuarios del Servicio LOLA, con fines que violen la ley.\r\n•\tNo utilizar el Servicio LOLA para concertar la prestación y compra de servicios con otro usuario y luego completar transacciones para esos servicios fuera del Servicio LOLA.\r\n•\tNo utilizar el Servicio LOLA con fines de competir con LOLA o para promocionar otros productos o servicios.\r\n•\tNo publicar reseñas que no se basen en su experiencia personal, que sean intencionalmente inexactas o engañosas o que violen estos Términos.\r\n•\tNo publicar contenido o materiales que sean pornográficos, amenazantes, acosadores, abusivos o difamatorios, o que contengan desnudez o violencia gráfica, inciten a la violencia, violen derechos de propiedad intelectual o violen la ley o los derechos legales (por ejemplo, derechos de privacidad) de otros.\r\n•\tNo publicar “spam” u otras comunicaciones comerciales no autorizadas.\r\n•\tUtilizar el Servicio LOLA únicamente para sus propios fines y no suplantar a ninguna otra persona.\r\n•\tNo transferir ni autorizar el uso de su cuenta del Servicio LOLA por parte de ninguna otra persona, ni participar en transacciones fraudulentas.\r\n•\tNo utilizar ningún crédito o recompensa promocional de una manera incompatible con el espíritu y el propósito del programa, incluido, entre otros, el uso de créditos o recompensas promocionales para crear reservas dentro del mismo hogar o con alguien con quien comparte una mascota.\r\n•\tNo proporcionar información falsa en su perfil o registro en el Servicio LOLA, ni crear cuentas múltiples o duplicadas.\r\n•\tNo interferir con nuestra prestación del Servicio LOLA ni con el uso que haga cualquier otro usuario del mismo.\r\n•\tNo solicitar el nombre de usuario o la contraseña de otro usuario para el Servicio LOLA ni ningún otro dato personal confidencial, incluidos datos bancarios.\r\n4.2 Suspensión y Terminación . Usted comprende y acepta que no tenemos obligación de proporcionar el Servicio LOLA en ninguna ubicación o territorio específico, ni de continuar brindándolo una vez que hayamos comenzado. Nos reservamos el derecho de suspender o terminar su acceso al Servicio LOLA: (1) si, a nuestra discreción, su conducta en el Sitio o el Servicio LOLA es inapropiada, insegura, deshonesta o infringe estos Términos; o (2) si es necesario, a nuestra discreción, para proteger a LOLA, a sus usuarios, mascotas o al público. Usted puede suspender o terminar su uso del Servicio LOLA en cualquier momento y por cualquier motivo. Si desea desactivar su cuenta, comuníquese con LOLA o inicie sesión en su cuenta y visite \"Configuración de la Cuenta\". Tenga en cuenta que si tiene alguna obligación de pago pendiente, esta seguirá vigente tras la suspensión o terminación de su cuenta.\r\n4.3 Protección contra Contenido Ilegal. LOLA toma medidas para promover un entorno en línea seguro. Estas medidas incluyen, entre otras, proteger a los usuarios de contenido ilegal mediante (a) la prohibición del uso del Servicio LOLA para publicar o enviar contenido ilegal en estos Términos de Servicio (véase la Sección 4.1); (b) la provisión de una función que permite a los usuarios bloquear o denunciar perfiles y conversaciones con otros usuarios, la cual revisaremos; (c) la prohibición y eliminación, si se nos informa, de reseñas, respuestas o partes de estas que infrinjan estos Términos; y (d) si se nos informa, el ejercicio de nuestro derecho a suspender o desactivar a un usuario que publique o envíe contenido ilegal. Puede denunciar contenido ilegal en cualquier momento poniéndose en contacto con nosotros como se describe en la Sección 21.\r\n"
                            },
                        new Seccion {
                            id = 5,
                            titulo = "Exención",
                            contenido="Para usar algunos aspectos del Servicio LOLA, deberá crear un nombre de usuario, una contraseña y un perfil de usuario. Si decide usar el Servicio LOLA, se compromete a proporcionar información precisa sobre usted y a mantenerla actualizada. Se compromete a no suplantar la identidad de otra persona ni a mantener más de una cuenta (o, si LOLA suspende o cancela su cuenta, a no crear cuentas adicionales). Usted es responsable de mantener la confidencialidad de su nombre de usuario y contraseña para el Servicio LOLA y de toda la actividad de su cuenta. Se compromete a notificarnos de inmediato cualquier uso no autorizado de su cuenta."                        },
                        new Seccion {
                            id = 6,
                            titulo = "Transacciones entre dueños y proveedores",
                            contenido = "La recopilación y el uso de su información personal en el Servicio LOLA se describen en nuestra Declaración de Privacidad. Al acceder o utilizar el Servicio LOLA, usted reconoce haber leído y comprendido la Declaración de Privacidad ."
                        },
                        new Seccion {
                            id = 7,
                            titulo = "Reservas",
                            contenido="7.1 Su Contenido . Podemos exigirle o permitirle (o a otra persona en su nombre) que envíe o cargue texto, fotografías, imágenes, vídeos, reseñas, información y materiales a su perfil en el Servicio LOLA o de cualquier otra forma en relación con el uso del Servicio LOLA o la participación en campañas promocionales que realizamos en el Sitio (en conjunto, \" Su Contenido \"). Por ejemplo, se invita a los Proveedores de Servicios a crear una página de perfil con una fotografía y otra información, y a enviar fotos de los perros a su cuidado a los Dueños de Mascotas, mientras que estos pueden enviar reseñas de los Proveedores de Servicios.\r\n7.2 Licencia . Salvo las limitaciones sobre el uso y la divulgación de información personal descritas en nuestra Declaración de Privacidad, usted otorga a LOLA, en la medida y duración máximas permitidas por la legislación aplicable, una licencia irrevocable, perpetua, no exclusiva y de pago completo a nivel mundial para usar, copiar, ejecutar, exhibir públicamente, reproducir, adaptar, modificar, transmitir, difundir, crear obras derivadas y/o distribuir su Contenido en relación con la prestación y/o promoción del Servicio LOLA, así como para sublicenciar estos derechos a terceros.\r\n7.3 Exención . Si su nombre, voz, imagen, personalidad, semejanza o interpretación se incluye en cualquiera de sus Contenidos, por la presente renuncia y exime a LOLA y a sus usuarios de cualquier reclamación o causa de acción, conocida o desconocida, por difamación, infracción de derechos de autor, invasión de los derechos de privacidad, publicidad o personalidad, o cualquier reclamación similar derivada del uso de sus Contenidos de acuerdo con la licencia de la Sección 7.2 y las demás disposiciones de estos Términos.\r\n7.4 Sus Declaraciones y Garantías sobre su Contenido . Usted declara y garantiza que (1) es el propietario o licenciante de su Contenido y que cuenta con todos los derechos, consentimientos y permisos necesarios para otorgar la licencia descrita en la Sección 7.2 y realizar la cesión de derechos descrita en la Sección 7.3 con respecto a su Contenido; (2) que cuenta con los consentimientos y autorizaciones necesarios de las personas que aparecen o cuyas mascotas aparecen en su Contenido; y (3) que su Contenido no infringe la ley ni estos Términos.\r\n7.5 Derecho a eliminar o filtrar su contenido . Si bien no estamos obligados a hacerlo, nos reservamos el derecho a supervisar, filtrar, editar o eliminar su contenido del Servicio LOLA. La aplicación de estos Términos con respecto a su contenido queda a nuestra discreción, y el incumplimiento de los Términos en un caso no implica una renuncia a nuestro derecho a aplicarlos en otro. No tenemos la obligación de conservar ni proporcionarle copias de su contenido, ni seremos responsables ante usted por la eliminación, divulgación, pérdida o modificación de este. Es su exclusiva responsabilidad mantener copias de seguridad de su contenido. Los residentes de la Unión Europea y Noruega pueden encontrar información sobre la Ley de Servicios Digitales.\r\n7.6 Reseñas . El Servicio LOLA puede ofrecer la posibilidad de dejar reseñas públicas o privadas de usuarios o sus mascotas. Usted reconoce que incluso las reseñas privadas pueden compartirse con terceros de acuerdo con la legislación aplicable y nuestra Declaración de Privacidad, y que LOLA no tiene la obligación de conservarlas ni almacenarlas indefinidamente. No tenemos la obligación de proporcionarle el contenido de las reseñas sobre usted enviadas por otros usuarios del Servicio LOLA, ni antes ni después de la desactivación de su cuenta. No nos responsabilizamos ante usted por la eliminación, divulgación, pérdida o modificación de estas reseñas. Nos reservamos el derecho de filtrar, editar o eliminar estas reseñas del Servicio LOLA en cualquier momento.\r\n"                        },
                        new Seccion {
                            id = 8,
                            titulo = "Emergencias",
                            contenido="8.1 Consentimiento para mensajes de texto y otras comunicaciones . Esta Sección 8.1 se aplica únicamente a usuarios en Estados Unidos y Colombia. Usted consiente recibir de LOLA, o en su nombre, comunicaciones que contengan información relacionada con el servicio (incluyendo avisos técnicos, actualizaciones, alertas de seguridad y mensajes de soporte y administrativos), y/o mensajes de ventas, marketing o publicidad, mediante llamadas de voz automáticas, pregrabadas o artificiales, SMS, mensajes de texto, correo electrónico, plataformas de mensajería OTT (como WhatsApp) y otros medios electrónicos, en cualquier número de teléfono o dirección de correo electrónico que proporcione en relación con su cuenta, incluso si su número de teléfono está en el registro nacional o de cualquier estado de \"no llamar\". Es posible que se apliquen las tarifas y cargos normales de mensajería, datos y otros de su operador a estas comunicaciones. No está obligado a dar este consentimiento para recibir mensajes de ventas, marketing o publicidad automáticos como condición para comprar algo o usar el Servicio LOLA, y puede optar por no recibir dichos mensajes en cualquier momento como se describe en nuestra Declaración de Privacidad (aunque puede continuar recibiendo mensajes mientras LOLA procesa su solicitud).\r\n8.2 Cambios de número de teléfono . En caso de desactivar un número de teléfono móvil que nos haya proporcionado, se compromete a actualizar la información de su cuenta LOLA lo antes posible para garantizar que no se envíen mensajes a la persona que adquiera su número anterior.\r\n"
                        },
                        new Seccion {
                            id = 9,
                            titulo = "Certificación de Cumplimiento de la Ley Aplicable",
                            contenido="9.1 Moneda . Todas las tarifas, deducibles y demás pagos mencionados en el Servicio LOLA o cobrados a través de él se indican y deben pagarse en moneda local.\r\n9.2 Tarifas para Dueños de Mascotas . Los Dueños de Mascotas pueden adquirir Servicios de Cuidado de Mascotas de un Proveedor de Servicios completando una Reserva, como se describe en la Sección 2.5. Si usted es Dueño de Mascotas, al aceptar una Reserva, realiza una transacción con el Proveedor de Servicios y se compromete a pagar el importe total indicado en la Reserva, que incluye las tarifas de servicio a LOLA. El Proveedor de Servicios, y no LOLA, es responsable de la prestación de los Servicios de Cuidado de Mascotas. Cuando la ley lo exija, el importe cobrado incluirá los impuestos aplicables. \r\n9.3 Tarifas para Proveedores de Servicios . Los Proveedores de Servicios pueden aceptar la prestación de Servicios de Cuidado de Mascotas al Dueño de una Mascota al aceptar una Reserva, como se describe en la Sección 2.5. Si usted es un Proveedor de Servicios, debe confirmar la Reserva antes de su vencimiento; de lo contrario, el Dueño de la Mascota no tendrá la obligación de completar la transacción. Una vez que ambas partes completen la Reserva, usted acepta respetar el precio establecido en ella. La compra de Servicios de Cuidado de Mascotas es una transacción entre el Dueño de la Mascota y el Proveedor de Servicios. La función de LOLA es facilitar la transacción. Nosotros (ya sea directa o indirectamente a través de un tercero autorizado) cobraremos el pago al Dueño de la Mascota en el momento de la Reserva y (excepto en la medida de cualquier retención de pago de conformidad con la Sección 9.7) iniciaremos el pago a la cuenta del Proveedor de Servicios 7 dias después de la finalización del período de servicio indicado en la Reserva. Salvo que se especifique lo contrario a través del Servicio LOLA, se calculan como un porcentaje de las tarifas que el Dueño de Mascota acepta pagar a un Proveedor de Servicios en una Reserva y se cobran en cada Reserva. Además, al registrarse como Proveedor de Servicios, se le podría cobrar una tarifa no reembolsable por la revisión de su perfil. Para los Proveedores de Servicios fuera de Estados Unidos que hayan proporcionado un número de identificación fiscal indirecto válido (por ejemplo, número de IVA o GST), los importes cobrados no incluirán los impuestos indirectos aplicables, como el IVA o el GST. De lo contrario, los importes cobrados a Proveedores de Servicios fuera de Estados Unidos incluirán los impuestos indirectos aplicables.\r\n9.4 Tarifas de servicio . Cobramos tarifas de servicio por algunos aspectos del Servicio LOLA. Si usted es un Proveedor de Servicios, salvo que se especifique lo contrario a través del Servicio LOLA, nuestra tarifa de servicio se calcula como un porcentaje de las tarifas que el Dueño de la Mascota se compromete a pagarle en una Reserva y se cobra por cada Reserva. \r\n9.5 Cargos por retraso y cargos adicionales . Si usted es dueño de una mascota, reconoce y acepta que, si no recupera a su mascota al final del período de servicio acordado en una Reserva, se le cobrará el tiempo de servicio adicional ( a prorrata por cada día parcial de retraso) a la tarifa diaria establecida en la Reserva. Además, acepta indemnizar a LOLA y que podamos cargar a su tarjeta de crédito u otro método de pago cualquier costo o gasto adicional en el que incurramos nosotros o el Proveedor de Servicios como resultado de su imposibilidad de recuperar a su mascota al final del período de servicio acordado en una Reserva.\r\n9.6 Cancelaciones y reembolsos .\r\n•\tLOLA , LOLA puede ayudarle a encontrar proveedores de servicios de reemplazo cuando estos cancelen reservas cerca de la fecha de inicio del período de servicio indicado en la reserva. La disponibilidad de la Protección de Reservas depende del momento de la cancelación y del tipo de servicios de cuidado de mascotas proporcionados. Consulte la página de protección de reservas para obtener más información.\r\n•\tCancelaciones por parte del Proveedor de Servicios. Si un Proveedor de Servicios cancela una Reserva antes o durante el período de servicio indicado en la Reserva, reembolsaremos las tarifas pagadas por el Dueño de la Mascota por los Servicios de Cuidado no prestados, así como cualquier cargo por servicio pagado a LOLA. Si usted es un Proveedor de Servicios, puede designar un Proveedor de Servicios sustituto (según lo acordado con el Dueño de la Mascota y siempre que este tenga una cuenta activa en el Servicio LOLA y haya aceptado por escrito la Reserva) contactando con LOLA para modificar la Reserva. Si no encuentra un sustituto y cancela repetidamente las Reservas aceptadas sin justificación, LOLA podrá desactivar su cuenta.\r\n•\tCancelaciones por parte del dueño de la mascota. Si el dueño de la mascota cancela una reserva antes o durante el período de servicio especificado en la misma, le reembolsaremos el importe de acuerdo con la política de cancelación seleccionada por el proveedor de servicios en el Servicio LOLA. Todos los proveedores de servicios deben seleccionar una política de cancelación antes de completar la reserva para que los dueños de la mascota la conozcan antes de realizarla. \r\n•\tCondiciones Generales para Cancelaciones. Si desea cancelar una Reserva, deberá utilizar los mecanismos disponibles a través del Servicio LOLA. A efectos de las políticas y términos de esta Sección 9.6, la fecha de cancelación es la fecha en que un usuario cancela a través del Servicio LOLA, independientemente de cualquier comunicación por separado entre usuarios fuera del Servicio LOLA.\r\n•\tDisputas de Pago; Pagos Fuera del Servicio LOLA. LOLA inicia los pagos a los Proveedores de Servicios 7 dias después de la finalización de una Reserva. Una vez desembolsados estos importes, cualquier disputa de pago posterior se resolverá entre el Dueño de la Mascota y el Proveedor de Servicios, y LOLA no tiene la obligación de mediar ni facilitar ninguna resolución. Asimismo, LOLA no asume ninguna responsabilidad con respecto a propinas, bonificaciones u otros pagos realizados fuera del Servicio LOLA.\r\n•\tReembolsos discrecionales por incumplimiento. A nuestra discreción razonable, si determinamos que un proveedor de servicios no ha prestado los servicios de cuidado de mascotas según lo acordado con el propietario de la mascota o que incumple estos Términos, podremos cancelar la reserva o emitir un reembolso total o parcial al propietario de la mascota.\r\n9.7 Retención de Pagos . Si usted es un Proveedor de Servicios, LOLA se reserva el derecho a retener los importes que le corresponderían de conformidad con la Sección 9.3 si existe una sospecha razonable de actividad fraudulenta relacionada con su(s) cuenta(s) o por otras razones igualmente imperiosas que impliquen la protección de LOLA, la comunidad LOLA o los derechos de terceros. También podemos recomendar a los proveedores de servicios de pago externos que restrinjan su acceso a los fondos de su cuenta en las circunstancias mencionadas.\r\n9.8 Autorización de Cobro . Al pagar por Servicios de Cuidado de Mascotas u otros servicios del Servicio LOLA, deberá proporcionarnos información válida y actualizada de su tarjeta de crédito u otra información de pago, y mantener dicha información (o un método de pago alternativo aceptable) en su cuenta mientras tenga Reservas pendientes y confirmadas. La función de LOLA es facilitar los pagos de los Dueños de Mascotas a los Proveedores de Servicios como agente de pago limitado para el Proveedor de Servicios. Nos autoriza a cargar a su tarjeta de crédito u otro método de pago las tarifas que genere por el Servicio LOLA a su vencimiento, y a cargar a cualquier método de pago alternativo que LOLA tenga registrado para usted en caso de que su método de pago principal haya caducado, no sea válido o no se pueda cobrar. Usted es responsable de mantener la información de pago actualizada. Si no podemos cobrarle las tarifas a su vencimiento porque su información de pago ya no es válida, o si no recibimos su pago a su vencimiento, usted entiende que ni LOLA ni el Proveedor de Servicios serán responsables de la falta de prestación de los servicios asociados con dichas tarifas. Salvo que se establezca expresamente en estos Términos, todas las tarifas pagadas a través del Servicio LOLA no son reembolsables una vez abonadas.\r\n9.9 Impuestos . Salvo los impuestos sobre la renta y los ingresos brutos de LOLA, o cuando LOLA esté obligado a recaudarlos, usted reconoce ser el único responsable de pagar cualquier impuesto aplicable que surja como resultado de su compra, prestación o uso de los Servicios de Cuidado de Mascotas a través del Servicio LOLA. Esto incluye, entre otros, cualquier tipo de impuesto sobre las ventas, IVA o impuesto sobre la renta sobre las tarifas pagadas o recibidas por usted a través del Servicio LOLA. En ciertas jurisdicciones, LOLA podría estar obligado por ley a recopilar o declarar información fiscal sobre usted. Usted acepta proporcionarnos la documentación que consideremos necesaria para cumplir con dichas obligaciones y, en caso de no hacerlo, LOLA podrá suspender o desactivar su cuenta hasta que se proporcione dicha documentación.\r\n9.10 Procesamiento de pagos . Los servicios de procesamiento de pagos pueden ser proporcionados por uno o más procesadores de pagos externos. LOLA se reserva el derecho de cambiar de proveedor de procesamiento de pagos o utilizar proveedores alternativos o de respaldo a su discreción. El procesamiento de pagos proporcionado por PAYU está sujeto al Acuerdo de Servicios de PAYU y, si recibe pagos a través del Servicio LOLA, al Acuerdo de Cuenta Conectada de PAYU (colectivamente, los \" Términos de PAYU \"). Para utilizar el Servicio LOLA para recibir pagos, es posible que deba configurar una cuenta de PAYU y aceptar los Términos de PAYU . Autoriza a LOLA a obtener todo el acceso necesario y a realizar todas las actividades necesarias en su Cuenta Conectada de PAYU (incluida la solicitud de reembolsos cuando corresponda) para facilitar el pago relacionado con los Servicios de Cuidado de Mascotas que compre o proporcione. Acepta proporcionar información precisa y completa sobre usted y su negocio, y autoriza a LOLA a compartir dicha información, así como la información de las transacciones, con PAYU con el fin de facilitar los servicios de procesamiento de pagos proporcionados por PAYU. Si se encuentra en Estados Unidos, acepta además que, según lo permita la ley y de acuerdo con estos Términos: (a) designa a LOLA para que actúe como su agente con el único propósito de aceptar pagos, en su nombre, de los Dueños de Mascotas por los Servicios de Cuidado de Mascotas que les proporciona y para que dichos pagos le sean entregados; (b) cualquier pago de este tipo recibido a través del Servicio LOLA se considerará pago a su nombre; y (c) en caso de no entrega de dicho pago, su único recurso es contra LOLA y no reclamará el pago a los Dueños de Mascotas. Si se encuentra dentro de la Unión Europea, el Reino Unido, Noruega o Suiza, LOLA puede liquidar los pagos a la filial española de LOLA que puede aceptar pagos en nombre de LOLA. \r\n"
                        },
                        new Seccion {
                            id = 10,
                            titulo = "Tarifas y pagos",
                            contenido="LOLA puede proporcionarle acceso a agencias externas de informes de consumidores o proveedores de verificación de identidad que realizan, entre otras cosas, servicios de verificación de identificación personal (\" Verificación de Identidad \") o verificaciones de antecedentes penales, verificaciones del registro de delincuentes sexuales o verificaciones de registros de vehículos motorizados (colectivamente, \" Verificaciones de Antecedentes \"). Fuera de los EE. UU. y Canadá, estos servicios se limitan a la Verificación de Identidad. No proporcionamos, ni somos responsables ni nos hacemos responsables de ninguna manera de, las Verificaciones de Antecedentes o Verificaciones de Identidad, y no respaldamos ni hacemos ninguna declaración o garantía con respecto a la confiabilidad de dichas Verificaciones de Antecedentes o Verificaciones de Identidad o la precisión, puntualidad o integridad de cualquier información en las Verificaciones de Antecedentes o Verificaciones de Identidad. No verificamos de forma independiente la información en las Verificaciones de Antecedentes o Verificaciones de Identidad.\r\nAl someterse a una Verificación de Antecedentes o de Identidad a través del Servicio LOLA, usted autoriza y consiente la recopilación, el uso y la divulgación de la información de dicha verificación a la agencia de informes de consumo o al proveedor de verificación de identidad externo, y se compromete a proporcionar información completa y precisa para dicha verificación. Autoriza a LOLA a obtener informes de consumo o de investigación en cualquier momento tras recibir su autorización y consentimiento, y durante su relación con LOLA, según lo permita la legislación aplicable.\r\nUsted comprende y acepta que LOLA puede revisar y basarse en la información de la Verificación de Antecedentes o la Verificación de Identidad para decidir si suspende, cancela o investiga una queja sobre un Proveedor de Servicios o Dueño de Mascota. Sin embargo, también acepta que no estamos obligados a hacerlo y no somos responsables de ninguna manera en caso de que la información de la Verificación de Antecedentes o la Verificación de Identidad no sea precisa, oportuna o completa. Si se le realiza una Verificación de Antecedentes, puede comunicarse con la agencia de informes de consumo externa correspondiente para disputar la precisión, puntualidad o integridad de dicha información. Puede comunicarse con LOLA para disputar una decisión de suspender o cancelar su cuenta basada total o parcialmente en los resultados de la Verificación de Antecedentes. Usted acepta que los derechos y obligaciones de LOLA en virtud del Acuerdo de Arbitraje (establecido en la Sección 17, a continuación) redundan en beneficio de la agencia de informes de consumo utilizada para las Verificaciones de Antecedentes o la Verificación de Identidad con respecto a cualquier reclamación que estaría sujeta al Acuerdo de Arbitraje si se presentara contra nosotros. LOLA se reserva el derecho de suspender o terminar su acceso al Servicio LOLA según la información en la Verificación de Antecedentes o Verificación de Identidad o por cualquier otra violación de estos Términos, según lo permita la ley aplicable.\r\nTenga en cuenta las siguientes limitaciones en las Verificaciones de Antecedentes: Salvo que se disponga expresamente lo contrario en estos Términos o a través del Servicio, LOLA podrá, pero no estará obligado a, volver a realizar las Verificaciones de Antecedentes de cualquier Usuario. Las Verificaciones de Antecedentes también pueden variar según el tipo, la amplitud y la profundidad, y los resultados pueden excluir lo siguiente:\r\n•\tCondados donde el individuo no tiene historial de direcciones.\r\n•\tInformación no disponible (o cuya divulgación demora) en jurisdicciones particulares, que podría incluir registros de arrestos, antecedentes de delincuentes sexuales y registros de vehículos motorizados.\r\n•\tInformación que no puede informarse o no aparecer en el registro público, como antecedentes juveniles y condenas expurgadas.\r\n•\tArrestos o condenas en países extranjeros.\r\n•\tAntecedentes civiles o infracciones de tránsito, a menos que una jurisdicción los reporte como delitos penales.\r\n•\tRegistros que las agencias de verificación de antecedentes tienen prohibido informar debido a leyes federales, estatales o locales, por ejemplo, arrestos que no resultan en condenas.\r\n•\tCualquier otra información no reportada por agencias de verificación de antecedentes de terceros.\r\nSiempre debe ser precavido y usar su criterio independiente antes de contratar a un Proveedor de Servicios, prestar Servicios de Cuidado de Mascotas o interactuar con usuarios a través del Servicio LOLA. La verificación de antecedentes o de identidad no sustituye la realización de entrevistas exhaustivas (como durante los encuentros) ni la evaluación e indagación independientes de la persona con la que interactúa. \r\n"                        },
                        new Seccion {
                            id = 11, 
                            titulo = "Contacto",
                            contenido="LOLA cumple con los procedimientos de la Ley de Derechos de Autor. Responderemos a las reclamaciones por infracción de derechos de autor que se notifiquen de conformidad con esta Sección. Nuestra política, en circunstancias apropiadas, es desactivar o cancelar el acceso de los usuarios que infrinjan repetidamente o sean acusados repetidamente de infringir los derechos de autor u otros derechos de propiedad intelectual de terceros.\r\nSi cree de buena fe que su obra protegida por derechos de autor ha sido infringida por el contenido publicado en el Servicio LOLA, proporcione a nuestro agente de derechos de autor designado un aviso por escrito que incluya toda la siguiente información:\r\n•\tUna descripción de la obra protegida por derechos de autor que usted cree que ha sido infringida;\r\n•\tUna descripción de la URL u otra ubicación en nuestro Sitio del material que usted cree que infringe sus derechos;\r\n•\tSu nombre, dirección postal, número de teléfono y dirección de correo electrónico;\r\n•\tUna declaración de que usted cree de buena fe que el uso en disputa no está autorizado por el propietario de los derechos de autor, su agente o la ley;\r\n•\tUna declaración suya, que hace bajo pena de perjurio, de que la información anterior en su notificación es precisa y que usted es el propietario de los derechos de autor o está autorizado para actuar en nombre del propietario de los derechos de autor; y\r\n•\tUna firma electrónica o física de la persona autorizada para actuar en nombre del propietario de los derechos de autor.\r\nPuede comunicarse con nuestro agente designado para notificaciones de infracción de derechos de autor en: lolacuidatumascota@gmail.com\r\n"
                            },
                        new Seccion {
                            id = 12,
                            titulo = "Naturaleza del Servicio LOLA",
                            contenido="Usted reconoce que LOLA depende de los servicios de proveedores externos (como procesadores de pagos) para prestar el Servicio LOLA. El Servicio LOLA también puede contener enlaces a sitios web o recursos de terceros. Usted reconoce y acepta que no nos hacemos responsables de: (i) la disponibilidad o exactitud de dichos sitios web o recursos; ni (ii) el contenido, los productos o los servicios disponibles en dichos sitios web o recursos. Los enlaces a dichos sitios web o recursos no implican ninguna aprobación de dichos sitios web o recursos, ni del contenido, los productos o los servicios disponibles en ellos. Usted reconoce ser el único responsable y asume todos los riesgos derivados del uso de dichos sitios web o recursos."
                        },
                        new Seccion {
                            id = 13,
                            titulo = "Mascotas Abandonadas; Reubicación",
                            contenido="HASTA EL MÁXIMO GRADO PERMITIDO POR CUALQUIER LEY APLICABLE Y SALVO QUE LA LEY APLICABLE PROHÍBA LO CONTRARIO, USTED ACEPTA DEFENDER, INDEMNIZAR Y EXIMIR DE RESPONSABILIDAD A LOLA FRENTE A TODAS Y CADA UNA DE LAS RECLAMACIONES, DEMANDAS, CAUSAS DE ACCIÓN, PÉRDIDAS, GASTOS, DAÑOS Y/O RESPONSABILIDADES, INCLUYENDO HONORARIOS RAZONABLES DE ABOGADOS Y COSTAS JUDICIALES, QUE ESTÉN RELACIONADAS DE ALGUNA MANERA CON SUS: (1) transacciones e interacciones, en línea o fuera de línea, con otros usuarios del Servicio LOLA; (2) incumplimiento de estos Términos; (3) disputas con otros usuarios del Servicio LOLA; (4) sus declaraciones erróneas, tergiversaciones o violación de la ley aplicable; (5) daños a la propiedad o lesiones personales a terceros causados por su mascota o mascotas bajo su cuidado; (6) Su Contenido; o (7) el uso que usted haga de cualquier información de verificación de antecedentes o de identidad en violación de cualquier ley aplicable. ADEMÁS, USTED ACEPTA COOPERAR CON NOSOTROS EN LA DEFENSA DE DICHAS RECLAMACIONES. NOS RESERVAMOS EL DERECHO DE ASUMIR LA DEFENSA Y EL CONTROL EXCLUSIVOS DE CUALQUIER ASUNTO SUJETO A INDEMNIZACIÓN EN VIRTUD DE ESTA SECCIÓN, Y USTED NO RESOLVERÁ NINGUNA RECLAMACIÓN O ASUNTO DE DICHA ÍNDOLE SIN NUESTRO CONSENTIMIENTO PREVIO POR ESCRITO."
                        },
                        new Seccion {
                            id = 14,
                            titulo = "Servicios de Consulta",
                            contenido="14.1 Servicio LOLA . LOLA y sus licenciantes conservan todos los derechos, títulos e intereses sobre el Servicio LOLA, la tecnología y el software utilizados para proporcionarlo, toda la documentación y el contenido electrónicos disponibles a través del Servicio LOLA (excepto Su Contenido), y todos los derechos de propiedad intelectual y de propiedad sobre el Servicio LOLA y dicha tecnología, software, documentación y contenido. Salvo sus derechos de acceso y uso del Servicio LOLA establecidos en estos Términos, ninguna disposición de estos Términos licencia ni transfiere nuestros derechos de propiedad intelectual o de propiedad a nadie, incluido usted. Usted acepta que tendremos derecho perpetuo a usar e incorporar en el Servicio LOLA cualquier comentario o sugerencia de mejora que nos proporcione sobre el Servicio LOLA, sin obligación de compensación.\r\n14.2 Marcas de LOLA . LOLA posee todos los derechos sobre sus marcas comerciales, marcas de servicio, nombres comerciales y logotipos (las \" Marcas de LOLA \"). Si usted es un Proveedor de Servicios, sujeto a estos Términos, LOLA le otorga, mientras tenga una buena relación con el Servicio LOLA, una licencia limitada, revocable, no exclusiva e intransferible para usar las Marcas de LOLA únicamente: (a) en la forma en que se incorporen a los productos, incluyendo materiales de marketing personalizables (tarjetas promocionales, señalización, etc.), si los hubiera, puestos a su disposición por LOLA, y/o (b) de otras maneras, únicamente en la medida en que se autorice específicamente por escrito a través del Servicio LOLA. Como condición de su uso del Servicio LOLA y la licencia anterior, usted acepta que (1) no tiene derechos de propiedad en las Marcas LOLA y que toda la buena voluntad asociada con su uso de las Marcas LOLA redunda únicamente en beneficio de LOLA, (2) dicha licencia finaliza inmediatamente cuando usted deja de ser un Proveedor de servicios en regla, ya sea por su propia opción o porque LOLA suspende o finaliza sus derechos de uso del Servicio LOLA, (3) LOLA puede finalizar su derecho a usar todas y cada una de las Marcas LOLA en cualquier momento por cualquier motivo o sin motivo alguno a discreción exclusiva de LOLA, y (4) no adoptará ni usará ninguna Marca LOLA excepto según lo autorizado explícitamente por LOLA, y no usará, registrará ni solicitará el registro de las Marcas LOLA, el término LOLA o cualquier otro término que incluya el término LOLA o cualquier otro término similar, como nombre comercial, nombre comercial, marca registrada, nombre de dominio, nombre de perfil de redes sociales o cualquier otro indicador de origen.\r\n"
                        },
                        new Seccion {
                            id = 15,
                            titulo = "Google Maps",
                            contenido="La información y los materiales del Servicio LOLA, incluyendo textos, gráficos, información, enlaces u otros elementos, se proporcionan \"tal cual\" y \"según disponibilidad\". Las reseñas, perfiles, consejos, opiniones, declaraciones, ofertas u otra información o contenido disponible a través del Servicio LOLA, pero no directamente por LOLA, pertenecen a sus respectivos autores, quienes son los únicos responsables de dicho contenido. EN LA MÁXIMA MEDIDA PERMITIDA POR LA LEY APLICABLE, LOLA NO: (1) GARANTIZA LA EXACTITUD, ADECUACIÓN O INTEGRIDAD DE LA INFORMACIÓN Y LOS MATERIALES DEL SERVICIO LOLA; (2) ADOPTA, RESPALDA NI ACEPTA LA RESPONSABILIDAD POR LA EXACTITUD O FIABILIDAD DE CUALQUIER OPINIÓN, CONSEJO O DECLARACIÓN REALIZADA POR CUALQUIER TERCERA PARTE EXCEPTO LOLA; (3) GARANTIZA QUE SU USO DE LOS SERVICIOS SERÁ SEGURO, LIBRE DE VIRUS INFORMÁTICOS, ININTERRUMPIDO, SIEMPRE DISPONIBLE, SIN ERRORES O CUMPLIRÁ CON SUS REQUISITOS, O QUE CUALQUIER DEFECTO EN EL SERVICIO LOLA SERÁ CORREGIDO. EN LA MEDIDA EN QUE LO PERMITA LA LEY APLICABLE, LOLA RENUNCIA EXPRESAMENTE A TODAS LAS GARANTÍAS, YA SEAN EXPRESAS, IMPLÍCITAS O ESTATUTARIAS, CON RESPECTO AL SERVICIO LOLA, Y RENUNCIA ESPECÍFICAMENTE A TODAS LAS GARANTÍAS IMPLÍCITAS DE COMERCIABILIDAD, IDONEIDAD PARA UN PROPÓSITO PARTICULAR, NO INFRACCIÓN Y PRECISIÓN. ADEMÁS Y SIN LIMITAR LO ANTERIOR, NO HACEMOS DECLARACIONES NI GARANTÍAS DE NINGÚN TIPO, YA SEAN EXPRESAS O IMPLÍCITAS, CON RESPECTO A LA IDONEIDAD DE CUALQUIER PROVEEDOR DE SERVICIOS QUE OFREZCA SERVICIOS DE CUIDADO DE MASCOTAS A TRAVÉS DEL SERVICIO LOLA."
                        },
                        new Seccion {
                            id = 16,
                            titulo = "Uso del Servicio LOLA; Suspensión",
                            contenido="16.1 Exclusión de ciertos tipos de daños . En la medida máxima permitida por la legislación aplicable, LOLA no será responsable en ningún caso ante usted por daños indirectos, especiales, incidentales o consecuentes, incluidos gastos de viaje, ni por pérdidas comerciales, de beneficios, ingresos, contratos, datos, fondo de comercio u otras pérdidas o gastos similares que surjan o estén relacionados con el uso o la imposibilidad de usar el Servicio LOLA, incluidos, entre otros, los daños relacionados con la información recibida del Servicio LOLA, la eliminación de su información de perfil o reseña (u otro contenido) del Servicio LOLA, la suspensión o cancelación de su acceso al Servicio LOLA, o cualquier fallo, error, omisión, interrupción, defecto o retraso en el funcionamiento o la transmisión del Servicio LOLA, incluso si somos conscientes de la posibilidad de dichos daños, pérdidas o gastos. Algunas jurisdicciones no permiten la exclusión o limitación de responsabilidad por daños consecuentes o incidentales, por lo que la limitación anterior podría no ser aplicable en su caso.\r\n16.2 Límite de nuestra responsabilidad hacia usted . SALVO QUE LA LEY APLICABLE LO PROHÍBA, EN NINGÚN CASO LA RESPONSABILIDAD TOTAL DE LOLA HACIA USTED O CUALQUIER TERCERO, EN CUALQUIER ASUNTO DERIVADO O RELACIONADO CON EL SERVICIO DE LOLA O ESTOS TÉRMINOS, EXCEDERÁ LOS IMPORTES PAGADOS POR USTED A LOLA (EXCLUYENDO ESPECÍFICAMENTE LOS IMPORTES PAGADOS A LOS PROVEEDORES DE SERVICIOS) DURANTE LOS DOCE (12) MESES ANTERIORES AL EVENTO QUE GENERÓ LA RESPONSABILIDAD O, SI NO HA PAGADO A LOLA POR EL USO DE ALGUNO DE LOS SERVICIOS, LA CANTIDAD DE $100.00 USD (O SU EQUIVALENTE EN MONEDA LOCAL). (Cualquier obligación financiera que LOLA pueda tener con sus usuarios en relación con la conducta del usuario se limita a las obligaciones de reembolso establecidas en la GARANTIA LOLA ). SIN PERJUICIO DE LO ANTERIOR, CON RESPECTO A UN RECLAMO DE UN PROVEEDOR DE SERVICIOS UBICADO EN LOS ESTADOS UNIDOS POR FALTA DE ENTREGA DE PAGO DE LOS DUEÑOS DE MASCOTAS QUE LOLA RECIBE EN NOMBRE DE DICHO PROVEEDOR DE SERVICIOS DE CONFORMIDAD CON LA SECCIÓN 9.10, LA RESPONSABILIDAD DE LOLA NO EXCEDERÁ LA CANTIDAD QUE LOLA NO ENTREGÓ AL PROVEEDOR DE SERVICIOS DE CONFORMIDAD CON ESTOS TÉRMINOS.\r\n16.3 Sin responsabilidad por acciones que no sean de LOLA . HASTA EL GRADO MÁXIMO PERMITIDO POR LA LEY APLICABLE, EN NINGÚN CASO LOLA SERÁ RESPONSABLE POR DAÑOS DE NINGÚN TIPO, YA SEAN DIRECTOS, INDIRECTOS, GENERALES, ESPECIALES, COMPENSATORIOS Y/O CONSECUENTES, QUE SURJAN DE O ESTÉN RELACIONADOS CON LA CONDUCTA DE USTED O CUALQUIER OTRA PERSONA, INCLUYENDO SERVICIOS DE TERCEROS, EN CONEXIÓN CON EL SERVICIO LOLA, INCLUYENDO, SIN LIMITACIÓN, DAÑOS A LA PROPIEDAD, ROBO, LESIONES CORPORALES, MUERTE, ANGUSTIA EMOCIONAL Y/O CUALQUIER OTRO DAÑO RESULTANTE DE LA CONFIANZA EN LA INFORMACIÓN O CONTENIDO PUBLICADO O TRANSMITIDO A TRAVÉS DEL SERVICIO LOLA O SERVICIOS DE TERCEROS, O POR CUALQUIER INTERACCIÓN CON OTROS USUARIOS DEL SERVICIO LOLA, YA SEA EN LÍNEA O FUERA DE LÍNEA. ESTO INCLUYE CUALQUIER RECLAMO, PÉRDIDA O DAÑO QUE SURJA DE LA CONDUCTA DE LOS USUARIOS QUE INTENTEN DEFRAUDARLO O DAÑARLO.\r\nSI TIENE UNA DISPUTA CON UN PROVEEDOR DE SERVICIOS O EL DUEÑO DE UNA MASCOTA, ACEPTA EXONERAR A LOLA DE TODAS LAS RECLAMACIONES, DEMANDAS Y DAÑOS DE CUALQUIER TIPO, CONOCIDOS O DESCONOCIDOS, QUE SURJAN DE O ESTÉN RELACIONADOS CON DICHAS DISPUTAS, SALVO LO ESTABLECIDO ESPECÍFICAMENTE EN LA GARANTIA LOLA . EN NINGÚN CASO LOLA SERÁ RESPONSABLE DE LAS CONSECUENCIAS DIRECTAS O INDIRECTAS DEL INCUMPLIMIENTO DE LAS LEYES Y REGLAMENTOS APLICABLES POR PARTE DEL DUEÑO DE UNA MASCOTA O EL PROVEEDOR DE SERVICIOS.\r\n"
                        },
                        new Seccion {
                            id = 17,
                            titulo = "Acuerdo de arbitraje y renuncia a acción colectiva",
                            contenido="LEA ESTA SECCIÓN DETENIDAMENTE, YA QUE ESTABLECE CÓMO SE RESOLVERÁN LAS DISPUTAS ENTRE NOSOTROS. SI RESIDE EN EL ESPACIO ECONÓMICO COLOMBIANO O ESTADOUNIDENSE, ESTA SECCIÓN SE APLICA SOLO A CLIENTES EMPRESARIALES (NO A «CONSUMIDORES»). SI RESIDE EN EL ESPACIO ECONÓMICO EUROPEO O EL REINO UNIDO, CONSULTE LA SECCIÓN 18.\r\n17.1 Acuerdo de Arbitraje; Reclamaciones . Esta Sección 17 se denomina \"Acuerdo de Arbitraje\" en estos Términos. A menos que usted opte por no participar en el Acuerdo de Arbitraje de acuerdo con el procedimiento descrito en la Sección 17.9 a continuación, usted y LOLA (en conjunto, las \" Partes \") acuerdan que cualquier disputa o reclamación que surja entre usted y LOLA en relación con el Servicio LOLA, las interacciones con otros usuarios del Servicio LOLA o estos Términos (incluido cualquier supuesto incumplimiento de estos Términos) (en conjunto, las \" Reclamaciones \"), excepto las Reclamaciones Excluidas, se resolverá según lo establecido en este Acuerdo de Arbitraje. “Reclamaciones excluidas” significa (1) reclamaciones individuales presentadas en un tribunal de reclamos menores (si sus reclamaciones califican), (2) reclamaciones presentadas por LOLA que surjan de o estén relacionadas con una violación de la Sección 4.1 anterior, (3) reclamaciones en las que cualquiera de las partes busca una medida cautelar u otra medida equitativa por un presunto uso ilegal de propiedad intelectual (incluidos, entre otros, derechos de autor, marcas comerciales, nombres comerciales, logotipos, secretos comerciales o patentes) o una medida cautelar de emergencia basada en circunstancias exigentes (por ejemplo, peligro inminente o comisión de un delito, piratería informática, ciberataque), y (4) reclamaciones que un árbitro determina que no pueden someterse a arbitraje como se describe en la Sección 17.2 a continuación.\r\n17.2 Acuerdo de arbitraje . A menos que opte por no participar en el Acuerdo de arbitraje de acuerdo con el procedimiento descrito en la Sección 17.9 a continuación, usted y LOLA acuerdan que todas y cada una de las Reclamaciones (excepto las Reclamaciones excluidas) se resolverán exclusivamente de forma individual mediante un arbitraje final y vinculante, en lugar de en un tribunal (excepto según lo permitido específicamente en la Sección 17.11(d)-(e)), de conformidad con este Acuerdo de arbitraje, y sus derechos en relación con todas las Reclamaciones (excepto las Reclamaciones excluidas) serán determinados por un árbitro neutral, no por un juez o jurado . Usted y LOLA acuerdan que la Ley Federal de Arbitraje rige la interpretación y ejecución de este Acuerdo de arbitraje. En la medida en que la disputa de las Partes involucre tanto Reclamaciones excluidas presentadas a tiempo como otras Reclamaciones sujetas a este Acuerdo, las Partes acuerdan bifurcar y suspender durante la duración del procedimiento de arbitraje dichas Reclamaciones excluidas. Si el árbitro (o el tribunal, si lo exige la ley) determina definitivamente que la ley aplicable impide la ejecución del Acuerdo de Arbitraje con respecto a cualquier reclamo, causa de acción o recurso solicitado, entonces dicho reclamo, causa de acción o recurso solicitado se separará y se suspenderá en espera del arbitraje del reclamo, causa de acción o recurso solicitado restante.\r\n17.3 Prohibición de acciones colectivas y representativas, y reparación no individualizada . USTED Y LOLA ACUERDAN QUE, HASTA DONDE LA LEY LO PERMITA, CADA UNO DE NOSOTROS PODRÁ PRESENTAR RECLAMACIONES CONTRA EL OTRO SOLO DE FORMA INDIVIDUAL Y NO COMO DEMANDANTE O MIEMBRO DE UN GRUPO EN CUALQUIER SUPUESTA ACCIÓN O PROCEDIMIENTO COLECTIVO O REPRESENTATIVO, SALVO LO PERMITIDO ESPECÍFICAMENTE EN LA SECCIÓN 17.11(d)-(e). A MENOS QUE USTED Y LOLA ACUERDEN LO CONTRARIO, USTED ACEPTA QUE EL ÁRBITRO NO PODRÁ CONSOLIDAR NI UNIR LAS RECLAMACIONES DE MÁS DE UNA PERSONA O PARTE, Y NO PODRÁ PRESIDIR DE OTRA MANERA NINGUNA FORMA DE PROCEDIMIENTO CONSOLIDADO, REPRESENTATIVO O COLECTIVO. ADEMÁS, EL ÁRBITRO O EL TRIBUNAL PODRÁN CONCEDER UNA COMPENSACIÓN (INCLUYENDO UNA COMPENSACIÓN MONETARIA, CAUTELARES Y DECLARATORIAS) SOLO A FAVOR DE LA PARTE QUE LA SOLICITE Y SOLO EN LA MEDIDA NECESARIA PARA PROPORCIONAR LA COMPENSACIÓN REQUERIDA POR SU(S) RECLAMACIÓN(ES) INDIVIDUAL(ES). CUALQUIER COMPENSACIÓN CONCEDIDA NO PODRÁ AFECTAR A OTROS USUARIOS DEL SERVICIO LOLA. Sin perjuicio de cualquier otra disposición de este Acuerdo de Arbitraje o de las Reglas (según se define en la Sección 17.5), las disputas relativas a la interpretación, aplicabilidad o cumplimiento de esta Sección 17.3 solo podrán ser resueltas por un tribunal y no por un árbitro. En cualquier caso en el que (a) la disputa se presente como una acción colectiva, de clase o representativa y (b) haya una determinación judicial final de que esta Sección 17.3 no es ejecutable con respecto a cualquier reclamo o cualquier remedio particular para un reclamo (como una solicitud de medida cautelar pública), entonces: (i) ese reclamo o remedio particular (y solo ese reclamo o remedio particular) se separará de cualquier reclamo y/o remedio restante y se suspenderá; (ii) esta Sección 17.3 se hará cumplir en arbitraje de forma individual en cuanto a todos los reclamos o remedios restantes en la mayor medida posible; y (iii) los reclamos y/o remedios suspendidos se pueden presentar en un tribunal de jurisdicción competente después de que el árbitro haya resuelto todos los reclamos restantes.\r\n17.4 Resolución de Disputas Previa al Arbitraje . Nuestra preferencia siempre será resolver las quejas de forma amistosa y eficiente, sin necesidad de arbitraje. Antes de iniciar el arbitraje, primero debe contactarnos por escrito para explicar su queja a través de su oficina local de LOLA que se indica a continuación y darnos la oportunidad de trabajar con usted para resolverla. Usted o su representante legal, si tiene uno, puede contactarnos por correo a la Calle 2b # 81ª 460, Medellín, Su queja por escrito debe incluir su nombre, la dirección de correo electrónico asociada a su cuenta, una descripción detallada de la naturaleza y el fundamento de la disputa, y la reparación específica que solicita. Su queja por escrito debe ser individual y estar firmada personalmente por usted. Para cualquier disputa que LOLA inicie, un representante de LOLA firmará personalmente una queja por escrito y la enviará a la dirección de correo electrónico asociada a su cuenta. Si el problema no se resuelve dentro de los 30 días siguientes a la recepción de la queja por escrito, cualquiera de las partes podrá iniciar un arbitraje según lo especificado en la Sección 17.5 “Procedimientos de arbitraje” o la Sección 17.11 “Arbitrajes de referencia”, según corresponda. \r\n17.5 Procedimientos de arbitraje .  Según se utiliza en esta Sección 17, las reglas de arbitraje aplicables que se describen a continuación se denominan colectivamente las « Reglas ».\r\n•\tDisputas en EE. UU.   En el caso de disputas que surjan en EE. UU., el arbitraje será llevado a cabo por un árbitro neutral de conformidad con las Reglas y Procedimientos de Arbitraje Simplificado de JAMS (modificadas por los Estándares Mínimos de Arbitraje del Consumidor de JAMS en casos de arbitrajes de consumo) (las \" Reglas de JAMS \") vigentes al momento de la presentación de la reclamación, disponibles en https://www.jamsadr.com/rules-streamlined-arbitration y https://www.jamsadr.com/consumer-minimum-standards/ , respectivamente. \r\n•\tDisputas fuera de EE. UU.   En el caso de disputas fuera de EE. UU., el arbitraje se llevará a cabo en inglés o español por un árbitro neutral, de conformidad con las Reglas aplicables del Centro Internacional para la Resolución de Disputas (CIRD) vigentes al momento de la presentación de la reclamación, disponibles en  https://www.icdr.org/icdrcanada o https://www.icdr.org/rules_forms_fees . \r\nEn todos los casos, el árbitro decidirá sobre el fondo de todas las reclamaciones de conformidad con la ley, tal como se especifica en la Sección 18, a continuación, incluyendo los principios reconocidos de equidad, y respetará todas las reclamaciones de privilegio reconocidas por la ley. El árbitro desestimará la demanda de arbitraje basándose en los alegatos si no contiene suficientes elementos de hecho para fundamentar una reclamación de reparación que sea plausible a primera vista. Toda demanda de arbitraje debe estar firmada personalmente por usted o por un representante de LOLA. El arbitraje se llevará a cabo en el condado donde usted reside o en otra ubicación acordada mutuamente. \r\nTodas las Reglas, según las modificaciones de este Acuerdo de Arbitraje, se incorporan a estos Términos por referencia. Usted reconoce y acepta haber leído y comprendido las Reglas o renuncia a su derecho a leerlas y a cualquier reclamación que considere injusta o inaplicable por cualquier motivo. En caso de inconsistencia entre las Reglas y este Acuerdo de Arbitraje, prevalecerán los términos de este Acuerdo de Arbitraje, a menos que el árbitro determine que la aplicación de los términos incompatibles del Acuerdo de Arbitraje no daría lugar a un arbitraje fundamentalmente justo. El árbitro también debe cumplir las disposiciones de estos Términos como lo haría un tribunal, incluyendo, entre otras, las disposiciones sobre limitación de responsabilidad de la Sección 16. Si bien los procedimientos de arbitraje suelen ser más sencillos y ágiles que los juicios y otros procedimientos judiciales, el árbitro puede conceder la misma indemnización por daños y perjuicios a título individual que un tribunal puede conceder a una persona en virtud de los Términos y la legislación aplicable. El árbitro presentará una decisión por escrito, que incluirá una declaración concisa de las conclusiones esenciales en las que se basa el laudo. Las decisiones del árbitro son vinculantes y ejecutables ante el tribunal y sólo pueden ser revocadas por éste por razones muy limitadas.\r\nEl árbitro no estará sujeto a las decisiones de arbitrajes anteriores que involucren a diferentes usuarios de LOLA, pero sí estará sujeto a las decisiones de arbitrajes anteriores que involucren al mismo usuario de LOLA en la medida en que lo exija la legislación aplicable. Según lo dispuesto por la legislación aplicable, estos Términos y las Reglas aplicables, el árbitro tendrá (1) la autoridad y jurisdicción exclusivas para tomar todas las decisiones procesales y sustantivas con respecto a una Reclamación, incluyendo la determinación de si una Reclamación es arbitrable, y (2) la autoridad para otorgar cualquier recurso que, de otro modo, estaría disponible en los tribunales.\r\n17.6 Costos del Arbitraje . El pago de todos los honorarios de presentación, administración, gestión del caso y del árbitro (en conjunto, los \" Honorarios de Arbitraje \") se regirá por las Reglas aplicables.   Cada parte será la única responsable de todos los demás honorarios en que incurra en relación con el arbitraje, incluyendo, entre otros, los honorarios de los abogados. Al finalizar cualquier arbitraje, el árbitro podrá conceder honorarios y costos de abogado razonables, o cualquier parte de estos, a cualquiera de las partes si determina que la demanda, la reconvención o la defensa son frívolas o se presentaron con un propósito indebido (según las normas de la Regla Federal de Procedimiento Civil 11(b)), en la medida autorizada por la ley aplicable.\r\n17.7 Confidencialidad . Todos los aspectos del procedimiento arbitral, así como cualquier decisión, laudo o laudo arbitral, serán estrictamente confidenciales para beneficio de todas las Partes.\r\n17.8 Divisibilidad . Si algún término, cláusula o disposición de esta Sección 17 se declara inválido o inaplicable, se considerará así en la medida mínima exigida por la ley, y todos los demás términos, cláusulas y disposiciones de esta Sección 17 seguirán siendo válidos y aplicables. Sin embargo, si la determinación de invalidez o inaplicabilidad parcial resulta en una determinación definitiva de que la renuncia a la acción colectiva establecida en la Sección 17.3 no prospera con respecto a todas las reclamaciones en arbitraje, la Sección 17 será inaplicable en su totalidad. \r\n17.9 Procedimiento de Exclusión . Puede optar por rechazar su acuerdo de arbitraje (Sección 17.2) y su renuncia al derecho a interponer o participar en demandas colectivas o representativas (Sección 17.3) enviándonos por correo un aviso de exclusión por escrito (\" Aviso de Exclusión \"), de acuerdo con los términos de esta Sección 17.9. El Aviso de Exclusión debe tener matasellos con fecha no posterior a 30 días posteriores a la fecha en que aceptó estos Términos por primera vez. Debe enviar el Aviso de Exclusión por correo la Calle 2b # 81ª 460 Medellín. El Aviso de Exclusión debe indicar que no acepta el Acuerdo de Arbitraje ni la renuncia a demandas colectivas, e incluir su nombre, dirección, número de teléfono y la(s) dirección(es) de correo electrónico utilizadas para registrarse en el Servicio LOLA al que se aplica la exclusión. Debe firmar personalmente el Aviso de Exclusión para que sea efectivo. Este procedimiento es la única forma de renunciar al Acuerdo de Arbitraje y a la renuncia a demandas colectivas. Si renuncia al acuerdo de arbitraje y a la renuncia al derecho a participar en demandas colectivas y representativas, ninguna de las disposiciones de arbitraje le será aplicable ni renunciará a su derecho a participar en demandas colectivas o representativas, pero el resto de estos Términos seguirá siendo aplicable. Renunciar a este Acuerdo de Arbitraje no afecta a ningún acuerdo de arbitraje previo, posterior o futuro que pueda tener con nosotros.  Las modificaciones a este Acuerdo de Arbitraje no le ofrecen una nueva oportunidad de renunciar al Acuerdo de Arbitraje si ya aceptó una versión de este Acuerdo de Arbitraje y no se retiró válidamente. Al renunciar al arbitraje vinculante, acepta resolver las Reclamaciones (incluidas las Reclamaciones Excluidas) de conformidad con la Sección 18.\r\n17.10 Cambios Futuros a este Acuerdo de Arbitraje . Sin perjuicio de cualquier disposición contraria en estos Términos, usted acepta que, si en el futuro realizamos algún cambio en este Acuerdo de Arbitraje (excepto un cambio en la dirección de notificación o el enlace al sitio web proporcionado en el presente), dicho cambio no se aplicará a ninguna reclamación presentada en un procedimiento legal contra LOLA antes de la fecha de entrada en vigor del cambio. Asimismo, si rescindimos este Acuerdo de Arbitraje eliminándolo de estos Términos, dicha rescisión no será efectiva hasta 30 días después de la publicación en el Sitio de la versión de estos Términos que no contenga el Acuerdo de Arbitraje, y no será efectiva respecto de ninguna reclamación presentada en un procedimiento legal contra LOLA antes de la fecha de entrada en vigor de la eliminación.\r\n17.11 Arbitrajes de Referencia . En caso de controversias que surjan en Estados Unidos, si, con poca diferencia de tiempo, se presentan 20 o más demandas de arbitraje de naturaleza similar contra o en nombre de las mismas partes o de partes relacionadas, se aplicarán los siguientes procedimientos:\r\na.\tLas demandas deben presentarse ante JAMS de forma individual y, de lo contrario, cumplir con todos los requisitos de presentación aplicables en este Acuerdo de Arbitraje, incluidos los requisitos de resolución previa a la disputa y de firma personal en las Secciones 17.4 y 17.5.\r\nb.\tLos demandantes seleccionarán conjuntamente cinco demandas y los demandados seleccionarán conjuntamente cinco demandas, para un total de diez, que se considerarán los \"Arbitrajes de Referencia\". Estos Arbitrajes de Referencia se arbitrarán individualmente, según lo dispuesto en este Acuerdo de Arbitraje. Las partes colaborarán de buena fe con los árbitros para completar cada Arbitraje de Referencia dentro de los 120 días siguientes a su primera audiencia preliminar.\r\nc.\tLas demandas restantes no seleccionadas como Arbitrajes Bellwether no serán arbitradas (ni litigadas en ninguna jurisdicción), salvo lo dispuesto en la Sección 17.11(e). JAMS suspenderá administrativamente de inmediato dichos asuntos (o, alternativamente, dará por concluidos los procedimientos), y ninguna de las partes será responsable del pago de tasas administrativas o de presentación relacionadas con la demanda hasta que se levante la suspensión o se restablezca la demanda conforme a la Sección 17.11(e). Los plazos de prescripción aplicables a una demanda sujeta a este párrafo se suspenderán a partir de la fecha de presentación de la demanda.\r\nd.\tDentro de los 60 días siguientes a la finalización de los Arbitrajes Bellwether (salvo que las partes acuerden un plazo mayor), todas las partes participarán de buena fe en una mediación global no vinculante para todas las demandas pendientes. Salvo que las partes acuerden un mediador, este será designado por JAMS y tendrá experiencia en el objeto de las disputas. LOLA pagará los honorarios del mediador. Sin perjuicio de lo dispuesto en la Sección 17.7, se podrán proporcionar al mediador los resultados de los Arbitrajes Bellwether para facilitar la resolución de todas o algunas de las demandas pendientes. Los abogados de todas las partes deberán presentar de inmediato a sus clientes cualquier oferta de acuerdo final resultante de la mediación.\r\ne.\tSi las partes no logran resolver la totalidad o parte de las demandas restantes dentro de los 90 días siguientes a la finalización de los Arbitrajes Bellwether (a menos que las partes acuerden un plazo mayor), JAMS levantará las suspensiones administrativas (o restablecerá los procedimientos concluidos) únicamente para las demandas no resueltas, y estas se tramitarán en arbitraje individual según lo dispuesto en este Acuerdo de Arbitraje. Sin embargo, cualquiera de las partes en una demanda no resuelta podrá, previa notificación razonable a la parte contraria, renunciar al proceso de arbitraje y proceder judicialmente respecto de dicha demanda. Cualquier demanda no resuelta que proceda judicialmente en virtud de este párrafo podrá hacerlo en procedimientos colectivos o representativos (sujeto a la legislación aplicable), sin perjuicio de cualquier disposición contraria en este Acuerdo de Arbitraje. Para evitar cualquier duda, el derecho a proceder judicialmente en virtud de la oración anterior se aplica únicamente a las demandas no resueltas que se presentaron y se sometieron a los protocolos establecidos en esta Sección 17.11.\r\nf.\tLas partes acuerdan que los protocolos establecidos en esta Sección 17.11 están diseñados para lograr un mecanismo general más rápido, más eficiente y menos costoso para resolver una gran cantidad de demandas de arbitraje similares, incluidas las demandas de demandantes que no son seleccionados para un Arbitraje Bellwether.\r\ng.\tLas partes podrán solicitar ayuda de un tribunal de jurisdicción competente para hacer cumplir esta Sección 17.11.\r\n"

                        },
                        new Seccion {
                            id = 18, 
                            titulo = "Privacidad",
                            contenido="18.1 Para los usuarios de Estados Unidos y Canadá, estos Términos y cualquier disputa entre usted y LOLA se regirán por las leyes del Estado de Washington y la legislación federal estadounidense aplicable, sin perjuicio de los principios de conflicto de leyes, salvo que la Ley Federal de Arbitraje rija la interpretación y aplicación de la Sección 17 (el Acuerdo de Arbitraje). Salvo que usted y nosotros acordemos lo contrario, o salvo que lo prohíba la legislación aplicable, en caso de que el Acuerdo de Arbitraje no se aplique a usted o a una reclamación o disputa en particular, usted acepta que cualquier reclamación o disputa que surja entre usted y LOLA deberá resolverse exclusivamente en un tribunal estatal o federal ubicado en el Estado de Washington, y usted y LOLA acuerdan someterse a la jurisdicción personal de los tribunales ubicados en Seattle, Washington, para litigar dichas reclamaciones o disputas. \r\n18.2 Para los usuarios del EEE y del Reino Unido, las leyes de Inglaterra regirán estos Términos. Si reside en un país fuera de Inglaterra, pero dentro del EEE, se aplicarán ciertas leyes obligatorias de su país para su beneficio y protección, además de o en lugar de ciertas disposiciones de la legislación inglesa. Cualquier disputa que surja entre usted y LOLA deberá resolverse en los tribunales ingleses o, si reside en el EEE fuera de Inglaterra, en los tribunales de su país.\r\n18.3 Si usted es un consumidor en el Espacio Económico Europeo, la plataforma europea de resolución de disputas en línea http://ec.europa.eu/consumers/odr proporciona información sobre la resolución alternativa de disputas, que puede utilizar si existe una disputa que no puede resolverse entre usted y la parte relevante.\r\n"
                        },
                        new Seccion {
                            id = 19,
                            titulo = "Renuncia a demanda colectiva",
                            contenido="USTED ACEPTA QUE, HASTA DONDE LO PERMITA LA LEY APLICABLE, CADA UNO DE NOSOTROS PODRÁ PRESENTAR RECLAMACIONES CONTRA EL OTRO SOLO DE FORMA INDIVIDUAL Y NO COMO DEMANDANTE O MIEMBRO DE UN GRUPO EN NINGUNA SUPUESTA ACCIÓN O PROCEDIMIENTO COLECTIVO O REPRESENTATIVO. A MENOS QUE USTED Y LOLA ACUERDEN LO CONTRARIO, EL TRIBUNAL NO PODRÁ CONSOLIDAR NI UNIR LAS RECLAMACIONES DE MÁS DE UNA PERSONA O PARTE, NI PODRÁ PRESIDIR NINGUNA FORMA DE PROCEDIMIENTO CONSOLIDADO, REPRESENTATIVO O COLECTIVO. ADEMÁS, EL TRIBUNAL PODRÁ OTORGAR UNA COMPENSACIÓN (INCLUYENDO UNA COMPENSACIÓN MONETARIA, POR CAUTELA Y DECLARATORIA) SOLO A FAVOR DE LA PARTE QUE LA SOLICITE Y SOLO EN LA MEDIDA NECESARIA PARA PROPORCIONAR LA COMPENSACIÓN QUE REQUIERE SU(S) RECLAMACIÓN(ES) INDIVIDUAL(ES). CUALQUIER COMPENSACIÓN OTORGADA NO PODRÁ AFECTAR A OTROS USUARIOS DEL SERVICIO MÓVIL."                        },
                        new Seccion {
                            id = 20,
                            titulo = "Fuerza mayor.",
                            contenido="LOLA no será responsable de ningún retraso o incumplimiento derivado de causas ajenas a su control razonable, incluyendo, entre otras, casos fortuitos, desastres naturales, terremotos, huracanes, incendios forestales, inundaciones, guerras, terrorismo, disturbios, embargos, incendios, accidentes, pandemias, enfermedades, huelgas u otros desastres similares. Asimismo, en tal caso, las políticas de cancelación descritas en la Sección 9.6 podrían no ser aplicables y LOLA podrá, a su discreción razonable, emitir reembolsos con condiciones diferentes a las de la política de cancelación seleccionada por el Proveedor de Servicios."                        },
                        new Seccion {
                            id = 21,
                            titulo = "Varios",
                            contenido="Nada de lo dispuesto en estos Términos se interpretará como que una de las partes sea socia, empresa conjunta, agente, representante legal, empleador, trabajador o empleado de la otra. Ninguna de las partes tendrá, ni se presentará ante terceros como tal, autoridad alguna para realizar declaraciones, representaciones o compromisos de ningún tipo, ni para tomar ninguna medida vinculante para la otra, salvo lo dispuesto en el presente documento o autorizado por escrito por la parte obligada. Estos Términos no son exclusivos y no prohíben a los Proveedores de Servicios ofrecer servicios de cuidado de mascotas a través de otros medios o a través de terceros. La invalidez, ilegalidad o inaplicabilidad de cualquier término o disposición de estos Términos no afectará en modo alguno la validez, legalidad o aplicabilidad de cualquier otro término o disposición de estos Términos. En caso de que un término o disposición se determine inválido o inaplicable, las partes acuerdan sustituirlo por uno que sea válido y aplicable y que se acerque más a la expresión de la disposición inválida o inaplicable, y estos Términos serán aplicables con sus modificaciones. En la medida máxima permitida por la legislación local aplicable, este Acuerdo será vinculante y redundará en beneficio de los representantes legales, sucesores y cesionarios de las partes. Estos Términos seguirán vigentes incluso después de la finalización de su relación con LOLA.\r\nSi tiene preguntas o inquietudes sobre el Servicio LOLA o estos Términos, comuníquese con su oficina local: Calle 2b # 81ª 46 Medellín, Tel. 3012048490, lolacuidatumascota@gmail.com\r\n\r\n"                        },
                        new Seccion {
                            id = 22,
                            titulo = "Registro; Seguridad de la cuenta",
                            contenido = "Para usar algunos aspectos del Servicio LOLA, deberá crear un nombre de usuario, una contraseña y un perfil de usuario. Si decide usar el Servicio LOLA, se compromete a proporcionar información precisa sobre usted y a mantenerla actualizada. Se compromete a no suplantar la identidad de otra persona ni a mantener más de una cuenta."
                        },
                        new Seccion {
                            id = 23,
                            titulo = "Propiedad intelectual",
                            contenido = "LOLA y sus licenciantes conservan todos los derechos, títulos e intereses sobre el Servicio LOLA, la tecnología y el software utilizados para proporcionarlo, toda la documentación y el contenido electrónicos disponibles a través del Servicio LOLA, y todos los derechos de propiedad intelectual y de propiedad sobre el Servicio LOLA y dicha tecnología, software, documentación y contenido."
                        },
                        new Seccion {
                            id = 24,
                            titulo = "Limitación de responsabilidad",
                            contenido = "En la medida máxima permitida por la legislación aplicable, LOLA no será responsable en ningún caso ante usted por daños indirectos, especiales, incidentales o consecuentes, incluidos gastos de viaje, ni por pérdidas comerciales, de beneficios, ingresos, contratos, datos, fondo de comercio u otras pérdidas o gastos similares que surjan o estén relacionados con el uso o la imposibilidad de usar el Servicio LOLA."
                        },
                      
                    }
                },
                terminos_servicio_completos = new TerminosServicioCompletos
                {
                    titulo = "Condiciones de servicio",
                    vigencia = "27 de marzo de 2025",
                    contenido_completo = @"Condiciones de servicio
Vigente a partir del 27 de marzo de 2025
LEA ESTOS TÉRMINOS DETENIDAMENTE. LA SECCIÓN 17 INCLUYE UNA CLÁUSULA DE ARBITRAJE
VINCULANTE Y UNA RENUNCIA A DEMANDAS COLECTIVAS, Y LA SECCIÓN 19 INCLUYE UNA RENUNCIA
A DEMANDAS COLECTIVAS, LO CUAL AFECTA LA RESOLUCIÓN DE DISPUTAS.
1. Aceptación de los Términos; Modificaciones.
Estos Términos de Servicio (los ""Términos"") constituyen un acuerdo legal vinculante entre usted y A
LOLA CUIDA TU MASCOTA S.A.S, una empresa constituida bajo las leyes de colombianas con domicilio en
Calle 2b # 81ª 460 Medellín, (""LOLA"", ""nosotros"", ""nos"" y ""nuestro""). Los Términos rigen el uso que
usted haga de nuestras aplicaciones de software, recursos y servicios para que los dueños de mascotas y
los proveedores de servicios para mascotas se encuentren, se comuniquen y organicen la prestación de
servicios de cuidado de mascotas, y cualquier otro servicio o producto que podamos ofrecer
ocasionalmente (colectivamente, nuestro ""Servicio LOLA""). Los Términos rigen todo uso del Servicio
Lola, ya sea que acceda a él desde nuestro sitio web en https://www.lolacuidatumascota.com (incluido
cualquier subdominio o versión localizada) (el ""Sitio""), nuestras aplicaciones móviles y sitios web
móviles, nuestra aplicación de Facebook, nuestras ofertas de soporte en línea o telefónico, o cualquier
otro punto de acceso que pongamos a su disposición. Nuestros TERMINOS DE GARANTIA LOLA, LA
POLITICA DE PROTECCION DE RESERVAS y otras Políticas aplicables a su uso del Servicio Lola se
incorporan por referencia a estos Términos de Servicio. AL ACEPTAR ESTOS TÉRMINOS DURANTE EL
PROCESO DE REGISTRO DE CUENTA O AL ACCEDER O USAR EL SERVICIO LOLA SIN UNA CUENTA, USTED
ACEPTA ESTOS TÉRMINOS. SI NO ESTÁ DE ACUERDO CON ESTOS TÉRMINOS, NO DEBE ACEPTARLOS, EN
CUYO CASO NO TENDRÁ DERECHO A USAR EL SERVICIO LOLA.
Usted comprende y acepta que podemos modificar los Términos ocasionalmente, y que dichos cambios
entrarán en vigor cuando publiquemos los Términos modificados en el Servicio LOLA, a menos que la
legislación aplicable exija lo contrario. Su acceso y uso continuado del Servicio LOLA después de la
publicación de los Términos modificados constituirá su consentimiento a quedar sujeto a los Términos
modificados.
2. Servicio LOLA.
2.1 Naturaleza del Servicio LOLA. El Servicio LOLA consta de una aplicación web de escritorio,
aplicaciones móviles y otras herramientas, soporte y servicios relacionados que los dueños de mascotas
(«Dueños de Mascotas») y los proveedores de servicios relacionados con mascotas («Proveedores de
Servicios») pueden utilizar para encontrarse, comunicarse e interactuar entre sí. El Servicio LOLA
incluye nuestros servicios de asistencia en caso de emergencia, materiales educativos para proveedores
de servicios y otros servicios. Cobramos por algunos aspectos del Servicio LOLA, como se describe más
adelante en la Sección 9.
2.2 LOLA no ofrece Servicios de Cuidado de Mascotas. LOLA es un foro neutral para Proveedores de
Servicios y Dueños de Mascotas. LOLA no es un Proveedor de Servicios y, salvo la asistencia telefónica de
emergencia y otros recursos y asistencia descritos específicamente en el Servicio LOLA, no ofrece
servicios de cuidado de mascotas. No realizamos declaraciones ni garantías sobre la calidad del
alojamiento, cuidado de mascotas, paseo de perros, cuidado de casas u otros servicios prestados por los
Proveedores de Servicios (""Servicios de Cuidado de Mascotas""), ni sobre sus interacciones y tratos con
los usuarios. Los Proveedores de Servicios que aparecen en LOLA no están bajo la dirección ni el control
de LOLA, y estos determinan a su propia discreción cómo prestar los Servicios de Cuidado de Mascotas.
Si bien en nuestro Sitio proporcionamos orientación general a los Proveedores de Servicios sobre
seguridad y cuidado de mascotas, y a los Dueños de Mascotas sobre la selección y contratación de
Proveedores de Servicios, LOLA no emplea, recomienda ni avala a los Proveedores de Servicios ni a los
Dueños de Mascotas y, en la medida máxima permitida por la legislación aplicable, no seremos
responsables del desempeño o la conducta de los Proveedores de Servicios ni de los Dueños de
Mascotas, ya sea en línea o fuera de línea. Realizamos una revisión inicial de los perfiles de los
Proveedores de Servicios y, según lo permita la ley, facilitamos Verificaciones de Antecedentes o de
Identidad (cada una de ellas como se describe en la Sección 10, más adelante) realizadas por un tercero.
Sin embargo, salvo que se especifique explícitamente en el Servicio LOLA (y solo en la medida
especificada), no realizamos evaluaciones adicionales de los Proveedores de Servicios ni de los Dueños
de Mascotas. Le recomendamos tener precaución y usar su criterio independiente antes de contratar a
un Proveedor de Servicios, prestar Servicios de Cuidado de Mascotas o interactuar con usuarios a través
del Servicio LOLA. Los Dueños de Mascotas y los Proveedores de Servicios son los únicos responsables
de tomar decisiones que beneficien su propio bienestar y el de sus mascotas. Por ejemplo, cada usuario
del Servicio LOLA es responsable de mantener al día las vacunas de su mascota, y no nos
responsabilizamos por el incumplimiento de las vacunas de ninguna persona.
2.3 Exención. De conformidad con la Sección 16 a continuación, LOLA no se responsabiliza de ninguna
reclamación, lesión, pérdida, daño o perjuicio derivado o relacionado de cualquier manera con sus
interacciones o tratos con otros usuarios, ni con las acciones u omisiones de los Proveedores de
Servicios y Dueños de Mascotas, ya sea en línea o fuera de línea. Usted reconoce y acepta que, en la
medida máxima permitida por la legislación aplicable, SU USO O PRESTACIÓN DE SERVICIOS DE
CUIDADO DE MASCOTAS ES BAJO SU PROPIO Y EXCLUSIVO RIESGO. (Cualquier obligación financiera
que LOLA pueda tener con sus usuarios en relación con su conducta se limita a las obligaciones de
reembolso establecidas en la GARANTIA LOLA).
2.4 Las transacciones se realizan entre los dueños de mascotas y los proveedores de servicios. El
Servicio LOLA puede utilizarse para buscar y ofrecer servicios de cuidado de mascotas y para facilitar el
pago, pero todas las transacciones realizadas a través del Servicio LOLA se realizan entre los dueños de
mascotas y los proveedores de servicios. Salvo los reembolsos limitados y la ""Protección de la reserva""
especificados en la Sección 9.6 y la GARANTIA LOLA, usted acepta que LOLA no se responsabiliza por los
daños asociados con los servicios de cuidado de mascotas (que pueden incluir lesiones corporales o la
muerte de una mascota) ni por cualquier otra transacción entre usuarios del Servicio LOLA.
2.5 Reservas. Los dueños de mascotas y los proveedores de servicios realizan transacciones entre sí en
el Servicio LOLA cuando ambos acuerdan una ""reserva"" que especifica las tarifas, el período de tiempo,
la política de cancelación y otros términos para la prestación de los servicios de cuidado de mascotas a
través del mecanismo de reserva proporcionado en el Servicio LOLA (una ""Reserva""). Una reserva
puede ser iniciada por un proveedor de servicios o un dueño de mascota seleccionando el tipo de
servicios de cuidado de mascotas que se proporcionarán y luego siguiendo las instrucciones que
aparecen en pantalla. Si usted es dueño de mascota e inicia una reserva, acepta pagar por los servicios
de cuidado de mascotas descritos en la reserva cuando haga clic en ""Solicitar reserva"". Si usted es dueño
de mascota y un proveedor de servicios inicia una reserva, acepta pagar por los servicios de cuidado de
mascotas descritos en la reserva cuando haga clic en ""Pagar ahora"". Todas las solicitudes están sujetas a
la aceptación de la parte receptora. La parte receptora no está obligada a aceptar su (ni ninguna)
solicitud y puede, a su discreción, rechazarla por cualquier motivo. Una vez que complete una reserva,
usted acepta respetar el precio y otros términos de esa reserva, como se reconoce en la confirmación de
la reserva.
2.6 Los dueños de mascotas son los únicos responsables de evaluar a los proveedores de servicios. Los
dueños de mascotas son los únicos responsables de evaluar la idoneidad de los proveedores de servicios
para los servicios que ofrecen. Si bien LOLA realiza una revisión limitada de los perfiles de los
proveedores de servicios y facilita la verificación de antecedentes o identidad de estos por parte de
terceros, dicha revisión es limitada y LOLA no garantiza que sea precisa, completa, concluyente ni esté
actualizada. Asimismo, LOLA no avala las reseñas de proveedores de servicios realizadas por otros
dueños de mascotas que puedan estar disponibles a través del Servicio LOLA, y no se compromete a que
dichas reseñas sean precisas o legítimas.
2.7 Mascotas Abandonadas; Reubicación. Los dueños de mascotas que contraten Servicios de Cuidado
de Mascotas y no recuperen a su mascota después del período de servicio indicado en una Reserva
aceptan que LOLA (o el Proveedor de Servicios) podrá, a su entera discreción, colocar a la mascota en un
hogar de acogida, transferir su cuidado a control animal u otras autoridades policiales, o buscar un
cuidado alternativo. El dueño de la mascota se compromete a reembolsar a LOLA y/o al Proveedor de
Servicios todos los costos y gastos asociados con dichas acciones. Además, LOLA se reserva
expresamente el derecho, a su entera discreción, de retirar la mascota del cuidado del Proveedor de
Servicios si LOLA lo considera necesario para la seguridad de la mascota, del Proveedor de Servicios o de
cualquier persona que conviva con él. Antes de retirar una mascota del cuidado del Proveedor de
Servicios, LOLA hará todo lo razonablemente posible durante su horario laboral para contactar al dueño
de la mascota y/o a su contacto de emergencia (si se proporciona) para organizar un cuidado
alternativo. Si LOLA no logra contactar al dueño de la mascota o al contacto de emergencia, hará todo lo
posible para encontrar un cuidado alternativo hasta que el dueño pueda recuperarla. Si usted es dueño
de una mascota, autoriza a su(s) veterinario(s) a compartir su historial veterinario con LOLA en relación
con cualquier reubicación o realojamiento. Además, usted es responsable y acepta pagar todos los
costos y gastos incurridos por LOLA en relación con dicho traslado.
2.8 Emergencias. Recomendamos a los Dueños de Mascotas que proporcionen a sus Proveedores de
Servicios la información de contacto para que puedan ser localizados en caso de que sea necesaria la
atención médica para una mascota, y que proporcionen un contacto de emergencia en el perfil del
Dueño de la Mascota que haya consentido la divulgación de su información. Los Proveedores de
Servicios se comprometen a contactar inmediatamente a los Dueños de Mascotas en caso de que dicha
atención sea necesaria o, si el Dueño de la Mascota no está disponible, a contactar a LOLA al número de
teléfono o dirección de correo electrónico correspondiente que figura en la tabla al final de estos
Términos. Si usted es Dueño de una Mascota, por la presente autoriza a su Proveedor de Servicios, a su
contacto de emergencia y/o a LOLA a obtener y autorizar la prestación de atención veterinaria para su
mascota si no puede ser localizado para autorizar la atención usted mismo en una situación de
emergencia. En tal caso, también autoriza al/los veterinario(s) de su mascota a divulgar el historial
veterinario de su mascota a LOLA y a su Proveedor de Servicios. Usted exime al Proveedor de Servicios y
a LOLA de cualquier lesión, daño o responsabilidad derivada de la prestación de atención de emergencia
o de la falta de solicitud de dicha atención conforme a esta sección, incluyendo el reembolso que de
otro modo podría haber estado disponible bajo la GARANTIA LOLA. Los dueños de mascotas son
responsables de los costos de cualquier tratamiento médico para sus mascotas y, si usted es dueño de
una mascota, por la presente autoriza a LOLA a cargar dichos costos a su tarjeta de crédito u otro
método de pago. En ciertas circunstancias, un dueño de mascota puede tener derecho a un reembolso
bajo la GARANTIA LOLA. LOLA recomienda que todos los usuarios cuenten con un seguro para mascotas
adecuado para cubrir los costos de la atención veterinaria, adicional a cualquier otro seguro que LOLA
pueda brindar o promocionar.
2.9 Servicios de Consulta. LOLA puede ofrecer a los Dueños de Mascotas y Proveedores de Servicios
servicios de consulta veterinaria de terceros por teléfono, chat o correo electrónico para brindarles un
recurso educativo que les ayude a tomar decisiones sobre sus mascotas o las mascotas bajo su cuidado.
Estos servicios de consulta son proporcionados por un tercero y no forman parte del Servicio LOLA. Si
utiliza estos servicios de consulta de terceros, debe usarlos únicamente junto con la atención veterinaria
profesional, y no como sustitutos de esta. Usted acepta recurrir únicamente al servicio de consulta de
terceros correspondiente en caso de cualquier reclamación derivada de sus servicios.
2.10 Google Maps. El uso del Servicio LOLA requiere el uso de las funciones y el contenido de Google
Maps, que están sujetos a las (1) Condiciones de Servicio Adicionales de Google Maps/Google Earth
vigentes, disponibles en https://maps.google.com/help/terms_maps.html (incluida la Política de Uso
Aceptable en https://cloud.google.com/maps-platform/terms/aup/); y a la (2) Política de Privacidad de
Google, disponible en https://www.google.com/policies/privacy/ (en conjunto, las ""Condiciones de
Google""). Al utilizar el Servicio LOLA, usted reconoce y acepta las Condiciones de Google aplicables (por
ejemplo, como ""Usuario Final""). Cualquier uso no autorizado de las funciones y el contenido de Google
Maps puede resultar en la suspensión o cancelación de su suscripción al Servicio LOLA.
3. Certificación de Cumplimiento de la Ley Aplicable.
Al acceder y utilizar el Servicio LOLA, usted certifica que: (1) tiene al menos 18 años de edad o la mayoría
de edad en su jurisdicción, lo que sea mayor, y (2) cumplirá con todas las leyes y reglamentaciones
aplicables a sus actividades realizadas a través de, o relacionadas con, el Servicio LOLA.
 Para los dueños de mascotas, esto significa, entre otras cosas, que usted se asegurará de que
sus mascotas estén vacunadas, autorizadas, tengan una etiqueta de identificación y/o microchip
según lo requieran las leyes o regulaciones locales; que ha obtenido y mantendrá todas las
pólizas de seguro obligatorias con respecto a las mascotas cuyo cuidado confía a los proveedores
de servicios (y que dichas pólizas beneficiarán a terceros, incluidos los proveedores de servicios,
en la misma medida en que lo benefician a usted).
 Para los proveedores de servicios, esto significa, entre otras cosas, que usted certifica que es
legalmente elegible para proporcionar servicios de cuidado de mascotas en la jurisdicción donde
proporciona dichos servicios; que ha cumplido y cumplirá con todas las leyes y reglamentaciones
que le sean aplicables; que ha obtenido todas las licencias comerciales, registros de impuestos
comerciales, licencias comerciales y permisos necesarios para proporcionar legalmente servicios
de cuidado de mascotas; y que, al proporcionar servicios de cuidado de mascotas, cumplirá con
las leyes aplicables sobre correas, eliminación de desechos de mascotas y leyes similares.
Usted reconoce que LOLA tiene derecho a confiar en estas certificaciones suyas, no es responsable de
garantizar que todos los usuarios hayan cumplido con las leyes y regulaciones aplicables y no será
responsable por el incumplimiento de un usuario al hacerlo.
4. Uso del Servicio LOLA; Suspensión.
4.1 Su conducta en el Servicio LOLA. Al utilizar el Servicio LOLA, usted acepta:
 Utilizar el Servicio LOLA únicamente de forma legal y únicamente para los fines previstos.
 No utilizar el Servicio LOLA para organizar el cuidado de: (a) mascotas exóticas o
inherentemente peligrosas, como serpientes venenosas o constrictoras, primates, lobos o
híbridos de lobo, gatos no domesticados, caimanes, caballos u otro ganado; (b) cualquier animal
cuya propiedad o cuidado por parte de terceros esté prohibido por la ley aplicable; o (c)
cualquier animal que tenga antecedentes de, o que haya sido entrenado para, ataques a
mascotas o personas.
 No enviar virus ni otros códigos maliciosos al Servicio LOLA ni a través de él.
 No utilizar el Servicio LOLA, ni interactuar con otros usuarios del Servicio LOLA, con fines que
violen la ley.
 No utilizar el Servicio LOLA para concertar la prestación y compra de servicios con otro usuario y
luego completar transacciones para esos servicios fuera del Servicio LOLA.
 No utilizar el Servicio LOLA con fines de competir con LOLA o para promocionar otros productos
o servicios.
 No publicar reseñas que no se basen en su experiencia personal, que sean intencionalmente
inexactas o engañosas o que violen estos Términos.
 No publicar contenido o materiales que sean pornográficos, amenazantes, acosadores, abusivos
o difamatorios, o que contengan desnudez o violencia gráfica, inciten a la violencia, violen
derechos de propiedad intelectual o violen la ley o los derechos legales (por ejemplo, derechos
de privacidad) de otros.
 No publicar ""spam"" u otras comunicaciones comerciales no autorizadas.
 Utilizar el Servicio LOLA únicamente para sus propios fines y no suplantar a ninguna otra
persona.
 No transferir ni autorizar el uso de su cuenta del Servicio LOLA por parte de ninguna otra
persona, ni participar en transacciones fraudulentas.
 No utilizar ningún crédito o recompensa promocional de una manera incompatible con el
espíritu y el propósito del programa, incluido, entre otros, el uso de créditos o recompensas
promocionales para crear reservas dentro del mismo hogar o con alguien con quien comparte
una mascota.
 No proporcionar información falsa en su perfil o registro en el Servicio LOLA, ni crear cuentas
múltiples o duplicadas.
 No interferir con nuestra prestación del Servicio LOLA ni con el uso que haga cualquier otro
usuario del mismo.
 No solicitar el nombre de usuario o la contraseña de otro usuario para el Servicio LOLA ni ningún
otro dato personal confidencial, incluidos datos bancarios."
                },
                contacto = new Contacto
                {
                    email = "lolacuidatumascota@gmail.com",
                    telefono = "+57 301 204 8490",
                    direccion = "Calle 2b # 81ª 460 Medellín, Colombia"
                }
            };
        }

        /// <summary>
        /// Retorna las políticas en formato JSON
        /// </summary>
        /// <returns>Las políticas en formato JSON</returns>
        [HttpGet("json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPoliciesJson()
        {
            return Ok(_politicasEsquema);
        }

        /// <summary>
        /// Retorna las políticas en formato texto plano
        /// </summary>
        /// <returns>Las políticas en formato texto plano</returns>
        [HttpGet("text")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPoliciesText()
        {
            var sb = new StringBuilder();
            
            // Añadir políticas
            sb.AppendLine($"{_politicasEsquema.politicas.titulo}");
            sb.AppendLine($"Última actualización: {_politicasEsquema.politicas.ultima_actualizacion}");
            sb.AppendLine();
            
            foreach (var seccion in _politicasEsquema.politicas.secciones)
            {
                sb.AppendLine($"{seccion.id}. {seccion.titulo}");
                sb.AppendLine($"{seccion.contenido}");
                sb.AppendLine();
            }
            
            // Añadir términos de servicio
            sb.AppendLine();
            sb.AppendLine($"{_politicasEsquema.terminos_servicio.titulo}");
            sb.AppendLine($"Vigencia: {_politicasEsquema.terminos_servicio.vigencia}");
            sb.AppendLine();
            
            foreach (var seccion in _politicasEsquema.terminos_servicio.secciones)
            {
                sb.AppendLine($"{seccion.id}. {seccion.titulo}");
                sb.AppendLine($"{seccion.contenido}");
                sb.AppendLine();
            }
            
            // Añadir información de contacto
            sb.AppendLine();
            sb.AppendLine("Contacto");
            sb.AppendLine($"Email: {_politicasEsquema.contacto.email}");
            sb.AppendLine($"Teléfono: {_politicasEsquema.contacto.telefono}");
            sb.AppendLine($"Dirección: {_politicasEsquema.contacto.direccion}");

            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }

        /// <summary>
        /// Retorna las políticas en formato HTML con estilos
        /// </summary>
        /// <returns>Las políticas en formato HTML</returns>
        [HttpGet("html")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPoliciesHtml()
        {
            var sb = new StringBuilder();
            
            // Inicio del documento HTML con estilos basados en el tema proporcionado
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"es\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("    <title>Políticas y Términos - LOLA</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        :root {");
            sb.AppendLine("            --primary: #7F00B2;");
            sb.AppendLine("            --black: black;");
            sb.AppendLine("            --primaryDark: #1B004B;");
            sb.AppendLine("            --primaryLight: #C797D4;");
            sb.AppendLine("            --primaryMain: #4C007D;");
            sb.AppendLine("            --white: white;");
            sb.AppendLine("            --colorTextDark: #1B004B;");
            sb.AppendLine("            --colorTextLight: rgba(27,0,75,0.5);");
            sb.AppendLine("            --lightGray: #F5F5F5;");
            sb.AppendLine("        }");
            sb.AppendLine("        body {");
            sb.AppendLine("            font-family: 'Helvetica', sans-serif;");
            sb.AppendLine("            line-height: 1.6;");
            sb.AppendLine("            color: var(--colorTextDark);");
            sb.AppendLine("            margin: 0;");
            sb.AppendLine("            padding: 0;");
            sb.AppendLine("            background-color: var(--white);");
            sb.AppendLine("        }");
            sb.AppendLine("        .container {");
            sb.AppendLine("            max-width: 1000px;");
            sb.AppendLine("            margin: 0 auto;");
            sb.AppendLine("            padding: 20px;");
            sb.AppendLine("        }");
            sb.AppendLine("        header {");
            sb.AppendLine("            background-color: var(--primary);");
            sb.AppendLine("            color: var(--white);");
            sb.AppendLine("            padding: 20px;");
            sb.AppendLine("            text-align: center;");
            sb.AppendLine("        }");
            sb.AppendLine("        h1 {");
            sb.AppendLine("            font-family: 'Prata', 'Helvetica-Bold', serif;");
            sb.AppendLine("            font-size: 24px;");
            sb.AppendLine("            margin: 0;");
            sb.AppendLine("        }");
            sb.AppendLine("        h2 {");
            sb.AppendLine("            font-family: 'Prata', 'Helvetica-Bold', serif;");
            sb.AppendLine("            font-size: 20px;");
            sb.AppendLine("            color: var(--primaryMain);");
            sb.AppendLine("            margin-top: 30px;");
            sb.AppendLine("            padding-bottom: 10px;");
            sb.AppendLine("            border-bottom: 1px solid var(--primaryLight);");
            sb.AppendLine("        }");
            sb.AppendLine("        .fecha {");
            sb.AppendLine("            font-size: 14px;");
            sb.AppendLine("            color: var(--colorTextLight);");
            sb.AppendLine("            margin-bottom: 20px;");
            sb.AppendLine("        }");
            sb.AppendLine("        .seccion {");
            sb.AppendLine("            background-color: var(--white);");
            sb.AppendLine("            padding: 15px;");
            sb.AppendLine("            margin-bottom: 15px;");
            sb.AppendLine("            border-radius: 5px;");
            sb.AppendLine("            box-shadow: 0 2px 5px rgba(0,0,0,0.1);");
            sb.AppendLine("        }");
            sb.AppendLine("        .seccion-titulo {");
            sb.AppendLine("            font-size: 16px;");
            sb.AppendLine("            font-weight: bold;");
            sb.AppendLine("            color: var(--primaryDark);");
            sb.AppendLine("            margin-top: 0;");
            sb.AppendLine("        }");
            sb.AppendLine("        .seccion-contenido {");
            sb.AppendLine("            font-size: 14px;");
            sb.AppendLine("        }");
            sb.AppendLine("        .contacto {");
            sb.AppendLine("            background-color: var(--lightGray);");
            sb.AppendLine("            padding: 20px;");
            sb.AppendLine("            border-radius: 5px;");
            sb.AppendLine("            margin-top: 30px;");
            sb.AppendLine("        }");
            sb.AppendLine("        footer {");
            sb.AppendLine("            text-align: center;");
            sb.AppendLine("            margin-top: 50px;");
            sb.AppendLine("            padding: 20px;");
            sb.AppendLine("            background-color: var(--primaryDark);");
            sb.AppendLine("            color: var(--white);");
            sb.AppendLine("            font-size: 14px;");
            sb.AppendLine("        }");
            sb.AppendLine("        a {");
            sb.AppendLine("            color: var(--primary);");
            sb.AppendLine("            text-decoration: none;");
            sb.AppendLine("        }");
            sb.AppendLine("        a:hover {");
            sb.AppendLine("            text-decoration: underline;");
            sb.AppendLine("        }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <header>");
            sb.AppendLine($"        <h1>LOLA - Documentos Legales</h1>");
            sb.AppendLine("    </header>");
            sb.AppendLine("    <div class=\"container\">");
            
            // Políticas
            sb.AppendLine($"        <h2>{_politicasEsquema.politicas.titulo}</h2>");
            sb.AppendLine($"        <p class=\"fecha\">Última actualización: {_politicasEsquema.politicas.ultima_actualizacion}</p>");
            
            foreach (var seccion in _politicasEsquema.politicas.secciones)
            {
                sb.AppendLine("        <div class=\"seccion\">");
                sb.AppendLine($"            <h3 class=\"seccion-titulo\">{seccion.id}. {seccion.titulo}</h3>");
                sb.AppendLine($"            <p class=\"seccion-contenido\">{seccion.contenido}</p>");
                sb.AppendLine("        </div>");
            }
            
            // Términos de servicio
            sb.AppendLine($"        <h2>{_politicasEsquema.terminos_servicio.titulo}</h2>");
            sb.AppendLine($"        <p class=\"fecha\">Vigencia: {_politicasEsquema.terminos_servicio.vigencia}</p>");
            
            foreach (var seccion in _politicasEsquema.terminos_servicio.secciones)
            {
                sb.AppendLine("        <div class=\"seccion\">");
                sb.AppendLine($"            <h3 class=\"seccion-titulo\">{seccion.id}. {seccion.titulo}</h3>");
                sb.AppendLine($"            <p class=\"seccion-contenido\">{seccion.contenido}</p>");
                sb.AppendLine("        </div>");
            }
            
            // Información de contacto
            sb.AppendLine("        <div class=\"contacto\">");
            sb.AppendLine("            <h2>Contacto</h2>");
            sb.AppendLine($"            <p>Email: <a href=\"mailto:{_politicasEsquema.contacto.email}\">{_politicasEsquema.contacto.email}</a></p>");
            sb.AppendLine($"            <p>Teléfono: <a href=\"tel:{_politicasEsquema.contacto.telefono}\">{_politicasEsquema.contacto.telefono}</a></p>");
            sb.AppendLine($"            <p>Dirección: {_politicasEsquema.contacto.direccion}</p>");
            sb.AppendLine("        </div>");
            
            // Fin del documento HTML
            sb.AppendLine("    </div>");
            sb.AppendLine("    <footer>");
            sb.AppendLine("        &copy; " + DateTime.Now.Year + " LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados");
            sb.AppendLine("    </footer>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return Content(sb.ToString(), "text/html", Encoding.UTF8);
        }
    }
}