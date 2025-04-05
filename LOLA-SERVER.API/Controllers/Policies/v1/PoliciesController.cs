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
                            contenido = "Podemos actualizar estas políticas ocasionalmente. Le notificaremos cualquier cambio significativo a través de nuestra aplicación o por correo electrónico."
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
                            contenido = "Estos Términos de Servicio (los \"Términos\") constituyen un acuerdo legal vinculante entre usted y LOLA CUIDA TU MASCOTA S.A.S, una empresa constituida bajo las leyes colombianas con domicilio en Calle 2b # 81ª 460 Medellín."
                        },
                        new Seccion {
                            id = 2,
                            titulo = "LEA ESTOS TÉRMINOS DETENIDAMENTE",
                            contenido = "LA SECCIÓN 17 INCLUYE UNA CLÁUSULA DE ARBITRAJE VINCULANTE Y UNA RENUNCIA A DEMANDAS COLECTIVAS, Y LA SECCIÓN 19 INCLUYE UNA RENUNCIA A DEMANDAS COLECTIVAS, LO CUAL AFECTA LA RESOLUCIÓN DE DISPUTAS."
                        },
                        new Seccion {
                            id = 3,
                            titulo = "Servicio LOLA",
                            contenido = "El Servicio LOLA consta de una aplicación web de escritorio, aplicaciones móviles y otras herramientas, soporte y servicios relacionados que los dueños de mascotas (\"Dueños de Mascotas\") y los proveedores de servicios relacionados con mascotas (\"Proveedores de Servicios\") pueden utilizar para encontrarse, comunicarse e interactuar entre sí. El Servicio LOLA incluye nuestros servicios de asistencia en caso de emergencia, materiales educativos para proveedores de servicios y otros servicios."
                        },
                        new Seccion {
                            id = 4,
                            titulo = "LOLA no ofrece Servicios de Cuidado de Mascotas",
                            contenido = "LOLA es un foro neutral para Proveedores de Servicios y Dueños de Mascotas. LOLA no es un Proveedor de Servicios y, salvo la asistencia telefónica de emergencia y otros recursos y asistencia descritos específicamente en el Servicio LOLA, no ofrece servicios de cuidado de mascotas. No realizamos declaraciones ni garantías sobre la calidad del alojamiento, cuidado de mascotas, paseo de perros, cuidado de casas u otros servicios prestados por los Proveedores de Servicios."
                        },
                        new Seccion {
                            id = 5,
                            titulo = "Exención",
                            contenido = "De conformidad con la Sección 16, LOLA no se responsabiliza de ninguna reclamación, lesión, pérdida, daño o perjuicio derivado o relacionado de cualquier manera con sus interacciones o tratos con otros usuarios, ni con las acciones u omisiones de los Proveedores de Servicios y Dueños de Mascotas, ya sea en línea o fuera de línea."
                        },
                        new Seccion {
                            id = 6,
                            titulo = "Transacciones entre dueños y proveedores",
                            contenido = "El Servicio LOLA puede utilizarse para buscar y ofrecer servicios de cuidado de mascotas y para facilitar el pago, pero todas las transacciones realizadas a través del Servicio LOLA se realizan entre los dueños de mascotas y los proveedores de servicios."
                        },
                        new Seccion {
                            id = 7,
                            titulo = "Reservas",
                            contenido = "Los dueños de mascotas y los proveedores de servicios realizan transacciones entre sí en el Servicio LOLA cuando ambos acuerdan una \"reserva\" que especifica las tarifas, el período de tiempo, la política de cancelación y otros términos para la prestación de los servicios de cuidado de mascotas."
                        },
                        new Seccion {
                            id = 8,
                            titulo = "Emergencias",
                            contenido = "Recomendamos a los Dueños de Mascotas que proporcionen a sus Proveedores de Servicios la información de contacto para que puedan ser localizados en caso de que sea necesaria la atención médica para una mascota, y que proporcionen un contacto de emergencia en el perfil del Dueño de la Mascota que haya consentido la divulgación de su información."
                        },
                        new Seccion {
                            id = 9,
                            titulo = "Certificación de Cumplimiento de la Ley Aplicable",
                            contenido = "Al acceder y utilizar el Servicio LOLA, usted certifica que: (1) tiene al menos 18 años de edad o la mayoría de edad en su jurisdicción, lo que sea mayor, y (2) cumplirá con todas las leyes y reglamentaciones aplicables a sus actividades realizadas a través de, o relacionadas con, el Servicio LOLA."
                        },
                        new Seccion {
                            id = 10,
                            titulo = "Tarifas y pagos",
                            contenido = "Cobramos tarifas de servicio por algunos aspectos del Servicio LOLA. Si usted es un Proveedor de Servicios, salvo que se especifique lo contrario a través del Servicio LOLA, nuestra tarifa de servicio se calcula como un porcentaje de las tarifas que el Dueño de la Mascota se compromete a pagarle en una Reserva y se cobra por cada Reserva."
                        },
                        new Seccion {
                            id = 11, 
                            titulo = "Contacto",
                            contenido = "Si tiene preguntas o inquietudes sobre el Servicio LOLA o estos Términos, comuníquese con su oficina local: Calle 2b # 81ª 460 Medellín, Tel. 3012048490, lolacuidatumascota@gmail.com"
                        },
                        new Seccion {
                            id = 12,
                            titulo = "Naturaleza del Servicio LOLA",
                            contenido = "El Servicio LOLA consta de una aplicación web de escritorio, aplicaciones móviles y otras herramientas, soporte y servicios relacionados que los dueños de mascotas (« Dueños de Mascotas ») y los proveedores de servicios relacionados con mascotas (« Proveedores de Servicios ») pueden utilizar para encontrarse, comunicarse e interactuar entre sí. El Servicio LOLA incluye nuestros servicios de asistencia en caso de emergencia, materiales educativos para proveedores de servicios y otros servicios. Cobramos por algunos aspectos del Servicio LOLA."
                        },
                        new Seccion {
                            id = 13,
                            titulo = "Mascotas Abandonadas; Reubicación",
                            contenido = "Los dueños de mascotas que contraten Servicios de Cuidado de Mascotas y no recuperen a su mascota después del período de servicio indicado en una Reserva aceptan que LOLA (o el Proveedor de Servicios) podrá, a su entera discreción, colocar a la mascota en un hogar de acogida, transferir su cuidado a control animal u otras autoridades policiales, o buscar un cuidado alternativo. El dueño de la mascota se compromete a reembolsar a LOLA y/o al Proveedor de Servicios todos los costos y gastos asociados con dichas acciones."
                        },
                        new Seccion {
                            id = 14,
                            titulo = "Servicios de Consulta",
                            contenido = "LOLA puede ofrecer a los Dueños de Mascotas y Proveedores de Servicios servicios de consulta veterinaria de terceros por teléfono, chat o correo electrónico para brindarles un recurso educativo que les ayude a tomar decisiones sobre sus mascotas o las mascotas bajo su cuidado. Estos servicios de consulta son proporcionados por un tercero y no forman parte del Servicio LOLA."
                        },
                        new Seccion {
                            id = 15,
                            titulo = "Google Maps",
                            contenido = "El uso del Servicio LOLA requiere el uso de las funciones y el contenido de Google Maps, que están sujetos a las (1) Condiciones de Servicio Adicionales de Google Maps/Google Earth vigentes, disponibles en https://maps.google.com/help/terms_maps.html; y a la (2) Política de Privacidad de Google, disponible en https://www.google.com/policies/privacy/."
                        },
                        new Seccion {
                            id = 16,
                            titulo = "Uso del Servicio LOLA; Suspensión",
                            contenido = "Al utilizar el Servicio LOLA, usted acepta utilizarlo únicamente de forma legal y únicamente para los fines previstos. No utilizará el Servicio LOLA para organizar el cuidado de mascotas exóticas o inherentemente peligrosas. No enviará virus ni otros códigos maliciosos al Servicio LOLA ni a través de él. No utilizará el Servicio LOLA con fines de competir con LOLA o para promocionar otros productos o servicios."
                        },
                        new Seccion {
                            id = 17,
                            titulo = "Acuerdo de arbitraje y renuncia a acción colectiva",
                            contenido = "A menos que usted opte por no participar en el Acuerdo de Arbitraje, usted y LOLA acuerdan que cualquier disputa o reclamación que surja entre usted y LOLA en relación con el Servicio LOLA, las interacciones con otros usuarios del Servicio LOLA o estos Términos, se resolverá según lo establecido en este Acuerdo de Arbitraje."
                        },
                        new Seccion {
                            id = 18, 
                            titulo = "Privacidad",
                            contenido = "La recopilación y el uso de su información personal en el Servicio LOLA se describen en nuestra Declaración de Privacidad. Al acceder o utilizar el Servicio LOLA, usted reconoce haber leído y comprendido la Declaración de Privacidad."
                        },
                        new Seccion {
                            id = 19,
                            titulo = "Renuncia a demanda colectiva",
                            contenido = "Usted acepta que, hasta donde lo permita la ley aplicable, cada uno de nosotros podrá presentar reclamaciones contra el otro solo de forma individual y no como demandante o miembro de un grupo en ninguna supuesta acción o procedimiento colectivo o representativo."
                        },
                        new Seccion {
                            id = 20,
                            titulo = "Comunicaciones telefónicas, de texto y móviles",
                            contenido = "Usted consiente recibir de LOLA, o en su nombre, comunicaciones que contengan información relacionada con el servicio, y/o mensajes de ventas, marketing o publicidad, mediante llamadas de voz automáticas, pregrabadas o artificiales, SMS, mensajes de texto, correo electrónico, plataformas de mensajería OTT (como WhatsApp) y otros medios electrónicos."
                        },
                        new Seccion {
                            id = 21,
                            titulo = "Cancelaciones y reembolsos",
                            contenido = "LOLA puede ayudarle a encontrar proveedores de servicios de reemplazo cuando estos cancelen reservas cerca de la fecha de inicio del período de servicio indicado en la reserva. La disponibilidad de la Protección de Reservas depende del momento de la cancelación y del tipo de servicios de cuidado de mascotas proporcionados."
                        },
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
                        new Seccion {
                            id = 25,
                            titulo = "Fuerza mayor",
                            contenido = "LOLA no será responsable de ningún retraso o incumplimiento derivado de causas ajenas a su control razonable, incluyendo, entre otras, casos fortuitos, desastres naturales, terremotos, huracanes, incendios forestales, inundaciones, guerras, terrorismo, disturbios, embargos, incendios, accidentes, pandemias, enfermedades, huelgas u otros desastres similares."
                        }
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
        /// Retorna las políticas de uso de la aplicación en formato HTML
        /// </summary>
        /// <returns>Contenido HTML con las políticas de uso</returns>
        [HttpGet("html")]
        [Produces("text/html")]
        public ContentResult GetPoliciesHtml()
        {
            var politicas = _politicasEsquema.politicas;
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>" + politicas.titulo + @"</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        max-width: 800px;
                        margin: 0 auto;
                        padding: 20px;
                        color: #333;
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
                        margin-bottom: 30px;
                    }
                    footer {
                        margin-top: 50px;
                        padding-top: 20px;
                        border-top: 1px solid #eee;
                        color: #7f8c8d;
                        font-size: 0.9em;
                    }
                </style>
            </head>
            <body>
                <h1>" + politicas.titulo + @"</h1>
                <p class='date'>Última actualización: " + politicas.ultima_actualizacion + @"</p>");

            foreach (var seccion in politicas.secciones)
            {
                stringBuilder.AppendLine(@"
                <h2>" + seccion.id + ". " + seccion.titulo + @"</h2>
                <p>" + seccion.contenido + @"</p>");
            }

            stringBuilder.AppendLine(@"
                <footer>
                    &copy; 2025 LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados
                </footer>
            </body>
            </html>");

            return Content(stringBuilder.ToString(), "text/html");
        }

        /// <summary>
        /// Retorna las políticas de uso de la aplicación en formato texto plano
        /// </summary>
        /// <returns>Contenido de texto con las políticas de uso</returns>
        [HttpGet("text")]
        [Produces("text/plain")]
        public ContentResult GetPoliciesText()
        {
            var politicas = _politicasEsquema.politicas;
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(politicas.titulo.ToUpper());
            stringBuilder.AppendLine("Última actualización: " + politicas.ultima_actualizacion);
            stringBuilder.AppendLine();

            foreach (var seccion in politicas.secciones)
            {
                stringBuilder.AppendLine(seccion.id + ". " + seccion.titulo.ToUpper());
                stringBuilder.AppendLine(new string('-', seccion.titulo.Length + 4));
                stringBuilder.AppendLine(seccion.contenido);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("© 2025 LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados");

            return Content(stringBuilder.ToString(), "text/plain");
        }

        /// <summary>
        /// Retorna las políticas de uso de la aplicación en formato JSON
        /// </summary>
        /// <returns>Objeto JSON con las políticas de uso</returns>
        [HttpGet("json")]
        [Produces("application/json")]
        public IActionResult GetPoliciesJson()
        {
            var politicas = _politicasEsquema.politicas;

            // Ahora podemos usar LINQ ya que tenemos tipos fuertes
            var sections = politicas.secciones.Select(s => new
            {
                Id = s.id,
                Title = s.titulo,
                Content = s.contenido
            }).ToArray();

            var result = new
            {
                Title = politicas.titulo,
                LastUpdated = politicas.ultima_actualizacion,
                Version = "1.2",
                Organization = "LOLA CUIDA TU MASCOTA S.A.S",
                Sections = sections,
                ContactInfo = _politicasEsquema.contacto
            };

            return Ok(result);
        }

        /// <summary>
        /// Retorna los términos de servicio en formato HTML
        /// </summary>
        /// <returns>Contenido HTML con los términos de servicio</returns>
        [HttpGet("terms/html")]
        [Produces("text/html")]
        public ContentResult GetTermsOfServiceHtml()
        {
            var terminos = _politicasEsquema.terminos_servicio;
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>" + terminos.titulo + @"</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        max-width: 800px;
                        margin: 0 auto;
                        padding: 20px;
                        color: #333;
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
                        margin-bottom: 30px;
                    }
                    footer {
                        margin-top: 50px;
                        padding-top: 20px;
                        border-top: 1px solid #eee;
                        color: #7f8c8d;
                        font-size: 0.9em;
                    }
                </style>
            </head>
            <body>
                <h1>" + terminos.titulo + @"</h1>
                <p class='date'>Vigente a partir de: " + terminos.vigencia + @"</p>");

            foreach (var seccion in terminos.secciones)
            {
                stringBuilder.AppendLine(@"
                <h2>" + seccion.id + ". " + seccion.titulo + @"</h2>
                <p>" + seccion.contenido + @"</p>");
            }

            stringBuilder.AppendLine(@"
                <footer>
                    &copy; 2025 LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados
                </footer>
            </body>
            </html>");

            return Content(stringBuilder.ToString(), "text/html");
        }

        /// <summary>
        /// Retorna los términos de servicio en formato texto plano
        /// </summary>
        /// <returns>Contenido de texto con los términos de servicio</returns>
        [HttpGet("terms/text")]
        [Produces("text/plain")]
        public ContentResult GetTermsOfServiceText()
        {
            var terminos = _politicasEsquema.terminos_servicio;
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(terminos.titulo.ToUpper());
            stringBuilder.AppendLine("Vigente a partir de: " + terminos.vigencia);
            stringBuilder.AppendLine();

            foreach (var seccion in terminos.secciones)
            {
                stringBuilder.AppendLine(seccion.id + ". " + seccion.titulo.ToUpper());
                stringBuilder.AppendLine(new string('-', seccion.titulo.Length + 4));
                stringBuilder.AppendLine(seccion.contenido);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("© 2025 LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados");

            return Content(stringBuilder.ToString(), "text/plain");
        }

        /// <summary>
        /// Retorna los términos de servicio en formato JSON
        /// </summary>
        /// <returns>Objeto JSON con los términos de servicio</returns>
        [HttpGet("terms/json")]
        [Produces("application/json")]
        public IActionResult GetTermsOfServiceJson()
        {
            var terminos = _politicasEsquema.terminos_servicio;

            var sections = terminos.secciones.Select(s => new
            {
                Id = s.id,
                Title = s.titulo,
                Content = s.contenido
            }).ToArray();

            var result = new
            {
                Title = terminos.titulo,
                EffectiveDate = terminos.vigencia,
                Version = "1.0",
                Organization = "LOLA CUIDA TU MASCOTA S.A.S",
                Sections = sections,
                ContactInfo = _politicasEsquema.contacto
            };

            return Ok(result);
        }

        /// <summary>
        /// Retorna la estructura completa de políticas y términos
        /// </summary>
        /// <returns>Estructura completa en formato JSON</returns>
        [HttpGet("structure")]
        [Produces("application/json")]
        public IActionResult GetStructure()
        {
            return Ok(_politicasEsquema);
        }

        /// <summary>
        /// Retorna los términos de servicio completos en formato texto plano
        /// </summary>
        /// <returns>Contenido de texto con los términos de servicio completos</returns>
        [HttpGet("terms/fulltext")]
        [Produces("text/plain")]
        public ContentResult GetFullTermsOfServiceText()
        {
            var terminosCompletos = _politicasEsquema.terminos_servicio_completos;
            return Content(terminosCompletos.contenido_completo, "text/plain");
        }

        /// <summary>
        /// Retorna los términos de servicio completos en formato HTML
        /// </summary>
        /// <returns>Contenido HTML con los términos de servicio completos</returns>
        [HttpGet("terms/fullhtml")]
        [Produces("text/html")]
        public ContentResult GetFullTermsOfServiceHtml()
        {
            var terminosCompletos = _politicasEsquema.terminos_servicio_completos;
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>" + terminosCompletos.titulo + @"</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        max-width: 800px;
                        margin: 0 auto;
                        padding: 20px;
                        color: #333;
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
                        margin-bottom: 30px;
                    }
                    footer {
                        margin-top: 50px;
                        padding-top: 20px;
                        border-top: 1px solid #eee;
                        color: #7f8c8d;
                        font-size: 0.9em;
                    }
                    pre {
                        white-space: pre-wrap;
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                    }
                </style>
            </head>
            <body>
                <h1>" + terminosCompletos.titulo + @"</h1>
                <p class='date'>Vigente a partir de: " + terminosCompletos.vigencia + @"</p>
                <pre>" + terminosCompletos.contenido_completo + @"</pre>
                <footer>
                    &copy; 2025 LOLA CUIDA TU MASCOTA S.A.S - Todos los derechos reservados
                </footer>
            </body>
            </html>");

            return Content(stringBuilder.ToString(), "text/html");
        }
    }
}