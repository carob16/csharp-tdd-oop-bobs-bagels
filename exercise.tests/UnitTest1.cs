using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;
using exercise.main;
using NUnit.Framework;

namespace exercise.tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void addProductToBasket()
    {
        Product product = new Product("BGLO");  
        Basket basket = new Basket();
        basket.AddProduct(product);
        Dictionary<int,Product> Result = basket.ProductList;

        Assert.That(Result.Count >= 1);
    }

    [Test]
    public void removeProductFromBasket()
    {
        Product product = new Product("BGLO");
        Basket basket = new Basket();

        basket.AddProduct(product);
        basket.AddProduct(product);
        basket.AddProduct(product);
        Assert.That(basket.ProductList.Count == 3);

        basket.RemoveProduct(2);
        
        Assert.That(basket.ProductList.Count < 3);

    }

    [Test]
    public void addingProductBeyondCapacity()
    {
        Product product = new Product("BGLO");
        Basket basket = new Basket();
        for(int i=0; i<= basket.Capacity;i++)
        {
            basket.AddProduct(product);
        }
        
        Assert.That(basket.ProductList.Count == basket.Capacity);

        bool Result = basket.AddProduct(product);

        Assert.That(basket.ProductList.Count == basket.Capacity && Result==false);

    }

    [TestCase("admin", 32)]
    [TestCase("customer", 12)]
    [TestCase("", 2)]
    [TestCase("admin", 5)]
    [TestCase("customer", 5)]

    public void changeCapacity(string adminLevel, int newCapacity)
    {
        Basket basket = new Basket();
        Person person = new Person(adminLevel);
        
        int oldCapacity = basket.Capacity;
        if (newCapacity != oldCapacity)
        {
            int Result = basket.changeCapacity(person, newCapacity);

            Assert.That(oldCapacity != Result, Is.EqualTo(person.AdminLevel == "admin"));
            Assert.That(oldCapacity != Result, Is.EqualTo(basket.Capacity == newCapacity));
        }
    }
    [TestCase(1)]
    [TestCase(0)]
    [TestCase(3)]
    public void removeNotExistingProduct(int basketID)
    {
        Basket basket = new Basket();
        Product bagel = new Product();
        Product coffe = new Product("COFW");
        

       basket.AddProduct(bagel);
       basket.AddProduct(coffe);

        bool doesExistInBasket = basket.ProductList.Keys.Contains(basketID);

         bool Result = basket.RemoveProduct(basketID);

        Assert.That(Result, Is.EqualTo(doesExistInBasket));

    }

    [TestCase("BGLO", "BGLP", "BGLE")]
    [TestCase("COFB", "COFW", "COFC")]
  //[TestCase("FILB", "FILE", "FILC")]
    [TestCase("BGLS", "COFB", "COFW")]

    public void getTotalCost(string A, string B, string C) {
        Basket basket = new Basket();

        //SKU = { "BGLO", "BGLP", "BGLE", "BGLS", "COFB", "COFW", "COFC", "COFL", "FILB", "FILE", "FILC", "FILX", "FILS", "FILH" };
        Product product1 = new Product(A);
        Product product2 = new Product(B);
        Product product3 = new Product(C);

        basket.AddProduct(product3); basket.AddProduct(product1); basket.AddProduct(product2);

        double Result = basket.GetTotal();
        double sum = Math.Round(product1.Price + product2.Price + product3.Price,2);

        Assert.That(Result, Is.EqualTo(sum));
    
    }

    [TestCase("BGLO")]
    [TestCase("BGLP")]
    [TestCase("BGLE")]
    [TestCase("BGLS")]

    public void getPriceOfSingleItem(string A)
    {
        //SKU Bagels= { "BGLO", "BGLP", "BGLE", "BGLS"}
        Product product = new Product(A);
        
        double Result = product.Price;

        Assert.That(Result > 0);

    }

    [Test]

    public void addFillingForBagel()
    {
        //SKU Bagels= { "BGLO", "BGLP", "BGLE", "BGLS"}
        //SKU Fillings = { "FILB", "FILE", "FILC", "FILX", "FILS", "FILH" };
        Product product = new Product("BGLO");
        Basket basket = new Basket();

        basket.AddProduct(product);
        bool Result = product.AddFilling("FILB");
                
        Assert.That(product.Fillings.Count>0,Is.EqualTo(Result));
        Assert.IsTrue(product.Fillings.Count > 0);

    }
    [Test]
    public void getCostOfFillig()
    {
        
        //SKU Fillings = { "FILB", "FILE", "FILC", "FILX", "FILS", "FILH" };
        Filling filling = new Filling("FILB");
        double Result = filling.Price;

        Assert.That(Result > 0, Is.EqualTo(filling.Price == 0.12));

    }


}