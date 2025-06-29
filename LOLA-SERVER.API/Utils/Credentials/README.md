# Configuración de Credenciales de Firebase

Este proveedor de credenciales permite obtener las credenciales de Firebase de múltiples fuentes, priorizando la seguridad para diferentes entornos.

## Orden de Prioridad

1. **Variable de entorno** `FIREBASE_CREDENTIALS_JSON` (Recomendado para producción)
2. **Configuración en appsettings.json** (Actual, bueno para desarrollo)
3. **Archivo de credenciales** (Fallback)

## Uso

### Opción 1: Variable de Entorno (Producción)

```bash
export FIREBASE_CREDENTIALS_JSON='{"type":"service_account","project_id":"lola-manager","private_key_id":"...","private_key":"-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----\n","client_email":"...","client_id":"...","auth_uri":"https://accounts.google.com/o/oauth2/auth","token_uri":"https://oauth2.googleapis.com/token","auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs","client_x509_cert_url":"..."}'
```

### Opción 2: Configuración en appsettings.json (Desarrollo)

Las credenciales ya están configuradas en la sección `Firebase` del appsettings.json con los nombres exactos que requiere Firebase.

### Opción 3: Archivo de Credenciales (Fallback)

Si no se encuentran credenciales en las opciones anteriores, se buscará el archivo `Utils/Credentials/Firebase-Credentials.json`.

## Seguridad

- **Desarrollo**: Usar appsettings.json (actual)
- **Producción**: Usar variable de entorno `FIREBASE_CREDENTIALS_JSON`
- **Nunca** subir archivos de credenciales al repositorio

## Migración

Los servicios han sido actualizados para usar inyección de dependencias:

- `ImageService`
- `FirebaseMessagingService`
- `NotificationsService`
- `PetServicesService`

Todos ahora reciben `IFirebaseCredentialsProvider` en su constructor.
