kulturBOT
=========

kulturBOT is a small floor bound robot which crawls it's way through art galleries and museums. At it's core kulturBOT is an art critic, you may hear it reciting phrases from The Futurist Manifesto because it is.

Project Road Map
=========

We want to keep kulturBOT as simple as possible. Our overall plan is to incrementally add new features and functionality as outline below. Some of the essential design mentalities is modular-ization. We want to be able to 'hot swap' pre-built modules/electronics quickly and with ease.

**Version 1**

This is the first version which we consider an early prototype. This is the *bare minimum* functionality required. This is designed to mimic the 'original' kulturBOT.

- [X] Raspberry Pi mounted on the iRobot Create, hopefully using a 3D printed bracket.
- [X] Pi Cam mounted such that it's FOV is pointed straight, again hopefully 3D printed. Mounting should be in such a way not to interfere with the front IR beacon of the iRobot Create.
- [X] Pi Cam vibration dampening. While I cannot yet say for sure, I expect there to be a huge vibration problem with the Pi Cam, making the pictures extremely blurry/un-useable. Some kind of gimbal mount may solve this problem. This was solved by making sure the robot stopped before taking a picture.
- [X] Pi using a 5V step down board (borrowed from hitchBOT) to bring the built in ~~NiCad's~~ NiMH's down for use with the Pi
- [X] Backend Server (not sure the implementation details yet) which will receive the pictures from Pi Cam via WiFi. Store them accordingly.
- [X] Python script to take a picture using the Pi Cam, connect to the backend server and send it over the WiFi.
- [X] Run iRobot Create in it's various 'demo' modes. This will require ~~no~~ some interfacing from the Pi.
- [X] Tweet pictures that KulturBOT takes with some generated Markov Chain text.
- [ ] Python Script always running - this is probably going to change
- [ ] Wacky inflatable airtube man ontop of a strainer for the robot. Let's do it!

**Shopping List**

- [X] Raspberry Pi x2
- [X] Pi Cam x2
- [X] WiFi Dongle x2
- [X] Fabric for Wacky Inflatable Tube Man
- [X] Desktop Fan for said Inflatable Tube Man
- [X] WiFi Router - Extended Range Preferred
- [X] Standalone Computer/Server to run backend. (server could be run in the cloud but this will heavily effect latency in the future versions)
- [X] DB-25 connector/saddle.
- [X] Raspberry Pi IO header/saddle.

**Version 2**

The idea of this version is to start the integration of the Pi and the iRobot Create. The Raspberry Pi should only act as a conveyor of information and instructions.

- [ ] Dynamic battery life monitoring. This means the robot should return to base when its batteries are low, but still ensure there is enough time for it to make it back to its charging base
- [ ] Further develop backend to communicate with the Pi. The sensor data which will be very important is whether the robot is charging or not, battery status, if it is docked, moving, in trouble.. etc.. 
- [ ] Abstacle avoidance. Using an Ultra Sonic sensor or similar. This should allow KulturBOT to stop before bumping into something. I am not sure whether or not the Pi should enact this or the backend. Obviously due to latency, we probably want to Pi to handle it.
- [ ] Mount the distance/optical sensor on the front, much like the Pi Cam.
- [ ] Motion sensor mounted on the front. When kulturBOT stops it will wait until the motion sensor is tripped to start moving again. (maybe this in the future)


**Shopping List**

- [ ] Ultrasonic/Laser distance sensor.

*Note:* There is a point where we have to decide whether or not the Pi should ever intervene and make a choice. What I mean by this is should the Pi detect an on coming collision and take action or should it wait for the backend to make that choice.

**Version 3**

This furthers the integration of the Pi with the backend, adding the remaining and less necessary functionality. This version also hopes to add some controlling options as well as safety features.

- [ ] On board dedicated batteries for the Raspberry Pi. This will allow for increased capacity and possibly reduced charging time.
- [ ] Create an add-on to the existing docking station to charge the new on board batteries for the Pi. This will most likely require an external charging circuit.
- [ ] Fully control the iRobot Create and view all the sensor data from the backend.
- [ ] Control the robot with a controller, much like the Botlr. It might be cool to make another stand-alone application which can view live information about KulturBOT, maybe post this onto a website/webpage? Let people in the room control it?
- [ ] Emergency shutdown control system. Noticed that this is common practice, there should be a way to immediately cut power to KulturBOT from the wall/computer desk system.

**Shopping List**

- [ ] PS4 OR XboxOne Controller and any interfacing hardware (possibly bluetooth) required.
- [ ] Lithium Batteries and Charger Supplies

**Version 4**

This is when things get more intense. Localization. Orientation. We've got to know it all. 

- [ ] Camera with Depth. We will use this to create a map of what is in front of KulturBOT. We will create maps of the environment and KulturBOT will use these to figure out where it is in the room.
- [ ] Ability to control and 'set loose' multiple KulturBOTs in the same environment. This should be interesting.
- [ ] Fully replaced power/battery system. Hopefully entirely integrate them together.
