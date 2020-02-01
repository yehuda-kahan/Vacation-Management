using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Email
    {
        PersonBO clientPerson;


        public string ToEmailAdd { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string HostName { get; set; }
        public HostingUnitBO Unit { get; set; }



        /// <summary>
        /// constractor.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="host"></param>
        /// <param name="unit"></param>
        public Email(GuestRequestBO request, HostingUnitBO unit, HostBO host, PersonBO clientPerson)
        {
            HostName = host.PersonalInfo.FirstName + " " + host.PersonalInfo.LastName;
            ToEmailAdd = clientPerson.Email;
            FromDate = request.EntryDate;
            ToDate = request.LeaveDate;
            Unit = unit;
        }

        /// <summary>
        /// send amil function that sends with smtp server.
        /// </summary>
        public void SendMail()
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(this.ToEmailAdd);
            mail.Priority = MailPriority.High;
            mail.From = new MailAddress("avrumi2018@gamil.com");
            mail.Subject = " Order confirmation";
            mail.Body = (@"<body style='margin: 0px;'>
      <div style='width: 5; height:200; padding:10px; border-radius: 10px; border:solid 2px #C0C0C0;'>
           <span>Hello !</span><br />
           <span>My name is : </span>          
           <span>" + HostName + @"</span><br />
           <span>I want to offer you my hosting unit - 
           <span>" + Unit.HostingUnitName + @"</span><br />
           <span>Whose location is in</span>
           <span>" + Unit.Area + @"</span><br />
           <span>From : </span>
           <span>" + FromDate.ToString(format: "dd/MM/yyyy") + @"</span><br />
           <span>To: </span>
           <span>" + ToDate.ToString(format: "dd/MM/yyyy") + @"</span><br />
           <span>as requested.</span><br />
           <span>If this suggestion is relevant to you, I would be happy to contact me in response to this email</span><br />
           <span>Thank you and have a nice day</span><br />
      </div>
</body>");
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("avrumi2018@gmail.com", "Aa5711268!");
            smtp.EnableSsl = true;

            try { smtp.Send(mail); }
            catch (ArgumentNullException ex) { throw ex; }
            catch (InvalidOperationException ex) { throw ex; }
            catch (SmtpException ex) { throw ex; }
        }
    }
}
