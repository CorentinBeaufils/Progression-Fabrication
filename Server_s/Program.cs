using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await StartServerAsync();
    }

    static async Task StartServerAsync()
    {
        try
        {
            // Adresse IP et numéro de port du serveur
            string serverIP = "127.0.0.1";
            int serverPort = 12345;

            // Convertir la chaîne en objet IPAddress
            IPAddress ipAddress = IPAddress.Parse(serverIP);

            // Créer un objet IPEndPoint avec l'adresse IP et le numéro de port
            IPEndPoint endPoint = new IPEndPoint(ipAddress, serverPort);

            // Créer la socket du serveur
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Lier la socket du serveur à l'adresse IP et au numéro de port
            serverSocket.Bind(endPoint);

            // Commencer à écouter les connexions entrantes avec une file d'attente de 10 connexions
            serverSocket.Listen(10);

            Console.WriteLine($"Serveur en attente de connexions sur {endPoint}");

            Socket clientSocket = await serverSocket.AcceptAsync();
            Console.WriteLine($"Client connecté: {clientSocket.RemoteEndPoint}");
            while (true)
            {
                // Accepter la connexion d'un client de manière asynchrone
                // Lire les données du client de manière asynchrone
                byte[] receiveBuffer = new byte[1024];
                int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);

                // Afficher les données reçues du client
                string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                Console.WriteLine($"Message reçu du client {clientSocket.RemoteEndPoint}: {receivedMessage}");

                // Fermer la connexion avec le client
                if(receivedMessage == "100")
                    clientSocket.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }
}
