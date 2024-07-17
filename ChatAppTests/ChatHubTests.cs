using ChatAppBAL.Interfaces;
using ChatAppBAL.Models;
using ChatAppPL.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

[TestFixture]
public class ChatHubTests
{
    private Mock<IMessagingService> _mockMessagingService;
    private Mock<IChatService> _mockChatService;
    private Mock<IUserService> _mockUserService;
    private ChatHub _chatHub;
    private Mock<IHubCallerClients> _mockClients;
    private Mock<IClientProxy> _mockClientProxy;
    private Mock<ISingleClientProxy> _mockSingleClientProxy;
    private Mock<HubCallerContext> _mockContext;
    private Mock<IGroupManager> _mockGroups;

    [SetUp]
    public void Setup()
    {
        _mockMessagingService = new Mock<IMessagingService>();
        _mockChatService = new Mock<IChatService>();
        _mockUserService = new Mock<IUserService>();
        _mockClients = new Mock<IHubCallerClients>();
        _mockClientProxy = new Mock<IClientProxy>();
        _mockContext = new Mock<HubCallerContext>();
        _mockGroups = new Mock<IGroupManager>();
        _mockSingleClientProxy = new Mock<ISingleClientProxy>();

        _mockClients.Setup(clients => clients.Caller).Returns(_mockSingleClientProxy.Object);
        _mockClients.Setup(clients => clients.Others).Returns(_mockClientProxy.Object);
        _mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        _mockClients.Setup(clients => clients.OthersInGroup(It.IsAny<string>())).Returns(_mockClientProxy.Object);

        _chatHub = new ChatHub(_mockMessagingService.Object, _mockChatService.Object, _mockUserService.Object)
        {
            Clients = _mockClients.Object,
            Context = _mockContext.Object,
            Groups = _mockGroups.Object
        };
    }

    [Test]
    public async Task SendMessage_ShouldBroadcastMessage()
    {
        var message = new MessageModel { ChatId = 1, Content = "Hello" };

        _chatHub.SendMessage(message);

        _mockMessagingService.Verify(service => service.SendMessage(message), Times.Once);

        // Verify the SendAsync calls using the Invocations collection
        var invocations = _mockClientProxy.Invocations;

        // Verify Group.SendAsync("ReceiveMessage", message)
        var groupSendAsyncInvocation = invocations.FirstOrDefault(inv =>
            inv.Method.Name == "SendCoreAsync" &&
            inv.Arguments.Count > 1 && inv.Arguments[0]?.ToString() == "ReceiveMessage");
        Assert.IsNotNull(groupSendAsyncInvocation, $"Group.SendAsync(\"ReceiveMessage\", {nameof(message)}) was not called");
    }


    [Test]
    public async Task CreateChat_ShouldAddToGroup()
    {
        var creator = new UserModel { Id = 1, UserName = "User1" };
        var newChat = new ChatModel { Id = 1, ChatName = "Chat1" };

        _mockChatService.Setup(service => service.AddChatAsync(creator, newChat)).ReturnsAsync(newChat.Id);

        var chatId = _chatHub.CreateChat(creator, newChat);

        _mockChatService.Verify(service => service.AddChatAsync(creator, newChat), Times.Once);
        _mockGroups.Verify(groups => groups.AddToGroupAsync(It.IsAny<string>(), newChat.Id.ToString(), default), Times.Once);

        // Verify the SendAsync calls using the Invocations collection
        var invocations = _mockSingleClientProxy.Invocations;

        // Verify Caller.SendAsync("ChatCreationSucces")
        var callerSendAsyncInvocation = invocations.FirstOrDefault(inv =>
            inv.Method.Name == "SendCoreAsync" &&
            inv.Arguments.FirstOrDefault()?.ToString() == "ChatCreationSucces");
        Assert.IsNotNull(callerSendAsyncInvocation, "Caller.SendAsync(\"ChatCreationSucces\") was not called");

        Assert.AreNotEqual(0, chatId);
    }
    [Test]
    public async Task DeleteChat_ShouldNotifyUsers()
    {
        var sender = new UserModel { Id = 1, UserName = "User1" };
        var chatToDelete = new ChatModel { Id = 1, ChatName = "Chat1" };

        _chatHub.DeleteChat(sender, chatToDelete);

        _mockChatService.Verify(service => service.RemoveChatAsync(sender, chatToDelete), Times.Once);

        // Verify the SendAsync calls using the Invocations collection
        var invocations = _mockSingleClientProxy.Invocations;

        // Verify Caller.SendAsync("ChatDeletionSucces")
        var callerSendAsyncInvocation = invocations.FirstOrDefault(inv =>
            inv.Method.Name == "SendCoreAsync" &&
            inv.Arguments.FirstOrDefault()?.ToString() == "ChatDeletionSucces");
        Assert.IsNotNull(callerSendAsyncInvocation, "Caller.SendAsync(\"ChatDeletionSucces\") was not called");

        // Verify Others.SendAsync("NewChatDeletion")
        invocations = _mockClientProxy.Invocations;
        var othersSendAsyncInvocation = invocations.FirstOrDefault(inv =>
            inv.Method.Name == "SendCoreAsync" &&
            inv.Arguments.FirstOrDefault()?.ToString() == "NewChatDeletion");
        Assert.IsNotNull(othersSendAsyncInvocation, "Others.SendAsync(\"NewChatDeletion\") was not called");
    }



    [Test]
    public async Task RegisterUser_ShouldReturnUserId()
    {
        var user = new UserModel { Id = 13, UserName = "User1", JoinedChatsIds = [] };
        _mockUserService.Setup(service => service.UserExist(user.Id)).Returns(true);
        _mockUserService.Setup(service => service.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        

        var userId = await _chatHub.RegisterUser(user);

        _mockUserService.Verify(service => service.GetUserByIdAsync(user.Id), Times.Once);
        _mockUserService.Verify(service => service.GetUserByIdAsync(user.Id), Times.Once);

        // Verify the SendAsync call using the invocations collection
        var invocations = _mockSingleClientProxy.Invocations;
        var sendAsyncInvocation = invocations.FirstOrDefault(inv => inv.Method.Name == "SendCoreAsync" && inv.Arguments[0].ToString() == "UserRegistrationSucces");
        Assert.IsNotNull(sendAsyncInvocation, "SendAsync with 'UserRegistrationSucces' was not called");

        Assert.AreEqual(user.Id, userId);
    }

    [TearDown]
    public void TearDown()
    {
        _chatHub.Dispose();
    }
}
