using ChatAppBAL.Models;
using ChatAppPL.Hubs;
using ChatAppTests.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatAppTests
{
    internal class IntegrationTests
    {
        private TestServer _server;
        private HubConnection _connection;
        private WebApiFactory _factory;

        [SetUp]
        public async Task Setup()
        {
            _factory = new WebApiFactory();
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            _connection = await StartConnectionAsync(server.CreateHandler(), "chat");

            _connection.Closed += async error =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }

        [TearDown]
        public async Task TearDown()
        {
            // Dispose the SignalR connection
            await _connection.DisposeAsync();
            _factory.Dispose();
        }

        [Test]
        public async Task ClientConnects_RegistersAndCreatesChat_DeletesChatSuccessfully()
        {
            // Arrange
            var sender = new UserModel { Id = 1, UserName = "User1" };
            var creator = new UserModel { Id = 2, UserName = "User2" };
            var newChat = new ChatModel { Id = 1, ChatName = "Chat1" };

            // Act
            await _connection.InvokeAsync("RegisterUser", sender);
            await _connection.InvokeAsync("RegisterUser", creator);

            bool chatCreated = false;
            bool chatDeleted = false;

            _connection.On("ChatCreationSucces", () => { chatCreated = true; });
            _connection.On("ChatDeletionSucces", () => { chatDeleted = true; });

            var chatId = await _connection.InvokeAsync<int>("CreateChat", creator, newChat);
            Assert.That(chatId, Is.Not.EqualTo(0));
            Assert.IsTrue(chatCreated, "ChatCreationSucces event was not received");

            await _connection.InvokeAsync("DeleteChat", sender, newChat);
            Assert.IsFalse(chatDeleted, "ChatDeletionSucces event should not be received by sender");

            await _connection.InvokeAsync("DeleteChat", creator, newChat);
            Assert.IsTrue(chatDeleted, "ChatDeletionSucces event was not received after deleting chat");
        }

        private static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string hubName)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/{hubName}", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }

    }
}
