---------------------------------------------------
SUIMONO: CUSTOMIZABLE WATER SYSTEM
        Copyright ©2013 Tanuki Digital
               Version 2.0 BETA (0.1)
---------------------------------------------------

Thank you for supporting SUIMONO!
if you have any questions, comments, or requests for new features
please visit the Tanuki Digital Forums and post your feedback:

Note: This is a BETA Release and the documentation is still being written.  For integration
advice or any other questions regarding Suimono 2.0, please visit the Tanuki Digital forum.

http://tanukidigital.com/forum/


---------------
BETA NOTES
---------------
BETA of course means that this is “unfinished” software. All features for 2.0 are in place and the purpose of the BETA is to get the code into the hands of as many real-life users as possible... both to identify and eliminate any last minute bugs, but to also judge performance and get essential feedback.

The BETA period is planned to be very short while we put the finishing touches on the code and shaders, and finish the new Suimono 2.0 documentation files.

Normal caveats apply... this is unfinished code and we are not responsible for computer meltdowns, lost productivity, or relationship problems that occur during use of this software... USE AT YOUR OWN RISK!!!! :D 


-----------------
INSTALLATION
-----------------
Version 2.0 is very similar in structure to the current version of Suimono. One of the main updates
however is that, unlike the current version, Suimono Surface objects no longer need to be parented
to the main Module object. This will allow you more flexibility in managing your water surfaces, and
even allow you to attach them to other objects and include them in prefabs! 

IMPORT SUIMONO BASE FILES INTO YOUR PROJECT
Go to: "Assets -> Import Package -> Custom Package..." in the Unity Menu. Then select the
"Suimono_watersystem_v2beta.unitypackage" file.  This will open an import dialog box.
Click the import button and all the Suimono files will be imported into your project list.

ADD THE SUIMONO MODULES TO YOUR SCENE
1) drag the "Suimono_Module" prefab located in the "/PREFABS" folder into your scene list.
2) If it isn't set already, make sure to set the Water_Module's position in the transform settings to 0,0,0
3) drag the "Suimono_Surface" prefab into your scene list.  This is your water surface.

That's it! you can now position "Water_Surface" anywhere in your scene that you like, and even rename the "Suimono_Surface" object to anything you wish. 

Note for 3.5:
if you're installing into a Unity 3.5 scene, you'll notice a number of errors pop-up in the console on first install. This is because of the included DX11 shaders, which 3.5 doesn't recognize. However these errors should go away after the first install.

Upgrade Note:
Version 2.0 has been almost completely redesigned, as such it will be installed in it's own folder in your project hierarchy. In fact, Suimono and Suimono 2.0 could be run concurrently with each other, though we don't recommend doing so. 

A Note on Pink Water:
If you're getting a pink water plane, this is because the correct shader for your system/setup isn't loading.  For example if you're running DX11 on Windows, and getting pink water, try manually switching the water surface shader to be Suimono2/water_pro_dx11.  Similarly if you're running Unity in Android or iOS mode, try switching the shader to Suimono2/water_mobile, and if using the Free version of Unity, try switching to the mobile shader as well... Suimono2/water_mobile.

Version 2.0 should be detecting and switching these shaders automatically, however this detection does seem to break from time to time on some systems.  We hope to be able to fix this on all systems over the course of the beta.





----------------------------
CUSTOMIZATION NOTES
----------------------------
The Full Suimono 2.0 Documentation File is still being written, however you can use the below notes to help you get the most out of Suimono 2.0...

Defining your Scene Camera
Suimono tracks a scene camera in order to determine when to initiate the underwater switch. Bydefault Suimono will track the scene camera object that has it’s object tag set to “MainCamera”. However, you can customize which camera Suimono tracks by changing the “Main Camera” attribute on the Suimono_Module objects settings before running your scene.

Completely rewritten (and automated) Preset Manager
The Preset Manager has been completely rewritten to be simple to use and automatic. No more copying and pasting code! You can select a preset simply by clicking on it in the list. The “+NEW” button will create a new preset at the bottom of the list, and will fill this preset with the current settings. You can overwrite settings on a preset by pressing the “+” button next to that preset, and delete a preset entirely by pressing the “-” button. You can also rename a preset by pressing the small round button to the left of the preset name, entering the new name, then pressing OK (or X to cancel). 




----------------------
VERSION HISTORY
----------------------
ver.2.0Beta01 - Initial Beta Release!







