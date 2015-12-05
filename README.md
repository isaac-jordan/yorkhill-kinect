# GUTS Hackathon Winner 2015

Morgan Stanley challenge winner, and top prize overall winner of [Glasgow University Tech Society](http://gutechsoc.com/) Hackathon 2015.

Blog post about the weekend online at [isaacjordan.me](http://isaacjordan.me/blog/2015/12/glasgow-uni-hackathon-winner/).

Challenge was to create an application that helped the Yorkhill Childrens hospital in some way, taking into account the situation of the children, and also uses the Microsoft Kinect 2.0.

Our project consisted of a game in which players can work collaboratively to clean up germs, as well as a background program that allowed users to control their PC using the Kinect by mapping hand movement and gestures to mouse movement and clicks.

The game shows players' bodies inside a hospital environment (involved low-level byte manipulation of images to detect which pixels contained a players' body, and which were background of the room), and attaches cleaning cloths to players hands which they must use to wipe up different enemies which awards points. The game supports up to 6 people working collaboratively.

The project also used .NET Tasks to allow parallel processing of the several sensors in the  which allowed our game to run at a smooth 60 frames-per-second.

The game now has exponential germ spawning (gets crazy towards the end), and a restart button, and clearer score display.

## Demo
There is a video demo of our application on [YouTube](https://www.youtube.com/watch?v=6Gfgfw0Kw1U).

## Developers
Created by Isaac Jordan ([Github](https://github.com/sheepzez), [LinkedIn](https://uk.linkedin.com/in/ijordan)), Dylan Stevenson ([Github](https://github.com/Dillnot)), and Ewan McCartney ([Github](https://github.com/projectgoav)).

## Installation

### To Play
Download the built version from [here](https://drive.google.com/file/d/0B4XWPcc2mZMjd0ltdzVPRlVNS1k/view?usp=sharing), and extract to a folder. Then install to run. 

If the game doesn't run then you may need to install XNA using [this](https://drive.google.com/file/d/0B4XWPcc2mZMjMlVPVEtZVTlhckE/view?usp=sharing) archive. Install items 1 - 4, but not the Visual Studio extension.

### To Develop
To set up the project to run in Visual Studio 2015 (community or enterpirse), download the [Kinect SDK 2](http://www.microsoft.com/en-gb/download/details.aspx?id=44561) and install the XNA Framework and VS Extension using the link in the paragraph above.
