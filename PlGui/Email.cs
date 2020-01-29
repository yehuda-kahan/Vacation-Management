using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace PlGui
{
    class Email
    {
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
        public Email(GuestRequestBO request, HostingUnitBO unit)
        {
            FromDate = request.EntryDate;
            ToDate = request.LeaveDate;
            //HostName = host;
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
           <span>" + this.HostName + @"</span><br />
           <span>I want to offer you my hosting unit in :</span>
           <span>" + Unit.Area + @"</span>
           <span>In Unit: </span>
           <span>" + this.Unit + @"</span><br />
           <span>From: </span>
           <span>" + this.FromDate + @"</span><br />
           <span>To: </span>
           <span>" + this.ToDate + @"</span><br />
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

            smtp.Send(mail);
        }
    }
}

