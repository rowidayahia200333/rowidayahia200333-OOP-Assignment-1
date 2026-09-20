
namespace Part1_ProcedualToOOP
{ 
    public class Program
    {
        public static void Main()
        {

            OrderSystem order = new OrderSystem();


            order.SeedSampleData();

            order.RunDemoScenario();

            order.PrintCustomers();

            order.PrintProducts();

            order.PrintAllOrders();

            Console.WriteLine(
                $"\nPaid sales total after demo: " +
                $"{order.TotalSalesPaidOnly()}"
            );

            order.RunInteractiveMenu();
        }
    }


}


public class Customer
{
    public int Id { get; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string City { get; private set; }
    public bool IsVip { get; }

    public Customer(int id, string name, string email, string city, bool isVip)
    {
        if(id >0 ) Id = id;

        Name = name;
        Email = email;
        City = city;
        IsVip = isVip;
    }
}


public class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Stock { get; private set; }

    public Product(int id, string name, decimal price, int stock)
    {
        if (id > 0) Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }

    public bool HasEnoughStock(int quantity)
    {
        return Stock >= quantity;
    }

    public void ReduceStock(int quantity)
    {
        Stock -= quantity;
    }
}


public class OrderLine
{
    public Product Product { get; }
    public int Quantity { get; }

    public OrderLine(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public decimal CalculateTotal()
    {
        return Product.Price * Quantity;
    }
}


public class Order
{
    public int Id { get; }
    public Customer Customer { get; }
    public string Date { get; }
    public bool IsPaid { get; private set; }

    private List<OrderLine> lines = new List<OrderLine>();

    public Order(int id, Customer customer, string date)
    {
        Id = id;
        Customer = customer;
        Date = date;
        IsPaid = false;
    }

    public void AddLine(Product product, int quantity)
    {
        if (IsPaid)
        {
            Console.WriteLine("ERROR: cannot change a paid order.");
            return;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("ERROR: quantity must be positive.");
            return;
        }

        if (!product.HasEnoughStock(quantity))
        {
            Console.WriteLine(
                $"ERROR: not enough stock for product #{product.Id}."
            );
            return;
        }

        product.ReduceStock(quantity);

        lines.Add(new OrderLine(product, quantity));
    }

    public decimal CalculateTotal()
    {
        decimal total = 0m;

        foreach (OrderLine line in lines)
        {
            total += line.CalculateTotal();
        }

        if (Customer.IsVip)
        {
            total *= 0.90m;
        }

        return total;
    }

    public void MarkAsPaid()
    {
        if (lines.Count == 0)
        {
            Console.WriteLine(" cannot pay an empty order.");
            return;
        }

        IsPaid = true;
    }

    public void Print()
    {
        Console.WriteLine($"Order {Id}");
        Console.WriteLine($"Date: {Date}");

        Console.WriteLine($"Customer: {Customer.Name} (#{Customer.Id})");



        Console.WriteLine($"Paid: {(IsPaid ? "yes" : "no")}");
        Console.WriteLine("Lines:");

        Console.WriteLine();

        foreach (OrderLine line in lines)
        {
            decimal lineTotal = line.CalculateTotal();
            Console.WriteLine(
                $" - {line.Product.Name} x{line.Quantity} " +
                $"@{line.Product.Price:F2} = {lineTotal}"
            );
        }

        Console.WriteLine($"TOTAL: {CalculateTotal()}");
    }
}


public class OrderSystem
{
    private List<Customer> customers = new List<Customer>();
    private List<Product> products = new List<Product>();
    private List<Order> orders = new List<Order>();

    public void AddCustomer(int id, string name, string email, string city, bool isVip)
    {
        if (FindCustomerById(id) != null)
        {
            Console.WriteLine($"customer id {id} already exists.");

            return;
        }

        customers.Add(
            new Customer(id, name, email, city, isVip)
        );
    }

    public void PrintCustomers()
    {
        Console.WriteLine(
            $"\n=== CUSTOMERS ({customers.Count}) ==="
        );

        foreach (Customer customer in customers)
        {
            Console.WriteLine(
                $"#{customer.Id}  " +
                $"{customer.Name}  " +
                $"<{customer.Email}>  " +
                $"{customer.City}  " +
                $"vip={(customer.IsVip ? "yes" : "no")}"
            );
        }
    }

    public void AddProduct(
        int id,
        string name,
        decimal price,
        int stock)
    {
        if (FindProductById(id) != null)
        {
            Console.WriteLine(
                $"ERROR: product id {id} already exists."
            );
            return;
        }

        products.Add(
            new Product(id, name, price, stock)
        );
    }

    public void PrintProducts()
    {
        Console.WriteLine(
            $"\n=== PRODUCTS ({products.Count}) ==="
        );

        foreach (Product product in products)
        {
            Console.WriteLine(
                $"#{product.Id}  " +
                $"{product.Name}  " +
                $"price={product.Price}  " +
                $"stock={product.Stock}"
            );
        }
    }

    public Order? CreateOrder(int orderId, int customerId, string date)
    {
        if (FindOrderById(orderId) != null)
        {
            Console.WriteLine($" order id {orderId} already exists.");

            return null;
        }

        Customer? customer = FindCustomerById(customerId);

        if (customer == null)
        {
            Console.WriteLine($" customer id {customerId} not found.");

            return null;
        }

        Order? order = new Order(orderId, customer , date);


        orders.Add(order);

        return order;
    }

