using Microsoft.Win32.SafeHandles;
List<Product> products = new List<Product>()
{
    new Product()
    {
        Name = "Football",
        Price = 15.00M,
        Sold = false,
        StockDate = new DateTime(2022, 10, 20),
        ManufactureYear = 2010,
        Condition = 4.2
    },
    new Product()
    {
        Name = "Hockey Stick",
        Price = 12.00M,
        Sold = false,
        StockDate = new DateTime(2021, 9, 15),
        ManufactureYear = 2018,
        Condition = 4.7
    },
    new Product()
    {
        Name = "Boomerang",
        Price = 11.00M,
        Sold = true,
        StockDate = new DateTime(2023, 06, 12),
        ManufactureYear = 2022,
        Condition = 3.2
    },
    new Product()
    {
        Name = "Soccer Ball",
        Price = 8.00M,
        Sold = false,
        StockDate = new DateTime(2018, 4, 20),
        ManufactureYear = 2015,
        Condition = 2.5
    },
    new Product()
    {
        Name = "Basketball",
        Price = 10.00M,
        Sold = true,
        StockDate = new DateTime(2019, 3, 14),
        ManufactureYear = 2017,
        Condition = 4.3
    },
    new Product()
    {
        Name = "Glove",
        Price = 6.00M,
        Sold = true,
        StockDate = new DateTime(2020, 12, 15),
        ManufactureYear = 2016,
        Condition = 3.8
    },
    new Product()
    {
        Name = "Hat",
        Price = 17.00M,
        Sold = true,
        StockDate = new DateTime(2023, 10, 15),
        ManufactureYear = 2023,
        Condition = 3.9
    },
    new Product()
    {
        Name = "Helmet",
        Price = 13.00M,
        Sold = false,
        StockDate = new DateTime(2023, 9, 15),
        ManufactureYear = 2023,
        Condition = 3.6
    }
};


string greeting = @"Welcome to Thrown for a Loop
Your one stop shop for used sporting equipment";

Console.WriteLine(greeting);
string choice = null;
while (choice != "0")
{
    Console.WriteLine(@"Choose an option:
    0. Exit
    1. View All Products
    2. View Product Details
    3. Latest Products");
    choice = Console.ReadLine();
    if (choice == "0")
    {
        Console.WriteLine("Goodbye!");
    }
    else if (choice == "1")
    {
        ListProducts();
    }
    else if (choice == "2")
    {
        ViewProductDetails();
    }
    else if (choice =="3")
    {
        ViewLatestProducts();
    }
}


void ViewProductDetails()
{
   ListProducts();
    

    Product chosenProduct = null;

    while (chosenProduct == null)
    {
        Console.WriteLine("Please enter a product number: ");
        try
        {
            //this converts the users response to an integer
            int response = int.Parse(Console.ReadLine().Trim());
            //the array is indexex by [response - 1] bc array indices usually start from zero
            //but user friendly product numbers might start from 1
            chosenProduct = products[response - 1];
        }
        catch (FormatException)
        {
            Console.WriteLine("Please type only integers!");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Please choose an existing item only!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Do better!");
        }
    }


    // We are calculating the time that the item has been in stock by
    // subtracting the StockDate of our product from now. 
    // When you do math on two DateTimes, the result is a TimeSpan. 
    // TimeSpans also have a Days property, which represents the total number of days in that time span. 
    // We are accessing the Year property on now to get the current year, and subtracting the Manufacture year of the chosen product to give us the product's current age. 
    // In the ternary, we are checking to see if the product is sold. If it is, we display "not available". 
    // Otherwise, we access the timeInStock variable's Days property to get the total number of days between the stock date and now. 
    DateTime now = DateTime.Now;
    TimeSpan timeInStock = now - chosenProduct.StockDate;
    Console.WriteLine(@$"You chose: 
{chosenProduct.Name}, which costs {chosenProduct.Price} dollars.
It is {now.Year - chosenProduct.ManufactureYear} years old. 
It {(chosenProduct.Sold ? "is not available." : $"has been in stock for {timeInStock.Days} days.")}");

}

void ListProducts()
{
    decimal totalValue = 0.0M;
    foreach (Product product in products)
    {
        if (!product.Sold)
        {
            totalValue += product.Price;
        }
    }
    Console.WriteLine($"Total inventory value: ${totalValue}");
    Console.WriteLine("Products:");
    for (int i = 0; i < products.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {products[i].Name}");
    }
}

void ViewLatestProducts()
{
    List<Product> latestProducts = new List<Product>();
    DateTime threeMonthsAgo = DateTime.Now - TimeSpan.FromDays(90);
    foreach (Product product in products)
    {
        if (product.StockDate > threeMonthsAgo && !product.Sold)
        {
            latestProducts.Add(product);
        }
    }
    for (int i = 0; i < latestProducts.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {latestProducts[i].Name}");
    }
}