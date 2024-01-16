This is a preliminary implementation of a physics based VR character controller.

It supports 3 movement modes, 2 direction modes and 2 rotation modes.

It also has a charge-up jump button.

- ## Usage

Movement:
- ArmSwinger: Hold both (or one) 'A' button(s) and move your arms, your hand movement is translated into forward movement based on your direction mode (Head, Hands)
- Joystick: The classic joystick movement everyone seems to be used to, use your joystick to walk, its direction is transformed based on your selected direction mode.
- Teleport: Teleportation generates a navmesh and will let you teleport anywhere it was generated.

Rotation modes:
- Snap: Snap turning in 30 degree increments (increments are adjustable on the VRPlayer prefab)
- Smooth: Smooth rotation in the yaw using the joystick (The speed is adjustable on the VRPlayer prefab)

Jumping:
- The right-hand 'B' button is pressed and held, this will make your character crouch a bit as the jump charges up, release to jump.

You can change any of these settings by hitting the left hand 'B' button.
Your preferences will be saved and loaded next time you hit play.

The 'Scenes' folder has a 'ControllerTest' scene you can open up to try out some basic movement.

- Download repo
- Add as a project in s&box
- Open Hierachy and Scene windows
- Open scene
- Press play
