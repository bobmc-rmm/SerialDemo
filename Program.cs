using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;


namespace Demo2
{
    class Program
    {
        static bool _continue;
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            Console.WriteLine("Terminal at 38400!....\n");
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();
            
            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName(_serialPort.PortName);

            //_serialPort.PortName = "COM1";
            _serialPort.BaudRate = 38400;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            

              // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
         
            Console.Write("Your Name: ");
            name = Console.ReadLine();
            readThread.Start();
            

            Console.WriteLine("Type QUIT to exit");

            while(_continue){
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>: {1}", name, message));
                }
            }
            readThread.Join();

            _serialPort.Close();
        }

        // reader thread;
        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }


        // Display Port values and prompt user to enter a port.
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        
    }
}

