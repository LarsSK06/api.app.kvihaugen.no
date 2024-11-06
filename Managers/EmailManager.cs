using KvihaugenAppAPI.Utilities;
using System.Net.Mail;
using System.Net;

namespace KvihaugenAppAPI.Managers;

public readonly struct EmailManager{

    public static async Task SendEmailAsync(string recipient, string subject, string content){
        string sender = "no-reply@kvihaugen.no";
        string password = (await Env.GetVariableAsync("EmailPassword"))!;

        SmtpClient client = new("smtp.domeneshop.no", 587){
            EnableSsl = true,
            Credentials = new NetworkCredential(sender, password)
        };

        await client.SendMailAsync(new MailMessage(
            from: sender,
            to: recipient,
            subject,
            body: content
        ));
    }

}