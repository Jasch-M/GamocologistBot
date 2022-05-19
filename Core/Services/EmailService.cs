using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spire.Email.IMap;
using Template.Modules.Mazzin;
using Template.Modules.Utils;
using MailMessage = Spire.Email.MailMessage;

namespace Template.Services;

public class EmailService
{
    private readonly ImapClient _emailClient;
    private Spire.Email.Smtp.SmtpClient _smtpClient;
    internal bool ShouldListen;
    private int _numberOfAnalysedMessages;
    internal Dictionary<Guid, string> _responses;

    public EmailService(string dataPath)
    {
        LoadData(dataPath);
    }

    public EmailService()
    {
        string dataPath = DataLoginLocation;
        _emailClient = new ImapClient();
        LoadData(dataPath);
    }

    private void LoadData(string dataPath)
    {
        DataAssociation dataAssociation = DataAssociation.FromFile(dataPath);
        Host = dataAssociation["host"];
        Username = dataAssociation["username"];
        Password = dataAssociation["password"];
        bool canRead = int.TryParse(dataAssociation["port"], out int port);
        if (canRead)
            Port = port;
        
        string numberOfAnalysedMessages = dataAssociation.GetValueOrDefault("Number of Analysed Messages", "0");
        _numberOfAnalysedMessages = int.Parse(numberOfAnalysedMessages);
        
        _smtpClient = new Spire.Email.Smtp.SmtpClient();
        _smtpClient.Host = Host;
        _smtpClient.Username = Username;
        _smtpClient.Password = Password;
        _smtpClient.Port = Port;
        _smtpClient.UseOAuth = true;
        _emailClient.ConnectionProtocols = ConnectionProtocols.StartTls;
        
        
        Connect();
        ShouldListen = true;
        _responses = new Dictionary<Guid, string>();
        Task listeningTask = new Task(Listen);
        listeningTask.Wait();
    }

    public string Username
    {
        get => _emailClient.Username;
        internal set => _emailClient.Username = value;
    }

    internal string Password
    {
        get => _emailClient.Password;
        set => _emailClient.Password = value;
    }

    internal int Port
    {
        get => _emailClient.Port;
        set => _emailClient.Port = value;
    }

    public bool UseOAuth
    {
        get => _emailClient.UseOAuth;
        internal set => _emailClient.UseOAuth = value;
    }
    
    public string Host
    {
        get => _emailClient.Host;
        internal set => _emailClient.Host = value;
    }
    
    internal string AccessToken
    {
        get => _emailClient.AccessToken;
        set => _emailClient.AccessToken = value;
    }

    internal ImapFolder ActiveFolder => _emailClient.ActiveFolder;

    public int Timeout => _emailClient.Timeout;

    private static string DataLoginLocation => "../../../Services/Data/EmailAccount.txt";

    private string DataEmailDataLocation => "../../../Services/Data/EmailData.txt";
    
    public ConnectionProtocols ConnectionProtocols
    {
        get => _emailClient.ConnectionProtocols;
        internal set => _emailClient.ConnectionProtocols = value;
    }

    public bool IsSsl => ConnectionProtocols == ConnectionProtocols.Ssl;

    public bool HasNoConnectionProtocol => ConnectionProtocols == ConnectionProtocols.None;

    public bool IsStartTls => ConnectionProtocols == ConnectionProtocols.StartTls;
    
    internal void Connect()
    {
        _emailClient.Connect();
        _emailClient.Login();
    }

    internal void Login()
    {
        _emailClient.Login();
    }

    internal void Disconnect()
    {
        _emailClient.Disconnect();
    }

    internal void Copy(int sequenceNumber, string destinationFolderName)
    {
        _emailClient.Copy(sequenceNumber, destinationFolderName);
    }
    
    internal void Copy(string folderUid, string destinationFolderName)
    {
        _emailClient.Copy(folderUid, destinationFolderName);
    }

    internal ImapMessageCollection Search(string query)
    {
        ImapMessageCollection searchResults = _emailClient.Search(query);
        return searchResults;
    }

    internal void CreateFolder(string folderName)
    {
        _emailClient.CreateFolder(folderName);
    }
    
    internal void DeleteFolder(string folderName)
    {
        _emailClient.DeleteFolder(folderName);
    }
    
    internal void RenameFolder(string folderName, string newName)
    {
        _emailClient.RenameFolder(folderName, newName);
    }

    internal void SelectFolder(string folderName)
    {
        _emailClient.Select(folderName);
    }

    internal void Subscribe(string folderName)
    {
        _emailClient.Subscribe(folderName);
    }

    internal void Unsubscribe(string folderName)
    {
        _emailClient.Unsubscribe(folderName);
    }

    internal void GetAttachment(int sequenceNumber, string attachmentName)
    {
        _emailClient.GetAttachment(sequenceNumber, attachmentName);
    }

    internal void DeleteMarkedMessages()
    {
        _emailClient.DeleteMarkedMessages();
    }

    internal ImapFolderCollection GetFolders()
    {
        ImapFolderCollection folders = _emailClient.GetFolderCollection();
        return folders;
    }

