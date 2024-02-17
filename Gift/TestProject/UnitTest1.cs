using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dotnetapp.Models;


[TestFixture]
public class Tests
{
    private HttpClient _httpClient;
    private string _generatedToken;
    [SetUp]
    public void Setup()
    {
         _httpClient = new HttpClient();
        // _httpClient.BaseAddress = new Uri("https://localhost:7266"); 
        // _httpClient.BaseAddress = new Uri("https://api.example.com/"); 
        _httpClient.BaseAddress = new Uri("https://8080-bfdeeddcedfabcfacbdcbaeadbebabcdebdca.premiumproject.examly.io/"); 


    }

    // [Test]
    // public void Test1()
    // {
    //     Assert.Pass();
    // }

    
    [Test, Order(1)]
    public async Task Backend_TestRegisterUser()
    {
        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"Role\": \"customer\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        Console.WriteLine(response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);

        // Assuming you get a 200 OK status for successful registration
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test, Order(2)]
    public async Task Backend_TestLoginUser()
    {
        string uniqueId = Guid.NewGuid().ToString();

        string uniqueusername = $"abcd_{uniqueId}";
        string uniquepassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
        string uniquerole = "customer";
        string requestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"{uniquerole}\" }}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        string requestBody1 = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
        HttpResponseMessage response1 = await _httpClient.PostAsync("/api/login", new StringContent(requestBody1, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        string responseBody = await response1.Content.ReadAsStringAsync();
    }

    [Test, Order(3)]
    public async Task Backend_TestRegisterAdmin()
    {
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"Role\": \"admin\"}}";
        
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseBody = await response.Content.ReadAsStringAsync();
        // Add your assertions based on the response if needed
    }

    [Test, Order(4)]
    public async Task Backend_TestLoginAdmin()
    {
        string uniqueId = Guid.NewGuid().ToString();

        string uniqueusername = $"abcd_{uniqueId}";
        string uniquepassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
        string uniquerole = "admin";
        string requestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"{uniquerole}\" }}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        string requestBody1 = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
        HttpResponseMessage response1 = await _httpClient.PostAsync("/api/login", new StringContent(requestBody1, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        string responseBody = await response1.Content.ReadAsStringAsync();
    }

    [Test, Order(5)]
    public async Task Backend_TestAddGiftAdmin()
    {
       string uniqueId = Guid.NewGuid().ToString();

       // Use a dynamic and unique userName for admin (appending timestamp)
       string uniqueusername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";


       // Assume you have a valid admin registration method, adjust the request body accordingly
       string adminRegistrationRequestBody = $"{{\"password\": \"abc@123A\", \"userName\": \"{uniqueusername}\",\"role\": \"admin\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
       HttpResponseMessage registrationResponse = await _httpClient.PostAsync("api/register", new StringContent(adminRegistrationRequestBody, Encoding.UTF8, "application/json"));

       Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

       // Now, perform the login for the admin user
       string adminLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"abc@123A\"}}";
       HttpResponseMessage loginResponse = await _httpClient.PostAsync("api/login", new StringContent(adminLoginRequestBody, Encoding.UTF8, "application/json"));

       Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
       string responseBody = await loginResponse.Content.ReadAsStringAsync();

       dynamic responseMap = JsonConvert.DeserializeObject(responseBody);

       string token = responseMap.token;

       Assert.IsNotNull(token);

       string uniquetitle = Guid.NewGuid().ToString();

       // Use a dynamic and unique userName for admin (appending timestamp)
       string uniqueGiftTitle = $"giftTitle_{uniquetitle}";

       string giftJson = $"{{\"GiftType\":\"{uniqueGiftTitle}\",\"GiftImageUrl\":\"test\",\"GiftDetails\":\"test\",\"GiftPrice\":10,\"Quantity\":1}}";
       _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
       HttpResponseMessage response = await _httpClient.PostAsync("/api/gift",
           new StringContent(giftJson, Encoding.UTF8, "application/json"));

       Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test, Order(6)]
    public async Task Backend_TestGetAllGiftsForAdminAndCustomer()
    {
       string uniqueId = Guid.NewGuid().ToString();

       // Use a dynamic and unique userName for admin (appending timestamp)
       string uniqueusername = $"admin_{uniqueId}";
       string uniqueEmail = $"abcd{uniqueId}@gmail.com";


       // Assume you have a valid admin registration method, adjust the request body accordingly
       string adminRegistrationRequestBody =  $"{{\"password\": \"abc@123A\", \"userName\": \"{uniqueusername}\",\"role\": \"admin\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
       HttpResponseMessage registrationResponse = await _httpClient.PostAsync("api/register", new StringContent(adminRegistrationRequestBody, Encoding.UTF8, "application/json"));

       Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

       // Now, perform the login for the admin user
       string adminLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"abc@123A\"}}";
       HttpResponseMessage loginResponse = await _httpClient.PostAsync("api/login", new StringContent(adminLoginRequestBody, Encoding.UTF8, "application/json"));

       Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
       string responseBody = await loginResponse.Content.ReadAsStringAsync();

       dynamic responseMap = JsonConvert.DeserializeObject(responseBody);

       string token = responseMap.token;

       Assert.IsNotNull(token);


       Console.WriteLine("admin111" + token);
       _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

       HttpResponseMessage response = await _httpClient.GetAsync("/api/gift");

       Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test, Order(7)]
    public async Task Backend_TestPostReview()
    {
        HttpResponseMessage response = null;

        // Register a new customer and obtain the authentication token
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register a new customer
        string registerRequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"customer\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
        HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

        // Log in the registered customer and obtain the authentication token
        string loginRequestBody = $"{{\"Email\": \"{uniqueEmail}\", \"Password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

        string responseString = await loginResponse.Content.ReadAsStringAsync();
        dynamic responseMap = JsonConvert.DeserializeObject(responseString);
        string customerAuthToken = responseMap.token;

        // Set the authentication token in the HTTP client headers
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

        // Create a review object
        var review = new
        {
            Subject = "Test subject",
            Body = "Test body",
            Rating = 5
        };

    try
        {
            string requestBody = JsonConvert.SerializeObject(review);
            response = await _httpClient.PostAsync("/api/review", new StringContent(requestBody, Encoding.UTF8, "application/json"));

            // Print response content for debugging purposes
            Console.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");

        // Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Additional assertions based on the properties of the posted review
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");

            if (response != null)
            {
                // Print response content for debugging purposes
                Console.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
            }

            throw;
        }
    }

    [Test, Order(8)]
    public async Task Backend_TestGetAllReviews()
    {
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string RegisterrequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"admin\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
        HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(RegisterrequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

        var adminLoginRequestBody = $"{{\"Email\": \"{uniqueEmail}\", \"Password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(adminLoginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

        string responseString = await loginResponse.Content.ReadAsStringAsync();
        dynamic responseMap = JsonConvert.DeserializeObject(responseString);
        string adminAuthToken = responseMap.token;

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        HttpResponseMessage getReviewsResponse = await _httpClient.GetAsync("/api/review");
    
        Assert.AreEqual(HttpStatusCode.OK, getReviewsResponse.StatusCode);
    }
 
// [Test, Order(9)]
// public async Task Backend_TestRegisterCustomer()
// {
//     HttpResponseMessage response = null;

//     // Register a new customer and obtain the authentication token
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueUsername = $"abcd_{uniqueId}";
//     string uniquePassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a new customer
//     string registerRequestBody = $"{{\"CustomerName\": \"John Doe\", \"Address\": \"123 Main St\", \"UserId\": 1, \"User\": {{\"Password\": \"{uniquePassword}\", \"UserName\": \"{uniqueUsername}\", \"Role\": \"admin\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}} }}";
//     HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/customer", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));

//     // Assert that the registration was successful
//     Assert.AreEqual(HttpStatusCode.Created, registrationResponse.StatusCode);

//     // Optionally, you can assert additional properties based on the response if needed
//     string responseString = await registrationResponse.Content.ReadAsStringAsync();
//     dynamic responseMap = JsonConvert.DeserializeObject(responseString);

//     // Check if the dynamic object and the property you want to access are not null
//     Assert.IsNotNull(responseMap, "ResponseMap should not be null");
//     Assert.IsTrue((responseMap?.customerId ?? 0) > 0, "Customer ID should be greater than 0");
//     Console.WriteLine($"Customer ID: {responseMap?.customerId}");

//     // Perform additional assertions based on the structure of the responseMap
// }

    [Test, Order(9)]
    public async Task Backend_TestEditGift()
    {
        HttpResponseMessage response = null;

        // Register a new admin and obtain the authentication token
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"admin_{uniqueId}";
        string uniquePassword = $"adminA{uniqueId}@123";
        string uniqueEmail = $"admin{uniqueId}@gmail.com";

        // Register a new admin
        string registerRequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"admin\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
        HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

        // Log in the registered admin and obtain the authentication token
        string adminLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(adminLoginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

        string responseString = await loginResponse.Content.ReadAsStringAsync();
        dynamic responseMap = JsonConvert.DeserializeObject(responseString);
        string adminAuthToken = responseMap.token;

        // Set the authentication token in the HTTP client headers
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Create a new gift to be updated
        var giftToAdd = new
        {
            GiftType = "Test Gift",
            GiftImageUrl = "test_image.jpg",
            GiftDetails = "Original details",
            GiftPrice = 20.0,
            Quantity = 5
        };

        string addGiftRequestBody = JsonConvert.SerializeObject(giftToAdd);
        HttpResponseMessage addGiftResponse = await _httpClient.PostAsync("/api/gift", new StringContent(addGiftRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, addGiftResponse.StatusCode);

        // Retrieve the added gift details
        string addedGiftResponseString = await addGiftResponse.Content.ReadAsStringAsync();
        dynamic addedGiftResponseMap = JsonConvert.DeserializeObject(addedGiftResponseString);
        long giftId = addedGiftResponseMap.giftId;

        // Create updated gift details
        var updatedGift = new
        {
            GiftType = "Updated Gift",
            GiftImageUrl = "updated_image.jpg",
            GiftDetails = "Updated details",
            GiftPrice = 25.0,
            Quantity = 8
        };

        string updateGiftRequestBody = JsonConvert.SerializeObject(updatedGift);
        HttpResponseMessage updateGiftResponse = await _httpClient.PutAsync($"/api/gift/{giftId}", new StringContent(updateGiftRequestBody, Encoding.UTF8, "application/json"));

        // Check if the response is OK (200)
        Assert.AreEqual(HttpStatusCode.OK, updateGiftResponse.StatusCode);

    }

    [Test, Order(10)]
    public async Task Backend_TestDeleteGift()
    {
        HttpResponseMessage response = null;

        // Register a new admin and obtain the authentication token
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"admin_{uniqueId}";
        string uniquePassword = $"adminA{uniqueId}@123";
        string uniqueEmail = $"admin{uniqueId}@gmail.com";

        // Register a new admin
        string registerRequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"admin\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
        HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

        // Log in the registered admin and obtain the authentication token
        string adminLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(adminLoginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

        string responseString = await loginResponse.Content.ReadAsStringAsync();
        dynamic responseMap = JsonConvert.DeserializeObject(responseString);
        string adminAuthToken = responseMap.token;

        // Set the authentication token in the HTTP client headers
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Create a new gift to be deleted
        var giftToAdd = new
        {
            GiftType = "Test Gift",
            GiftImageUrl = "test_image.jpg",
            GiftDetails = "Original details",
            GiftPrice = 20.0,
            Quantity = 5
        };

        string addGiftRequestBody = JsonConvert.SerializeObject(giftToAdd);
        HttpResponseMessage addGiftResponse = await _httpClient.PostAsync("/api/gift", new StringContent(addGiftRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, addGiftResponse.StatusCode);

        // Retrieve the added gift details
        string addedGiftResponseString = await addGiftResponse.Content.ReadAsStringAsync();
        dynamic addedGiftResponseMap = JsonConvert.DeserializeObject(addedGiftResponseString);
        long giftId = addedGiftResponseMap.giftId;

        // Attempt to delete the gift
        HttpResponseMessage deleteGiftResponse = await _httpClient.DeleteAsync($"/api/gift/{giftId}");

        // Check if the response is OK (200)
        Assert.AreEqual(HttpStatusCode.OK, deleteGiftResponse.StatusCode);

    }
    
//  [Test, Order(11)]
// public async Task Backend_TestUpdateReview()
// {
//     HttpResponseMessage response = null;

//     // Register a new user and obtain the authentication token
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueUsername = $"user_{uniqueId}";
//     string uniquePassword = $"userA{uniqueId}@123";
//     string uniqueEmail = $"user{uniqueId}@gmail.com";

//     // Register a new user
//     string registerRequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"customer\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
//     HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

//     // Log in the registered user and obtain the authentication token
//     string userLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(userLoginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

//     string responseString = await loginResponse.Content.ReadAsStringAsync();
//     dynamic responseMap = JsonConvert.DeserializeObject(responseString);
//     string userAuthToken = responseMap.token;

//     // Set the authentication token in the HTTP client headers
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userAuthToken);

//     // Create a new review to be updated
//     var reviewToAdd = new
//     {
//         UserId = 1, // Replace with the actual user ID
//         Subject = "Test Subject",
//         Body = "Test Body",
//         Rating = 4,
//         DateCreated = DateTime.Now
//         // Add other necessary properties
//     };

//     string addReviewRequestBody = JsonConvert.SerializeObject(reviewToAdd);
//     HttpResponseMessage addReviewResponse = await _httpClient.PostAsync("/api/review", new StringContent(addReviewRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, addReviewResponse.StatusCode);

//     // Retrieve the added review details
//     string addedReviewResponseString = await addReviewResponse.Content.ReadAsStringAsync();
//     dynamic addedReviewResponseMap = JsonConvert.DeserializeObject(addedReviewResponseString);
//     int id = addedReviewResponseMap.id;

//     // Create updated review details
//     var updatedReview = new
//     {
//         UserId = 1, // Replace with the actual user ID
//         Subject = "Updated Subject",
//         Body = "Updated Body",
//         Rating = 5,
//         DateCreated = DateTime.Now
//         // Add other necessary properties
//     };

//     string updateReviewRequestBody = JsonConvert.SerializeObject(updatedReview);
//     HttpResponseMessage updateReviewResponse = await _httpClient.PutAsync($"/api/review/{id}", new StringContent(updateReviewRequestBody, Encoding.UTF8, "application/json"));

//     // Check if the response is OK (200)
//     Assert.AreEqual(HttpStatusCode.OK, updateReviewResponse.StatusCode);
// }

[Test, Order(11)]
public async Task Backend_TestUpdateReview()
{
    HttpResponseMessage response = null;

    // Register a new customer and obtain the authentication token
    string uniqueId = Guid.NewGuid().ToString();
    string uniqueUsername = $"user_{uniqueId}";
    string uniquePassword = $"userA{uniqueId}@123";
    string uniqueEmail = $"user{uniqueId}@gmail.com";

    // Register a new customer
    string registerRequestBody = $"{{\"password\": \"{uniquePassword}\", \"userName\": \"{uniqueUsername}\",\"role\": \"customer\",\"email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\"}}";
    HttpResponseMessage registrationResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, registrationResponse.StatusCode);

    // Log in the registered customer and obtain the authentication token
    string customerLoginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(customerLoginRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

    string responseString = await loginResponse.Content.ReadAsStringAsync();
    dynamic responseMap = JsonConvert.DeserializeObject(responseString);
    string customerAuthToken = responseMap.token;

    // Set the authentication token in the HTTP client headers
    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

    // Create a new review to be updated
    var reviewToAdd = new
    {
        UserId = 1, // Replace with the actual user ID
        Subject = "Test Subject",
        Body = "Test Body",
        Rating = 4,
        DateCreated = DateTime.Now
        // Add other necessary properties
    };

    string addReviewRequestBody = JsonConvert.SerializeObject(reviewToAdd);
    HttpResponseMessage addReviewResponse = await _httpClient.PostAsync("/api/review", new StringContent(addReviewRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, addReviewResponse.StatusCode);

    // Retrieve the added review details
    string addedReviewResponseString = await addReviewResponse.Content.ReadAsStringAsync();
    dynamic addedReviewResponseMap = JsonConvert.DeserializeObject(addedReviewResponseString);
    int id = addedReviewResponseMap.reviewId;

    // Log the reviewId for debugging
    Console.WriteLine($"Review ID: {id}");

    // Create updated review details
    var updatedReview = new
    {
        Subject = "Updated Subject",
        Body = "Updated Body",
        Rating = 5,
        DateCreated = DateTime.Now
        // Add other necessary properties
    };

    string updateReviewRequestBody = JsonConvert.SerializeObject(updatedReview);

    // Log the updateReviewRequestBody for debugging
    Console.WriteLine($"Update Review Request Body: {updateReviewRequestBody}");

    // HttpResponseMessage updateReviewResponse = await _httpClient.PutAsync($"/api/review/{id}", new StringContent(updateReviewRequestBody, Encoding.UTF8, "application/json"));
    HttpResponseMessage updateReviewResponse = await _httpClient.PutAsync($"/api/review/{id}", new StringContent(updateReviewRequestBody, Encoding.UTF8, "application/json"));

    // Log the updateReviewResponse for debugging
    Console.WriteLine($"Update Review Response Status Code: {updateReviewResponse.StatusCode}");
    Console.WriteLine($"Update Review Response Body: {await updateReviewResponse.Content.ReadAsStringAsync()}");

    // Check if the response is OK (200)
    Assert.AreEqual(HttpStatusCode.OK, updateReviewResponse.StatusCode);
}




    [TearDown]
    public void TearDown()
    {
        // Cleanup or additional teardown logic if needed.
        _httpClient.Dispose();
    }
}