using System;
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

        public static void Main()
        {
            Thread.Sleep(1000);
            led.Write(false);

            raspSetup();

            while (true)
            {
                byte[] commandDesc = new byte[2];

                Raspi.Read(commandDesc, 0, 2);
                //is sending a sentence
                if (commandDesc[0] == 129)
                {
                    Raspi.Read(commandDesc, 0, 2);
                    byte[] sentence = new byte[commandDesc[0]];

                    Raspi.WriteByte(128);
                    System.Text.Encoding enc = System.Text.Encoding.UTF8;

                    Raspi.Read(sentence, 0, sentence.Length);

                    string myString = new string(enc.GetChars(sentence));

                    PrinterTest(myString);
                }
                else
                {
                    Raspi.WriteByte(255);
                    for (int i = 0; i < 1; i++)
                    {
                        led.Write(true);
                        Thread.Sleep(250);
                        led.Write(false);
                        Thread.Sleep(250);
                    }
                }

            }
        }

        public static void raspSetup()
        {
            Raspi = new SerialPort(SerialPorts.COM3, 57600, Parity.None, 8, StopBits.None);
            Raspi.WriteTimeout = 1000;
            Raspi.Open();

            //Raspi.DataReceived += new SerialDataReceivedEventHandler(Raspi_DataReceived);
        }

        public static void Raspi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

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

        //IO from the SD card breaks it :(
        //public static string ReadLineFromSD()
        //{
        //    var file = new StreamReader(@"\SD\t.txt");

        //    return file.ReadLine();

        //    //return string.Empty;
        //}

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

            Printer.Dispose();
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
