using System.CommandLine;
using System.CommandLine.Binding;
using RestSharp;

namespace DotnetCommandLine;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var amountOfJokesOption = new Option<int>(
            name: "--amountOfJokes",
            description: "The number of jokes to return");

        var rootCommand = new RootCommand("Joke generator");
        rootCommand.AddOption(amountOfJokesOption);

        rootCommand.SetHandler(async (amountOfJokesValue, restClient) => 
            { 
                await GetJokes(amountOfJokesValue, restClient); 
            },
            amountOfJokesOption, new RestClientBinder());

        return await rootCommand.InvokeAsync(args);
    }

    public static async Task GetJokes(int amountOfJokes, RestClient restClient)
    {
        try
        {
            var request = new RestRequest("https://v2.jokeapi.dev/joke/Programming").AddParameter("amount", amountOfJokes.ToString())
                .AddParameter("type", "single");
            var response = await restClient.ExecuteAsync<JokeResponse>(request);
            Console.WriteLine("Joke found: ");
            Console.WriteLine(response.Data?.Joke);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public class RestClientBinder : BinderBase<RestClient>
    {
        protected override RestClient GetBoundValue(
            BindingContext bindingContext) => GetRestClient(bindingContext);

        RestClient GetRestClient(BindingContext bindingContext)
        {
            return new RestClient();
        }
    }
}

