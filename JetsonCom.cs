using System;
using System.Net.Sockets;
using System.Collections;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    public Transform headsetTransform;
    public string serverIPAddress = "10.157.181.229";
    public int serverPort = 8888;
    // public TMP_Text coordText; // the TextMeshPro object to display

    void Start()
    {
        ConnectToServer();
        StartCoroutine(SendDataCoroutine());
    }

    IEnumerator SendDataCoroutine()
    {
        while (true)
        {
            SendData(GetCurrentPosition());
            yield return new WaitForSeconds(0.025f);
        }
    }

    void ConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient(serverIPAddress, serverPort);
            stream = tcpClient.GetStream();
            Debug.Log("Connected to server!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void SendData(string data)
    {
        try
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            stream.Write(dataBytes, 0, dataBytes.Length);
            Debug.Log($"Sent data to server: {data}");
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data to server: " + e.Message);
        }
    }

    void OnDestroy()
    {
        if (tcpClient != null)
            tcpClient.Close();
    }

    // 
    string GetCurrentPosition()
    {
        if (headsetTransform != null)
        {
            // Sending the positon:
            Vector3 currentPosition = headsetTransform.position;
            return $"({currentPosition.x}, {currentPosition.y}, {currentPosition.z})";
            // Displaying the location as text:
            // string formattedCoordinates = $"{currentPosition.x:.##}, {currentPosition.y:.##}, {currentPosition.z:.##}";
            // Vector3 displayPosition = head.transform.position + head.transform.forward * 2f;
            // coordText.transform.position = displayPosition;
            // coordText.SetText(formattedCoordinates);
        }

        return ""; // Return an empty string if headsetTransform is not assigned
    }    
}
