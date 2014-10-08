kulturBOT
=========

kulturBOT is a small floor bound robot which crawls it's way through art galleries and museums. At it's core kulturBOT is an art critic, you may hear it reciting phrases from The Futurist Manifesto because it is.

Project Road Map
=========

We want to keep kulturBOT as simple as possible. Our overall plan is to incrementally add new features and functionality as outline below.

**Version 1**

This is the first version which we consider an early prototype. This is the *bare minimum* functionality required. This is designed to mimic the 'original' kulturBOT.

- [ ] Raspberry Pi mounted on the iRobot Create, hopefully using a 3D printed bracket.
- [ ] Pi Cam mounted such that it's FOV is pointed straight, again hopefully 3D printed.
- [ ] Pi using a 5V step down board (borrowed from hitchBOT) to bring the build in NiCad's down for use with the Pi
- [ ] Backend Server (not sure the implementation details yet) which will receive the pictures from Pi Cam via WiFi. Store them accordingly.
- [ ] Run iRobot Create in it's various 'demo' modes. This will require no interfacing from the Pi.
- [ ] Tweet pictures that KulturBOT takes with generated Markov Chain text. 


**Shopping List**

- [ ] Raspberry Pi x2
- [ ] Pi Cam x2
- [ ] WiFi Dongle x2
- [ ] WiFi Router - Extended Range Preferred
- [ ] Standalone Computer/Server to run backend.

**Version 2**

The idea of this version is to start the integration of the Pi and the iRobot Create. The Raspberry Pi should only act as a conveyor of information and instructions.

- [ ] Further develop backend to communication with the Pi. There should be the ability to fully control the iRobot Create and view all the sensor data from the backend.
- [ ] Abstacle avoidance. Using an Ultra Sonic sensor or similar. This should allow KulturBOT to stop before bumping into something. I am not sure whether or not the Pi should enact this or the backend. Obviously due to latency, we probably want to Pi to handle it. 


**Version 3**

This is when things get more intense. Localization. Orientation. We've got to know it all. 
