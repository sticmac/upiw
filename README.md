# Unity Player Input Wrapper

**UPIW** is a tool designed to help you handling Unity’s Player Input component, from their new Input System.

## What’s the problem with Player Input?

When using Player Input component, you have 4 options to handle inputs (Send Messages, Broadcast Messages, Unity Events and C# Events).
All of them asks you to handle a monstrous `InputAction.CallbackContext` object that contains all the information from the input… And maybe, a bit too much information compared to what you need at start.
Either you let those objects go everywhere into your project (so that means that a character’s move function has to handle a `CallbackContext` instead of a simple `float` parameter…), or you build a central point where you handle all of these to then pass the right parameters to the right methods.
But such a handler might quickly grow in complexity as you add more and more controls to handle.

## What’s UPIW?

Well, that’s a tool I started to design in order to solve that problem! (yay!)

**UPIW** (for *Unity Player Input Wrapper*) is a tool helping you with Player Input component.
That means that it handles the inputs detected by some Player Input component, and redirects them to the right method calls with simple parameters, if needed (`float`,…).
It is designed so you’re not supposed to expand upon it: each new input is just an entry you have to configure inside the editor (no more code!).

All the logic is divided into a few data structure objects, one script and an editor script.

## How to use it?

1. Clone this repo inside your Unity project.
2. Place the `InputHandler` script, which is at the root of the newly created folder, as a component of any of your game objects.
3. Assign the `PlayerInput` component you’re using in the right field.

There you go! Now it works and you should have a component like this:

![UPIW1](https://sticmac.fr/img/UPIW1.png)

If you want to add a new event, select some type of argument (the one you’ll need to treat, or None if you don’t need to send any) in the dropdown menu, among those available, an click on *Create New Event*.
The Input Action Events menu will unfold.
Click on the newly created entry to unfold it.

![UPIW2](https://sticmac.fr/img/UPIW2.png)

You can now assign an input action to that event.
Choose the right name in the *Action Name* dropdown menu.
If you need the action to be treated only in some state (Started, Performed, Canceled, see the Input System documentation for more information), you can also select one.
Finally, some Unity Event matching the argument you specified earlier allows you to specify the methods to be called when this input is triggered under the conditions you specified.
This works like a common Unity Event, and if they are unknown to you, I invite you to consult the [Unity documentation entry about them](https://docs.unity3d.com/Manual/UnityEvents.html).

## I have a problem / I think some feature is missing

Don’t be shy, open an issue. I know as for now, the tool is in a very early stage of development, and it probably needs some more work.
So really, just open an issue explaining your problem and, if you think it’s a bug, how to reproduce it.

## Can I modify the project?

Sure! This asset’s source code is considered free software, through the permissive MIT license (which means you can use it in your game without having to make your game open source, but that’s another topic.)
If you have any modification to bring to the asset’s code, just make a fork of the project, and submit your pull request! I’ll be more than happy to review it and to merge it, if it brings nice improvements ;)

## What’s to come (TODO)

* Support of more argument types (`Vector2`, `Vector3`, `Quaternions`, etc…)
* Support of different action maps
* Allow the modification of one event entry’s argument type on the go
* Your suggestions!

## Contact me

* Twitter: @Sticmac_JV
* Instagram: sticmac
* Website: https://sticmac.fr
