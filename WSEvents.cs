namespace wsserver
{
    public enum EventType { REQUEST_USERS = 0, PM_USER = 1 };
    class WSEvents
    {
        public int Type { get; set; }
        public string Payload { get; set; }
    }
}
