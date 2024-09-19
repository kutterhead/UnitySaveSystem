using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        // Definir la ruta donde se guardará el archivo.En la carpeta persistente tenemos permiso de escritura.
        filePath = Application.persistentDataPath + "/playerData.json";

        GuardarDatos(1,2,3);
        //CargarDatos();
        //GuardarDatosEncriptados(1,2,3);

        //CargarDatosEncriptados();
    }

    // Función para guardar en formato JSON
    public void GuardarDatos(int vida, int puntos, int nivel)
    {
        playerData data = new playerData(vida, puntos, nivel);

        // Convertimos los datos a formato JSON
        string json = JsonUtility.ToJson(data, true);

        // Escribimos el archivo en el sistema
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en " + filePath);
    }

    // Función para cargar los datos del jugador desde un archivo JSON
    public playerData CargarDatos()
    {
        if (File.Exists(filePath))
        {
            // Leemos el archivo
            string json = File.ReadAllText(filePath);

            // Convertimos el JSON a la clase playerData
            playerData data = JsonUtility.FromJson<playerData>(json);

            Debug.Log("Datos cargados correctamente: Vida: " + data.vida + ", Puntos: " + data.puntos + ", Nivel: " + data.nivel);
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
            return null;
        }
    }

    public void GuardarDatosEncriptados(int vida, int puntos, int nivel)
    {
        playerData data = new playerData(vida, puntos, nivel);

        // Convertimos los datos a JSON
        string json = JsonUtility.ToJson(data, true);

        // Encriptamos el JSON antes de guardarlo
        string encryptedJson = EncryptionUtility.Encrypt(json);

        // Guardamos el texto encriptado en el archivo
        File.WriteAllText(filePath, encryptedJson);
        Debug.Log("Datos encriptados guardados en " + filePath);
    }
    public playerData CargarDatosEncriptados()
    {
        if (File.Exists(filePath))
        {
            // Leemos el archivo encriptado
            string encryptedJson = File.ReadAllText(filePath);

            // Desencriptamos el JSON
            string json = EncryptionUtility.Decrypt(encryptedJson);

            // Convertimos el JSON a la clase PlayerData
            playerData data = JsonUtility.FromJson<playerData>(json);

            Debug.Log("Datos desencriptados y cargados: Vida: " + data.vida + ", Puntos: " + data.puntos + ", Nivel: " + data.nivel);
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
            return null;
        }
    }

    public string desenCriptarDatos(string datoADecript)
    {
        return EncryptionUtility.Decrypt(datoADecript);
    }

}
