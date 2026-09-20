namespace Part2_HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing Hotel Reservation System ===");

            try
            {
       
                Room room101 = new Room(101, RoomType.Single, 450m);
                Guest guest = new Guest("Rowida Yahia", "01000000000");

                Console.WriteLine($"Created Guest: {guest.FullName}");
                Console.WriteLine($"Created Room: #{room101.roomNumber} (Rate: {room101.NightlyRate} EGP)");

                DateTime checkIn = DateTime.Now;
                DateTime checkOut = checkIn.AddDays(3);

                Reservation res1 = new Reservation(checkIn, checkOut, ref room101);
                guest.AddReservation(res1);

                Console.WriteLine($"\n[Reservation Created]");
                Console.WriteLine($"Total Nights: {(res1.Check_out - res1.Check_in).Days}");
                Console.WriteLine($"Total Cost: {res1.TotalCost} EGP");
                Console.WriteLine($"Status: {res1.Status}");

                res1.Confirm();
                Console.WriteLine($"Status after Confirm: {res1.Status}");

                res1.CheckIn();
                Console.WriteLine($"Status after CheckIn: {res1.Status}");

                Console.WriteLine("\n--- Testing Double-Booking Prevention ---");
                try
                {
                   
                    Guest guest2 = new Guest("Ahmed Ali", "01111111111");
                    Reservation res2 = new Reservation(checkIn.AddDays(1), checkOut.AddDays(1),ref room101);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Success (Caught Error): {ex.Message}");
                }

              
                res1.CheckOut();
                Console.WriteLine($"\nStatus after CheckOut: {res1.Status}");

              
                Console.WriteLine("\n--- Testing Booking After CheckOut ---");
                Reservation res3 = new Reservation(checkOut, checkOut.AddDays(2),ref room101);
                guest.AddReservation(res3);
                Console.WriteLine($"New Reservation Created Successfully! Cost: {res3.TotalCost} EGP");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }







    public enum RoomType
    {
        Single,
        Double,
        Suite
    }
    public class Room
    {
       public int roomNumber{ get; set; }
        public RoomType roomType { get; }
        public decimal NightlyRate { get; private set; }
        public bool isUnderMaintaince { get; private set; }

        public Room(int roomnum, RoomType type, decimal nightlyRate)
        {
            roomNumber = roomnum;
            roomType = type;
            if (nightlyRate > 0) NightlyRate = nightlyRate;
            isUnderMaintaince = false;
        }

        public void UpdateRate(decimal newRate)
        {
            if (newRate > 0) NightlyRate = newRate;
            else throw new ArgumentException("Nightly rate must be a positive value.");
        }

        public void StartMaintance() => isUnderMaintaince = true;
        public void EndMaintance() => isUnderMaintaince = false;




        private List<Reservation> _roomReservations = new List<Reservation>[];
        public IReadOnlyList<Reservation> ReservationsOfRoom => _roomReservations;

        public bool HasOverlap(DateTime checkIn, DateTime checkOut)
        {
            return _roomReservations.Any(r =>
                r.Status != ReservationStatus.Cancelled &&
                r.Status != ReservationStatus.CheckedOut &&
                checkIn < r.Check_out &&
                checkOut > r.Check_in
            );
        }
        public void AddReservation(Reservation reservation)
        {
            if (reservation != null)
                _roomReservations.Add(reservation);
        }

    }

    public enum ReservationStatus{
        pending,
        confirmed,
        checkedIn,
        checkedOut,
        cancelled
    }
    public class Reservation
    {
        private Guid Id { get; }
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
        public  ReservationStatus Status { get; private set; }
        private  Room room { get; }

        public Reservation(DateTime checkIn , DateTime checkOut ,ref Room _room) 
        {
            if (checkOut <= checkIn) throw new ArgumentException("This room not checked in already.");

            if(!_room.isUnderMaintaince ) room = _room;

            Id = Guid.NewGuid();
            Check_out = checkOut;
            Check_in = checkIn;
            Status = ReservationStatus.pending;
        }

        public void Confirm()
        {
            if (Status != ReservationStatus.pending) throw new InvalidOperationException("Only pending reservation can be confirmed.");

            Status = ReservationStatus.confirmed;
        }
        public void CheckIn() 
        {
            if (Status != ReservationStatus.confirmed)
                throw new InvalidOperationException("Cannot check in unless the reservation is confirmed.");

            Status = ReservationStatus.checkedIn; 
        }
        public void CheckOut()
        {
            if (Status != ReservationStatus.checkedIn)
                throw new InvalidOperationException("Cannot check out unless current status is checked in.");

            Status = ReservationStatus.checkedOut;
        }
        public void Cancel()
        {
            if (Status != ReservationStatus.pending && Status != ReservationStatus.confirmed)
                throw new InvalidOperationException("Can only cancel a reservation if it is Pending or Confirmed.");

            Status = ReservationStatus.cancelled;
        }

        public decimal TotalCost
        {
            get
            {
                int nights = (Check_out - Check_in).Days;
                return (nights * room.NightlyRate);
            }
        }
    }

    public class Guest
    {
        private Guid Id { get; }
        public string FullName { get;  }
        private string PhoneNumber{ get;  }

        private List<Reservation> _reservation = new List<Reservation>[];
        public IReadOnlyList<Reservation> ReservationsOfGuest => _reservation;

        public Guest(string name,string phonenumber) 
        {
            Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Full name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(phonenumber)) throw new ArgumentException("Phone number cannot be null or empty.");

            FullName = name;
            PhoneNumber = phonenumber;
        }

        public void AddReservation(Reservation reservation)
        {
            if(reservation != null)
            _reservation.Add(reservation);
        }
    }


}
