using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Toolbox.NETMF.Hardware;
using System.IO.Ports;
using System.Text;

namespace kulturBOT
{
    public class Program
    {
        public static SerialPort Raspi;
        static OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        public static string PrintText;

        public static void Main()
        {
            Thread.Sleep(1000);
            led.Write(false);

            raspSetup();

            while (true)
            {
                if (PrintText != null)
                {
                    lock (PrintText)
                    {
                        //there is text to be printed!

                        // todo: stop robot

                        // todo: start lights flashing (probably around the printer)

                        PrintPoem(PrintText);
                        PrintText = null;

                        //wait some period of time
                        // todo: start moving robot 
                    }
                }
                Thread.Sleep(100);
            }
        }

        public static void raspSetup()
        {
            Raspi = new SerialPort(SerialPorts.COM3, 57600, Parity.None, 8, StopBits.One);
            Raspi.Handshake = Handshake.XOnXOff;
            Raspi.WriteTimeout = 1000;

            Raspi.Open();
            Raspi.DataReceived += new SerialDataReceivedEventHandler(Raspi_DataReceived);

        }

        public static void Raspi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            int bufferSize = 512;
            int bufferPtr = 0;
            byte[] command1 = new byte[bufferSize];

            while (port.BytesToRead > 0 && bufferPtr < bufferSize)
            {
                port.Read(command1, bufferPtr++, 1);
            }

            port.WriteByte(128);

            //is sending a sentence
            if (command1[0] == 1)
            {
                byte[] sentence = new byte[command1[2]];

                //filtering out the stop bits -- there is probably a better way to do this!
                for (int i = 0; i < command1[1]; i++)
                {
                    if (command1[i + 2] == 0)
                    {
                        return;
                    }

                    sentence[i] = command1[i + 2];
                }

                System.Text.Encoding enc = System.Text.Encoding.UTF8;

                try
                {
                    string myString = new string(enc.GetChars(sentence));
                    PrintText = myString;
                }
                catch
                {
                }

            }
            else
            {
                PrinterTest(command1[0].ToString());
                for (int i = 0; i < 1; i++)
                {
                    led.Write(true);
                    Thread.Sleep(250);
                    led.Write(false);
                    Thread.Sleep(250);
                }
            }
        }

        public static void iRobotTest()
        {
            Thread.Sleep(2000);
            var serial = new SerialPort(SerialPorts.COM2, 57600);

            serial.Open();

            serial.WriteByte(128);

            serial.Write(new byte[] { 136, 0 }, 0, 2);
            Thread.Sleep(15000);
            serial.Write(new byte[] { 136, 255 }, 0, 2);

            serial.Close();
        }

        public static void PrintPoem(string poem)
        {
            ThermalPrinter Printer = new ThermalPrinter();
            foreach (string myString in poem.Split('\n', ',', '.', ';'))
            {
                Printer.PrintLine(myString);
            }

            Printer.PrintLine("@kulturBOT");
            Printer.LineFeed(1);
            Printer.Print(DateTime.UtcNow.ToString());
            Printer.LineFeed(3);
            //Printer.Dispose();
        }

        public static void PrinterTest()
        {
            Debug.Print("Setting up the printer!");
            ThermalPrinter Printer = new ThermalPrinter();
            Debug.Print("Printer is set up!");
            // Use this to make the prints of better quality
            // It will slow down the printer, and perhaps shorten the lifespan of the printer too
            //Printer.HeatingTime = 255;
            //Printer.HeatingInterval = 255;

            // Font demo
            Printer.PrintLine("Hello I am kulturBOT.");
            //Printer.PrintLine("I like to write poems and make peoples day.");
            //Printer.PrintLine("What do you think of me?");
            Printer.PrintLine("@kulturBOT");
            Printer.LineFeed(1);
            Printer.Print(DateTime.UtcNow.ToString());
            Printer.LineFeed(3);

            //Printer.Dispose();
        }

        public static void PrinterTest(string printText)
        {
            Debug.Print("Setting up the printer!");
            ThermalPrinter Printer = new ThermalPrinter();
            Debug.Print("Printer is set up!");
            // Use this to make the prints of better quality
            // It will slow down the printer, and perhaps shorten the lifespan of the printer too
            //Printer.HeatingTime = 255;
            //Printer.HeatingInterval = 255;

            // Font demo
            Printer.PrintLine(printText);
            Printer.LineFeed(3);

            Printer.Dispose();
        }
    }
}
