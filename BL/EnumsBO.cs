namespace BO
{
        public enum IdTypesBO { ID, PASSPORT, DRIVING_LICENSE }

        public enum StatusBO { ACTIVE , NOT_ACTIVE }

        public enum UnitTypeBO { ZIMMER, HOTEL, CAMPING, APARTMENT }

        public enum RequestStatusBO { OPEN, EXPIRED, CANCELLED, ORDERED }

        public enum OrderStatusBO { PROCESSING, MAIL_SENT, APPROVED, NO_CLIENT_RESPONSE, UNIT_NOT_AVALABELE, CANCELED }

        public enum AreaLocationBO { ALL, NORTH, SOUTH, CENTER, JERUSALEM }

        public enum ThreeOptionsBO { YES, MAYBE, NO }
    }