    internal Spire.Email.MailMessage GetFullMessage(int sequenceNumber)
    {
        Spire.Email.MailMessage fullMessage = _emailClient.GetFullMessage(sequenceNumber);
        return fullMessage;
    }
    
    internal Spire.Email.MailMessage GetFullMessage(string uid)
    {
        Spire.Email.MailMessage fullMessage = _emailClient.GetFullMessage(uid);
        return fullMessage;
    }

    internal int NumberOfMessages(string folderName)
    {
        return _emailClient.GetMessageCount(folderName);
    }
    
    internal Spire.Email.MailMessage GetMessageText(int sequenceNumber)
    {
        Spire.Email.MailMessage fullMessage = _emailClient.GetMessageText(sequenceNumber);
        return fullMessage;
    }

    internal void MarkAsDeleted(int sequenceNumber)
    {
        _emailClient.MarkAsDeleted(sequenceNumber);
    }
    
    internal void MarkAsSeen(int sequenceNumber)
    {
        _emailClient.MarkAsDeleted(sequenceNumber);
    }
    
    internal void MarkAsUndeleted(int sequenceNumber)
    {
        _emailClient.MarkAsUndeleted(sequenceNumber);
    }
    
    internal void MarkAsUnseen(int sequenceNumber)
    {
        _emailClient.MarkAsUnseen(sequenceNumber);
    }

    internal ImapMessageCollection GetMessageCollection()
    {
        ImapMessageCollection messageCollection = _emailClient.GetAllMessageHeaders();
        return messageCollection;
    }

    internal async Task SendMessage(string header, string message)
    {
        Spire.Email.MailAddress fromAddress = new(Username);
        Spire.Email.MailAddress toAddress = new(Username);
        MailMessage mailMessage = new (fromAddress, toAddress);
        mailMessage.Subject = header;
        mailMessage.BodyText = message;

        void SendMessageAction() => _smtpClient.SendOne(mailMessage);
        Task sendMessageTask = new(SendMessageAction);
        await sendMessageTask;
    }

    internal async Task<string> Communicate(string header, string message)
    {
        Guid guid = Guid.NewGuid();
        string fullMessage = $"+{guid}:{message}";
        await SendMessage(header, fullMessage);

        string WaitForResponse()
        {
            int timeElapsed = 0;
            while (!_responses.ContainsKey(guid) && timeElapsed < 5000)
            {
                Thread.Sleep(5);
                timeElapsed += 5;
            }

            if (timeElapsed == 5000)
                return "TIMEOUT";

            string response = _responses[guid];
            return response;
        }

        Task<string> waitForResponseTask = new (WaitForResponse);
        return await waitForResponseTask;
    }

    internal async void Listen()
    {
        void ListenActivity()
        {
            while (ShouldListen)
            {
                int numberOfMessages = NumberOfMessages("Inbox");
                
                for (; _numberOfAnalysedMessages < numberOfMessages; _numberOfAnalysedMessages += 1)
                {
                    int sequenceNumber = numberOfMessages - _numberOfAnalysedMessages;
                    MailMessage message = GetFullMessage(sequenceNumber);
                    string messageBody = message.BodyText;
                    string[] messageComponents = messageBody.Split(':');
                    string identifierComponent = messageComponents[0];
                    int identifierComponentInt = identifierComponent.Length;

                    char leadingChar = identifierComponentInt > 0 ? identifierComponent[0] : '+';

                    AnalyseMessage(leadingChar, identifierComponent, messageComponents, message);
                }
            }
        }

        Task listenTask = new(ListenActivity);
        await listenTask;
    }

    private void AnalyseMessage(char leadingChar, string identifierComponent, string[] messageComponents,
        MailMessage message)
    {
        if (leadingChar == '-')
        {
            identifierComponent = identifierComponent.Remove(0);
            (string extractedMessage, Guid? guid) = ExtractMessage(identifierComponent, messageComponents);

            if (extractedMessage != "")
                _responses[(Guid) guid] = extractedMessage;
        }
        else if (leadingChar == '+')
        {
            identifierComponent = identifierComponent.Remove(0);
            (string extractedMessage, Guid? guid) = ExtractMessage(identifierComponent, messageComponents);

            if (extractedMessage != "")
            {
                string response = AnalyseRequest(message.Subject, extractedMessage);
                SendMessage(message.Subject, $"-{(Guid) guid}:{response}");
            }
        }
    }

    private (string, Guid?) ExtractMessage(string identifierComponent, string[] messageComponents)
    {
        try
        {
            Guid guid = Guid.Parse(identifierComponent);
            int numberOfComponents = messageComponents.Length;
            string messageComponent = numberOfComponents == 1 ? "" : messageComponents[1];
            return (messageComponent, guid);
        }
        catch (ArgumentNullException _)
        {
        }
        catch (FormatException _)
        {
        }

        return ("", null);
    }

    private string AnalyseRequest(string header, string message)
    {
        string response = "UNRECOGNIZED HEADER";
        if (header == "MAZZIN")
            response = MazzinRemoteRequestHandler.Handle(message);
        return response;
    }
}