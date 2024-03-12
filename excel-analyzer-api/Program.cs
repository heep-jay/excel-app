
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



static List<int> ShortestSeriesToNumber(List<int> numbers, int target)
{
    // Initialize a dictionary to store calculated results for each target
    Dictionary<int, List<int>> memo = [];

    // Helper function for recursive calculation
    List<int>? CalculateSeries(int remainingTarget)
    {
        // If the remaining target is 0, return an empty list (base case)
        if (remainingTarget == 0)
        {
            return [];
        }

        // If the remaining target is negative or cannot be reached using the provided numbers, return null
        if (remainingTarget < 0 || !numbers.Any(num => num <= remainingTarget))
        {
            return null;
        }

        // If the result for the current target is already calculated, return it from the memo
        if (memo.ContainsKey(remainingTarget))
        {
            return memo[remainingTarget];
        }

        // Initialize the shortest series to null
        List<int>? shortestSeries = null;

        // Iterate through each number in the input list
        foreach (int num in numbers)
        {
            // Recursively calculate the series for the remaining target after subtracting the current number

            List<int> series = CalculateSeries(remainingTarget - num);

            // If the series is not null and is shorter than the current shortest series, update the shortest series
            if (series != null && (shortestSeries == null || series.Count + 1 < shortestSeries.Count))
            {
                shortestSeries = new List<int>(series); // Make a copy of the series
                shortestSeries.Insert(0, num); // Insert the current number at the beginning of the series
            }
        }

        // Memoize the result for the current target
        memo[remainingTarget] = shortestSeries;

        // Return the shortest series for the current target
        return shortestSeries;
    }

    // Call the helper function to calculate the shortest series for the target
    List<int> result = CalculateSeries(target);

    // If the result is null (i.e., the target cannot be reached using the provided numbers), return an empty list
    return result ?? [];
}
static int SumOfEvenNumbers(List<int> numbers)
{
    // Initialize the sum of even numbers
    int sum = 0;

    // Iterate through each number in the list
    foreach (int num in numbers)
    {
        // Check if the number is even
        if (num % 2 == 0)
        {
            // Add the even number to the sum
            sum += num;
        }
    }

    // Return the sum of even numbers
    return sum;
}

static int SumOfOddNumbers(List<int> numbers)
{
    // Initialize the sum of odd numbers
    int sum = 0;

    // Iterate through each number in the list
    foreach (int num in numbers)
    {
        // Check if the number is odd
        if (num % 2 != 0)
        {
            // Add the odd number to the sum
            sum += num;
        }
    }

    // Return the sum of odd numbers
    return sum;
}

static int SumOfSingleDigitNumbers(List<int> numbers)
{
    // Initialize the sum of single-digit numbers
    int sum = 0;

    // Iterate through each number in the list
    foreach (int num in numbers)
    {
        // Check if the number has only one digit
        if (Math.Abs(num) < 10)
        {
            // Add the single-digit number to the sum
            sum += num;
        }
    }

    // Return the sum of single-digit numbers
    return sum;
}

static int SumOfDoubleDigitNumbers(List<int> numbers)
{
    // Initialize the sum of double-digit numbers
    int sum = 0;

    // Iterate through each number in the list
    foreach (int num in numbers)
    {
        // Check if the number has two digits
        if (Math.Abs(num) >= 10 && Math.Abs(num) < 100)
        {
            // Add the double-digit number to the sum
            sum += num;
        }
    }

    // Return the sum of double-digit numbers
    return sum;
}




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; });

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
app.UseAntiforgery();



app.MapPost("/api/upload", ([FromForm] IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest();
    }

    // Check if the file extension is .xlsx
    if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
    {
        return Results.BadRequest("Only .xlsx files are allowed.");
    }
    using (var stream = new MemoryStream())
    {
        System.Console.WriteLine(stream);
        file.CopyTo(stream);
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.First();
            var numbers = worksheet.Cells["A:A"].Select(cell => Convert.ToInt32(cell.Value)).ToList();
            var divisibleBy2 = numbers.Where(num => num % 2 == 0).Select(num => num.ToString()).ToArray();
            var divisibleBy7 = numbers.Where(num => num % 7 == 0).Select(num => num.ToString()).ToArray();
            var divisibleBy3 = numbers.Where(num => num % 3 == 0).Select(num => num.ToString()).ToArray();
            var mean = numbers.Average();
            var median = numbers.OrderBy(num => num).ToList()[numbers.Count / 2];

            var shortestTo65 = ShortestSeriesToNumber(numbers, 65);
            var shortestTo35 = ShortestSeriesToNumber(numbers, 35);
            var sumofOdd = SumOfOddNumbers(numbers);
            var sumOfEven = SumOfEvenNumbers(numbers);
            var SumOfSingleDigit = SumOfSingleDigitNumbers(numbers);
            var SumOfDoubleDigit = SumOfDoubleDigitNumbers(numbers);
            var output = new
            {
                divisibleBy2,
                divisibleBy7,
                divisibleBy3,
                mean,
                median,
                shortestTo65,
                shortestTo35,
                sumOfEven,
                sumofOdd,
                SumOfSingleDigit,
                SumOfDoubleDigit,
            };
            return Results.Ok(output);
        }
    }

}).DisableAntiforgery();

app.MapGet("antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    var xsrfToken = tokens.RequestToken!;
    return Results.Ok(xsrfToken);
});







app.Run();
