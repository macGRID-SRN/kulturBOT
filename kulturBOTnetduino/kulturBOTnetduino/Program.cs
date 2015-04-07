﻿using System;
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

        public static void Main()
        {
            Thread.Sleep(1000);

            raspSetup();

            while (true)
            {
                Thread.Sleep(100000);
            }
        }

        public static void raspSetup()
        {
            Raspi = new SerialPort(SerialPorts.COM3, 57600);

            Raspi.Open();

            Raspi.DataReceived += new SerialDataReceivedEventHandler(Raspi_DataReceived);
        }

        public static void Raspi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] commandDesc = new byte[2];

            Raspi.Read(commandDesc, 0, 2);

            //is sending a sentence
            if (commandDesc[0] == 129)
            {
                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                byte[] sentence = new byte[commandDesc[1]];

                Raspi.Read(sentence, 0, sentence.Length);

                string myString = new string(enc.GetChars(sentence));

                PrinterTest(myString);
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
