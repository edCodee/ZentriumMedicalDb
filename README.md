# 🏥 HospitalAPI — Backend para Base de Datos Replicada en PostgreSQL

HospitalAPI es una API REST desarrollada en C# (.NET Core) que permite la gestión de usuarios y roles en un entorno hospitalario. Está diseñada para funcionar con una base de datos PostgreSQL replicada en tres máquinas virtuales en Google Cloud Platform (GCP).  
Forma parte de una arquitectura distribuida compuesta por backend, frontend y base de datos, cada uno con su respectivo repositorio.

⚠️ Este repositorio corresponde únicamente al backend.

---

## 🚀 Características principales

- Conexión a base de datos PostgreSQL con replicación.
- Gestión de usuarios y autenticación.
- Administración de roles.
- Documentación integrada con Swagger.
- Arquitectura preparada para despliegue en entornos cloud (GCP, AWS, etc.).

---

## 🛠 Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/edCodee/ZentriumMedicalDb.git
cd HospitalAPI
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar conexión a PostgreSQL

Edita el archivo `appsettings.json` con tus credenciales:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<IP_SERVIDOR>;Port=5432;Database=<DB_NAME>;Username=<USER>;Password=<PASSWORD>"
  }
}
```

### 4. Aplicar migraciones a la base de datos

```bash
dotnet ef database update
```

### 5. Ejecutar la API

```bash
dotnet run
```

### 6. Acceder a la documentación Swagger

La documentación de la API estará disponible en:  
[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## 🔐 Uso de la API

### 📌 Autenticación de usuarios

**POST /api/User/Login**

```json
{
  "UserId": "1234567890",
  "UserPassword": "TuContraseña"
}
```

Retorna los datos del usuario autenticado junto a sus roles si las credenciales son válidas.

---

### 📌 Obtener roles disponibles

**GET /api/Role**  
Devuelve una lista de roles registrados en el sistema.

---

### 📌 Obtener todos los usuarios

**GET /api/User**  
Retorna un listado completo con la información básica de los usuarios.

---

### 📌 Buscar usuario por cédula

**GET /api/User/ByCedula/{cedula}**

Ejemplo:

```http
GET /api/User/ByCedula/1234567890
```

Retorna los datos del usuario si existe; de lo contrario, muestra un mensaje de error.

---

### 📌 Crear un nuevo usuario

**POST /api/User**

```json
{
  "UserId": "1234567890",
  "UserFirstName": "Juan",
  "UserMiddleName": "Carlos",
  "UserLastName": "Pérez",
  "UserSecondLastName": "Gómez",
  "UserBirthDate": "1990-05-15",
  "UserUserName": "juanperez",
  "UserEmail": "juan@example.com",
  "UserPassword": "password123"
}
```

El servidor retornará los datos del usuario creado, excluyendo la contraseña.

---

## 🤝 Contribuciones

¡Gracias por tu interés en contribuir!  
Sigue estos pasos para proponer mejoras:

### Clona el repositorio:

```bash
git clone https://github.com/edCodee/HospitalAPI.git
cd HospitalAPI
```

### Crea una nueva rama:

```bash
git checkout -b feature/nueva-funcionalidad
```

### Realiza tus cambios:

```bash
git add .
git commit -m "Añadir funcionalidad de validación de usuarios"
```

### Sube la rama:

```bash
git push origin feature/nueva-funcionalidad
```

Abre un Pull Request en GitHub, describiendo claramente los cambios realizados.  
Para más detalles, consulta la guía oficial de GitHub sobre Pull Requests.

---

## 📄 Licencia

Este proyecto está licenciado bajo los términos de la **Licencia MIT**, lo que te permite:

- Usar el software libremente.
- Modificarlo y redistribuirlo.
- Integrarlo en proyectos personales o comerciales.

Sin embargo, **este proyecto tiene un autor claramente identificado**, por lo tanto, **cualquier uso, modificación o distribución del código debe conservar el aviso de copyright y el nombre del autor original**.

> **Se debe dar crédito a Edison Joel Acosta Nuñez como el creador original del software.**

Consulta el archivo [`LICENSE`](./LICENSE) incluido en este repositorio para más información.

```text
MIT License

Copyright (c) 2025 Edison Joel Acosta

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

```

---
## 👨‍💻 Autor

**Edison Joel Acosta Nuñez**

📧 Contacto:

- joel1ookk@gmail.com  
- eedcodee@gmail.com
