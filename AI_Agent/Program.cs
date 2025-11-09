using AutoGen;
using AutoGen.Core;
var agent = new LocalLlamaAgent();

// const string system = """
// user 
// """;

string role = "You are programmer...";
string task = "Напиши на C# hello world приложение";

if (Console.IsInputRedirected)
{
    task = Console.In.ReadLine();
}

string content = $"<s>[INST] <<SYS>>\n{role}\n<</SYS>>\n### Instruction:\n{task}\n### Response:\n";

var messages = new List<TextMessage>()
{
    // new TextMessage(Role.System, system),
    new TextMessage(Role.User, content)
};

var result = agent.GenerateReplyAsync(messages);
var result_string = (result.Result as TextMessage).Content;
result_string = result_string.Replace("<|im_end|>", "").Trim(); //TODO 

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