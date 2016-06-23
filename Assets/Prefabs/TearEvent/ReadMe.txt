This package is to create a Tear Event.

It includes:
-TearEvent prefab with two subcomponenets TearSprite and TearBackground
-TearSprite Material
-TearBackground Material
-WorldSpaceTransparencySprite shader
-TearEvent script

The prefab components don't have sprite images attached to them so you will have to place correct ones there for it to work.
The Make sure the TearSprite Material shader properties are set to Inverse Y, and TearBackground has Inverse Y turned off.
The script is attached to the TearSprite prefab component.  Attach the hand object to it there.
Make sure the TearBackground object is allso attached to the script.
Possition the TearSprite prefab component where you want the tear to take place.
Adjust the collider attached to it to choose where the event will be triggered.
Adjust the duration in seconds to change how fast the tear happens.

If I missed anything contact me.
