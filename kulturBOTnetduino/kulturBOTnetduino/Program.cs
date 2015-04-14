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
using System.IO;

namespace kulturBOT
{
    public class Program
    {
        static OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
        static string PrintText = string.Empty;
        static int IROBOT_RUN_TIME_MS = 15000;

        public static void Main()
        {
            Thread.Sleep(1000);
            led.Write(false);

            var stream = File.Open(@"SD\t.txt", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[30];

            stream.Read(buffer, 0, 30);

            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            PrintText = new string(enc.GetChars(buffer));

            while (true)
            {
                iRobotTest();
                //there is text to be printed!

                // todo: stop robot

                // todo: start lights flashing (probably around the printer)
                PWM printerLight = new PWM(Pins.GPIO_PIN_D5);

                printerLight.SetPulse(100, 0);
                printerLight.SetDutyCycle(100);

                for (int pulse = 0; pulse < 5; pulse++)
                {
                    for (uint i = 0; i < 100; i++)
                    {
                        printerLight.SetDutyCycle(i);
                        Thread.Sleep(3);
                    }

                    for (uint i = 0; i < 100; i++)
                    {
                        printerLight.SetDutyCycle(100 - i);
                        Thread.Sleep(3);
                    }
                }

                PrintPoem(PrintText);

                for (int pulse = 0; pulse < 5; pulse++)
                {
                    for (uint i = 0; i < 100; i++)
                    {
                        printerLight.SetDutyCycle(i);
                        Thread.Sleep(10);
                    }

                    for (uint i = 0; i < 100; i++)
                    {
                        printerLight.SetDutyCycle(100 - i);
                        Thread.Sleep(10);
                    }
                }

                printerLight.SetDutyCycle(0);

                PowerState.RebootDevice(false);
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
            Printer.PrintLine("Algorithmic poem derived");
            Printer.PrintLine("from David Thompson's");
            Printer.PrintLine("narrative, 1770-1857.");
            //Printer.Print(DateTime.UtcNow.ToString());
            Printer.LineFeed(4);
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

        //public static void raspSetup()
        //{
        //    Raspi = new SerialPort(SerialPorts.COM3, 57600, Parity.None, 8, StopBits.One);
        //    Raspi.Handshake = Handshake.XOnXOff;
        //    Raspi.WriteTimeout = 1000;
        //    Raspi.ReadTimeout = 1000;
        //    Raspi.Open();
        //    Raspi.DataReceived += new SerialDataReceivedEventHandler(Raspi_DataReceived);

        //}

        //public static int bufferOffset = 0;
        //public static byte int1;
        //public static byte int2;

        //public static void Raspi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    var port = (SerialPort)sender;
        //    int bufferSize = 512;
        //    int bufferPtr = 0;
        //    byte[] command1 = new byte[bufferSize];
        //    Thread.Sleep(1);

        //    if (bufferOffset != 0)
        //    {
        //        bufferPtr = bufferOffset;
        //        command1[0] = int1;
        //        command1[1] = int2;
        //        bufferOffset = 0;
        //    }


        //    while (port.BytesToRead > 0 && bufferPtr < bufferSize)
        //    {
        //        port.Read(command1, bufferPtr, 4);
        //        bufferPtr += 4;
        //        Thread.Sleep(1);
        //    }

        //    port.WriteByte(128);

        //    is sending a sentence
        //    if (command1[0] == 1)
        //    {
        //        if (bufferPtr == 2)
        //        {
        //            int1 = command1[0];
        //            int2 = command1[1];
        //            bufferOffset = 2;
        //            return;
        //        }

        //        byte[] sentence = new byte[command1[1]];

        //        filtering out the stop bits -- there is probably a better way to do this!
        //        for (int i = 0; i < command1[1]; i++)
        //        {
        //            if (command1[i + 2] == 0)
        //            {
        //                return;
        //            }

        //            sentence[i] = command1[i + 2];
        //        }

        //        System.Text.Encoding enc = System.Text.Encoding.UTF8;

        //        try
        //        {
        //            string myString = new string(enc.GetChars(sentence));
        //            if (myString.Length > 100)
        //                PrintText = myString;
        //        }
        //        catch
        //        {
        //        }

        //    }
        //    else
        //    {
        //        PrinterTest(string.Concat(command1[0], " ", command1[1], " ", command1[2], " ", command1[3]));
        //        for (int i = 0; i < 1; i++)
        //        {
        //            led.Write(true);
        //            Thread.Sleep(250);
        //            led.Write(false);
        //            Thread.Sleep(250);
        //        }
        //    }
        //}


    }
}
