using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class mono_gmail : MonoBehaviour
{

    public static void Send(string address)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("PaperCastleGame@gmail.com");
        mail.To.Add(address);
        mail.Subject = "Your Paper Castle Screenshot";
        mail.Body = "Enjoy!";

        string attachmentPath = Application.persistentDataPath + "Screenshot.png";
        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
        mail.Attachments.Add(attachment);

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("PaperCastleGame@gmail.com", "P4p3rC4stl3g4m3") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }
}

