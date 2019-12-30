using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public enum IdTypes { ID, PASSPORT, DRIVING_LICENSE }

    public enum Status { ACTIVE, INACTIVE }

    public enum UnitType { ZIMMER, HOTEL, CAMPING, APARTMENT }

    public enum RequestStatus { OPEN, EXPIRED, CANCELLED, ORDERED }

    public enum OrderStatus { PROCESSING, MAIL_SENT, APPROVED, NO_CLIENT_RESPONSE }

    public enum AreaLocation { ALL, NORTH, SOUTH, CENTER, JERUSALEM }

    public enum ThreeOptions { YES, MAYBE, NO }
}
