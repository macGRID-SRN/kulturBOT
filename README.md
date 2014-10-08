kulturBOT
=========

kulturBOT is a small floor bound robot which crawls it's way through art galleries and museums. At it's core kulturBOT is an art critic, you may hear it reciting phrases from The Futurist Manifesto because it is.

Project Road Map
=========

We want to keep kulturBOT as simple as possible. Our overall plan is to incrementally add new features and functionality as outline below. Some of the essential design mentalities is modular-ization. We want to be able to 'hot swap' pre-built modules/electronics quickly and with ease.

**Version 1**

This is the first version which we consider an early prototype. This is the *bare minimum* functionality required. This is designed to mimic the 'original' kulturBOT.

- [ ] Raspberry Pi mounted on the iRobot Create, hopefully using a 3D printed bracket.
- [ ] Pi Cam mounted such that it's FOV is pointed straight, again hopefully 3D printed. Mounting should be in such a way not to interfere with the front IR beacon of the iRobot Create.
- [ ] Pi using a 5V step down board (borrowed from hitchBOT) to bring the build in NiCad's down for use with the Pi
- [ ] Backend Server (not sure the implementation details yet) which will receive the pictures from Pi Cam via WiFi. Store them accordingly.
- [ ] Python script to take a picture using the Pi Cam, connect to the backend server and send it over the WiFi.
- [ ] Run iRobot Create in it's various 'demo' modes. This will require no interfacing from the Pi.
- [ ] Tweet pictures that KulturBOT takes with some generated Markov Chain text. 


**Shopping List**

- [ ] Raspberry Pi x2
- [ ] Pi Cam x2
- [ ] WiFi Dongle x2
- [ ] WiFi Router - Extended Range Preferred
- [ ] Standalone Computer/Server to run backend. (server could be run in the cloud but this will heavily effect latency in the future versions)
- [ ] DB-25 connector/saddle.
- [ ] Raspberry Pi IO header/saddle.

**Version 2**

The idea of this version is to start the integration of the Pi and the iRobot Create. The Raspberry Pi should only act as a conveyor of information and instructions.

- [ ] Further develop backend to communicate with the Pi. There should be the ability to fully control the iRobot Create and view all the sensor data from the backend. The sensor data which will be very important is whether the robot is charging or not, battery status, if it is docked, moving, in trouble.. etc.. 
- [ ] Abstacle avoidance. Using an Ultra Sonic sensor or similar. This should allow KulturBOT to stop before bumping into something. I am not sure whether or not the Pi should enact this or the backend. Obviously due to latency, we probably want to Pi to handle it.
- [ ] Mount the distance/optical sensor on the front, much like the Pi Cam.
- [ ] Control the robot with a controller, much like the Botlr. It might be cool to make another stand-alone application which can view live information about KulturBOT, maybe post this onto a website/webpage?
- [ ] Emergency shutdown control system. Noticed that this is common practice, there should be a way to immediately cut power to KulturBOT from the wall/computer desk system.

**Shopping List**

- [ ] Ultrasonic/Laser distance sensor.

*Note:* There is a point where we have to decide whether or not the Pi should ever intervene and make a choice. What I mean by this is should the Pi detect an on coming collision and take action or should it wait for the backend to make that choice.

**Version 3**

This is when things get more intense. Localization. Orientation. We've got to know it all. 

- [ ] Camera with Depth. We will use this to create a map of what is in front of KulturBOT. We will create maps of the environment and KulturBOT will use these to figure out where it is in the room. We also plan to
