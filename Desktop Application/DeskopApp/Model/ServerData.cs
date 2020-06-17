namespace RpiApp.Model
{
    /**
     * @brief Simple parseable data model for IoT server response
     */
    public class ServerData
    {
        public double data { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
