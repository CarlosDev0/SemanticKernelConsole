using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.ChatCompletion;

namespace KernelSemantico
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var result = CallAzureOpenAi().Result;
            Console.WriteLine(result);

            Console.WriteLine("Hello, World!");
        }
        static async Task<string> CallAzureOpenAi()
        {
            // Create kernel
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt-4.1",
                endpoint: "https://azureopenaitests01.openai.azure.com/",
                apiKey: "",
                modelId: "gpt-4.1" // optional
            );
            
            // Import the ConversationSummaryPlugin. DIDN'T WORK:
            //builder.Plugins.AddFromType<ConversationSummaryPlugin>();
            //kernel.ImportPluginFromObject(new ConversationSummaryPlugin(), "ConversationSummaryPlugin");
            var kernel = builder.Build();
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            var result = await kernel.InvokePromptAsync(
                "Give me a list of breakfast foods with eggs and cheese");

            string input = @"I'm a vegan in search of new recipes. I love spicy food! 
            Can you give me a list of breakfast recipes that are vegan friendly?";


            //var result2 = await kernel.InvokeAsync(
            //    "ConversationSummaryPlugin",
            //    "GetConversationActionItems",
            //    new() { { "input", input } });
            //
            //Console.WriteLine(result2);


            //var currentDay = await kernel.InvokeAsync("TimePlugin", "DayOfWeek");
            var answer = string.Concat(result.ToString(), "/n Today: "); // + result2);

            //Non-streaming chat completion
            ChatHistory history = [];
            history.AddUserMessage("Hello, how are you?");

            var response = await chatCompletionService.GetChatMessageContentAsync(
                history,
                kernel: kernel
            );

            //Streaming chat completion
            //ChatHistory history = [];
            //history.AddUserMessage("Hello, how are you?");

            //var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
            //    chatHistory: history,
            //    kernel: kernel
            //);

            //await foreach (var chunk in response)
            //{
            //    Console.Write(chunk);
            //}
            return response.ToString();
        }
    }
}
