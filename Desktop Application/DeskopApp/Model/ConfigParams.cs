namespace RpiApp.Model
{
    public class ConfigParams
    {
        static readonly string ipAddressDefault = "192.168.1.26";
        public string IpAddress;
        static readonly int sampleTimeDefault = 500;
        public int SampleTime;
        public readonly int MaxSampleNumberDefault = 100;
        public int MaxSampleNumber;
        public double XAxisMax
        {
            get
            {
                return MaxSampleNumber * SampleTime / 1000.0;
            }
            private set { }
        }

        public ConfigParams()
        {
            IpAddress = ipAddressDefault;
            SampleTime = sampleTimeDefault;
            MaxSampleNumber = MaxSampleNumberDefault;

        }

        public ConfigParams(string ip, int st, int msn)
        {
            IpAddress = ip;
            SampleTime = st;
            MaxSampleNumber = msn;
        }
    }
}
