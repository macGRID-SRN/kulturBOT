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

        public static string[] tempPrint = new string[] { "Imself for a red with. these people trees east contain the 17th may we!", "the loaded eight. the brooks compassage of theirteen seventy miles w!", "'Ave before from the basalt was paddling been feet in the lake, some?" };
        public static DateTime fakeTime = new DateTime(2015, 4, 4, 10, 12, 23);
        public static void Main()
        {
            Microsoft.SPOT.Hardware.Utility.SetLocalTime(fakeTime);

            Thread.Sleep(1000);
            led.Write(false);

            for (int i = 0; i < 3; i++)
            {
                PrintPoem(tempPrint[i]);
                Thread.Sleep(5000);
            }
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        public static void raspSetup()
        {
            Raspi = new SerialPort(SerialPorts.COM3, 57600, Parity.None, 8, StopBits.One);
            Raspi.WriteTimeout = 1000;

            Raspi.DataReceived += new SerialDataReceivedEventHandler(Raspi_DataReceived);
            Raspi.Open();
        }

        public static void Raspi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bufferSize = 255;
            int bufferPtr = 0;
            byte[] command1 = new byte[bufferSize];

            while (Raspi.BytesToRead > 0 && bufferPtr < bufferSize)
            {
                Raspi.Read(command1, bufferPtr++, 1);
                bufferPtr++;
            }

            Raspi.WriteByte(128);

            //is sending a sentence
            if (command1[0] == 1)
            {
                byte[] sentence = new byte[command1[2]];

                //filtering out the stop bits -- there is probably a better way to do this!
                for (int i = 0; i < command1[2]; i++)
                {
                    if (command1[i * 2 + 4] == 0 || command1[i * 2 + 5] != 0)
                    {
                        return;
                    }

                    sentence[i] = command1[i * 2 + 4];
                }

                System.Text.Encoding enc = System.Text.Encoding.UTF8;

                try
                {
                    string myString = new string(enc.GetChars(sentence));
                    lock (PrintText) { PrintText = myString; }
                    //PrintPoem(myString);
                    // Create a delegate of type "ThreadStart", which is a pointer to the worker thread's main function
                    ThreadStart delegateWorkerMain = new ThreadStart(PrintThread);

                    // Next create the actual thread, passing it the delegate to the main function
                    Thread threadWorker = new Thread(delegateWorkerMain);

                    // Now start the thread.
                    threadWorker.Start();
                }
                catch
                {
                }

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

        public static string PrintText;

        public static void PrintThread()
        {
            lock (PrintText) { PrintPoem(PrintText); }
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
            foreach (string myString in poem.Split('\n'))
            {
                Printer.PrintLine(myString);
            }

            Printer.PrintLine("@kulturBOT");
            Printer.LineFeed(1);
            Printer.Print(DateTime.UtcNow.ToString());
            Printer.LineFeed(3);
            Printer.Dispose();
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
