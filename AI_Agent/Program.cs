using AutoGen;
using AutoGen.Core;
var agent = new LocalLlamaAgent();

var userProxyAgent = new UserProxyAgent(
            name: "user",
            humanInputMode: HumanInputMode.ALWAYS)
            .RegisterPrintMessage();

var groupChat = new RoundRobinGroupChat(
            agents: [userProxyAgent, agent]);


const string role = """
You are C# coder. 
Please write only code in the codeblock. 
Don't add comment befor and after codeblock.
Please prefer modern language features. 

""";

const string defaultPrompt = "Write application which read text from input stream and add it to string variable. ";

string content = "Write nothing. Just stop working.";

if(Console.IsInputRedirected)
{
    using var reader = new StreamReader(Console.OpenStandardInput());
    content = reader.ReadToEnd();
}

var messages = new List<TextMessage>()
{
    new TextMessage(Role.User, $"{role} {content}")
};
var result = agent.GenerateReplyAsync(messages);
var result_string = (result.Result as TextMessage).Content;

// var groupChatAgent = new GroupChatManager(groupChat);
// var history = await userProxyAgent.InitiateChatAsync(
//             receiver: groupChatAgent,
//             message: "How to deploy an openai resource on azure",
//             maxRound: 10);

Console.WriteLine(result_string);
