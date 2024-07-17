namespace ChatAppBAL.Exeptions
{
    [Serializable]
    internal class ChatNotFoundExeption : Exception
    {
        public ChatNotFoundExeption()
        {
        }

        public ChatNotFoundExeption(string? message) : base(message)
        {
        }

        public ChatNotFoundExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}