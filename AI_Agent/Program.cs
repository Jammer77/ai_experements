using AutoGen;
using AutoGen.Core;
var agent = new LocalLlamaAgent();

const string role = """
You are C# coder. 
Please write only code in the codeblock. 
Don't add comment befor and after codeblock.
Please prefer modern language features. 

""";

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

Console.WriteLine(result_string);

// var userProxyAgent = new UserProxyAgent(
//             name: "user",
//             humanInputMode: HumanInputMode.ALWAYS)
//             .RegisterPrintMessage();

// var groupChat = new RoundRobinGroupChat(
//             agents: [userProxyAgent, agent]);


// var groupChatAgent = new GroupChatManager(groupChat);
// var history = await userProxyAgent.InitiateChatAsync(
//             receiver: groupChatAgent,
//             message: "Can you help me with coding?",
//             maxRound: 10);

// Console.WriteLine("");