# Network Specifications #

This document indents to describe the method of sending and receiving information for the kulturBOT project.

***

The following packets are used in various combinations to define the networking interface used by the client and server.

## Init Packet

The initialization packet should be sent immediately by the client when connecting to the server. It has a length of 4 bytes. 

It is implemented as follows:

``` 
byte[] initPacket = new byte[4];
 
//the ID of the robot.
initPacket[0] = ROBOT_ID;

//The type of packet it is, eg, Sending an Image or Requesting Instructions (or updates)
initPacket[1] = CommType;
 
//some more context on the type of packet. When sending an image it is the file type
initPacket[2] = CommTypeContext;

//an extra byte for something we might want to send! 
//Haven't used this for anything specific yet, but I expect to eventually.
initPacket[3] = AdditonalInfo;
```

## Confirm Packet ##
A confirm packet is a packet send from the server to client or vise versa. It contains a single byte, implemented as follows:

```
byte[] confirmPacket = new byte[1];

//contains the value byte.MAX_VALUE or 255
confirmPacket[0] = 255;
```

## Fail Packet ##
A fail packet is send by either parties to indicate something has gone wrong :(. Infinite unhappy face :(.

```
//same idea as Confirm Packet, but for the opposite purpose.
byte[] failPacket = new byte[] {0};
```

## Partial Image Packet ##

A Partial Image Packet is used for sending large images from the client to server. It has a default length of 4096 bytes for no specific reason.

```
byte[] partialImagePacket;

//getting the next 4096 bytes from a file.
partialImagePacket = file.NextBytes(4096);
```

## Hash Packet ##

NOT YET IMPLEMENTED.

## Other Details

I think it makes sense that a connection should timeout after 5 seconds. The server shouldn't be busy for too long and if it is taking this long the client should definitely wait it out. (Take a break from sending those packets)