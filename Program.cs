using Microsoft.Win32.SafeHandles;
List<Product> products = new List<Product>()
{
    new Product()
    {
        Name = "Football",
        Price = 15.00M,
        SoldOnDate = null,
        StockDate = new DateTime(2022, 10, 20),
        ManufactureYear = 2010,
        Condition = 4.2
    },
    new Product()
    {
        Name = "Hockey Stick",
        Price = 12.00M,
        SoldOnDate = null,
        StockDate = new DateTime(2021, 9, 15),
        ManufactureYear = 2018,
        Condition = 4.7
    },
    new Product()
    {
        Name = "Boomerang",
        Price = 11.00M,
        SoldOnDate = new DateTime(2023, 3, 14, 4, 20, 00),
        StockDate = new DateTime(2023, 06, 12),
        ManufactureYear = 2022,
        Condition = 3.2
    },
    new Product()
    {
        Name = "Soccer Ball",
        Price = 8.00M,
        SoldOnDate = null,
        StockDate = new DateTime(2018, 4, 20),
        ManufactureYear = 2015,
        Condition = 2.5
    },
    new Product()
    {
        Name = "Basketball",
        Price = 10.00M,
        SoldOnDate = null,
        StockDate = new DateTime(2019, 3, 14),
        ManufactureYear = 2017,
        Condition = 4.3
    },
    new Product()
    {
        Name = "Glove",
        Price = 6.00M,
        SoldOnDate = new DateTime(2023, 9, 19, 5, 30, 00),
        StockDate = new DateTime(2020, 12, 15),
        ManufactureYear = 2016,
        Condition = 3.8
    },
    new Product()
    {
        Name = "Hat",
        Price = 17.00M,
        SoldOnDate = null,
        StockDate = new DateTime(2023, 10, 15),
        ManufactureYear = 2023,
        Condition = 3.9
    },
    new Product()
    {
        Name = "Helmet",
        Price = 13.00M,
        SoldOnDate = new DateTime(2023, 10, 31, 2, 45, 00),
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
    3. Latest Products
    4. Monthly Sales Report");
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
    else if (choice == "3")
    {
        ViewLatestProducts();
    }
    else if (choice == "4")
    {
        MonthlySalesReport();
    }
}


void ViewProductDetails()
{
    ListProducts();


    Product chosenProduct = null;
    int response = 0;

    while (chosenProduct == null)
    {
        Console.WriteLine("Please enter a product number: ");
        try
        {
            //this converts the users response to an integer
            response = int.Parse(Console.ReadLine().Trim());
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
It {(chosenProduct.SoldOnDate != null ? "is not available." : $"has been in stock for {chosenProduct.TimeInStock} days.")}");

    Console.WriteLine("Would you like to mark the item as sold? Y/N");
    string userInput = Console.ReadLine();
    if (userInput == "Y")
    {
        MarkAsSold(response);
    }
}

void ListProducts()
{
    decimal totalValue = 0.0M;
    foreach (Product product in products)
    {
        if (product.SoldOnDate != null)
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
    Console.WriteLine($"Average Product Time in Stock: {AverageTimeStocked()} days");
    Console.WriteLine($"Average Product Time on Shelf Before Sale: {AverageTimeBeforeSold()} days");
    BusiestSaleHours();
}

void ViewLatestProducts()
{
    List<Product> latestProducts = new List<Product>();
    DateTime threeMonthsAgo = DateTime.Now - TimeSpan.FromDays(90);
    foreach (Product product in products)
    {
        if (product.StockDate > threeMonthsAgo && product.SoldOnDate != null)
        {
            latestProducts.Add(product);
        }
    }
    for (int i = 0; i < latestProducts.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {latestProducts[i].Name}");
    }
}

void MonthlySalesReport()
{
    //these lines prompt user to input a year&month for sales report
    //the input is parsed into an integer and stored in 'userInputYear' and 'userInputMonth'
    Console.WriteLine("Input the year for the report you want as YYYY");
    int userInputYear = int.Parse(Console.ReadLine());
    Console.WriteLine("Input the month for the report you want as MM");
    int userInputMonth = int.Parse(Console.ReadLine());

    //this filters the products list to include only those products that were sold in the specified year&month
    //inside the Where method, a lambda expression is used to check if the product was sold int he specified year&month
    //if it was sold when specified, the product is included in the foundProuducts collection
    IEnumerable<Product> foundProducts = products.Where(product =>
    {
        if (product.SoldOnDate != null)
        {
            return product.SoldOnDate.Value.Year == userInputYear &&
                  product.SoldOnDate.Value.Month == userInputMonth;
        }
        return false;
    });

    //this calculates the total price of all products in the foundProducts collection
    //Sum method is used to sum up prices of all products in the collection
    decimal totalPrice = foundProducts.Sum(product => product.Price);
    Console.WriteLine($"Total sales for products sold in {userInputMonth}/{userInputYear}: ${totalPrice}");
}

void AddProduct()
{
    Console.WriteLine("Please enter the details of the product you want to add:");
    Console.WriteLine("What is the name of the product?");
    string productNameToAdd = Console.ReadLine();
    Console.WriteLine("What is the price of the product?");
    decimal productPriceToAdd = decimal.Parse(Console.ReadLine());
    Console.WriteLine("What is the manufacture year of the product?");
    int productManufactureYearToAdd = int.Parse(Console.ReadLine());
    Console.WriteLine("What is the condition of the product on a scale of 0 - 5?");
    double productConditionToAdd = double.Parse(Console.ReadLine());

    DateTime now = new DateTime();
    Product productToAdd = new Product()
    {
        Name = productNameToAdd,
        Price = productPriceToAdd,
        ManufactureYear = productManufactureYearToAdd,
        Condition = productConditionToAdd,
        StockDate = now,
        SoldOnDate = null
    };

    products.Add(productToAdd);
    Console.WriteLine($"Your {productNameToAdd} was successfully added.");
}

void MarkAsSold(int userInput)
{
    //this marks a product as sold
    //sets the SoldOnDate property of the product at the index userInput -1 in the products list to the current date and time
    // the newDateTime() creates a new DateTime object that represents the current date and time
    products[userInput - 1].SoldOnDate = new DateTime();
}

TimeSpan AverageTimeStocked()
{
    //this calculates total time that all products have been in stock
    //Sum sums up the Ticks of the TimeInStock property of each product in products
    //total time in ticks is then used to create a new TimeSpan object, 'totalTime'
    TimeSpan totalTime = new TimeSpan(products.Sum(p => p.TimeInStock.Ticks));
    //calculates the avarage time a product was in stock
    //divides total time that all roducts have been in stock by number of products
    //result is returned as 'TimeSpan' object
    return totalTime / (double)products.Count;
}

TimeSpan AverageTimeBeforeSold()
{
    // Allow the user to see the average amount of time the sold products were on the shelf before sale.
    var soldProducts = products.Where(p => p.SoldOnDate != null);
    // Allow the user to see the average time that currently stocked products have been on the shelf (in days).
    //this calculates total time that sold products have been in stock
    //uses Sum method to sum up the Ticks of the TimeInStock property of each product in soldProducts
    //Ticks is a property of TimeSpan that gets the number of ticks that represent the value of the current TimeSpan instance
    //A tick is a unit of time equal to 100 nano seconds or 1 10-millionth of a second
    //Sume method is then used to sum up these ticks to get total time in ticks
    //this total time in ticks is then used to create a new TimeSpan object, 'totalTime'
    TimeSpan totalTime = new TimeSpan(soldProducts.Sum(p => p.TimeInStock.Ticks));
    //coutns the number of sold products by using .Count on soldProducts
    int soldProductsCount = soldProducts.Count();
    //calculates average time a product was o shelf before it was sold
    //does so by, dividing total time that the sold products have been in stock by number of sold products
    //the result is returned as a 'TimeSpan' object
    return totalTime / (double)soldProductsCount;

}

void BusiestSaleHours()
{
    //this creates a new list of Product objects, soldProducts, from the products list
    //uses the Where method to filter out any products where SoldOnDate is null
    //ToList is converts the result into a list
    List<Product> soldProducts = products.Where(p => p.SoldOnDate != null).ToList();
    //creates the storeHours
    int[] storeHours = { 9, 10, 11, 12, 1, 2, 3, 4, 5, 6 };
    // iterate through store hours 9-6
    foreach (int h in storeHours)
    {
        //for each hour it initializes a counter, numberSold, to 0
        //this will keep track of the number of products sold during that hour
        int numberSold = 0;
        //this nested loop iterates over each product in soldProducts
        //if the hour of the products SoldOnDate matches the current hour,
        //it increments the numberSold counter
        foreach (Product p in soldProducts)
        {
            if (h == p.SoldOnDate.Value.Hour)
            {
                numberSold++;
            }
        }
        //this message includes the hour and number of products sold during that hour
        Console.WriteLine($"{h}:00 -- {numberSold} items sold");
    }
    // if product was sold in that hours, console it
}