    public void AddLineToOrder(
        int orderId,
        int productId,
        int quantity)
    {
        Order? order = FindOrderById(orderId);

        if (order == null)
        {
            Console.WriteLine($" order id {orderId} not found.");

            return;
        }

        Product? product = FindProductById(productId);

        if (product == null)
        {
            Console.WriteLine($"product id {productId} not found.");



            return;
        }

        order.AddLine(product, quantity);
    }
    public void MarkOrderPaid(int orderId)
    {
        Order? order = FindOrderById(orderId);

        if (order == null)
        {
            Console.WriteLine($" order id {orderId} not found.");

            return;
        }

        order.MarkAsPaid();
    }

    public void PrintOrder(int orderId)
    {
        Order? order = FindOrderById(orderId);

        if (order == null)
        {
            Console.WriteLine($" order id {orderId} not found.");



            return;
        }

        order.Print();
    }

    public void PrintAllOrders()
    {
        Console.WriteLine(
            $"\n=== ALL ORDERS ({orders.Count}) ==="
        );

        foreach (Order order in orders)
        {
            order.Print();
        }
    }

    public decimal TotalSalesPaidOnly()
    {
        decimal sum = 0m;

        foreach (Order order in orders)
        {
            if (order.IsPaid)
            {
                sum += order.CalculateTotal();
            }
        }

        return sum;
    }

    private Customer? FindCustomerById(int id)
    {
        foreach (Customer customer in customers)
        {
            if (customer.Id == id)
            {
                return customer;
            }
        }

        return null;
    }

    private Product? FindProductById(int id)
    {
        foreach (Product product in products)
        {
            if (product.Id == id)
            {
                return product;
            }
        }

        return null;
    }

    private Order? FindOrderById(int id)
    {
        foreach (Order order in orders)
        {
            if (order.Id == id)
            {
                return order;
            }
        }

        return null;
    }

    
    public void SeedSampleData()
    {
        AddCustomer(
            1,
            "Mona Ali",
            "mona@example.com",
            "Cairo",
            true
        );

        AddCustomer(
            2,
            "Omar Hassan",
            "omar@example.com",
            "Alexandria",
            false
        );

        AddCustomer(
            3,
            "Sara Nabil",
            "sara@example.com",
            "Giza",
            false
        );

        AddProduct(
            101,
            "USB Cable",
            50.0m,
            100
        );

        AddProduct(
            102,
            "Wireless Mouse",
            250.0m,
            40
        );

        AddProduct(
            103,
            "Mechanical Keyboard",
            1200.0m,
            15
        );

        AddProduct(
            104,
            "Laptop Stand",
            400.0m,
            25
        );
    }

    public void RunDemoScenario()
    {
        CreateOrder(
            1001,
            1,
            "2026-09-15"
        );

        AddLineToOrder(
            1001,
            101,
            2
        );

        AddLineToOrder(
            1001,
            102,
            1
        );

        MarkOrderPaid(1001);


        CreateOrder(
            1002,
            2,
            "2026-09-15"
        );
        AddLineToOrder(
            1002,
            103,
            1
        );

        AddLineToOrder(
            1002,
            104,
            1
        );


        CreateOrder(
            1003,
            3,
            "2026-09-16"
        );

        AddLineToOrder(
            1003,
            101,
            5
        );

        MarkOrderPaid(1003);
    }

  

    public void PrintMenu()
    {
        Console.WriteLine("\n---------- MENU ----------");
        Console.WriteLine("1) Print customers");
        Console.WriteLine("2) Print products");
        Console.WriteLine("3) Print all orders");
        Console.WriteLine("4) Print one order by id");
        Console.WriteLine("5) Create order");
        Console.WriteLine("6) Add line to order");
        Console.WriteLine("7) Mark order paid");
        Console.WriteLine("8) Show paid sales total");
        Console.WriteLine("0) Exit");
        Console.Write("Choice: ");
    }

    public void RunInteractiveMenu()
    {
        int choice = -1;

        while (choice != 0)
        {
            PrintMenu();

            choice = int.Parse(Console.ReadLine()!);

            if (choice == 1)
            {
                PrintCustomers();
            }
            else if (choice == 2)
            {
                PrintProducts();
            }
            else if (choice == 3)
            {
                PrintAllOrders();
            }
            else if (choice == 4)
            {
                Console.Write("Order id: ");

                int orderId =
                    int.Parse(Console.ReadLine()!);

                PrintOrder(orderId);
            }
            else if (choice == 5)
            {
                Console.Write("Order id: ");
                int orderId =
                    int.Parse(Console.ReadLine()!);

                Console.Write("Customer id: ");
                int customerId =
                    int.Parse(Console.ReadLine()!);

                Console.Write("Date (YYYY-MM-DD): ");
                string date =
                    Console.ReadLine()!;

                CreateOrder(
                    orderId,
                    customerId,
                    date
                );
            }
            else if (choice == 6)
            {
                Console.Write("Order id: ");
                int orderId =
                    int.Parse(Console.ReadLine());

                Console.Write("Product id: ");
                int productId =
                    int.Parse(Console.ReadLine());

                Console.Write("Quantity: ");
                int quantity =
                    int.Parse(Console.ReadLine());

                AddLineToOrder(
                    orderId,
                    productId,
                    quantity
                );
            }
            else if (choice == 7)
            {
                Console.Write("Order id: ");

                int orderId =
                    int.Parse(Console.ReadLine());

                MarkOrderPaid(orderId);
            }
            else if (choice == 8)
            {
                Console.WriteLine(
                    $"Paid sales total: " +
                    $"{TotalSalesPaidOnly()}"
                );
            }
            else
            {
                Console.WriteLine("Wrong choice.");
            }
        }
    }
}

