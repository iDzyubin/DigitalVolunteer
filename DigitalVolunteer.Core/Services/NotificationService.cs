using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace DigitalVolunteer.Core.Services
{
    public class SmtpSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string BaseUrl { get; set; }
    }

    public class NotificationService
    {
        private readonly SmtpSettings _settings;

        public NotificationService( SmtpSettings settings )
        {
            _settings = settings;
        }

        public void SendAccountConfirmation( string email, string confirmUrl )
        {
            var content = new StringBuilder();
            content.AppendLine( "Ваш адрес был указан при регистрации на портале \"Цифровое Волонтерство\"." );
            content.AppendLine( "Для завершения регистрации перейдите по указанной ссылке:" );
            content.AppendLine( _settings.BaseUrl.TrimEnd( '/' ) + confirmUrl );
            SendMessage( email, "Регистрация в системе \"Цифровое Волонтерство\"", content.ToString() );
        }

        private void SendMessage( string receiver, string subject, string content )
        {
            var message = new MimeMessage();
            message.From.Add( new MailboxAddress( "Цифровое Волонтерство", _settings.Address ) );
            message.To.Add( new MailboxAddress( receiver ) );
            message.Subject = subject;
            message.Body = new TextPart( "plain" ) { Text = content };

            using( var client = new SmtpClient() )
            {
                client.Connect( _settings.HostName, _settings.Port, _settings.EnableSsl );
                client.Authenticate( _settings.Login, _settings.Password );

                client.Send( message );
                client.Disconnect( true );
            }
        }
    }
}
