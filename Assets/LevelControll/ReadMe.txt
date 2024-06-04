I wrote two scripts, checkpoint script attaches to the checkpoint obejct, 
the object attaches to the boxcollider and sets trigger on, and binds the levelController to the checkpoint object.

Fill the LevelController's checkpoints array with all the waypoints you need, and deactivate none of them. 

When the game starts, the system will automatically open the first checkpoint, and each checkpoint will automatically close the current checkpoint and open the next one.