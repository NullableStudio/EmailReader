using System;
using System.Collections.Generic;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using System.IO;

namespace Prezent
{
    static class Mail
    {
        public static void Start(string SearchIndex, string Path = null)
        {
            if (Path == null)
                Path = "c:\\Users\\" + Environment.UserName + "\\Desktop\\";
            if (!Directory.Exists(Path + "Klasy\\" + SearchIndex))
                Directory.CreateDirectory(Path + "Klasy\\" + SearchIndex);
            DownloadMessages(Class.klasa.EmailAdd, Class.klasa.EmailPass, SearchIndex, Path + "Klasy\\" + SearchIndex + "\\" + DateTime.Now.ToString("dddd, MMMM yyyyr HH mm ss"));
        }

        private static void DownloadMessages(string addres, string pass, string Index, string To_File)
        {
            Directory.CreateDirectory(To_File);
            using (ImapClient client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(addres, pass);
                client.Inbox.Open(FolderAccess.ReadWrite);

                IList<UniqueId> uids = client.Inbox.Search(SearchQuery.NotSeen);

                foreach (UniqueId uid in uids)
                {
                    MimeKit.MimeMessage message = client.Inbox.GetMessage(uid);
                    // write the message to a file
                    DispachMessege(message, Index, To_File);
                }
                foreach(UniqueId uid in uids)
                {
                    client.Inbox.AddFlags(uid, MessageFlags.Seen, true);
                }
                client.Disconnect(true);
            }
        }
        private static void DispachMessege(MimeKit.MimeMessage msg, string Index, string Dir_Path)
        {
            string Theme = msg.Subject.ToUpper();
            if (!Theme.Contains(Index))
                return;
            DirectoryInfo di = Directory.CreateDirectory(Dir_Path + "\\" + Theme);
            foreach(MimeKit.MimeEntity Attach in msg.Attachments)
            {
                string fileName = Attach.ContentDisposition.FileName + Attach.ContentType.Name;
                using (FileStream stream = File.Create(di.FullName + "\\" + fileName))
                {
                    if (Attach is MimeKit.MessagePart)
                    {
                        MimeKit.MessagePart rfc822 = (MimeKit.MessagePart)Attach;
                        rfc822.Message.WriteTo(stream);
                    }
                    else
                    {
                        MimeKit.MimePart part = (MimeKit.MimePart)Attach;
                        part.Content.DecodeTo(stream);
                    }
                }
            }
        }
    }
}
