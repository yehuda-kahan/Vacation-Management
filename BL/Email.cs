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
       
        public string ToEmailAdd { get; set; }
        public string FromEmailAdd { get; set; }
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
            FromEmailAdd = host.PersonalInfo.Email;
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
            mail.From = new MailAddress(FromEmailAdd);
            mail.Subject = " Order confirmation";
            mail.Body = (@"<body style='margin: 0px;'>
      <div style='width: 5; height:200; padding:10px; border-radius: 10px; border:solid 2px #C0C0C0;'>
           <span>שלום וברכה !</span><br />
           <span>שמי : </span>          
           <span>" + HostName + @"</span><br />
           <span>אני רוצה להציע לך את יחידת האירוח שלי - <span>
           <span>" + Unit.HostingUnitName + @"</span><br />
           <span>שנמצאת ב -</span>
           <span>" + Unit.Area + @"</span><br />
           <span>מתאריך : </span>
           <span>" + FromDate.ToString(format: "dd/MM/yyyy") + @"</span><br />
           <span>עד תאריך :</span>
           <span>" + ToDate.ToString(format: "dd/MM/yyyy") + @"</span><br />
           <span>כפי בקשתך.</span><br />
           <span>אם הצעה זו עדיין רלוונטי בשבילך, אשמח שתחזיר לי תשובה במייל המצורף</span><br />
           <span>תודה רבה ושיהיה לך יום נפלא !</span><br />
      </div>
</body>");
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential();
            smtp.EnableSsl = true;

            try { smtp.Send(mail); }
            catch (ArgumentNullException ex) { throw ex; }
            catch (InvalidOperationException ex) { throw ex; }
            catch (SmtpException ex) { throw ex; }
        }
    }
}
