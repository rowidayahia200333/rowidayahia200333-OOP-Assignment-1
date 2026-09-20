
namespace OOP_Design_Assignment
{

    class Program
    {
        static void Main(string[] args)
        {

           
                
                Invoice invoice = new InvoiceBuilder("INV-1001", "Rowida Yahia")
                    .WithCustomerInfo("rowida@example.com", "01012345678")
                    .WithBillingAddress(address => address
                        .WithStreet("123 Gamal Abd El-Naser St")
                        .WithCity("Alexandria")
                        .WithState("Alexandria Governorate")
                        .WithZipCode("21500")
                        .WithCountry("Egypt"))
                    .WithShippingAddress(address => address
                        .WithStreet("456 Corniche St")
                        .WithCity("Alexandria")
                        .WithCountry("Egypt"))
                    .WithAmounts(1000m, 140m)
                    .Build();

                Console.WriteLine("=== Invoice Details ===");
                Console.WriteLine($"ID: {invoice.InvoiceId}");
                Console.WriteLine($"Customer: {invoice.CustomerName}");
                Console.WriteLine($"Email: {invoice.CustomerEmail}");
                Console.WriteLine($"Billing City: {invoice.BillingAddress?.City}");
                Console.WriteLine($"Shipping Street: {invoice.ShippingAddress?.Street}");
                Console.WriteLine($"Total Amount: {invoice.TotalAmount}");
            
        }


    }



    public class Invoice
    {
        public string InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public Address BillingAddress { get; set; } 
        public Address ShippingAddress { get; set; } 


        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        
    }

    public class InvoiceBuilder
    {
        // mandatory fields
        private readonly string _invoiceId;
        private readonly string _customerName;

        // Optional Fields
        private string _customerEmail ;
        private string _customerPhone ;
        private Address _billingAddress ;
        private Address _shippingAddress ;
        private decimal _subTotal;
        private decimal _taxAmount;
        

        public InvoiceBuilder(string invoiceId, string customerName)
        {
           
            _invoiceId = invoiceId;
            _customerName = customerName;
        }

        public InvoiceBuilder WithCustomerInfo(string email, string phone)
        {
            _customerEmail = email;
            _customerPhone = phone;
            return this;
        }

        public InvoiceBuilder WithBillingAddress(Action<AddressBuilder> action)
        {
            var builder = new AddressBuilder();
            action(builder);
            _billingAddress = builder.Build(); 
            return this;
        }

        public InvoiceBuilder WithShippingAddress(Action<AddressBuilder> action)
        {
            var builder = new AddressBuilder();
            action(builder);
            _shippingAddress = builder.Build(); 
            return this;
        }

        public InvoiceBuilder WithAmounts(decimal subTotal, decimal taxAmount)
        {
            _subTotal = subTotal;
            _taxAmount = taxAmount;
            return this;
        }

        public Invoice Build()
        {
            return new Invoice
            {
                InvoiceId = _invoiceId,
                CustomerName = _customerName,
                CustomerEmail = _customerEmail,
                CustomerPhone = _customerPhone,
                BillingAddress = _billingAddress,
                ShippingAddress = _shippingAddress,
                SubTotal = _subTotal,
                TaxAmount = _taxAmount,
                TotalAmount = _subTotal + _taxAmount
            };
        }
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class AddressBuilder
    {
        private readonly Address _address = new();

        public AddressBuilder WithStreet(string street)
        {
            _address.Street = street;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _address.City = city;
            return this;
        }
        public AddressBuilder WithState(string state)
        {
            _address.State = state;
            return this;
        }

        public AddressBuilder WithZipCode(string zipCode)
        {
            _address.ZipCode = zipCode;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            _address.Country = country;
            return this;
        }

        public Address Build()
        {
            
            return _address;
        }
    }
}
