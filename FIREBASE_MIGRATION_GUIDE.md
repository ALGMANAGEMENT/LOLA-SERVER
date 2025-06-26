# Guía de Migración de Firebase - lola-manager → lola-app-e5f71

## Resumen de la Migración

Este documento describe la migración del proyecto Firebase de `lola-manager` a `lola-app-e5f71`.

## Cambios Realizados

### ✅ Configuración Actualizada

1. **Project ID**: `lola-manager` → `lola-app-e5f71`
2. **Authority**: `https://securetoken.google.com/lola-manager` → `https://securetoken.google.com/lola-app-e5f71`
3. **Audiences**: Actualizadas con los nuevos client IDs de OAuth

### 📁 Archivos Modificados

- `LOLA-SERVER.API/Startup.cs` - Configuración de autenticación
- `LOLA-SERVER.API/appsettings.json` - Configuración de Firebase
- `LOLA-SERVER.API/appsettings.Development.json` - Configuración de desarrollo
- `LOLA-SERVER.API/Controllers/Test/FirebaseMigrationTestController.cs` - Controlador de prueba

## Pasos Pendientes

### 🔑 Obtener Nuevas Credenciales

**IMPORTANTE**: Necesitas obtener las nuevas credenciales del proyecto `lola-app-e5f71`.

1. Ve a [Firebase Console](https://console.firebase.google.com/)
2. Selecciona el proyecto **`lola-app-e5f71`**
3. Ve a **"Configuración del proyecto"** → **"Cuentas de servicio"**
4. Haz clic en **"Generar nueva clave privada"**
5. Descarga el archivo JSON
6. **Reemplaza** el contenido de `LOLA-SERVER.API/Utils/Credentials/Firebase-Credentials.json`

### 🔧 Configuración en Google Cloud Console

1. Ve a [Google Cloud Console](https://console.cloud.google.com/)
2. Selecciona el proyecto **`lola-app-e5f71`**
3. Ve a **"APIs y servicios"** → **"Biblioteca"**
4. Habilita estas APIs:
   - Firebase Admin SDK
   - Firebase Cloud Messaging API
   - Google Identity Toolkit API

## Verificación de la Migración

### 🧪 Endpoints de Prueba

Una vez que tengas las nuevas credenciales, puedes probar estos endpoints:

- **Información del Proyecto**: `GET /api/firebasemigrationtest/project-info`
- **Endpoint Público**: `GET /api/firebasemigrationtest/public`
- **Endpoint Protegido**: `GET /api/firebasemigrationtest/protected` (requiere token)
- **Configuración Firebase**: `GET /api/firebasemigrationtest/firebase-config`

### 📊 Verificación en Swagger

1. Ejecuta la aplicación
2. Ve a `https://localhost:5001/swagger`
3. Prueba los endpoints de `FirebaseMigrationTestController`

## Configuración del Frontend

### 🔧 Actualizar Configuración del Cliente

```javascript
// firebase-config.js
const firebaseConfig = {
  apiKey: "tu-nueva-api-key",
  authDomain: "lola-app-e5f71.firebaseapp.com",
  projectId: "lola-app-e5f71",
  storageBucket: "lola-app-e5f71.appspot.com",
  messagingSenderId: "tu-nuevo-sender-id",
  appId: "tu-nuevo-app-id"
};

firebase.initializeApp(firebaseConfig);
```

### 🔑 Obtener Configuración del Cliente

1. En Firebase Console, ve a **"Configuración del proyecto"**
2. En la sección **"Tus apps"**, selecciona tu aplicación web
3. Copia la configuración de Firebase

## Troubleshooting

### ❌ Errores Comunes

**Error: "Firebase app not initialized"**

- Verifica que el archivo de credenciales corresponde al proyecto `lola-app-e5f71`
- Asegúrate de que el Project ID en las credenciales sea correcto

**Error: "Invalid token"**

- Verifica que el token proviene del proyecto `lola-app-e5f71`
- Asegúrate de que el frontend esté configurado con el nuevo proyecto

**Error: "Unauthorized"**

- Verifica que las APIs estén habilitadas en Google Cloud Console
- Asegúrate de que las credenciales tengan los permisos correctos

### 🔍 Logs de Debug

En desarrollo, los logs mostrarán:

```
Firebase inicializado exitosamente para el proyecto: lola-app-e5f71
```

## Rollback (Si es Necesario)

Si necesitas volver al proyecto anterior:

1. Restaura el Project ID a `lola-manager` en todos los archivos
2. Restaura las credenciales originales
3. Actualiza las audiences y authority

## Estado de la Migración

- ✅ Configuración del backend actualizada
- ✅ Controladores de prueba creados
- ⏳ **PENDIENTE**: Obtener nuevas credenciales de Firebase
- ⏳ **PENDIENTE**: Actualizar configuración del frontend
- ⏳ **PENDIENTE**: Probar autenticación completa

## Contacto

Si tienes problemas con la migración:

1. Verifica que las credenciales correspondan al proyecto correcto
2. Revisa los logs de la aplicación
3. Asegúrate de que todas las APIs estén habilitadas
