using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;


namespace DemoDP4500
{
    public class prueba
    {

        static async Task main()
        {
            // Crear datos en formato JSON
            string datosJson = "{\"biometricdata\": \"\", \"id\": 0}"; // Reemplaza con tus propios datos

            // URL del servidor donde se enviarán los datos
            string urlServidor = "https://ejemplo.com/api/enviar_datos";

            // Configurar cliente HttpClient
            using (HttpClient cliente = new HttpClient())
            {
                // Configurar encabezados de la solicitud HTTP
                cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Crear contenido JSON
                var contenido = new StringContent(datosJson, Encoding.UTF8, "application/json");

                // Realizar la solicitud HTTP POST al servidor web
                HttpResponseMessage respuesta = await cliente.PostAsync(urlServidor, contenido);

                // Verificar la respuesta del servidor
                if (respuesta.IsSuccessStatusCode)
                {
                    Console.WriteLine("Datos enviados correctamente al servidor.");
                }
                else
                {
                    Console.WriteLine("Error al enviar los datos al servidor. Código de estado: " + respuesta.StatusCode);
                }
            }
        }

    }
}