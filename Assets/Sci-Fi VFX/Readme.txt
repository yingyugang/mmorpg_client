Thanks for you purshase!

---------------------------------------------------------------------------------------------------------
HELPFUL NOTE:

- For the materialization effect you need to use a ShaderParameterSetter script

---------------------------------------------------------------------------------------------------------
Please read how to add bloom and other effects:
---------------------------------------------------------------------------------------------------------

For correct work as in demo scene you need enable "HDR" on main camera and. 

https://www.assetstore.unity3d.com/en/#!/content/83912 link on free unity physically correct bloom.
Use follow settings:
Threshold 2
Radius 7
Intencity 1
High quality true
Anti flicker true

In forward mode, HDR does not work with antialiasing. So you need disable antialiasing (edit->project settings->quality)
or use deffered rendering mode.

Add Post processing behaviour(script) into your camera and choose PostProcessingProfile.asset Sci-Fi VFX folder

---------------------------------------------------------------------------------------------------------
CHANGE LOG
---------------------------------------------------------------------------------------------------------

v 1.0.1

fixed issues
added shield effects

v 1.0.2

added hologram effects

v 1.0.3

added engine effects
added shield on object effects

v 1.0.4

added materialization effects

---------------------------------------------------------------------------------------------------------

Best Regards,
Max